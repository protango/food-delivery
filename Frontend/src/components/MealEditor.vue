<template>
  <div ref="modal" class="modal fade" tabindex="-1" id="newMealModal" v-if="!readonly">
    <div class="modal-dialog">
      <form class="modal-content" @submit="newMealSubmit" :class="{ 'was-validated': newMealFormValidated }" novalidate>
        <div class="modal-header">
          <h5 class="modal-title">New meal</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
        </div>
        <div class="modal-body">
          <div class="mb-3">
            <label for="mealName" class="form-label">Name</label>
            <input v-model="newMeal.name" class="form-control" id="mealName" type="text" required :disabled="newMealLoading"  />
          </div>
          <div class="mb-3">
            <label for="mealDesc" class="form-label">Description</label>
            <textarea v-model="newMeal.description" class="form-control" id="mealDesc" required :disabled="newMealLoading" ></textarea>
          </div>
          <div class="mb-3">
            <label for="mealPrice" class="form-label">Price</label>
            <input v-model.number="newMeal.price" class="form-control" id="mealPrice" type="number" step=".01" required :disabled="newMealLoading"  />
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal" :disabled="newMealLoading" >Discard</button>
          <button type="submit" class="btn btn-primary" :disabled="newMealLoading">
            <span class="spinner-border spinner-border-sm" role="status" v-if="newMealLoading"></span>
            Save changes
          </button>
        </div>
      </form>
    </div>
  </div>

  <div class="meal-container card" :class="{readonly}">
    <div class="card-body">
      <span v-if="meals.length === 0" class="text-muted">No meals</span>
      <div class="card meal-card" v-for="(meal, index) in meals" :key="index">
        <div class="card-body">
          <h5 class="card-title">{{meal.name}}</h5>
          <h6 class="card-subtitle mb-2 text-muted">${{meal.price.toFixed(2)}}</h6>
          <p class="card-text">{{meal.description}}</p>
          <a v-if="!readonly" class="card-link text-danger" @click="deleteMeal(meal)">Delete</a>
        </div>
      </div>
    </div>
    <div v-if="!readonly" class="card-footer text-muted">
      <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal" data-bs-target="#newMealModal" @click="openNewMeal">Add a meal</button>
    </div>
  </div>
</template>

<script lang="ts">
import { Meal, MealService, UpsertMeal } from '@/services/mealService';
import { Options, Vue } from 'vue-class-component';
import $ from 'jquery';
import 'bootstrap';

@Options({
  props: {
    restaurantId: Number,
    readonly: Boolean
  },
  emits: ['mealsChanged']
})
export default class MealEditor extends Vue {
  public readonly = false;
  public restaurantId?: number;
  public meals: Meal[] = [];

  public newMealFormValidated = false;
  public newMealLoading = false;
  public newMeal: UpsertMeal = { name: '', description: '', price: 0 };

  public openNewMeal (): void {
    this.newMealFormValidated = false;
    this.newMeal = { name: '', description: '', price: 0 };
  }

  public async deleteMeal (meal: Meal): Promise<void> {
    if (this.restaurantId == null) {
      if (this.meals.includes(meal)) {
        this.meals.splice(this.meals.indexOf(meal), 1);
      }
    } else {
      this.newMealLoading = true;
      await MealService.delete(meal.id);
      this.meals = await MealService.getForRestaurant(this.restaurantId);
      this.newMealLoading = false;
    }

    this.$emit('mealsChanged', this.meals);
  }

  public async newMealSubmit (e: Event): Promise<void> {
    e.preventDefault();
    this.newMealFormValidated = true;
    const target = e.target as HTMLFormElement;
    if (target.reportValidity()) {
      this.newMealFormValidated = false;
      if (this.restaurantId != null) {
        this.newMealLoading = false;
        await MealService.create(this.restaurantId, this.newMeal);
        this.meals = await MealService.getForRestaurant(this.restaurantId);
        this.newMealLoading = false;
      } else {
        this.meals.push({ ...this.newMeal, id: 0, restaurantId: 0 });
      }
      this.$emit('mealsChanged', this.meals);
      $(this.$refs.modal as any).modal('hide');
    }
  }

  public async beforeCreate (): Promise<void> {
    if (this.restaurantId != null) {
      this.meals = await MealService.getForRestaurant(this.restaurantId);
    } else {
      this.meals = [];
    }
  }
}
</script>

<style scoped lang="scss">
.meal-container {
  min-height: 100px;
  width: 100%;
  overflow: hidden;
  .meal-card {
    width: 300px;
    display: inline-block;
  }
}

.meal-container.readonly {
  border: none;
  &>.card-body {
    padding: 0;
  }
}
</style>
