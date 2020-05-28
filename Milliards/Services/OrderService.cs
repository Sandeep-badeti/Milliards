using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Milliards.DTO;
using Milliards.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Milliards.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        private ILogService LogServiceObj;
        private IMailService MailServiceObj;
        public OrderService(AppDbContext context, IConfiguration IConfiguration)
        {
            this.context = context;
            _iconfiguration = IConfiguration;
            MailServiceObj = new MailService(_iconfiguration);
            LogServiceObj = new LogService(_iconfiguration);
        }

        public Object GetOrdersList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string FromDate, string ToDate)
        {
            try
            {
                List<OrderDTO> ordersList = new List<OrderDTO>();
                SqlParameter pagenumber = new SqlParameter(_iconfiguration["PAGENO"], pageno);
                SqlParameter pagesize = new SqlParameter(_iconfiguration["PAGESIZE"], recordsize);
                SqlParameter sortcolumn = new SqlParameter(_iconfiguration["SORTCOLUMN"], columnsort == null ? _iconfiguration["ORDERID"] : columnsort);
                SqlParameter sortorder = new SqlParameter(_iconfiguration["SORTORDER"], ordersort == null ? _iconfiguration["DESC"] : ordersort.ToUpper());
                SqlParameter searchvalue = new SqlParameter(_iconfiguration["SEARCHVALUE"], searchValue == null ? "" : searchValue);
                SqlParameter fromdate = new SqlParameter(_iconfiguration["FROMDATE"], FromDate == null ? "" : FromDate);
                SqlParameter todate = new SqlParameter(_iconfiguration["TODATE"], ToDate == null ? "" : ToDate);
                ordersList = context.OrderList.FromSqlRaw("EXEC [dbo].[usp_getOrderCountList] @PageNo,@PageSize,@SortColumn,@SortOrder,@SearchValue,@FromDate,@ToDate", pagenumber, pagesize, sortcolumn, sortorder, searchvalue, fromdate, todate).ToList();
                var RecordCount = ordersList.Select(s => s.TotalRecs).FirstOrDefault();
                return new { TotalRecordsCount = RecordCount, Data = ordersList };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetOrderLineDetailsList(int orderId)
        {
            try
            {
                var result = (from ol in context.OrderLine.Where(s => s.OrderId == orderId)
                              join p in context.Product on ol.ProductId equals p.ProductId into p1
                              from p in p1.DefaultIfEmpty()
                              select new OrderLineDTO
                              {
                                  OrderLineId = ol.OrderLineId,
                                  ProductID = p.ProductId,
                                  ProductName = p.Name,
                                  SKU = ol.SKU,
                                  UPC = ol.UPC,
                                  ShipsAlone = p.ShipsAlone_FLG,
                                  UnitPrice = ol.UnitPrice,
                                  Quantity = ol.Quantity
                              }).OrderBy(s => s.OrderLineId).ToList();
                return new { Data = result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetfulfilledOrdersDetailList(int orderId)
        {
            try
            {
                var result = (from oline in context.OrderLine.Where(s => s.OrderId == orderId)
                              join fitem in context.FulItem on oline.OrderLineId equals fitem.OrderLineId into fitem1
                              from fitem in fitem1.DefaultIfEmpty()
                              join p in context.FulOrder on fitem.FulOrderId equals p.FulOrderId into p1
                              from p in p1.DefaultIfEmpty()
                              join fos in context.FulOrderStatus on p.FulOrderStatusId equals fos.FulOrderStatusId into fos1
                              from fos in fos1.DefaultIfEmpty()
                              select new FulfillmentOrderDTO
                              {
                                  FulOrderId = p.FulOrderId,
                                  Status = fos.Name,
                                  AssignedWarehouseId = p.AssignedWarehouseId,
                                  AssignedCarrierId = p.AssignedCarrierId,
                                  CarrierUpdateDate = p.CarrierUpdateDT,
                                  BoxId = p.BoxId,
                                  NoofFulfillmentItems = fitem.Quantity
                              }).OrderBy(s => s.FulOrderId).ToList();
                return new { Data = result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object GetOrdersDetailList(int orderId)
        {
            try
            {
                var result = (from or in context.Order.Where(s => s.OrderId == orderId)
                              join cn in context.Channel on or.ChannelId equals cn.ChannelId into cn1
                              from cn in cn1.DefaultIfEmpty()
                              join otype in context.OrderType on or.OrderTypeId equals otype.OrderTypeId into otype1
                              from otype in otype1.DefaultIfEmpty()
                              join stype in context.OrderStatus on or.OrderStatusId equals stype.OrderStatusId into stype1
                              from stype in stype1.DefaultIfEmpty()
                              join ceir in context.Carrier on or.BillToCarrierId equals ceir.CarrierId into ceir1
                              from ceir in ceir1.DefaultIfEmpty()
                              join k in context.Contact on or.BillToAddressId equals k.ContactId into k1
                              from k in k1.DefaultIfEmpty()
                              join fl in context.FulOrder on or.OrderId equals fl.OrderId into fl1
                              from fl in fl1.DefaultIfEmpty()
                              join w in context.Warehouse on fl.AssignedWarehouseId equals w.WarehouseId into w1
                              from w in w1.DefaultIfEmpty()
                              select new OrderDetailsDTO
                              {
                                  OrderId = or.OrderId,
                                  Channel = cn.Name,
                                  OrderType = otype.Name,
                                  OrderStatus = stype.Name,
                                  OrderNo = or.OrderNumber,
                                  Ref1 = or.Ref1,
                                  Ref2 = or.Ref2,
                                  Ref3 = or.Ref3,
                                  OrderDate = or.OrderDT,
                                  ShipByDate = or.ShipByDT,
                                  ReqShipService = or.RequiredShippingService,
                                  DeliverByDate = or.DeliverByDate,
                                  AmountPaid = or.AmountPaid,
                                  ShippingPaid = or.ShippingPaid,
                                  TaxPaid = or.TaxPaid,
                                  BilltoAddress = k.City,
                                  ShiptoAddress = k.City,
                                  BilltoAcctNo = or.BillToAccountNumber,
                                  InternalNotes = or.InternalNotes,
                                  BilltoCarrier = ceir.Name,
                                  CustomerNotes = or.CustomerNotes,
                                  BilltoAcctZip = or.BillToAccountZip,
                                  Warehouse = w.Name
                              }).OrderBy(s => s.OrderId).SingleOrDefault();

                return new { Data = result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Fulfillment Process
        public bool FulfillUnProcessedOrders()
        {
            try
            {
                CultureInfo culture = CultureInfo.CreateSpecificCulture(_iconfiguration["CULTURE"]);
                DateTimeStyles styles = DateTimeStyles.None;
                string[] DateFormats = { _iconfiguration["DATETIMEFORMAT_YYYY-MM-DD"], _iconfiguration["DATETIMEFORMAT_MM-DD-YYYY"], _iconfiguration["DATETIMEFORMAT_YYYY/MM/DD"], _iconfiguration["DATETIMEFORMAT_MM/DD/YYYY"] };

                bool response = true;
                XmlDocument OrderDoc = new XmlDocument();
                List<StagingOrders> UnProcessedOrders = GetUnProcessedOrders();
                List<Order> LocalOrderList = new List<Order>();
                List<OrderLine> LocalOrderLineList = new List<OrderLine>();
                int OrderStatusId = 0;
                OrderStatusId = context.OrderStatus.Where(StatusRec => StatusRec.Name.ToUpper() == _iconfiguration["UNSHIPPED"]).Select(StatusRec => StatusRec.OrderStatusId).FirstOrDefault();
                int ChannelId = context.Channel.Where(c => c.Name.ToUpper() == _iconfiguration["AMAZON"]).Select(c => c.ChannelId).FirstOrDefault();

                //Read data from XML and insert into Orders table.
                foreach (StagingOrders OrderObj in UnProcessedOrders)
                {
                    string OrderXML = OrderObj.JSON;
                    OrderDoc.LoadXml(OrderXML);
                    XmlNodeList ChildNodes = OrderDoc.ChildNodes;
                    foreach (XmlNode OrdersRec in ChildNodes)
                    {
                        //Get data from XML and insert in to Order table.
                        for (int i = 1, j = 0; j < OrdersRec.ChildNodes.Count; j++, i++)
                        {
                            XmlNode OrderRec = OrdersRec.ChildNodes[j];
                            XmlNamespaceManager mwsNameSpace = new XmlNamespaceManager(OrderDoc.NameTable);
                            mwsNameSpace.AddNamespace("mws", OrderRec.NamespaceURI);

                            //Excluding Orders which are in pending and canceled status
                            string OrderStatus = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:OrderStatus", mwsNameSpace).InnerText;
                            if (OrderStatus != null && (OrderStatus.ToUpper() == "PENDING" || OrderStatus.ToUpper() == "CANCELED" || OrderStatus.ToUpper() == "CANCELLED"))
                                continue;
                            string OrderNumber = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:AmazonOrderId", mwsNameSpace).InnerText;

                            if (OrderRec != null && LocalOrderList.Any(OrdRec => OrdRec.OrderNumber == OrderNumber))
                                continue;
                            else if (context.Order.Any(OrderRow => OrderRow.OrderNumber == OrderNumber))
                                continue;

                            Order localOrderObj = new Order();
                            localOrderObj.ChannelId = ChannelId;
                            localOrderObj.OrderNumber = OrderNumber;
                            localOrderObj.OrderStatusId = OrderStatusId;
                            DateTime OutDate = new DateTime();
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:PurchaseDate", mwsNameSpace) != null)
                            {
                                if (DateTime.TryParseExact(OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:PurchaseDate", mwsNameSpace).InnerText, DateFormats, culture, styles, out OutDate))
                                    localOrderObj.OrderDT = OutDate;
                            }
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:LatestShipDate", mwsNameSpace) != null)
                            {
                                if (DateTime.TryParseExact(OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:LatestShipDate", mwsNameSpace).InnerText, DateFormats, culture, styles, out OutDate))
                                    localOrderObj.ShipByDT = OutDate;
                            }
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:LatestDeliveryDate", mwsNameSpace) != null)
                            {
                                if (DateTime.TryParseExact(OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:LatestDeliveryDate", mwsNameSpace).InnerText, DateFormats, culture, styles, out OutDate))
                                    localOrderObj.DeliverByDate = OutDate;
                            }
                            localOrderObj.RequiredShippingService = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShipServiceLevel", mwsNameSpace) != null ? OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShipServiceLevel", mwsNameSpace).InnerText : "";
                            string OrderType = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:OrderType", mwsNameSpace) != null ? OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:OrderType", mwsNameSpace).InnerText : "UPC";
                            localOrderObj.OrderTypeId = context.OrderType.Where(OrdType => OrdType.Name == OrderType).Select(x => x.OrderTypeId).FirstOrDefault();


                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:IsPrime", mwsNameSpace) != null)
                            {
                                string IsPrime_FLG = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:IsPrime", mwsNameSpace).InnerText;
                                localOrderObj.IsPrime_FLG = (IsPrime_FLG.ToUpper() == "TRUE");
                            }
                            /*Get Shipping Address*/
                            string City = string.Empty;
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:City", mwsNameSpace) != null)
                            {
                                City = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:City", mwsNameSpace).InnerText.ToUpper();
                            }
                            string StateOrRegion = string.Empty;
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:StateOrRegion", mwsNameSpace) != null)
                            {
                                StateOrRegion = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:StateOrRegion", mwsNameSpace).InnerText.ToUpper();
                            }
                            string PostalCode = string.Empty;
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:PostalCode", mwsNameSpace) != null)
                            {
                                PostalCode = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:PostalCode", mwsNameSpace).InnerText.ToUpper();
                            }
                            string CountryCode = string.Empty;
                            if (OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:CountryCode", mwsNameSpace) != null)
                            {
                                CountryCode = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ShippingAddress/mws:CountryCode", mwsNameSpace).InnerText.ToUpper();
                            }
                            // Fetching Contact information, if it is not available then we are creating new record in Contact.
                            int ShippingId = context.Contact.Where(C => C.City.ToUpper() == City && C.State == StateOrRegion && C.ZIP == PostalCode).Select(C => C.ContactId).FirstOrDefault();

                            if (ShippingId == 0)
                            {
                                Contact AddContact = new Contact();
                                AddContact.City = City;
                                AddContact.State = StateOrRegion;
                                AddContact.ZIP = PostalCode;
                                AddContact.Line1 = _iconfiguration["LINE1"];
                                AddContact.Line2 = _iconfiguration["LINE2"];
                                AddContact.Name = _iconfiguration["NAME"];
                                AddContact.CountryId = 1;
                                AddContact.Verified_FLG = false;
                                context.Contact.Add(AddContact);
                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (Exception Ex)
                                {
                                    LogServiceObj.LogException(Ex);
                                    MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                                    response = false;
                                }
                                ShippingId = AddContact.ContactId;
                            }
                            localOrderObj.BillToAddressId = ShippingId;
                            localOrderObj.ShipToAddressId = ShippingId;
                            localOrderObj.IsFulfilled = false;

                            localOrderObj.AmountPaid = OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:OrderTotal/mws:Amount", mwsNameSpace) != null ? Decimal.Parse(OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:OrderTotal/mws:Amount", mwsNameSpace).InnerText) : 0;
                            localOrderObj.TaxPaid = 0;
                            if (OrderRec.SelectNodes("//mws:Order[" + i.ToString() + "]/mws:ListOrderItemsResponse/mws:ListOrderItemsResult/mws:OrderItems/mws:OrderItem/mws:ItemTax", mwsNameSpace) != null)
                            {
                                foreach (XmlNode NodeObj in OrderRec.SelectNodes("//mws:Order[" + i.ToString() + "]/mws:ListOrderItemsResponse/mws:ListOrderItemsResult/mws:OrderItems/mws:OrderItem/mws:ItemTax", mwsNameSpace))
                                {
                                    localOrderObj.TaxPaid += NodeObj != null ? decimal.Parse(NodeObj.LastChild.InnerText ?? "0") : 0;
                                }
                            }
                            if (OrderRec.SelectNodes("//mws:Order[" + i.ToString() + "]/mws:ListOrderItemsResponse/mws:ListOrderItemsResult/mws:OrderItems/mws:OrderItem/mws:PromotionDiscountTax", mwsNameSpace) != null)
                            {
                                foreach (XmlNode NodeObj in OrderRec.SelectNodes("//mws:Order[" + i.ToString() + "]/mws:ListOrderItemsResponse/mws:ListOrderItemsResult/mws:OrderItems/mws:OrderItem/mws:PromotionDiscountTax", mwsNameSpace))
                                {
                                    localOrderObj.TaxPaid -= NodeObj != null ? decimal.Parse(NodeObj.LastChild.InnerText ?? "0") : 0;
                                }
                            }
                            // Get Order line data;
                            LocalOrderList.Add(localOrderObj);
                            context.Order.Add(localOrderObj);
                            try
                            {
                                context.SaveChanges();
                            }
                            catch (Exception Ex)
                            {
                                LogServiceObj.LogException(Ex);
                                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                                response = false;
                            }
                            List<OrderLine> OrderLineList = new List<OrderLine>();
                            try
                            {
                                OrderLineList = CreateOrderLines(OrderRec.SelectSingleNode("//mws:Order[" + i.ToString() + "]/mws:ListOrderItemsResponse", mwsNameSpace), OrderDoc.NameTable, localOrderObj.OrderId, localOrderObj.OrderNumber);
                            }
                            catch (Exception ex)
                            {
                                this.LogServiceObj.LogException(ex);
                                this.MailServiceObj.SendMail(ex.Message ?? "", ex.InnerException != null ? ex.InnerException.Message : "", ex.StackTrace ?? "");
                                response = false;
                            }
                            if (OrderLineList.Count > 0)
                            {
                                localOrderObj.OrderLines = OrderLineList;
                                context.OrderLine.AddRange(localOrderObj.OrderLines);
                                LocalOrderLineList.AddRange(localOrderObj.OrderLines);
                                try
                                {
                                    context.SaveChanges();
                                    SaveFulfillOrders(localOrderObj.OrderNumber, OrderLineList);
                                }
                                catch (Exception Ex)
                                {
                                    LogServiceObj.LogException(Ex);
                                    MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                                    response = false;
                                }
                            }
                            else if (OrderLineList.Count == 0)
                            {
                                context.Order.Remove(localOrderObj);
                                try
                                {
                                    context.SaveChanges();
                                }
                                catch (Exception Ex)
                                {
                                    LogServiceObj.LogException(Ex);
                                    MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                                    response = false;
                                }
                            }
                        }
                    }
                    /*for testing purpose. to execute entire application for one time.*/
                    OrderObj.Status = true;
                    context.StagingOrders.Update(OrderObj);
                    context.SaveChanges();
                }
                return response;
            }
            catch (Exception Ex)
            {
                LogServiceObj.LogException(Ex);
                MailServiceObj.SendMail(Ex.Message ?? "", (Ex.InnerException != null) ? Ex.InnerException.Message : "", Ex.StackTrace ?? "");
                return false;
            }
        }
        private List<OrderLine> CreateOrderLines(XmlNode ListOrderItemsResponse, XmlNameTable TableName, int OrderId, string OrderNumber)
        {
            try
            {
                XmlDocument orderItemsResponse = new XmlDocument();
                orderItemsResponse.LoadXml(ListOrderItemsResponse.OuterXml);
                XmlNamespaceManager mwsOrderLineNameSpace = new XmlNamespaceManager(TableName);
                mwsOrderLineNameSpace.AddNamespace("mwsOrderList", ListOrderItemsResponse.NamespaceURI);
                List<OrderLine> OrderLineList = new List<OrderLine>();
                XmlNodeList OrderItemNodes = orderItemsResponse.SelectNodes("//mwsOrderList:ListOrderItemsResponse/mwsOrderList:ListOrderItemsResult/mwsOrderList:OrderItems", mwsOrderLineNameSpace);

                for (int OrderItemNodeIndex = 0; OrderItemNodeIndex < OrderItemNodes.Count; OrderItemNodeIndex++)
                {
                    int index = 0;
                    foreach (XmlNode OrderLineNode in OrderItemNodes[OrderItemNodeIndex].ChildNodes)
                    {
                        OrderLine OrderLineObj = null;
                        OrderLineObj = new OrderLine();
                        XmlDocument xmlDocument2 = new XmlDocument();
                        xmlDocument2.LoadXml(OrderLineNode.OuterXml);
                        XmlNamespaceManager OrderItemXMLNameSpace = new XmlNamespaceManager(xmlDocument2.NameTable);
                        OrderItemXMLNameSpace.AddNamespace("mwsOrderList", OrderLineNode.NamespaceURI);

                        OrderLineObj.UnitPrice = (OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:ItemPrice/mwsOrderList:Amount", OrderItemXMLNameSpace) != null && OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:ItemPrice/mwsOrderList:Amount", OrderItemXMLNameSpace).Count > 0 ? Decimal.Parse(OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:ItemPrice/mwsOrderList:Amount", OrderItemXMLNameSpace)[index].InnerText ?? "0") : 0) - (OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:PromotionDiscount/mwsOrderList:Amount", OrderItemXMLNameSpace) != null && OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:PromotionDiscount/mwsOrderList:Amount", OrderItemXMLNameSpace).Count > 0 ? Decimal.Parse(OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:PromotionDiscount/mwsOrderList:Amount", OrderItemXMLNameSpace)[index].InnerText) : 0);
                        OrderLineObj.Name = OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:Title", OrderItemXMLNameSpace) != null ? OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:Title", OrderItemXMLNameSpace)[index].InnerText : "";
                        OrderLineObj.OrderId = OrderId;
                        OrderLineObj.SKU = OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:SellerSKU", OrderItemXMLNameSpace) != null ? OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:SellerSKU", OrderItemXMLNameSpace)[index].InnerText : "";
                        OrderLineObj.Quantity = OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:QuantityOrdered", OrderItemXMLNameSpace) != null || OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:QuantityOrdered", OrderItemXMLNameSpace).Count > 0 ? int.Parse(OrderLineNode.SelectNodes("//mwsOrderList:OrderItem/mwsOrderList:QuantityOrdered", OrderItemXMLNameSpace)[index].InnerText) : 0;

                        Product productObj = (from ps in context.ProductSKU
                                              join p in context.Product on ps.ProductId equals p.ProductId
                                              where ps.SKU == OrderLineObj.SKU
                                              select p).FirstOrDefault();
                        if (productObj == null)
                        {
                            /* When SKU is not found in product list, send a mail and delete the Order*/
                            string Msg = string.Format(_iconfiguration["ProductNotFoundMail"], OrderLineObj.SKU ?? "", OrderNumber ?? "");
                            MailServiceObj.SendMail(Msg);
                            return new List<OrderLine>();
                        }

                        OrderLineObj.ProductId = productObj.ProductId;
                        OrderLineObj.UPC = productObj.UPC ?? "";

                        // if Product exists then Add Orderline
                        if (OrderLineObj.ProductId > 0 && OrderId > 0)
                            OrderLineList.Add(OrderLineObj);
                        index++;
                    }
                }
                return OrderLineList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private bool SaveFulfillOrders(string OrderNumber, List<OrderLine> OrderLineList)
        {
            try
            {
                /*Pending Part: Need to check whether inventory is available or not*/
                List<FulOrder> FulOrderList = new List<FulOrder>();
                List<FulItem> FulItemList = new List<FulItem>();
                List<ProductInventory> ProductInvListUpdate = new List<ProductInventory>();
                List<Order> OrderListUpdate = new List<Order>();
                Order OrderObj = (from o in context.Order where o.OrderNumber == OrderNumber select o).FirstOrDefault();
                bool isPrime = OrderObj.IsPrime_FLG;
                string CarrierDescription = OrderObj.CarrierDescription;
                DateTime ShipByDT = OrderObj.ShipByDT;
                int FulOrderStatusId = context.FulOrderStatus.Where(StatusRec => StatusRec.Name.ToUpper() == _iconfiguration["ENTERED"]).Select(StatusRec => StatusRec.FulOrderStatusId).FirstOrDefault();

                foreach (OrderLine OrdrLineObj in OrderLineList)
                {
                    ProductInventory ProductInvObj = context.Product.Join(context.ProductVersion, p => p.ProductId, PI => PI.ProductId, (p, PI) => PI)
                        .Join(context.ProductInventory, pi => pi.ProductVersionId, pv => pv.ProductVersionId, (pi, pv) => new { pi, pv })
                        .Where(pv => pv.pi.ProductId == OrdrLineObj.ProductId).Select(x => x.pv).FirstOrDefault();
                    if (ProductInvObj != null && ProductInvObj.Quantity >= OrdrLineObj.Quantity)
                    {
                        Product productObj = context.Product.Where(P => P.ProductId == OrdrLineObj.ProductId).Select(p => p).FirstOrDefault();
                        if (productObj != null && productObj.ShipsAlone_FLG)
                        {
                            int iteration = 0;
                            while (iteration < OrdrLineObj.Quantity)
                            {
                                iteration++;
                                FulOrder FulOrderObj = new FulOrder();
                                FulOrderObj.OrderId = OrdrLineObj.OrderId;
                                FulOrderObj.FulOrderStatusId = FulOrderStatusId;
                                FulOrderObj.OnHold_FLG = false;
                                FulOrderObj.Error_FLG = false;
                                FulOrderObj.IsPrime_FLG = isPrime;
                                FulOrderObj.CarrierDescription = CarrierDescription;
                                FulOrderObj.ShipByDT = ShipByDT;
                                FulOrderList.Add(FulOrderObj);
                            }
                        }
                        else if (productObj != null && !productObj.ShipsAlone_FLG)
                        {
                            FulOrder FulOrderObj = new FulOrder();
                            FulOrderObj.OrderId = OrdrLineObj.OrderId;
                            FulOrderObj.FulOrderStatusId = FulOrderStatusId;
                            FulOrderObj.OnHold_FLG = false;
                            FulOrderObj.Error_FLG = false;
                            FulOrderObj.IsPrime_FLG = isPrime;
                            FulOrderObj.CarrierDescription = CarrierDescription;
                            FulOrderObj.ShipByDT = ShipByDT;
                            FulOrderList.Add(FulOrderObj);
                        }
                    }
                    else if (ProductInvObj != null && ProductInvObj.Quantity < OrdrLineObj.Quantity)
                    {
                        MailServiceObj.SendMail(string.Format(_iconfiguration["ProductInventoryShortage"],
                                        OrdrLineObj.SKU,
                                        OrderNumber ?? "",
                                        OrdrLineObj.Quantity.ToString(),
                                        ProductInvObj.Quantity.ToString(),
                                       (OrdrLineObj.Quantity - ProductInvObj.Quantity).ToString()));
                    }
                }
                context.FulOrder.AddRange(FulOrderList);
                try
                {
                    context.SaveChanges();
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                bool OrderLineSaveResponse = false;
                if (FulOrderList.Count > 0)
                    OrderLineSaveResponse = SaveFulItems(OrderLineList, FulOrderList);
                return OrderLineSaveResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool SaveFulItems(List<OrderLine> OrderLineList, List<FulOrder> FulOrderList)
        {
            try
            {
                int FOIteration = 0;
                foreach (var OrderLineGroup in OrderLineList.GroupBy(x => x.OrderId))
                {
                    List<FulItem> FulItemList = new List<FulItem>();
                    List<int> FullOrderList = new List<int>();

                    foreach (OrderLine OrdrLineObj in OrderLineGroup)
                    {
                        /*If product is having ShipAlone Flag TRUE then mutiple Fulfill order may exists, for each Fulfill Order a Fulfill item record will be added. */
                        FullOrderList = FulOrderList.Where(x => x.OrderId == OrdrLineObj.OrderId).OrderBy(x=>x.FulOrderId).Select(x => x.FulOrderId).ToList();
                        if (FullOrderList.Count == 0)
                            return false;

                        ProductInventory ProductInvObj = context.Product.Join(context.ProductVersion, p => p.ProductId, PI => PI.ProductId, (p, PI) => PI)
                        .Join(context.ProductInventory, pi => pi.ProductVersionId, pv => pv.ProductVersionId, (pv, pi) => new { pv, pi })
                        .Where(pi => pi.pv.ProductId == OrdrLineObj.ProductId)
                        .Select(x => x.pi).FirstOrDefault();

                        string SKU = (from ps in context.ProductSKU
                                      join pst in context.ProductSKUType on ps.SKUTypeId equals pst.ProductSKUTypeId
                                      where ps.ProductId == OrdrLineObj.ProductId && pst.Name.ToUpper() == _iconfiguration["SKU"]
                                      select ps.SKU).FirstOrDefault();

                        if (ProductInvObj != null && ProductInvObj.Quantity >= OrdrLineObj.Quantity)
                        {
                            // Updating the Inventory Quantity once product is assigned to any order.
                            ProductInvObj.Quantity = ProductInvObj.Quantity - OrdrLineObj.Quantity;
                            context.ProductInventory.Update(ProductInvObj);

                            if (FullOrderList.Count >= FOIteration + 1)
                            {
                                Product productObj = context.Product.Where(P => P.ProductId == OrdrLineObj.ProductId).Select(p => p).FirstOrDefault();
                                if (productObj != null && productObj.ShipsAlone_FLG)
                                {
                                    int iteration = 0;
                                    while (iteration < OrdrLineObj.Quantity)
                                    {
                                        FulItem FulItemObj = new FulItem();
                                        FulItemObj.Quantity = 1;
                                        FulItemObj.ProductId = OrdrLineObj.ProductId ?? 0;
                                        FulItemObj.SKU = SKU;
                                        FulItemObj.OrderLineId = OrdrLineObj.OrderLineId;
                                        FulItemObj.FulOrderId = FulOrderList[FOIteration].FulOrderId;
                                        FulItemList.Add(FulItemObj);
                                        iteration++;
                                        FOIteration++;
                                    }
                                }
                                else if (productObj != null && !productObj.ShipsAlone_FLG)
                                {
                                    FulItem FulItemObj = new FulItem();
                                    FulItemObj.Quantity = OrdrLineObj.Quantity;
                                    FulItemObj.ProductId = OrdrLineObj.ProductId ?? 0;
                                    FulItemObj.SKU = SKU;
                                    FulItemObj.OrderLineId = OrdrLineObj.OrderLineId;
                                    FulItemObj.FulOrderId = FulOrderList[FOIteration].FulOrderId;
                                    FulItemList.Add(FulItemObj);
                                    FOIteration++;
                                }
                            }
                        }
                    }
                    /*Adding all available FulItems to database table */
                    context.FulItem.AddRange(FulItemList);
                    context.SaveChanges();
                    int OrderLineQty = OrderLineGroup.Sum(x => x.Quantity);
                    int OrderId = OrderLineGroup.Select(x => x.OrderId).FirstOrDefault();
                    int OrderFulLineQty = (from olg in OrderLineGroup
                                           join fil in FulItemList on olg.OrderLineId equals fil.OrderLineId
                                           where olg.OrderId == OrderId
                                           select fil.Quantity).Sum();

                    if (OrderLineQty == OrderFulLineQty)
                    {
                        // Marking the IsFulfilled flag as true once inventory is assigned to the order.
                        Order OrderObj = context.Order.Where(o => o.OrderId == OrderId).Select(O => O).FirstOrDefault();
                        OrderObj.IsFulfilled = true;
                        context.Order.Update(OrderObj);
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //This Method will prepare the OrderLines once Orders are get saved then OrderLines will be updated automatically.
        private List<StagingOrders> GetUnProcessedOrders()
        {
            return context.StagingOrders.Where(x => x.Status == false).OrderBy(x => x.StartDate).Select(x => x).ToList();
        }
        #endregion
    }
}
