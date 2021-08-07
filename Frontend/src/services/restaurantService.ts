import axios from 'axios';
import { UpsertMeal } from './mealService';

export interface Restaurant {
  id: number;
  name: string;
  description: string;
  ownerUserId: string;
}

export interface CreateRestaurant {
  name: string;
  description: string;
  meals: UpsertMeal[]
}

export abstract class RestaurantService {
  public static async get (): Promise<Restaurant[]>;
  public static async get (id: number): Promise<Restaurant>;
  public static async get (id?: number): Promise<Restaurant | Restaurant[]> {
    return (
      await axios.get<Restaurant[] | Restaurant>(
        '/api/Restaurants' + (id != null ? '/' + id : '')
      )
    ).data;
  }

  public static async create (restaurant: CreateRestaurant): Promise<Restaurant> {
    return (await axios.post<Restaurant>('/api/Restaurants', restaurant)).data;
  }

  public static async update (id: number, restaurant: Omit<CreateRestaurant, 'meals'>): Promise<void> {
    await axios.put<void>('/api/Restaurants/' + id, restaurant);
  }

  public static async delete (id: number): Promise<void> {
    await axios.delete<void>('/api/Restaurants/' + id);
  }
}
