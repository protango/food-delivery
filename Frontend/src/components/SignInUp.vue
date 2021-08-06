<template>
  <div class="modal fade" id="exampleModal" tabindex="-1">
    <div class="modal-dialog">
      <form class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Sign in</h5>
          <button type="button" class="btn-close" @click="$emit('closed')"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="usernameInput" class="form-label">Email address</label>
            <input type="email" class="form-control" id="usernameInput">
            <div class="form-text">We'll never share your email with anyone else.</div>
          </div>
          <div class="mb-3">
            <label for="passwordInput" class="form-label">Password</label>
            <input type="password" class="form-control" id="passwordInput">
          </div>
          <div class="mb-3">
            <label class="form-label">I am a</label>
            <div class="form-check">
              <input class="form-check-input" type="radio" name="roleSelector" id="roleCustomer" checked>
              <label class="form-check-label" for="roleCustomer">
                Customer
              </label>
            </div>
            <div class="form-check">
              <input class="form-check-input" type="radio" name="roleSelector" id="roleOwner">
              <label class="form-check-label" for="roleOwner">
                Restaurant owner
              </label>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-link">Register a new account</button>
          <button type="button" class="btn btn-primary">Login</button>
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
  props: {
    role: String,
    signUp: Boolean
  }
})
export default class SignInUp extends Vue {
  public role!: string
  public signUp!: boolean

  show (): void {
    $(this.$el).modal('show');
  }

  hide (): void {
    $(this.$el).modal('hide');
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
</style>
