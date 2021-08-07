import axios from 'axios';
import jwtDecode from 'jwt-decode';

interface JwtData {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  jti: string;
  aud: string;
  iss: string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string[] | string;
  exp: number;
}

class UserData {
  public username: string;
  public token: string;
  public expiry: Date;
  public roles: string[];
  public id: string;
  public get isExpired () {
    return this.expiry < new Date();
  }

  constructor (token: string) {
    const jwtData = jwtDecode<JwtData>(token);
    const roles = jwtData['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    this.expiry = new Date(jwtData.exp * 1000);
    this.roles = typeof roles === 'string' ? [roles] : roles;
    this.token = token;
    this.username = jwtData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    this.id = jwtData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
  }
}

export abstract class AuthService {
  public static loggedInUser?: UserData = AuthService.loginFromStorage();
  private static renewalTimer?: number;
  private static active = true;

  private static loginFromStorage (): UserData | undefined {
    const token = localStorage.getItem('jwt');
    if (!token) return undefined;

    const userData = new UserData(token);
    if (!userData.isExpired) {
      this.startSession(userData);
      return userData;
    } else {
      this.logout();
      return undefined;
    }
  }

  private static startSession (userData: UserData): void {
    if (userData.isExpired) { throw new Error('Cannot start session for expired user data'); }
    if (this.renewalTimer !== undefined || this.loggedInUser) throw new Error('A session is already in progress');

    localStorage.setItem('jwt', userData.token);
    this.loggedInUser = userData;
    axios.defaults.headers = {
      Authorization: 'Bearer ' + userData.token
    };

    const msTillExpiry = userData.expiry.valueOf() - (new Date()).valueOf();
    let msTillRenewal = msTillExpiry - 30 * 1000;
    if (msTillRenewal < 0) msTillRenewal = 0;

    this.renewalTimer = setTimeout(async () => {
      if (this.loggedInUser !== userData) throw new Error('Session was replaced without canceling old session');
      this.renewalTimer = undefined;
      this.loggedInUser = undefined;
      if (this.active) {
        this.active = false;
        const response = await axios.get<{token: string, expiration: string}>('/api/Auth/Refresh');
        const newUserData = new UserData(response.data.token);
        this.startSession(newUserData);
      }
    }, msTillRenewal);
  }

  /** Logs out of the current session */
  public static logout (): void {
    localStorage.removeItem('jwt');
    if (this.renewalTimer !== undefined) {
      clearTimeout(this.renewalTimer);
      this.renewalTimer = undefined;
    }
    if (this.loggedInUser) {
      this.loggedInUser = undefined;
    }
  }

  /** Logs into a user account */
  public static async login (username: string, password: string): Promise<boolean> {
    try {
      const response = await axios.post<{token: string, expiration: string}>('/api/Auth/Login', {
        username,
        password
      });

      if (this.loggedInUser) this.logout();
      const userData = new UserData(response.data.token);
      this.startSession(userData);

      return true;
    } catch {
      return false;
    }
  }

  /** Registers a new user account and logs in */
  public static async register (username: string, password: string, role: string): Promise<boolean> {
    try {
      await axios.post('/api/Auth/Register', {
        username,
        password,
        role
      });
      await this.login(username, password);

      return true;
    } catch {
      return false;
    }
  }

  /** Announce user activity, session may be cancelled if user activity is not announced within a token's expiry */
  public static announceActive (): void {
    this.active = true;
  }
}
