export class FulOrder {
    fullOrderId: number;
    orderId: number;
    warehouse: string;
    carrier: string;
    carrierservice: string;
    fulorderstatus: string;
    pickingbatchID: number;
    labelbatchID: number;
    palletID: number;
    nooffulitems: number;
    onhold: boolean;
}
export class FullOrderList {
    totalRecordsCount: number;
    data: FulOrder[];
}
export class FulOrderDetails {
    status: boolean;
    data: {
        trackingNumber: string;
        fulOrderId: number;
        productName: string;
        fulOrderStatus: string;
        sku: string;
        productPic:string;
    }
    message: string;
}
export class CarrierServiceAndType {
    carrierId: number;
    carrierServiceTypeId: number;
    carrierName: string;
}
export class CarrierServiceAndTypeResp {
    status: boolean;
    data: CarrierServiceAndType[];
    message: string;
}
export class Pallet {
    palletId: number;
    name: string;
}
export class CreatePalletResp {
    status: boolean;
    palletId: number;
    message: string;
}
export class FullOrerListByPallet {
    status: boolean;
    data: FulFillmentOrder[];
    message: string;
}
export class FulFillmentOrder {
    fulOrderId: number;
    productName: string;
    sku: string;
    fulOrderStatus: string;
}
export class CarrierServiceAndTypeResponse {
    status: boolean;
    data: CarrierServiceAndType;
    message: string;
}