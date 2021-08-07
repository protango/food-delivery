<template>
  <div class="modal fade" id="exampleModal" tabindex="-1">
    <div class="modal-dialog">
      <form class="modal-content" :class="{ 'was-validated': wasValidated }" @submit="submit" novalidate>
        <div class="modal-header">
          <h5 class="modal-title">Sign {{ signUp ? "up" : "in" }}</h5>
          <button type="button" class="btn-close" @click="hide"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="usernameInput" class="form-label">Username</label>
            <input type="text" class="form-control" id="usernameInput" required v-model="username">
            <div class="invalid-feedback">Username is required</div>
          </div>
          <div class="mb-3">
            <label for="passwordInput" class="form-label">Password</label>
            <input type="password" class="form-control" id="passwordInput" :pattern="signUp ? `(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,}` : null" required v-model="password">
            <div class="invalid-feedback">
              {{
                signUp ?
                  "Password must be at least 6 characters long, containing at least one number with a mix of upper and lower case letters" :
                  "Password is required"
              }}
            </div>
          </div>
          <div class="mb-3" v-if="signUp">
            <label for="passwordInputConfim" class="form-label">Confirm Password</label>
            <input type="password" class="form-control" id="passwordInputConfim"
              @input="$event.target.setCustomValidity($event.target.value !== password ? 'Must match password' : '')">
            <div class="invalid-feedback">Must match password above</div>
          </div>
          <div class="mb-3" v-if="signUp">
            <label class="form-label">I am a</label>
            <div class="form-check">
              <input class="form-check-input" type="radio" name="roleSelector" id="roleCustomer" value="CUSTOMER" v-model="role">
              <label class="form-check-label" for="roleCustomer">
                Customer
              </label>
            </div>
            <div class="form-check">
              <input class="form-check-input" type="radio" name="roleSelector" id="roleOwner" value="RESTAURANT_OWNER" v-model="role">
              <label class="form-check-label" for="roleOwner">
                Restaurant owner
              </label>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-link" @click="switchMode">{{ signUp ? "Existing account login" : "Register a new account" }}</button>
          <button type="submit" class="btn btn-primary">{{ signUp ? "Register" : "Login" }}</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';
import $ from 'jquery';
import 'bootstrap';

@Options({
})
export default class SignInUp extends Vue {
  public role = 'CUSTOMER'
  public signUp = false
  public wasValidated = false;
  public username = '';
  public password = '';

  show (signUp?: boolean, role?: string): void {
    this.signUp = signUp ?? this.signUp;
    this.role = role ?? this.role;
    this.wasValidated = false;
    this.username = '';
    this.password = '';
    $(this.$el).modal('show');
  }

  hide (): void {
    $(this.$el).modal('hide');
  }

  switchMode (): void {
    this.show(!this.signUp, this.role);
  }

  submit (e: Event): void {
    this.wasValidated = true;
    const target = e.target as HTMLFormElement;
    if (target.checkValidity()) {
      console.log("We're in! " + this.username);
    }

    e.preventDefault();
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
