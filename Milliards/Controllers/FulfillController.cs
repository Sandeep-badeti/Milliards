using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Milliards.DTO;
using Milliards.Services;

namespace Milliards.Controllers
{
    [ApiController]
    [TypeFilter(typeof(Interceptor))]
    [Route("api/[controller]/[action]")]
    public class FulfillController : ControllerBase
    {
        private readonly IFulfillService fulfillService;
        public FulfillController(IFulfillService fulfillService)
        {
            this.fulfillService = fulfillService;
        }

        [HttpGet]
        [ActionName("GetCarrierService")]
        public ActionResult<Object> GetCarrierService(int carrierId)
        {
            var result = fulfillService.GetCarrierService(carrierId);
            return Ok(result);
        }
        [HttpPut]
        [ActionName("AssignWarehouseCarrier")]
        public ActionResult<Object> AssignWarehouseCarrier(AssignFulWarehouse AssignWarehouse)
        {
            if (AssignWarehouse != null)
            {
                int fulOrderId = AssignWarehouse.fulOrderId;
                int warehouseId = AssignWarehouse.warehouseId;
                int carrierId = AssignWarehouse.carrierId;
                int carrierServiceId = AssignWarehouse.carrierServiceId;
                var result = fulfillService.AssignWarehouseCarrier(fulOrderId, warehouseId, carrierId, carrierServiceId);
                return Ok(result);
            }
            else
                return Ok(false);
        }
        [HttpGet]
        [ActionName("list")]
        public ActionResult<Object> GetfullFilList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string mode, int warehouseId)
        {
            var result = fulfillService.GetfullFilList(pageno, recordsize, columnsort, ordersort, searchValue, mode, warehouseId);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("view")]
        public ActionResult<Object> GetFulOrder(int fulOrderId)
        {
            var result = fulfillService.GetFullOrder(fulOrderId);
            return Ok(result);
        }
        [HttpPost]
        [ActionName("edit")]
        public ActionResult<Object> EditFulorder(FulfillOrderViewDTO FulOrder)
        {
            var result = fulfillService.EditFulorder(FulOrder);
            return Ok(result);
        }

        [HttpPost]
        [ActionName("CreatePickListBatch")]
        public ActionResult<Object> CreatePickListBatch(List<int> fulOrderList)
        {
            var result = fulfillService.CreatePickListBatch(fulOrderList);
            return result;
        }

        [HttpPost]
        [ActionName("GenerateLabel")]
        public ActionResult<Object> GenerateLabel(ProductDTO product)
        {
            var result = fulfillService.GenerateLabel(product.UPC);
            return result;
        }
        [HttpPut]
        [ActionName("CreatePackage")]
        public ActionResult<Object> CreatePackage(FulfillDTO fullOrder)
        {
            var result = fulfillService.CreatePackage(fullOrder.fullOrderId);
            return result;
        }
        [HttpGet]
        [ActionName("GetPalletList")]
        public ActionResult<Object> GetPalletList()
        {
            var result = fulfillService.GetPalletIds();
            return result;
        }
        [HttpGet]
        [ActionName("GetCarrierAndServiceTypes")]
        public ActionResult<Object> GetCarrierAndServiceTypes()
        {
            var result = fulfillService.GetCarrierServiceInfo();
            return result;
        }
        [HttpPost]
        [ActionName("CreatePallet")]
        public ActionResult<Object> CreatePallet(CreatePalletDTO PalletObj)
        {
            var result = fulfillService.CreatePallet(PalletObj.carrierId, PalletObj.carrierServiceTypeId);
            return result;
        }
        [HttpPost]
        [ActionName("Pallatize")]
        public ActionResult<Object> Pallatize(PalletizeDTO palletObj)
        {
            var result = fulfillService.PalletizeFulOrder(palletObj.trackingNumber, palletObj.palletId);
            return result;
        }
        [HttpGet]
        [ActionName("GetFulOrderListByPallet")]
        public ActionResult<Object> GetFulOrderListByPallet(int palletId, string columnsort, string ordersort, string searchValue)
        {
            var result = fulfillService.GetFulOrderListByPallet(palletId, columnsort, ordersort, searchValue);
            return result;
        }
        [HttpPost]
        [ActionName("ExitScan")]
        public ActionResult<Object> ExitScan(ExitScanRequestDTO fulOrderlist)
        {
            var result = fulfillService.ExitScan(fulOrderlist.fulOrderIdList, fulOrderlist.palletId);
            return result;
        }
        [HttpGet]
        [ActionName("GetCarrierServiceInfoByPallet")]
        public ActionResult<Object> GetCarrierServiceInfoByPallet(int palletId) {
            var result = fulfillService.GetCarrierServiceInfoByPallet(palletId);
            return result;
        }
    }
}