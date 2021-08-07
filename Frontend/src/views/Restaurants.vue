<template>
  <h1>Restaurants</h1>
  <div class="card" style="width: 18rem;">
    <div class="card-img-top bg-secondary text-light">
      <i class="fas" :class="getRandomFoodIcon()"></i>
    </div>
    <div class="card-body">
      <h5 class="card-title">Card title</h5>
      <p class="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
      <a href="#" class="btn btn-primary">Go somewhere</a>
    </div>
    <div class="card-body">
      <a href="#" class="card-link">Card link</a>
      <a href="#" class="card-link">Another link</a>
    </div>
  </div>
</template>

<style lang="scss">
  div.card-img-top {
    text-align: center;
    i {
      line-height: 200px;
      font-size: 50px;
    }
  }
</style>

<script lang="ts">
import AuthenticatedVue from '../components/AuthenticatedVue';
export default class Restaurants extends AuthenticatedVue {
  foodIcons: string[] = [
    'fa-utensils',
    'fa-hamburger',
    'fa-pizza-slice',
    'fa-ice-cream',
    'fa-hotdog',
    'fa-fish',
    'fa-egg',
    'fa-drumstick-bite',
    'fa-cheese',
    'fa-bread-slice',
    'fa-bacon',
    'fa-cookie',
    'fa-pepper-hot',
    'fa-apple-alt'
  ];

  public getRandomFoodIcon (): Record<string, boolean> {
    const icon = this.foodIcons[Math.floor(Math.random() * this.foodIcons.length)];
    return {
      [icon]: true
    };
  }
}
</script>
