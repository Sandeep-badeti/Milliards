export class OrderLine {
  orderLineId: number;
  productID: number;
  productName: string;
  sku: string;
  upc: string;
  shipsAlone: boolean;
  unitPrice: string;
  quantity: number;
}
export class OrderLineList {
  data: OrderLine[]
}
