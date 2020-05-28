export class Types {
    boxList: Box[];
    dimensionUnitList: DimensionUnit[];
    weightUnitList: WeightUnit[];
    productStatusList: ProductStatus[];
    productSKUStatusList: ProductSKUStatus[];
    warehouseList: WareHouse[];
    productInventoryStatusList: ProductInventoryStatus[];
    manufacturerList: Manufacturer[];
    conditionList: Condition[];
    colorList: Color[];
    productTagList: ProductTag[];
    productCategoryList: ProductCategory[];
    tagList:Tag[];
    modeList: Mode[];
    carrierList: Carrier[];
}

export class Box {
    boxId: number;
    name: string;
    length: number;
    width: number;
    height: number;
    dimensionUnitId: number;
}
export class DimensionUnit {
    dimensionUnitId: number;
    name: string;
    coversionRate: number;

}
export class WeightUnit {
    weightUnitId: number;
    name: string;
    coversionRate: number;
}
export class ProductStatus {
    productStatusId: number;
    name: string;
}
export class ProductSKUStatus {
    productSKUStatusId: number;
    name: string;
}
export class WareHouse {
    warehouseId: number;
    name: string;
    whStatusId: string;
    timezoneId: number;
    shipFromAddressId: number;
}
export class ProductInventoryStatus {
    productInventoryStatusId: number;
    name: string;
}
export class Manufacturer {
    manufacturerId: number;
    name: string;
}
export class Condition {
    conditionId: number;
    name: string;
}
export class Color {
    colorId: number;
    name: string;
}
export class ProductTag {
    productId: number;
    tagId: number;
}
export class ProductCategory {
    productCategoryId: number;
    name: string;
}
export class Tag {
    tagId:number;
    name: string;
}
export class Mode {
    modeId: number;
    modeName : string;
}
export class Carrier{
    carrierId: number;
    name: string;
}
export class CarrierService {
    carrierServiceId:number;
    name: string;
}
