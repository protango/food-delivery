import axios from 'axios';

export interface UpsertMeal {
  name: string;
  description: string;
  price: number;
}

export interface Meal {
  id: number;
  name: string;
  description: string;
  price: number;
  restaurantId: number;
}

export abstract class MealService {
  public async getForRestaurant (restaurantId: number): Promise<Meal[]> {
    return (await axios.get<Meal[]>('/api/Meals/ForRestaurant/' + restaurantId)).data;
  }

  public async update (id: number, meal: UpsertMeal): Promise<void> {
    await axios.put<void>('/api/Meals/' + id, meal);
  }

  public async delete (id: number): Promise<void> {
    await axios.delete<void>('/api/Meals/' + id);
  }

  public async create (restaurantId: number, meal: UpsertMeal): Promise<void> {
    await axios.post<void>('/api/Meals/' + restaurantId, meal);
  }
}
