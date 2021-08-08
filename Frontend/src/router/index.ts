import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import Home from '../views/Home.vue';
import Dashboard from '../views/Dashboard.vue';
import Restaurants from '../views/Restaurants.vue';
import NewRestaurant from '../views/NewRestaurant.vue';
import Restaurant from '../views/Restaurant.vue';
import Blocks from '../views/Blocks.vue';
import NewOrder from '../views/NewOrder.vue';
import Orders from '../views/Orders.vue';

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/dashboard',
    name: 'Dashboard',
    component: Dashboard,
    children: [
      {
        path: 'restaurants',
        name: 'Restaurants',
        component: Restaurants
      },
      {
        path: 'new-restaurant',
        name: 'NewRestaurant',
        component: NewRestaurant
      },
      {
        path: 'edit-restaurant/:id',
        name: 'EditRestaurant',
        component: NewRestaurant
      },
      {
        path: 'blocks',
        name: 'Blocks',
        component: Blocks
      },
      {
        path: 'new-order/:restaurantId',
        name: 'NewOrder',
        component: NewOrder
      },
      {
        path: 'new-order',
        name: 'NewOrderWithoutRestaurant',
        component: Restaurants
      },
      {
        path: 'orders/:mode',
        name: 'Orders',
        component: Orders
      },
      {
        path: 'restaurant/:id',
        name: 'Restaurant',
        component: Restaurant
      }
    ]
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
  linkExactActiveClass: 'active'
});

export default router;
