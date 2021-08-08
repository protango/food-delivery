<template>
  <ul class="list-group list-group-flush">
    <li v-if="orders.length === 0" class="list-group-item text-muted">
      There are no orders
    </li>
    <li v-for="order in orders" :key="order.id" class="list-group-item">
      <div class="ms-2 me-auto">
        <div class="fw-bold">
          Order #{{order.id}}
          <span :class="{[statusInfo[order.status].class]: true}" class="badge float-end fs-6">Status: {{statusInfo[order.status].prettyName}}</span>
        </div>
        <div class="mb-1">
          <span class="text-muted me-3 text-nowrap">{{order.createdAt}}</span>
          &#8203;
          <span class="text-muted text-nowrap">Placed by: {{order.userName}}</span>
        </div>
        <div>
          <table style="min-width: 310px;" class="table table-striped">
            <tbody>
              <tr v-for="orderMeal in order.orderMeals" :key="orderMeal.mealId">
                <td style="width: 20%">{{orderMeal.qty}}x</td>
                <td>{{orderMeal.meal.name}}</td>
                <td style="width: 20%">${{(orderMeal.qty * orderMeal.meal.price).toFixed(2)}}</td>
              </tr>
              <tr>
                <td colspan="2" class="text-end fw-bold">TOTAL</td>
                <td class="fw-bold">${{orderTotals[order.id].toFixed(2)}}</td>
              </tr>
            </tbody>
          </table>

          <button
            v-if="isOwner && order.status === 'PLACED'" class="btn btn-primary me-1" :disabled="ordersLoading.has(order.id)"
            @click="changeOrderStatus(order.id, 'PROCESSING')"
          >
            <div class="spinner-border spinner-border-sm" v-if="ordersLoading.has(order.id)"></div>
            Mark as processing
          </button>
          <button
            v-if="isCustomer && order.status === 'PLACED'" class="btn btn-danger me-1" :disabled="ordersLoading.has(order.id)"
            @click="changeOrderStatus(order.id, 'CANCELLED')"
          >
            <div class="spinner-border spinner-border-sm" v-if="ordersLoading.has(order.id)"></div>
            Cancel order
          </button>
          <button
            v-if="isOwner && order.status === 'PROCESSING'" class="btn btn-primary me-1" :disabled="ordersLoading.has(order.id)"
            @click="changeOrderStatus(order.id, 'EN_ROUTE')"
          >
            <div class="spinner-border spinner-border-sm" v-if="ordersLoading.has(order.id)"></div>
            Mark as en route
          </button>
          <button
            v-if="isOwner && order.status === 'EN_ROUTE'" class="btn btn-success me-1" :disabled="ordersLoading.has(order.id)"
            @click="changeOrderStatus(order.id, 'DELIVERED')"
          >
            <div class="spinner-border spinner-border-sm" v-if="ordersLoading.has(order.id)"></div>
            Mark as delivered
          </button>
          <button
            v-if="isCustomer && order.status === 'DELIVERED'" class="btn btn-success me-1" :disabled="ordersLoading.has(order.id)"
            @click="changeOrderStatus(order.id, 'RECEIVED')"
          >
            <div class="spinner-border spinner-border-sm" v-if="ordersLoading.has(order.id)"></div>
            Mark as received
          </button>
        </div>
      </div>
    </li>
  </ul>
</template>

<script lang="ts">
import { Meal } from '@/services/mealService';
import { Order, OrderService, OrderStatus } from '@/services/orderService';
import { Options } from 'vue-class-component';
import AuthenticatedVue from './AuthenticatedVue';

@Options({
  props: {
    restaurantId: Number
  }
})
export default class OrderEditor extends AuthenticatedVue {
  public restaurantId?: number;
  public orders: Order[] = [];
  public orderTotals: Record<number, number> = {};
  public ordersLoading = new Set<number>();

  public async refresh (): Promise<void> {
    if (this.restaurantId != null) {
      this.orders = await OrderService.getForRestaurant(this.restaurantId);
    } else {
      this.orders = await OrderService.get();
    }

    this.orderTotals = this.orders.reduce((a, x) => {
      a[x.id] = x.orderMeals.reduce((sum, g) => sum + g.qty * g.meal.price, 0);
      return a;
    }, {} as Record<number, number>);

    this.$emit('loaded');
  }

  public async mounted (): Promise<void> {
    this.refresh();
  }

  public statusInfo = {
    [OrderStatus.PLACED]: { prettyName: 'Placed', class: 'bg-secondary' },
    [OrderStatus.CANCELLED]: { prettyName: 'Cancelled', class: 'bg-danger' },
    [OrderStatus.PROCESSING]: { prettyName: 'Processing', class: 'bg-primary' },
    [OrderStatus.EN_ROUTE]: { prettyName: 'En Route', class: 'bg-primary' },
    [OrderStatus.DELIVERED]: { prettyName: 'Delivered', class: 'bg-primary' },
    [OrderStatus.RECEIVED]: { prettyName: 'Recieved', class: 'bg-success' }
  };

  public groupMeals (meals: Meal[]): {id: number, qty: number, name: string, total: number}[] {
    const reduction = meals.reduce((a, x) => {
      let val = a[x.id];
      if (!val) {
        val = { qty: 0, name: x.name, total: 0, id: x.id };
        a[x.id] = val;
      }

      val.qty++;
      val.total += x.price;
      return a;
    }, {} as Record<number, {id: number, qty: number, name: string, total: number}>);

    return Object.values(reduction);
  }

  public async changeOrderStatus (orderId: number, status: OrderStatus): Promise<void> {
    this.ordersLoading.add(orderId);
    try {
      await OrderService.changeStatus(orderId, status);
      await this.refresh();
    } finally {
      this.ordersLoading.delete(orderId);
    }
  }
}
</script>

<style scoped lang="scss">
</style>
