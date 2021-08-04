<template>
  <main>
    <div class="container py-4">
      <header class="pb-3 mb-4 border-bottom">
        <a href="/" class="d-flex align-items-center text-dark text-decoration-none">
          <img src="./assets/hamburger.png" class="logo" />
          <span class="fs-4">Food Delivery Co</span>
        </a>
      </header>

      <div class="p-5 mb-4 bg-light rounded-3">
        <div class="container-fluid py-5">
          <h1 class="display-5 fw-bold">Food Delivery Co</h1>
          <p class="col-md-8 fs-4">We deliver high quality food, right to your door! Sign up today to get two meals for the price of two!</p>
          <button class="btn btn-primary btn-lg me-1" type="button">Sign up</button>
          <button class="btn btn-outline-secondary btn-lg" type="button">Sign in</button>
        </div>
      </div>

      <div class="p-5 bg-dark rounded-3">
        <div class="text-white">
          <h2>Are you a restaurant owner?</h2>
          <p>We're always looking for new restaurants to come into our network, register now!</p>
          <button class="btn btn-outline-light me-1 mb-1" type="button">Register as a restaurant</button>
          <button class="btn btn-outline-light mb-1" type="button">Sign in as a restaurant</button>
        </div>
      </div>

      <footer class="pt-3 mt-4 text-muted border-top">
        &copy; 2021 - Zachary Murray
      </footer>
    </div>
  </main>
</template>

<style lang="css">
  .logo {
    width: 32px;
    height: 32px;
    margin-right: 10px;
  }
</style>
