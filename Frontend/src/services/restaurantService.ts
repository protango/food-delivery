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
  public async get (): Promise<Restaurant[]>;
  public async get (id: number): Promise<Restaurant>;
  public async get (id?: number): Promise<Restaurant | Restaurant[]> {
    return (
      await axios.get<Restaurant[] | Restaurant>(
        '/api/Restaurants' + (id != null ? '/' + id : '')
      )
    ).data;
  }

  public async create (restaurant: CreateRestaurant): Promise<void> {
    await axios.post<void>('/api/Restaurants', restaurant);
  }

  public async update (id: number, restaurant: Omit<CreateRestaurant, 'meals'>): Promise<void> {
    await axios.put<void>('/api/Restaurants/' + id, restaurant);
  }

  public async delete (id: number): Promise<void> {
    await axios.delete<void>('/api/Restaurants/' + id);
  }
}
