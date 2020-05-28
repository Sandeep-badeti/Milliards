export class Product {
    productId:number;
    productName:string;
    sku:string;
    upc:string;
    description:string;
    status:string;
    noofVersion:number;
    inventory:number;
    category:string
    shipAlone:boolean;
    manufacturer:string;
    totalRecs:number;
}
export class ProductList {
    totalRecordsCount: number
    data: Product[]
}