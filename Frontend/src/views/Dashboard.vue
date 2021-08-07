<template>
  <header class="navbar navbar-dark sticky-top bg-dark flex-md-nowrap p-0 shadow">
    <a class="navbar-brand col-md-3 col-lg-2 me-0 px-3" href="#">
      <img src="../assets/hamburger.png" class="logo" />
      <span>Food Delivery Co</span>
    </a>
    <button class="navbar-toggler position-absolute d-md-none collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#sidebarMenu" aria-controls="sidebarMenu" aria-expanded="false" aria-label="Toggle navigation">
      <span class="navbar-toggler-icon"></span>
    </button>
    <div class="navbar-nav d-none d-md-block">
      <div class="nav-item text-nowrap">
        <span class="text-white-50 px-3">Signed in as: {{ username }}</span>
      </div>
    </div>
    <div class="d-md-none w-100 bg-light text-muted py-1">
      <span class="px-3">Signed in as: {{ username }}</span>
    </div>
  </header>

  <div class="container-fluid">
    <div class="row" style="flex-wrap: nowrap">
      <nav id="sidebarMenu" class="col-md-3 col-lg-2 d-md-block bg-light sidebar collapse">
        <div class="position-sticky pt-3">
          <ul class="nav flex-column">
            <li v-if="isCustomer" class="nav-item">
              <router-link class="nav-link" to="/dashboard/new-order">
                <i class="fas fa-shopping-basket"></i>
                Order food
              </router-link>
            </li>
            <li v-if="isOwner" class="nav-item">
              <router-link class="nav-link" to="/dashboard/restaurants">
                <i class="fas fa-utensils"></i>
                Restaurants
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link" to="/dashboard/orders">
                <i class="fas fa-file-invoice-dollar"></i>
                {{isCustomer ? 'Past' : 'All'}} Orders
              </router-link>
            </li>
          </ul>

          <h6 class="sidebar-heading d-flex justify-content-between align-items-center px-3 mt-4 mb-1 text-muted">
            <span>Account management</span>
          </h6>
          <ul class="nav flex-column mb-2">
            <li v-if="isOwner" class="nav-item">
              <router-link class="nav-link" to="/dashboard/blocks">
                <i class="fas fa-ban"></i>
                Blocked users
              </router-link>
            </li>
            <li class="nav-item">
              <a class="nav-link" @click="logout">
                <i class="fas fa-sign-out-alt"></i>
                Sign out
              </a>
            </li>
          </ul>
        </div>
      </nav>

      <div class="side-spacer col-md-3 col-lg-2 d-md-block collapse"></div>

      <main class="main-view position-relative">
        <router-view></router-view>
      </main>
    </div>
  </div>
</template>

<style lang="scss">
  body {
    font-size: .875rem;
  }

  .main-view {
    width: auto !important;
    flex-grow: 1;
    flex-shrink: unset !important;
  }

  /*
  * Sidebar
  */

  .side-spacer {
    min-width: 225px;
  }

  .sidebar {
    min-width: 225px;
    position: fixed;
    top: 0;
    /* rtl:raw:
    right: 0;
    */
    bottom: 0;
    /* rtl:remove */
    left: 0;
    z-index: 100; /* Behind the navbar */
    padding: 48px 0 0; /* Height of navbar */
    box-shadow: inset -1px 0 0 rgba(0, 0, 0, .1);
  }

  @media (max-width: 767.98px) {
    .sidebar {
      top: 5rem;
    }
  }

  .sidebar-sticky {
    position: relative;
    top: 0;
    height: calc(100vh - 48px);
    padding-top: .5rem;
    overflow-x: hidden;
    overflow-y: auto;
  }

  .sidebar .nav-link {
    font-weight: 500;
    color: #333;
    i {
      margin-right: 5px;
    }
    &.active {
      color: #2470dc;
    }
  }

  .sidebar-heading {
    font-size: .75rem;
    text-transform: uppercase;
  }

  /*
  * Navbar
  */

  .navbar-brand {
    min-width: 225px;
    padding-top: 9px !important;
    padding-bottom: 9px !important;
    font-size: 1rem;
    background-color: rgba(0, 0, 0, .25);
    box-shadow: inset -1px 0 0 rgba(0, 0, 0, .25);
    .logo {
      width: 27px;
      height: 27px;
    }
    &>* {
      vertical-align: bottom;
    }
  }

  .navbar .navbar-toggler {
    top: .25rem;
    right: 1rem;
  }

  .navbar .form-control {
    padding: .75rem 1rem;
    border-width: 0;
    border-radius: 0;
  }

  .form-control-dark {
    color: #fff;
    background-color: rgba(255, 255, 255, .1);
    border-color: rgba(255, 255, 255, .1);
  }

  .form-control-dark:focus {
    border-color: transparent;
    box-shadow: 0 0 0 3px rgba(255, 255, 255, .25);
  }
</style>

<script lang="ts">
import { AuthService } from '@/services/authService';
import { Vue } from 'vue-class-component';
export default class Dashboard extends Vue {
  public isCustomer!: boolean;
  public isOwner!: boolean;

  public get username (): string {
    const lgi = AuthService.loggedInUser;
    if (!lgi) {
      this.$router.push('/');
      return '';
    }
    return lgi.username;
  }

  public beforeCreate (): void {
    if (!AuthService.loggedInUser || AuthService.loggedInUser.isExpired) {
      this.$router.push('/');
      return;
    }
    this.isCustomer = AuthService.loggedInUser.roles.includes('CUSTOMER');
    this.isOwner = AuthService.loggedInUser.roles.includes('RESTAURANT_OWNER');

    if (this.$route.path.endsWith('/dashboard')) {
      if (this.isOwner) this.$router.push('/dashboard/restaurants');
      else this.$router.push('/dashboard/new-order');
    }
  }

  public logout (): void {
    AuthService.logout();
    this.$router.push('/');
  }
}
</script>
