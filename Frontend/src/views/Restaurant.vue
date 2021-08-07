<template>
  <h1>{{restaurant.name}}</h1>
  <h2>{{restaurant.description}}</h2>
  <table class="table">

  </table>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { Vue, Options } from 'vue-class-component';
@Options({
  components: {
  }
})
export default class Restaurants extends Vue {

}
</script>
