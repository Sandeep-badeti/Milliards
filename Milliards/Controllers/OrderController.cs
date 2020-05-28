using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Milliards.Services;

namespace Milliards.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [TypeFilter(typeof(Interceptor))]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        public OrderController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpGet]
        [ActionName("list")]
        public ActionResult<Object> GetOrdersList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue, string FromDate, string ToDate)
        {
            var result = orderService.GetOrdersList(pageno, recordsize, columnsort, ordersort, searchValue, FromDate, ToDate);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("orderlines")]
        public ActionResult<Object> GetOrderLineDetailsList(int orderId)
        {
            var result = orderService.GetOrderLineDetailsList(orderId);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("fulfilledOrders")]
        public ActionResult<Object> GetfulfilledOrdersDetailList(int orderId)
        {
            var result = orderService.GetfulfilledOrdersDetailList(orderId);
            return Ok(result);
        }

        [HttpGet]
        [ActionName("details")]
        public ActionResult<Object> GetOrdersDetailList(int orderId)
        {
            var result = orderService.GetOrdersDetailList(orderId);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("CreateFulfillOrders")]
        public bool CreateFulfillOrders()
        {
            bool response = orderService.FulfillUnProcessedOrders();
            return response;
        }
    }
}