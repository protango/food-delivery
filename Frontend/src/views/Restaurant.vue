<template>
  <loading-overlay v-if="!restaurant" message="Loading"></loading-overlay>
  <div v-if="restaurant">
    <h1>{{restaurant.name}}</h1>
    <h2>{{restaurant.description}}</h2>
    <table class="table">

    </table>
  </div>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { Vue, Options } from 'vue-class-component';
import { Restaurant, RestaurantService } from '../services/restaurantService';
import LoadingOverlay from '../components/LoadingOverlay.vue';
@Options({
  components: {
    LoadingOverlay
  }
})
export default class Restaurants extends Vue {
  public restaurant: Restaurant | null = null;
  public restaurantId!: number;

  public async beforeCreate (): Promise<void> {
    try {
      this.restaurantId = Number(this.$route.params.id);
      if (isNaN(this.restaurantId)) {
        throw new Error('bad id');
      }

      this.restaurant = await RestaurantService.get(this.restaurantId);
    } catch {
      this.$router.push('/');
    }
  }
}
</script>
