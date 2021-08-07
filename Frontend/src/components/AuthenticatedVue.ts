import { AuthService } from '@/services/authService';
import { Vue } from 'vue-class-component';

export default class AuthenticatedVue extends Vue {
    public isCustomer: boolean = AuthService.loggedInUser!.roles.includes('CUSTOMER');
    public isOwner: boolean = AuthService.loggedInUser!.roles.includes('RESTAURANT_OWNER');
    public username: string = AuthService.loggedInUser!.username;
}
