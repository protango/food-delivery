<template>
  <loading-overlay v-if="loading" message="Loading"></loading-overlay>
  <h1>Order details</h1>
  <order-editor :orderId="orderId" @loaded="loading = false"></order-editor>
</template>

<style lang="scss">

</style>

<script lang="ts">
import AuthenticatedVue from '@/components/AuthenticatedVue';
import { Options } from 'vue-class-component';
import LoadingOverlay from '../components/LoadingOverlay.vue';
import OrderEditor from '../components/OrderEditor.vue';
@Options({
  components: {
    LoadingOverlay,
    OrderEditor
  }
})
export default class Orders extends AuthenticatedVue {
  public loading = true;

  public get orderId (): number {
    return Number(this.$route.params.id);
  }
}
</script>
