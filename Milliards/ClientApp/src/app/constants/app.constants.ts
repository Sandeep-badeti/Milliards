export class AppConstants {

    /**
     * Common app constants
     */
    public static readonly DefaultLang: string = 'en';
    public static readonly Data: string = 'data';
    public static readonly Asc: string = 'asc';
    public static readonly Desc: string = 'desc';
    public static readonly HasMore: string = 'hasmore';
    public static readonly NoMore: string = 'nomore';
    public static readonly ShowMore: string = 'showMore';
    public static readonly FetchType: string = 'orderLines';
    public static readonly MMDDYYYY = 'MM/DD/YYYY';

    public static readonly ActivePath: string = 'products/list'
    public static readonly baseUrl: string = '../assets/images/';
    public static readonly UpArrow: string = 'upArraow.png';
    public static readonly DownArrow: string = 'upArraow.png';
    public static readonly selectRecsForPick: string = 'Please select at least one order for picking';
    public static readonly selectwarehouseForPick: string = 'Please select a warehouse for picking';
    public static readonly SelectCarrierAndService: string = 'Please Select Carrier And Service';
    public static readonly SelectPalletId: string = 'Please Select Pallet Id';
    public static readonly EnterTrackingNumber: string = 'Please Enter Tracking Number';
    public static readonly Sku: string = 'SKU';
    public static readonly Upc: string = 'UPC';
    public static readonly Add: string = 'Add';
    public static readonly Edit: string = 'Edit';
    public static readonly View: string = 'View';
    public static readonly Clone: string = 'Clone';
    public static readonly MainSKUIndex: number = 9999;
    public static readonly MainSKUTypeId: number = 2;
    public static readonly DefaultDescription: string = 'Default Version';
    public static readonly ModeRegular: string = 'Regular';

    public static readonly PagingTypeFull: string = 'full_numbers';
    public static readonly PageLength: number = 10;

    public static readonly SrchFullOrderList: string = 'Search By: FulOrder ID / Order ID / Warehouse / Carrier / Carrier Service / FulOrder Status / Packing Batch ID / Label Batch ID / Pallet ID / No. Of Full items / On hold(T/F)';
    public static readonly SrchFullOrderForExitScan: string = 'Search By: FulOrder ID / Product Name / Main SKU /FulOrder Status';
    
    public static readonly FulOrderListColumns: Array<any> = [
        {
            className: 'sorting_disabled',
            data: null,
            defaultContent: '',
            orderable: false,
        },
        { data: 'fullOrderId' }, { data: 'orderId' }, { data: 'warehouse' },
        { data: 'carrier' }, { data: 'carrierservice' }, { data: 'fulorderstatus' },
        { data: 'pickingbatchID' }, { data: 'labelbatchID' }, { data: 'palletID' },
        { data: 'nooffulitems' }, { data: 'onhold' },
        {
            className: 'sorting_disabled',
            data: 'action',
            defaultContent: 'empty',
            orderable: false,
        }];
    public static readonly FulOrderListForExitScanColumns: Array<any> = [

        { data: 'fulOrderId' }, { data: 'productName' }, { data: 'sku' },   { data: 'fulOrderStatus' },
        {
            className: 'sorting_disabled',
            data: null,
            defaultContent: '',
            orderable: false,
        }];


    public static readonly SrchOrderListPlaceHolder: string = "Search By: Order ID / Channel / SKU / Quantity / Order Type / Order Number / Order Date / Ship By Date / Deliver By Date / Order Status";

    public static readonly orderListColums: Array<any> = [
        {
            className: 'sorting_disabled',
            data: null,
            defaultContent: '',
            orderable: false,
        },
        { data: 'orderId' }, { data: 'channel' }, { data: 'sku' }, { data: 'quantity' }, { data: 'ordeType' },
        { data: 'orderNumber' }, { data: 'orderDate' }, { data: 'shipByDate' }, { data: 'deliverByDate' }, { data: 'orderStatus' }];

    //Nav Urls for product
    public static readonly ProductAddUrl: string = '/products/add';
    public static readonly ProductEdttUrl: string = '/products/edit'
    public static readonly ProductCloneUrl: string = '/products/clone'
    public static readonly ProductviewUrl: string = '/products/view'
    public static readonly ProductListUrl: string = '/products/list';

    //Nav Urls for Order
    public static readonly OrderListUrl: string = '/orders/list';
    public static readonly OrderDetailsUrl: string = '/orders/orderDetails';

    //Nav Urls for FulOrder
    public static readonly FulOrderViewUrl: string = '/fulorders/view'
    public static readonly FulOrderListUrl: string = '/fulorders/list';

    public static readonly ProductListColumns: Array<any> = [{ data: 'productName' }, { data: 'sku' }, { data: 'upc' }, { data: 'description' },
    { data: 'status' }, { data: 'noofVersions' }, { data: 'inventory' }, { data: 'category' }, { data: 'shipAlone' }, { data: 'manufacturer' }];
    public static readonly searchProductPlaceholder: string = 'Search By: Product Name / SKU / UPC / Description / Status / No of Versions / Inventory / Category / Ships Alone / Manufacturer';

    // API End Points
    //OrderAPI
    public static readonly OrderListAPI: string = 'order/list';
    public static readonly OrderLinesAPI: string = 'order/orderlines';
    public static readonly FulOrderAPI: string = 'order/fulfilledOrders';
    public static readonly OrderDetailsAPI: string = 'order/details';
    //Fulfillment APIs
    public static readonly FulOrderListAPI: string = 'fulfill/list';
    public static readonly AssignAPI: string = 'fulfill/AssignWarehouseCarrier';
    public static readonly CarrierServiceAPI: string = 'fulfill/GetCarrierService';
    public static readonly FulOrderViewAPI: string = 'fulfill/view';
    public static readonly CreatePickListAPI: string = 'fulfill/CreatePickListBatch';
    public static readonly CreateLabelAPI: string = 'fulfill/GenerateLabel';
    public static readonly CreatePackageAPI: string = 'fulfill/CreatePackage';
    public static readonly GetPlalletListAPI: string = 'fulfill/GetPalletList';
    public static readonly CreatePlalletAPI: string = 'fulfill/CreatePallet';
    public static readonly GetCarrierServiceAndTypesAPI: string = 'fulfill/GetCarrierAndServiceTypes';
    public static readonly PalletizeAPI: string = 'fulfill/Pallatize';
    public static readonly FulOrderListByPalletAPI: string = 'fulfill/GetFulOrderListByPallet';
    public static readonly ExitScanAPI: string = 'fulfill/ExitScan';
    public static readonly GetCarrierServiceInfoByPalletAPI: string = 'fulfill/GetCarrierServiceInfoByPallet';

    //Product APIs
    public static readonly ProductListAPI: string = 'product/list';
    public static readonly TypesAPI: string = 'product/types';
    public static readonly ProducSaveUpdateAPI: string = 'product/save';
    public static readonly ProductDetailsAPI: string = 'product/details';
    public static readonly ProductViewAPI: string = 'product/view';
    public static readonly ProductSKUUPCValidateAPI: string = 'product/validate';

}
export enum ModeEnum {
    Regular = 'Regular',
    Picking = 'Picking',
    Labelling = 'Labelling',
    Assigning = 'Assigning',
    Palletizing = 'Palletizing',
    ExitScan = 'Exit Scan'
}
