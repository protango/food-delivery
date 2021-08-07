import axios from 'axios';

export interface Block {
  blockingUserId: string;
  blockedUserId: string;
  blockedUsername: string;
}

export abstract class BlockService {
  public async get (): Promise<Block[]> {
    return (await axios.get<Block[]>('/api/Blocks')).data;
  }

  public async create (userId: string): Promise<void> {
    await axios.post<void>('/api/Blocks/' + userId);
  }

  public async delete (userId: string): Promise<void> {
    await axios.delete<void>('/api/Blocks/' + userId);
  }
}
