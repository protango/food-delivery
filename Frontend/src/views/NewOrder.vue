<template>
  <loading-overlay v-if="!restaurant || loading" message="Loading"></loading-overlay>
  <div v-if="restaurant">
    <h1>New Order</h1>
    <h5>Ordering from: <span class="text-primary">{{restaurant.name}}</span></h5>
    <form @submit="placeOrder">
      <table class="table">
        <thead>
          <tr>
            <th>Item</th>
            <th>Description</th>
            <th>Price</th>
            <th style="width: 200px;">Order Quantity</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="meal in meals" :key="meal.id">
            <td>{{meal.name}}</td>
            <td>{{meal.description}}</td>
            <td>${{meal.price.toFixed(2)}}</td>
            <td class="order-qty-cell">
              <a class="link-secondary" @click="orderMeals[meal.id] > 0 ? orderMeals[meal.id]-- : null">
                <i class="fas fa-minus"></i>
              </a>
              <input type="number" class="form-control" min=0 v-model="orderMeals[meal.id]" />
              <a class="link-secondary" @click="orderMeals[meal.id]++">
                <i class="fas fa-plus"></i>
              </a>
            </td>
          </tr>
        </tbody>
      </table>
      <button type="submit" class="btn btn-primary">Place Order</button>
      <span class="text-danger d-block mt-3" v-if="orderError">{{orderError}}</span>
    </form>
  </div>
</template>

<style lang="scss">
  .order-qty-cell {
    display: flex;
    i {
      line-height: 38px;
    }
    a {
      margin-right: 10px;
      margin-left: 10px;
    }
    input[type="number"] {
      -moz-appearance: textfield;
      text-align: center;
      &::-webkit-outer-spin-button, &::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
      }
    }
  }
</style>

<script lang="ts">
import { Meal, MealService } from '@/services/mealService';
import { Order, OrderService } from '@/services/orderService';
import { Restaurant, RestaurantService } from '@/services/restaurantService';
import { Vue, Options } from 'vue-class-component';
import LoadingOverlay from '../components/LoadingOverlay.vue';
@Options({
  components: {
    LoadingOverlay
  }
})
export default class NewOrder extends Vue {
  public restaurantId!: number;
  public restaurant: Restaurant | null = null;
  public meals: Meal[] = [];
  public orderMeals: Record<number, number> = {}
  public orderError = '';
  public loading = false;

  public async beforeCreate (): Promise<void> {
    try {
      this.restaurantId = Number(this.$route.params.restaurantId);
      if (isNaN(this.restaurantId)) {
        throw new Error('bad id');
      }

      this.restaurant = await RestaurantService.get(this.restaurantId);
      this.meals = await MealService.getForRestaurant(this.restaurantId);
      this.orderMeals = this.meals.reduce((a, x) => ({ ...a, [x.id]: 0 }), {} as Record<number, number>);
    } catch {
      this.$router.push('/');
    }
  }

  public async placeOrder (e: Event): Promise<void> {
    e.preventDefault();
    const mealIds = Object.keys(this.orderMeals).reduce((a, x) => {
      const mealId = Number(x);
      a.push(...Array(this.orderMeals[mealId]).fill(mealId));
      return a;
    }, [] as number[]);

    if (mealIds.length === 0) {
      this.orderError = 'Order must contain at least one meal';
      return;
    } else {
      this.orderError = '';
    }
    this.loading = true;
    await OrderService.create({
      restaurantId: this.restaurantId,
      mealIds
    });
    this.loading = false;

    this.$router.push('/dashboard/orders');
  }
}
</script>
