export class AddProduct {
    productId?: number;
    name: string;
    description: string;
    upc: string;
    boxId: number;
    statusId: number;
    length: number;
    width: number;
    height: number;
    weight: number;
    dimensionUnitId: number;
    weightUnitId: number;
    shipsAlone_FLG: boolean = true;
    manufacturerId: number;
    conditionId: number;
    categoryId: number;
    colorId: number;
    ref1: string;
    ref2: string;
    productSku: ProductSKU[];
    productVersion: ProductVersion[];
    productInventory: ProductInventory[];
    productTag: ProductTag[];
    mainSKU: string;
}
export class ProductSKU {
    productId: number;
    sku: string;
    statusId: number;
    description: string;
    skuTypeId: number;
}
export class ProductVersion {
    productId: number;
    description: string;
    statusId: number;
    productVersionId: number;
}
export class ProductInventory {
    productVersionId: number;
    warehouseId: number;
    quantity: number;
    statusId: number;
    productInventoryId:number;
}
export class ProductTag {
    productId: number;
    tagId: number;
}
export class ProductDetails {
    productId: number;
    name: string;
    description: string;
    mainSKU: string;
    upc: string;
    weight: number;
    length: number;
    height: number;
    width: number;
    shipsAlone_FLG: boolean;
    statusId: number;
    weightUnitId: number;
    dimensionUnitId: number;
    colorId: number;
    conditionId: number;
    manufacturerId: number;
    categoryId: number;
    boxId: number;
    ref1: string;
    ref2: string;
    productSku: ProductSKU[];
    productVersion: ProductVersion[];
    productInventory: ProductInventory[];
    productTag: ProductTag[];

}
export class ProductView {
    productId: number;
    name: string;
    description: string;
    mainSKU: string;
    upc: string;
    weight: number;
    length: number;
    height: number;
    width: number;
    shipsAlone_FLG: boolean;
    statusId: number;
    statusName: string;
    weightUnitId: number;
    weightUnitName: string;
    dimensionUnitId: number;
    dimensionName: string;
    colorId: number;
    colorName: string;
    conditionId: number;
    conditionName: string;
    manufacturerId: number;
    manufacturerName: string;
    categoryId: number;
    categoryName: string;
    boxId: number;
    boxName: string;
    ref1: string;
    ref2: string;
    productSku: Array<any>;
    productVersion: Array<any>;
    productInventory: Array<any>;
    productTag: Array<any>;
}