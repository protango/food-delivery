<template>
  <div>Placeholder meal editor for {{restaurantId}}</div>
</template>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';

@Options({
  props: {
    restaurantId: Number
  }
})
export default class MealEditor extends Vue {
  public restaurantId?: number;
}
</script>

<style scoped lang="scss">
</style>
