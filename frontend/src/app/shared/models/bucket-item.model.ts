export interface BucketItemResponse {
  itemId: number;
  title: string;
  description: string | null;
  priority: 'LOW' | 'MEDIUM' | 'HIGH';
  isCompleted: number;
  completedAt: string | null;
  createdAt: string;
  categoryId: number | null;
  categoryName: string | null;
}

export interface CreateBucketItemRequest {
  title: string;
  description: string | null;
  priority: 'LOW' | 'MEDIUM' | 'HIGH';
  categoryId: number | null;
}

export interface UpdateBucketItemRequest {
  title: string;
  description: string | null;
  priority: 'LOW' | 'MEDIUM' | 'HIGH';
  categoryId: number | null;
  isCompleted: number;
}
