using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Milliards.DTO;
using Milliards.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Milliards.Services
{
    public class FulfillService : IFulfillService
    {
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        private ILogService LogServiceObj;
        private IMailService MailServiceObj;
        public FulfillService(AppDbContext context, IConfiguration IConfiguration)
        {
            this.context = context;
            _iconfiguration = IConfiguration;
            MailServiceObj = new MailService(_iconfiguration);
            LogServiceObj = new LogService(_iconfiguration);
        }
        public object GetCarrierService(int CarrierId)
        {
            try
            {
                List<FulfillCarrierService> CarrierServiceList = (from cs in context.CarrierService
                                                                  where cs.CarrierId == CarrierId
                                                                  select new FulfillCarrierService { carrierServiceId = cs.CarrierServiceId, name = cs.Name })
                                          .OrderBy(cs => cs.name).ToList();
                return new { carrierService = CarrierServiceList };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { carrierService = new List<FulfillCarrierService>() };
            }
        }

        public object AssignWarehouseCarrier(int FulOrderId, int WarehouseId, int CarrierId, int CarrierServiceId)
        {
            try
            {
                int FulStatusId = (from fs in context.FulOrderStatus where fs.Name.ToUpper() == _iconfiguration["ASSIGNED"] select fs.FulOrderStatusId).FirstOrDefault();
                bool FulOrderAutoHold_FLG = (from cs in context.CarrierService where cs.CarrierServiceId == CarrierServiceId select cs.FulOrderAutoHold_FLG).FirstOrDefault();
                int OnHoldReasonId = (from r in context.OnHoldReason where r.Name.ToUpper() == _iconfiguration["EXPENSIVE_SERVICE_WAS_ASSIGNED"] select r.OnHoldReasonId).FirstOrDefault();
                FulOrder FulOrderObj = (from fo in context.FulOrder
                                        where fo.FulOrderId == FulOrderId
                                        select fo).FirstOrDefault();
                if (FulOrderObj != null)
                {
                    FulOrderObj.AssignedWarehouseId = WarehouseId;
                    FulOrderObj.AssignedCarrierId = CarrierId;
                    FulOrderObj.AssignedCarrierServiceId = CarrierServiceId;
                    FulOrderObj.FulOrderStatusId = FulStatusId;
                    FulOrderObj.Assignment_DT = DateTime.Now;
                    if (FulOrderAutoHold_FLG)
                    {
                        FulOrderObj.OnHold_FLG = true;
                        FulOrderObj.OnHoldReasonId = OnHoldReasonId;
                    }
                    context.FulOrder.Update(FulOrderObj);
                    context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return false;
            }
        }
        public Object GetfullFilList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string mode, int warehouseId)
        {
            try
            {
                List<FulfillDTO> fulfillList = new List<FulfillDTO>();
                SqlParameter pagenumber = new SqlParameter(_iconfiguration["PAGENO"], pageno);
                SqlParameter pagesize = new SqlParameter(_iconfiguration["PAGESIZE"], recordsize);
                SqlParameter sortcolumn = new SqlParameter(_iconfiguration["SORTCOLUMN"], columnsort == null ? _iconfiguration["fullOrderId"] : columnsort);
                SqlParameter sortorder = new SqlParameter(_iconfiguration["SORTORDER"], ordersort == null ? _iconfiguration["DESC"] : ordersort.ToUpper());
                SqlParameter searchvalue = new SqlParameter(_iconfiguration["SEARCHVALUE"], searchValue == null ? "" : searchValue);
                SqlParameter modevalue = new SqlParameter(_iconfiguration["MODE"], mode == null ? "" : mode);
                SqlParameter warehouseIdvalue = new SqlParameter(_iconfiguration["WAREHOUSEIDPARAM"], warehouseId);
                fulfillList = context.fulfillList.FromSqlRaw("EXEC [dbo].[usp_getFullFillCountList] @PageNo,@PageSize,@SortColumn,@SortOrder,@SearchValue,@Mode,@WarehouseId", pagenumber, pagesize, sortcolumn, sortorder, searchvalue, modevalue, warehouseIdvalue).ToList();
                var RecordCount = fulfillList.Select(s => s.TotalRecs).FirstOrDefault();
                return new { TotalRecordsCount = RecordCount, Data = fulfillList };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { TotalRecordsCount = 0, Data = new List<FulfillDTO>() };
            }
        }

        public object GetFullOrder(int FulOrderId)
        {
            FulfillOrderViewDTO FulOrderObj = new FulfillOrderViewDTO();
            FulOrderObj = (from fo in context.FulOrder
                           join c in context.CancelReason on fo.CancelReasonId equals c.CancelReasonId into crjoin
                           from cjoin in crjoin.DefaultIfEmpty()
                           join awh in context.Warehouse on fo.AssignedWarehouseId equals awh.WarehouseId into awhjoined
                           from awhj in awhjoined.DefaultIfEmpty()
                           join ac in context.Carrier on fo.AssignedCarrierId equals ac.CarrierId into acjoined
                           from acj in acjoined.DefaultIfEmpty()
                           join b in context.Box on fo.BoxId equals b.BoxId into bjoin
                           from bj in bjoin.DefaultIfEmpty()
                           join acs in context.CarrierService on fo.AssignedCarrierServiceId equals acs.CarrierServiceId into acsjoined
                           from acsj in acsjoined.DefaultIfEmpty()
                           join p in context.Pallet on fo.PalletId equals p.PalletId into pjoin
                           from pj in pjoin.DefaultIfEmpty()
                           join e in context.ErrorReason on fo.ErrorReasonId equals e.ErrorReasonId into ejoin
                           from ej in ejoin.DefaultIfEmpty()
                           join ohr in context.OnHoldReason on fo.OnHoldReasonId equals ohr.OnHoldReasonId into ohrjoined
                           from ohrj in ohrjoined.DefaultIfEmpty()
                           join fos in context.FulOrderStatus on fo.FulOrderStatusId equals fos.FulOrderStatusId into fosjoined
                           from fosj in fosjoined.DefaultIfEmpty()
                           where fo.FulOrderId == FulOrderId
                           select new FulfillOrderViewDTO
                           {
                               assignedCarrier = (acj != null ? acj.Name : ""),
                               assignedCarrierService = (acsj != null ? acsj.Name : ""),
                               assignedWareHouse = (awhj != null ? awhj.Name : ""),
                               assignmentDate = fo.Assignment_DT,
                               box = (bj != null ? bj.Name : ""),
                               cancelReason = (cjoin != null ? cjoin.Name : ""),
                               carrierDescription = fo.CarrierDescription,
                               carrierUpdateDate = fo.CarrierUpdateDT,
                               errorFlag = (fo.Error_FLG != null ? fo.Error_FLG.ToString() : "No"),
                               errorReason = (ej != null ? ej.Name : ""),
                               fullItemDetails = new List<FulItemViewDTO>(),
                               fulOrderId = fo.FulOrderId,
                               fulOrderStatus = (fosj != null ? fosj.Name : ""),
                               isPrime = (fo.IsPrime_FLG != null ? fo.IsPrime_FLG.ToString() : "No"),
                               labelBatchId = fo.LabelBatchId,
                               onHoldFlag = (fo.OnHold_FLG != null ? fo.OnHold_FLG.ToString() : "No"),
                               onHoldReason = (ohrj != null ? ohrj.Name : ""),
                               orderId = fo.OrderId,
                               originalFulOrderId = fo.OriginalFulOrderId,
                               palletName = (pj != null ? pj.Name : ""),
                               pickingBatchId = fo.PickingBatchId,
                               shipByDate = fo.ShipByDT
                           }).FirstOrDefault();
            if (FulOrderObj != null)
                FulOrderObj.fullItemDetails = (from fi in context.FulItem
                                               join psku in context.ProductSKU on new { p1 = 1, p2 = fi.ProductId } equals new { p1 = psku.SKUTypeId, p2 = psku.ProductId } into pskujoined
                                               from pskuj in pskujoined.DefaultIfEmpty()
                                               join pv in context.ProductVersion on pskuj.ProductId equals pv.ProductId into pvjoined
                                               from pvj in pvjoined.DefaultIfEmpty()
                                               where fi.FulOrderId == FulOrderId
                                               select new FulItemViewDTO
                                               {
                                                   fulItemId = fi.FulItemId,
                                                   orderLineId = fi.OrderLineId,
                                                   productId = fi.ProductId,
                                                   productVersion = (pvj != null) ? pvj.Version : 0,
                                                   quantity = fi.Quantity,
                                                   sku = (pskuj != null) ? pskuj.SKU : ""
                                               }).OrderBy(x => x.fulItemId).ToList();

            return FulOrderObj;
        }

        public object CreatePickListBatch(List<int> fulOrderList)
        {
            try
            {
                int FulOrderStatusId = (from fs in context.FulOrderStatus where fs.Name.ToUpper() == _iconfiguration["PICKING"] select fs.FulOrderStatusId).FirstOrDefault();
                int? MaxPickBatchId = (from p in context.FulOrder select p.PickingBatchId).Max(p => p);
                int CurrentPickBatchId = (MaxPickBatchId ?? 0) + 1;
                List<FulOrder> FulOrderList = (from fo in context.FulOrder
                                               where fulOrderList.Contains(fo.FulOrderId)
                                               select fo).ToList();

                foreach (FulOrder FoObj in FulOrderList)
                {
                    FoObj.PickingBatchId = CurrentPickBatchId;
                    FoObj.FulOrderStatusId = FulOrderStatusId;
                }
                context.FulOrder.UpdateRange(FulOrderList);

                context.SaveChanges();
                return new { status = true, message = string.Format(_iconfiguration["PICKLIST_BATCH_CREATION_SUCCESS"], CurrentPickBatchId.ToString()) };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["PICKLIST_BATCH_CREATION_FAIL"] };
            }
        }

        public object GenerateLabel(string UPC)
        {
            try
            {
                Product ProductObj = (from p in context.Product
                                      join ps in context.ProductSKU on p.ProductId equals ps.ProductId
                                      join pt in context.ProductSKUType on ps.SKUTypeId equals pt.ProductSKUTypeId
                                      where pt.Name.ToUpper() == _iconfiguration["SKU"]
                                      && p.UPC == UPC
                                      select p).FirstOrDefault();
                Guid? RandomTrackingNumberStr = Guid.Empty;

                if (ProductObj != null)
                {
                    bool GenerateShipment = false;
                    string ProductSKU = (from psku in context.ProductSKU
                                         join pskutype in context.ProductSKUType on psku.SKUTypeId equals pskutype.ProductSKUTypeId
                                         where psku.ProductId == ProductObj.ProductId && pskutype.Name.ToUpper() == _iconfiguration["SKU"]
                                         select psku.SKU).FirstOrDefault();
                    FulOrder fulOrderObj = (from fi in context.FulItem
                                            join fo in context.FulOrder on fi.FulOrderId equals fo.FulOrderId
                                            join fs in context.FulOrderStatus on fo.FulOrderStatusId equals fs.FulOrderStatusId
                                            where fs.Name.ToUpper() == _iconfiguration["LABELCREATED"] &&
                                                    (fo.OnHold_FLG ?? false) == false &&
                                                    fi.ProductId == ProductObj.ProductId
                                            select fo).OrderBy(fo => fo.OrderId).FirstOrDefault();

                    if (fulOrderObj != null)
                    {
                        GenerateShipment = true;
                        RandomTrackingNumberStr = (from s in context.Shipment where s.FulOrderId == fulOrderObj.FulOrderId select s.TrackingNumber).FirstOrDefault();
                    }
                    else
                        fulOrderObj = (from fi in context.FulItem
                                       join fo in context.FulOrder on fi.FulOrderId equals fo.FulOrderId
                                       join fs in context.FulOrderStatus on fo.FulOrderStatusId equals fs.FulOrderStatusId
                                       where fs.Name.ToUpper() == _iconfiguration["PICKING"] &&
                                               (fo.OnHold_FLG ?? false) == false &&
                                               (fo.Error_FLG ?? false) == false &&
                                               fi.ProductId == ProductObj.ProductId
                                       select fo).OrderBy(fo => fo.OrderId).FirstOrDefault();
                    if (fulOrderObj == null)
                        return new { status = false, message = _iconfiguration["UPC_PICKED_NOTFOUND"] };

                    FulOrderStatus fulOrderStatusObj = (from fs in context.FulOrderStatus where fs.Name.ToUpper() == _iconfiguration["LABELCREATED"] select fs).FirstOrDefault();
                    Box BoxObj = (from b in context.Box
                                  where b.BoxId == ProductObj.BoxId
                                  select b).FirstOrDefault();
                    if (!GenerateShipment && fulOrderObj != null && ProductObj != null)
                    {
                        fulOrderObj.FulOrderStatusId = fulOrderStatusObj.FulOrderStatusId;

                        RandomTrackingNumberStr = Guid.NewGuid();
                        /* Insertion of Shipment record */
                        Shipment ShipmentObj = new Shipment();
                        ShipmentObj.CarrierId = fulOrderObj.AssignedCarrierId ?? 0;
                        ShipmentObj.CarrierServiceId = fulOrderObj.AssignedCarrierServiceId ?? 0;
                        ShipmentObj.FulOrderId = fulOrderObj.FulOrderId;
                        ShipmentObj.Height = (BoxObj != null) ? BoxObj.Height : 0;
                        ShipmentObj.Length = (BoxObj != null) ? BoxObj.Length : 0;
                        ShipmentObj.StatusUpdatedDT = DateTime.Now;
                        ShipmentObj.Width = (BoxObj != null) ? BoxObj.Width : 0;
                        ShipmentObj.Weight = ProductObj.Weight;
                        ShipmentObj.TrackingNumber = RandomTrackingNumberStr;
                        ShipmentObj.ShippingLabel = null;
                        ShipmentObj.ShipmentStatusId = (from ss in context.ShipmentStatus
                                                        where ss.Name.ToUpper() == _iconfiguration["ACTIVE"]
                                                        select ss.ShipmentStatusId).FirstOrDefault();
                        context.Shipment.Add(ShipmentObj);
                        context.FulOrder.Update(fulOrderObj);
                        context.SaveChanges();
                    }
                    UpdateTrackingDTO TrackingDTOObj = new UpdateTrackingDTO();
                    TrackingDTOObj.fulOrderId = fulOrderObj.FulOrderId;
                    TrackingDTOObj.productName = ProductObj.Name;
                    TrackingDTOObj.fulOrderStatus = fulOrderStatusObj.Name;
                    TrackingDTOObj.sku = ProductSKU;
                    TrackingDTOObj.trackingNumber = (RandomTrackingNumberStr != null) ? RandomTrackingNumberStr.ToString() : "";
                    return new { status = true, data = TrackingDTOObj };
                }
                else
                {
                    return new { status = false, message = _iconfiguration["UPC_NOT_FOUND"] };
                }
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["UNHANDLEDEXCEPTION"] };
            }

        }

        public object CreatePackage(int fulOrderId)
        {
            try
            {
                UpdateTrackingDTO response = new UpdateTrackingDTO();
                FulOrder FulOrderObj = (from fo in context.FulOrder
                                        join fos in context.FulOrderStatus on fo.FulOrderStatusId equals fos.FulOrderStatusId
                                        where fo.FulOrderId == fulOrderId &&
                                        fos.Name.ToUpper() == _iconfiguration["LABELCREATED"]
                                        select fo).FirstOrDefault();
                if (FulOrderObj == null)
                    return new
                    {
                        status = false,
                        message = _iconfiguration["FULORDER_NOT_LABELED"]
                    };
                FulOrderStatus FulOrderStatus = (from fos in context.FulOrderStatus
                                                 where fos.Name.ToUpper() == _iconfiguration["PACKAGED"]
                                                 select fos).FirstOrDefault();

                Product productObj = (from fi in context.FulItem
                                      join p in context.Product on fi.ProductId equals p.ProductId
                                      where fi.FulOrderId == fulOrderId
                                      select p).FirstOrDefault();
                ProductSKU productSKU = (from p in context.Product
                                         join ps in context.ProductSKU on p.ProductId equals ps.ProductId
                                         join pt in context.ProductSKUType on ps.SKUTypeId equals pt.ProductSKUTypeId
                                         where p.ProductId == productObj.ProductId && pt.Name.ToUpper() == _iconfiguration["SKU"]
                                         select ps).FirstOrDefault();
                Shipment shipmentObj = (from s in context.Shipment
                                        where s.FulOrderId == fulOrderId
                                        select s).FirstOrDefault();

                FulOrderObj.FulOrderStatusId = FulOrderStatus.FulOrderStatusId;
                context.FulOrder.Update(FulOrderObj);
                context.SaveChanges();

                if (productObj != null && productSKU != null && shipmentObj != null)
                {
                    response.fulOrderId = fulOrderId;
                    response.fulOrderStatus = FulOrderStatus.Name;
                    response.productName = productObj.Name;
                    response.sku = productSKU.SKU;
                    response.trackingNumber = shipmentObj.TrackingNumber.ToString();
                }
                else if (productObj == null)
                {
                    return new { status = false, message = _iconfiguration["PRODUCTNOTFOUND"] };
                }
                else if (productSKU == null)
                {
                    return new { status = false, message = _iconfiguration["PRODUCTSKUNOTFOUND"] };
                }
                else if (shipmentObj == null)
                {
                    return new { status = false, message = _iconfiguration["SHIPMENTNOTFOUND"] };
                }
                return new { status = true, message = string.Format(_iconfiguration["PACKAGE_SUCCESS"], fulOrderId.ToString()), data = response };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["ERROR_PACKAGE"] };
            }
        }

        public object GetPalletIds()
        {
            try
            {
                List<PalletDTO> PalletList = new List<PalletDTO>();
                PalletList = (from p in context.Pallet
                              join ps in context.PalletStatus on p.PalletStatusId equals ps.PalletStatusId
                              where ps.Name.ToUpper() == _iconfiguration["OPEN"]
                              select new PalletDTO { palletId = p.PalletId, name = p.Name })
                              .OrderBy(x => x.palletId).ToList();
                return new { status = true, data = PalletList };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["ERROR_GETPALLETIDS"] };
            }
        }

        public object GetCarrierServiceInfoByPallet(int PalletId)
        {
            try
            {
                CarrierServiceDTO CarrierServiceList = new CarrierServiceDTO();
                CarrierServiceList = (from p in context.Pallet
                                      join c in context.Carrier on p.CarrierId equals c.CarrierId
                                      join cs in context.CarrierService on c.CarrierId equals cs.CarrierId
                                      join cst in context.CarrierServiceType on new { p1 = cs.CarrierServiceTypeId, p2 = p.CarrierServiceTypeId ?? 0 } equals new { p1 = cst.CarrierServiceTypeId, p2 = cst.CarrierServiceTypeId }
                                      where p.PalletId == PalletId
                                      select new CarrierServiceDTO
                                      {
                                          carrierId = c.CarrierId,
                                          carrierName = c.Name + "-" + cst.Name,
                                          carrierServiceTypeId = cst.CarrierServiceTypeId
                                      })
                                      .OrderBy(x => x.carrierName).FirstOrDefault();
                return new { status = true, data = CarrierServiceList };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["ERROR_GETCARRIERSERVICEINFO"] };
            }
        }
        public object GetCarrierServiceInfo()
        {
            try
            {
                List<CarrierServiceDTO> CarrierServiceList = new List<CarrierServiceDTO>();
                CarrierServiceList = (from c in context.Carrier
                                      join cs in context.CarrierService on c.CarrierId equals cs.CarrierId
                                      join cst in context.CarrierServiceType on cs.CarrierServiceTypeId equals cst.CarrierServiceTypeId
                                      select new CarrierServiceDTO
                                      {
                                          carrierId = c.CarrierId,
                                          carrierName = c.Name + "-" + cst.Name,
                                          carrierServiceTypeId = cst.CarrierServiceTypeId
                                      })
                                      .OrderBy(x => x.carrierName).ToList();
                return new { status = true, data = CarrierServiceList };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["ERROR_GETCARRIERSERVICEINFO"] };
            }
        }
        public object CreatePallet(int carrierId, int carrierServiceTypeId)
        {
            try
            {
                Pallet PalletObj = new Pallet();
                PalletObj.CarrierId = carrierId;
                PalletObj.CarrierServiceTypeId = carrierServiceTypeId;
                PalletObj.PickedUpDT = DateTime.Now;
                PalletObj.PalletStatusId = (from ps in context.PalletStatus
                                            where ps.Name.ToUpper() == _iconfiguration["OPEN"]
                                            select ps.PalletStatusId).FirstOrDefault();
                context.Pallet.Add(PalletObj);
                context.SaveChanges();
                /* Updating PalletId as Pallet Name as per the specifications.*/
                PalletObj.Name = PalletObj.PalletId.ToString();
                context.Pallet.Update(PalletObj);
                context.SaveChanges();

                return new { status = true, message = string.Format(_iconfiguration["PALLET_CREATED_SUCCESS"], PalletObj.Name), palletId = PalletObj.PalletId };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new
                {
                    status = false,
                    message = _iconfiguration["ERROR_CREATEPALLET"]
                };
            }
        }

        public Object PalletizeFulOrder(string trackingNumber, int palletId)
        {
            try
            {
                Guid TrackingNumber;
                if (!Guid.TryParse(trackingNumber, out TrackingNumber))
                {
                    return new { status = false, message = _iconfiguration["INVALID_TRACKINGFORMAT"] };
                }
                /*get FulOrder information based on given tracking number and palletId*/
                Shipment shipmentObj = new Shipment();
                shipmentObj = (from s in context.Shipment
                               where s.TrackingNumber == TrackingNumber
                               select s).FirstOrDefault();
                if (shipmentObj == null)
                {
                    return new { status = false, message = _iconfiguration["TRACKINGNO_NOTFOUND"] };
                }

                Pallet PalletObj = (from p in context.Pallet
                                    where p.PalletId == palletId
                                    select p).FirstOrDefault();
                if (PalletObj == null)
                    return new { status = false, message = _iconfiguration["PALLETID_NOTFOUND"] };

                int? CarrierServiceTypeId = (from cs in context.CarrierService
                                             where cs.CarrierServiceId == shipmentObj.CarrierServiceId && cs.CarrierId == shipmentObj.CarrierId
                                             select cs.CarrierServiceTypeId).FirstOrDefault();

                if (PalletObj.CarrierId != shipmentObj.CarrierId || PalletObj.CarrierServiceTypeId != CarrierServiceTypeId)
                    return new { status = false, message = _iconfiguration["PALLET_SHIPMENT_CARRIER_NOT_MATCH"] };

                if (shipmentObj.CarrierId == PalletObj.CarrierId && CarrierServiceTypeId != null && CarrierServiceTypeId == PalletObj.CarrierServiceTypeId)
                {
                    FulOrder FulOrderObj = new FulOrder();
                    FulOrderObj = (from fo in context.FulOrder
                                   join fos in context.FulOrderStatus on fo.FulOrderStatusId equals fos.FulOrderStatusId
                                   where fo.FulOrderId == shipmentObj.FulOrderId && fos.Name == _iconfiguration["PACKAGED"]
                                   select fo).FirstOrDefault();

                    if (FulOrderObj == null)
                        return new { status = false, message = _iconfiguration["FULORDER_NOT_IN_PACKAGE"] };

                    FulOrderStatus FulOrderStatusObj = (from fos in context.FulOrderStatus
                                                        where fos.Name.ToUpper() == _iconfiguration["PALLETIZED"]
                                                        select fos).FirstOrDefault();
                    FulOrderObj.FulOrderStatusId = FulOrderStatusObj.FulOrderStatusId;
                    FulOrderObj.PalletId = palletId;
                    context.FulOrder.Update(FulOrderObj);
                    context.SaveChanges();

                    Product ProductObj = new Product();
                    ProductObj = (from fi in context.FulItem
                                  join p in context.Product on fi.ProductId equals p.ProductId
                                  select p).FirstOrDefault();
                    ProductSKU productSKUObj = (from ps in context.ProductSKU
                                                where ps.ProductId == ProductObj.ProductId
                                                select ps).FirstOrDefault();

                    UpdateTrackingDTO response = new UpdateTrackingDTO();
                    response.fulOrderId = FulOrderObj.FulOrderId;
                    response.fulOrderStatus = FulOrderStatusObj.Name;
                    response.productName = ProductObj.Name;
                    response.sku = productSKUObj.SKU;
                    response.trackingNumber = shipmentObj.TrackingNumber.ToString();
                    return new { status = true, data = response, message = string.Format(_iconfiguration["PALLETIZED_SUCCESS"], FulOrderObj.FulOrderId.ToString()) };
                }
                return new { status = false, message = _iconfiguration["ERROR_NOFULORDERPACKAGEFOUND"] };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new
                {
                    status = false,
                    message = _iconfiguration["ERROR_CREATEPALLET"]
                };
            }
        }

        public object GetFulOrderListByPallet(int PalletId, string SortCol, string SortOrder, string SearchValue)
        {
            List<UpdateTrackingDTO> response = new List<UpdateTrackingDTO>();

            SqlParameter PalletIdValue = new SqlParameter(_iconfiguration["PALLETID"], PalletId);
            SqlParameter SortColValue = new SqlParameter(_iconfiguration["SORTCOLUMN"], SortCol ?? "FullOrderId");
            SqlParameter SortOrderValue = new SqlParameter(_iconfiguration["SORTORDER"], SortOrder ?? "ASC");
            SqlParameter searchValue = new SqlParameter(_iconfiguration["SEARCHVALUE"], SearchValue ?? "");

            response = context.UpdateTrackingDTO.FromSqlRaw("EXEC [dbo].[usp_getFullFillCount_ExitScan] @PalletId,@SortColumn,@SortOrder,@SearchValue", PalletIdValue, SortColValue, SortOrderValue, searchValue).ToList();
            if (response.Count == 0)
            {
                return new { status = true, data = new List<UpdateTrackingDTO>(), message = _iconfiguration["NO_FO_ON_PALLET"] };
            }
            else
                return new { status = true, data = response };
        }

        public object ExitScan(List<int> FulOrderIdList, int PalletId)
        {
            try
            {
                List<FulOrder> FulOrderList = new List<FulOrder>();
                FulOrderList = (from fo in context.FulOrder
                                where FulOrderIdList.Contains(fo.FulOrderId) && fo.PalletId == PalletId
                                select fo).ToList();
                if (FulOrderList.Count == 0)
                    return new { status = false, message = _iconfiguration["NO_FO_ON_PALLET"] };
                /* Considerting all the FO are in Palletized status and updateing all at a time */
                FulOrderStatus FulOrderStatusObj = (from fos in context.FulOrderStatus
                                                    where fos.Name.ToUpper() == _iconfiguration["EXITSCAN"]
                                                    select fos).FirstOrDefault();
                Pallet PalletObj = (from p in context.Pallet
                                    where p.PalletId == PalletId
                                    select p).FirstOrDefault();
                if (PalletObj == null)
                    return new { status = false, message = _iconfiguration["PALLETID_NOTFOUND"] };
                PalletStatus palletStatusObj = (from ps in context.PalletStatus
                                                where ps.Name.ToUpper() == _iconfiguration["CLOSED"]
                                                select ps).FirstOrDefault();
                foreach (FulOrder FulOrderObj in FulOrderList)
                    FulOrderObj.FulOrderStatusId = FulOrderStatusObj.FulOrderStatusId;
                PalletObj.PalletStatusId = palletStatusObj.PalletStatusId;
                context.Pallet.Update(PalletObj);
                context.FulOrder.UpdateRange(FulOrderList);
                context.SaveChanges();
                return new { status = true, message = _iconfiguration["EXISSCAN_SUCCESS"] };
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return new { status = false, message = _iconfiguration["ERROR_EXITSCAN"] };
            }
        }
    }
}
