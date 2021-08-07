<template>
  <h1>New Restaurant</h1>
  <form @submit="newRestaurantSubmit" :class="{ 'was-validated': wasValidated }" novalidate>
    <div class="mb-3">
      <label for="resName" class="form-label">Restaurant name</label>
      <input type="text" class="form-control" id="resName" required :disabled="loading" v-model="newRestaurant.name">
    </div>
    <div class="mb-3">
      <label for="resName" class="form-label">Description</label>
      <textarea class="form-control" id="resName" required :disabled="loading" v-model="newRestaurant.description"></textarea>
    </div>
    <div class="mb-3">
      <label class="form-label">Meals offered</label>
      <MealEditor @mealsChanged="newRestaurant.meals = $event"></MealEditor>
    </div>
    <button type="submit" class="btn btn-primary" :disabled="loading">
      <span class="spinner-border spinner-border-sm" role="status" v-if="loading"></span>
      Submit
    </button>
  </form>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { CreateRestaurant, RestaurantService } from '@/services/restaurantService';
import { Options } from 'vue-class-component';
import AuthenticatedVue from '../components/AuthenticatedVue';
import LoadingOverlay from '../components/LoadingOverlay.vue';
import MealEditor from '../components/MealEditor.vue';
@Options({
  components: {
    LoadingOverlay,
    MealEditor
  }
})
export default class NewRestaurant extends AuthenticatedVue {
  public loading = false;
  public wasValidated = false;
  public newRestaurant: CreateRestaurant = {
    name: '',
    description: '',
    meals: []
  };

  public async newRestaurantSubmit (e: Event): Promise<void> {
    e.preventDefault();

    this.wasValidated = true;
    const target = e.target as HTMLFormElement;

    if (target.reportValidity()) {
      this.wasValidated = false;
      this.loading = true;
      const newRestaurant = await RestaurantService.create(this.newRestaurant);
      this.loading = false;
      this.$router.push('/dashboard/restaurant/' + newRestaurant.id);
    }
  }
}
</script>
