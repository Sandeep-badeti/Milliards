export class ViewFulOrder {
    fulOrderId: number;
    orderId: number;
    fulOrderStatus: string;
    cancelReason: string
    assignedWareHouse: string;
    assignedCarrier: string;
    carrierUpdateDate: Date;
    box: string;
    assignedCarrierService: string;
    pickingBatchId: number;
    labelBatchId: number
    palletName: string;
    originalFulOrderId: number
    onHoldFlag: string;
    onHoldReason: string;
    errorFlag: string;
    errorReason: string;
    isPrime: string;
    carrierDescription: string;
    shipByDate: Date;
    assignmentDate: Date;
    fullItemDetails:FullItem[];
}
export class FullItem {
    fulItemId:number;
    orderLineId:number;
    sku:string;
    productId:number;
    quantity:number;
    productVersion:number;
}