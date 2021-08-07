<template>
  <loading-overlay v-if="!restaurant" message="Loading"></loading-overlay>
  <div v-if="restaurant">
    <h1>Viewing Restaurant</h1>
    <div class="accordion">
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#panelDetails">
            Restaurant details
          </button>
        </h2>
        <div id="panelDetails" class="accordion-collapse collapse show">
          <div class="accordion-body">
            <h5>{{restaurant.name}}</h5>
            <p>{{restaurant.description}}</p>
            <router-link :to="`/dashboard/edit-restaurant/${restaurant.id}`" class="btn btn-primary me-1">Edit restaurant</router-link>
            <a class="btn btn-danger" @click="deleteRestaurant">Delete restaurant</a>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#panelMenu">
            Menu
          </button>
        </h2>
        <div id="panelMenu" class="accordion-collapse collapse show">
          <div class="accordion-body">
            <meal-editor :restaurantId="restaurantId" :readonly="true"></meal-editor>
          </div>
        </div>
      </div>
      <div class="accordion-item">
        <h2 class="accordion-header">
          <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#panelOrders">
            Orders for this restaurant
          </button>
        </h2>
        <div id="panelOrders" class="accordion-collapse collapse show">
          <div class="accordion-body">
            <order-editor :restaurantId="restaurantId"></order-editor>
          </div>
        </div>
      </div>
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
import MealEditor from '../components/MealEditor.vue';
import OrderEditor from '../components/OrderEditor.vue';
@Options({
  components: {
    LoadingOverlay,
    MealEditor,
    OrderEditor
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
