export interface Author {
  id: string;
  firstName: string;
  lastName: string;
  phone: string;
  address?: string;
  city?: string;
  state?: string;
  zip?: string;
  contract: boolean;
}

export interface Publisher {
  id: string;
  name: string;
  city?: string;
  state?: string;
  country?: string;
}

export interface Title {
  id: string;
  title: string;
  type: string;
  publisherId: string;
  price?: number;
  advance?: number;
  royalty?: number;
  ytdSales?: number;
  notes?: string;
  publishedDate?: Date;
}

export interface Sale {
  id: string;
  storeId: string;
  orderId: string;
  titleId: string;
  orderDate: Date;
  quantity: number;
  payTerms: string;
}

export interface Employee {
  id: string;
  firstName: string;
  middleInitial?: string;
  lastName: string;
  jobId: number;
  jobLevel: number;
  publisherId: string;
  hireDate: Date;
}

export interface Store {
  id: string;
  name: string;
  address?: string;
  city?: string;
  state?: string;
  zip?: string;
}

export interface Notification {
  id: string;
  userId: string;
  title: string;
  message: string;
  type: NotificationType;
  status: NotificationStatus;
  createdAt: Date;
  readAt?: Date;
}

export enum NotificationType {
  Info = 'Info',
  Warning = 'Warning',
  Error = 'Error',
  Success = 'Success',
  SaleCompleted = 'SaleCompleted',
  InventoryLow = 'InventoryLow',
  NewOrder = 'NewOrder'
}

export enum NotificationStatus {
  Unread = 'Unread',
  Read = 'Read',
  Deleted = 'Deleted'
}
