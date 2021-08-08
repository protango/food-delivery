import axios from 'axios';
import { Meal } from './mealService';

export enum OrderStatus {
  PLACED = 'PLACED',
  CANCELLED = 'CANCELLED',
  PROCESSING = 'PROCESSING',
  EN_ROUTE = 'EN_ROUTE',
  DELIVERED = 'DELIVERED',
  RECEIVED = 'RECEIVED',
}

export interface OrderMeal {
  orderId: number,
  mealId: number,
  qty: number,
  meal: Meal
}

export interface Order {
  id: number;
  restaurantId: number;
  createdAt: Date;
  status: OrderStatus;
  userId: string;
  userName: string;
  orderMeals: OrderMeal[]
}

export interface CreateOrder {
  restaurantId: number;
  mealIds: number[];
}

export abstract class OrderService {
  public static async get (): Promise<Order[]> {
    return (await axios.get<Order[]>('/api/Orders')).data;
  }

  public static async create (order: CreateOrder): Promise<Order> {
    return (await axios.post<Order>('/api/Orders', order)).data;
  }

  public static async getForRestaurant (restaurantId: number): Promise<Order[]> {
    return (await axios.get<Order[]>('/api/Orders/ForRestaurant/' + restaurantId)).data;
  }

  public static async changeStatus (orderId: number, status: OrderStatus): Promise<void> {
    await axios.put<void>(`/api/Orders/Status/${orderId}/${status}`);
  }
}
