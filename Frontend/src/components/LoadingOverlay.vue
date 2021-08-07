<template>
  <div class="overlay" :style="{height: `${height}px`}">
    <div class="w-100 h-100 d-flex align-items-center justify-content-center">
      <div class="text-center">
        <div class="spinner-border text-light">
        </div>
        <br>
        <span class="text-light">{{message}}</span>
      </div>
    </div>
  </div>
</template>

<style scoped lang="scss">
  @keyframes fadein {
      from {
          opacity:0;
      }
      to {
          opacity:1;
      }
  }

  .overlay {
    animation: fadein 0.5s;
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba($color: #000000, $alpha: 0.5);
    z-index: 9;

    .spinner-border {
      margin: 0 auto;
      line-height: 100%;
    }
  }
</style>

<script lang="ts">
import { Options, Vue } from 'vue-class-component';

@Options({
  props: {
    message: String
  }
})
export default class LoadingOverlay extends Vue {
  public message?: string;
  public height = 0;

  public mounted (): void {
    const rect = this.$el.getBoundingClientRect() as DOMRect;
    this.height = window.innerHeight - rect.y;
  }
}
</script>
