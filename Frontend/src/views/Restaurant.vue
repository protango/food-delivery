<template>
  <loading-overlay v-if="!restaurant" message="Loading"></loading-overlay>
  <div v-if="restaurant">
    <h1>Viewing Restaurant</h1>
    <div class="card">
      <div class="card-body">
        <h5 class="card-title">{{restaurant.name}}</h5>
        <p class="card-text">{{restaurant.description}}</p>
        <router-link :to="`/dashboard/edit-restaurant/${restaurant.id}`" class="btn btn-primary me-1">Edit restaurant</router-link>
        <a class="btn btn-danger" @click="deleteRestaurant">Delete restaurant</a>
      </div>
      <div class="card-header border-top">
        Orders for this restaurant
      </div>
      <ul class="list-group list-group-flush">
        <li v-if="!orders.length" class="list-group-item text-muted">No orders yet</li>
        <li v-for="order in orders" :key="order.id" class="list-group-item">{{order.id}}</li>
      </ul>
    </div>
  </div>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { Vue, Options } from 'vue-class-component';
import { Restaurant, RestaurantService } from '../services/restaurantService';
import { Order, OrderService } from '../services/orderService';
import LoadingOverlay from '../components/LoadingOverlay.vue';
@Options({
  components: {
    LoadingOverlay
  }
})
export default class Restaurants extends Vue {
  public restaurant: Restaurant | null = null;
  public orders: Order[] = [];
  public restaurantId!: number;

  public async deleteRestaurant (): Promise<void> {
    this.restaurant = null;
    await RestaurantService.delete(this.restaurantId);
    this.$router.push('/dashboard/restaurants');
  }

  public async beforeCreate (): Promise<void> {
    try {
      this.restaurantId = Number(this.$route.params.id);
      if (isNaN(this.restaurantId)) {
        throw new Error('bad id');
      }

      this.restaurant = await RestaurantService.get(this.restaurantId);
      this.orders = await OrderService.getForRestaurant(this.restaurantId);
    } catch {
      this.$router.push('/');
    }
  }
}
</script>
