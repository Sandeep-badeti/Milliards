export class Order {
  orderId: number
  channel: string;
  sku: string;
  ouantity: number;
  orderType: string;
  orderNumber: string;
  orderDate: Date;
  shipByDate: Date;
  deliverByDate: Date;
  orderStatus: string;
  isFulfilled: boolean;
}
export class OrderList {
    totalRecordsCount: number
    data: Order[]
}


