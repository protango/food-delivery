import axios from 'axios';

export interface Block {
  blockingUserId: string;
  blockedUserId: string;
  blockedUsername: string;
}

export abstract class BlockService {
  public static async get (): Promise<Block[]> {
    return (await axios.get<Block[]>('/api/Blocks')).data;
  }

  public static async create (username: string): Promise<void> {
    await axios.post<void>('/api/Blocks/' + username);
  }

  public static async delete (userId: string): Promise<void> {
    await axios.delete<void>('/api/Blocks/' + userId);
  }
}
