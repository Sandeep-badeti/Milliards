using Milliards.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    public interface IOrderService
    {
        public Object GetOrdersList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string fromDate, string toDate);
        public object GetOrderLineDetailsList(int orderId);
        public object GetfulfilledOrdersDetailList(int orderId);
        public object GetOrdersDetailList(int orderId);
        public bool FulfillUnProcessedOrders();
    }
}
