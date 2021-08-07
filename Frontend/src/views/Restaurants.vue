<template>
  <loading-overlay v-if="loading" message="Loading"></loading-overlay>
  <h1>Restaurants</h1>
  <router-link class="btn btn-primary mb-2" v-if="isOwner" to="/dashboard/new-restaurant"><i class="fas fa-plus"></i> New Restaurant</router-link>
  <div class="d-flex" style="flex-wrap: wrap;">
    <div class="card restaurant-card" v-for="item in restaurants" :key="item.id">
      <div class="card-img-top bg-secondary text-light">
        <i class="fas" :class="getFoodIcon(item.id)"></i>
      </div>
      <div class="card-body">
        <h5 class="card-title">{{item.name}}</h5>
        <p class="card-text">{{item.description}}</p>
        <router-link class="btn btn-primary" :to="`/dashboard/new-order/${item.id}`" v-if="isCustomer">Place order</router-link>
      </div>
      <div class="card-footer text-muted" v-if="item.ownerUserId === userId">
        <router-link class="card-link" :to="`/dashboard/restaurant/${item.id}`">View orders</router-link>
        <router-link class="card-link" :to="`/dashboard/edit-restaurant/${item.id}`">Edit</router-link>
        <a class="card-link text-danger" @click="deleteRestaurant(item.id)">Delete</a>
      </div>
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

  .card {
    margin-right: 10px;
    margin-bottom: 10px;
  }

  .restaurant-card {
    width: 18rem;
    min-width: 300px !important;
  }
</style>

<script lang="ts">
import { Options } from 'vue-class-component';
import AuthenticatedVue from '../components/AuthenticatedVue';
import { Restaurant, RestaurantService } from '../services/restaurantService';
import LoadingOverlay from '../components/LoadingOverlay.vue';
@Options({
  components: {
    LoadingOverlay
  }
})
export default class Restaurants extends AuthenticatedVue {
  public foodIcons: string[] = [
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

  public loading = true;
  public restaurants: Restaurant[] = [];

  private iconRestaurantMap: Record<number, string> = {};

  public getFoodIcon (restaurantId: number): Record<string, boolean> {
    let icon: string = this.iconRestaurantMap[restaurantId];
    if (!icon) {
      icon = this.foodIcons[Math.floor(Math.random() * this.foodIcons.length)];
      this.iconRestaurantMap[restaurantId] = icon;
    }
    return {
      [icon]: true
    };
  }

  public async beforeCreate (): Promise<void> {
    this.restaurants = await RestaurantService.get();
    this.loading = false;
  }

  public async deleteRestaurant (restaurantId: number): Promise<void> {
    this.loading = true;
    try {
      await RestaurantService.delete(restaurantId);
      this.restaurants = await RestaurantService.get();
    } finally {
      this.loading = false;
    }
  }
}
</script>
