using Milliards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    public interface IFulfillService
    {
        public object GetCarrierService(int carrierId);
        public object AssignWarehouseCarrier(int FulOrderId, int WarehouseId, int CarrierId, int CarrierServiceId);
        public object GetfullFilList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string mode, int warehouseId);
        public object GetFullOrder(int FulOrderId);
        public Object EditFulorder(FulfillOrderViewDTO FulOrder);
        public object CreatePickListBatch(List<int> fulOrderList);
        public object GenerateLabel(string UPC);
        public object CreatePackage(int fulOrderId);
        public object GetPalletIds();
        public object GetCarrierServiceInfo();
        public object CreatePallet(int carrierId, int carrierTypeId);
        public object PalletizeFulOrder(string trackingNumber, int palletId);
        public object GetFulOrderListByPallet(int PalletId, string SortCol, string SortOrder, string SearchValue);
        public object ExitScan(List<int> FulOrderIdList, int PalletId);
        public object GetCarrierServiceInfoByPallet(int PalletId);
    }
}
