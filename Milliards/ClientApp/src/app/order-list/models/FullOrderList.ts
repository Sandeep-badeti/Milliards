export class FullOrder {
  FulOrderId: number;
  Status: string;
  AssignedWarehouseId: number;
  AssignedCarrierId: string;
  CarrierUpdateDate: Date;
  BoxId: number;
  NoofFulfillmentItems: number;
}
export class FullOrderList {
  data: FullOrder[]
}
