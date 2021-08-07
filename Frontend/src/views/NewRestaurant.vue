<template>
  <h1>New Restaurant</h1>
  <form>
    <div class="mb-3">
      <label for="resName" class="form-label">Restaurant name</label>
      <input type="text" class="form-control" id="resName">
    </div>
    <div class="mb-3">
      <label for="resName" class="form-label">Description</label>
      <textarea class="form-control" id="resName"></textarea>
    </div>
    <div class="mb-3">
      <label class="form-label">Meals offered</label>
      <MealEditor></MealEditor>
    </div>
    <button type="submit" class="btn btn-primary">Submit</button>
  </form>
</template>

<style lang="scss">

</style>

<script lang="ts">
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
}
</script>
