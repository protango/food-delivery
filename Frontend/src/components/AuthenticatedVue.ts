import { AuthService, UserData } from '@/services/authService';
import { Vue } from 'vue-class-component';

function getLoggedInUser (): UserData {
  if (AuthService.loggedInUser) return AuthService.loggedInUser;
  return {
    id: '',
    expiry: new Date(),
    isExpired: true,
    roles: [],
    token: '',
    username: 'UNAUTHENTICATED'
  };
}
export default class AuthenticatedVue extends Vue {
    public isCustomer: boolean = getLoggedInUser().roles.includes('CUSTOMER');
    public isOwner: boolean = getLoggedInUser().roles.includes('RESTAURANT_OWNER');
    public username: string = getLoggedInUser().username;
    public userId: string = getLoggedInUser().id;
}
