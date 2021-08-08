<template>
  <h1>{{customerMode ? "My" : "All"}} Orders</h1>
</template>

<style lang="scss">

</style>

<script lang="ts">
import { Vue, Options } from 'vue-class-component';
@Options({
  components: {
  }
})
export default class Orders extends Vue {
  public get customerMode (): boolean {
    return this.$route.params.mode === 'customer';
  }

  public async beforeCreate (): Promise<void> {
    console.log('lmao');
  }
}
</script>
