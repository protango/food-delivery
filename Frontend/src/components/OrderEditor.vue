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
              <tr v-for="orderMeal in orderMealGroups[order.id]" :key="orderMeal.id">
                <td>{{orderMeal.qty}}x</td>
                <td>{{orderMeal.name}}</td>
                <td>${{orderMeal.total.toFixed(2)}}</td>
              </tr>
              <tr>
                <td colspan="2" class="text-end fw-bold">TOTAL</td>
                <td class="fw-bold">${{orderTotals[order.id].toFixed(2)}}</td>
              </tr>
            </tbody>
          </table>
          <button v-if="isOwner && order.status === 'PLACED'" class="btn btn-primary me-1">Mark as processing</button>
          <button v-if="isCustomer && order.status === 'PLACED'" class="btn btn-danger me-1">Cancel order</button>
          <button v-if="isOwner && order.status === 'PROCESSING'" class="btn btn-primary me-1">Mark as en route</button>
          <button v-if="isOwner && order.status === 'EN_ROUTE'" class="btn btn-success me-1">Mark as delivered</button>
          <button v-if="isCustomer && order.status === 'DELIVERED'" class="btn btn-success me-1">Mark as received</button>
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
  public orderMealGroups: Record<number, {id: number, qty: number, name: string, total: number}[]> = {};
  public orderTotals: Record<number, number> = {};

  public async beforeCreate (): Promise<void> {
    if (this.restaurantId != null) {
      this.orders = await OrderService.getForRestaurant(this.restaurantId);
    } else {
      this.orders = await OrderService.get();
    }

    this.orderMealGroups = this.orders.reduce((a, x) => {
      a[x.id] = this.groupMeals(x.meals);
      return a;
    }, {} as Record<number, {id: number, qty: number, name: string, total: number}[]>);

    this.orderTotals = this.orders.reduce((a, x) => {
      a[x.id] = this.orderMealGroups[x.id].reduce((sum, g) => sum + g.total, 0);
      return a;
    }, {} as Record<number, number>);
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
}
</script>

<style scoped lang="scss">
</style>
