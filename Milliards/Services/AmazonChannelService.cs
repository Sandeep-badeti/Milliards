using Microsoft.Extensions.Configuration;
using Milliards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
    public class AmazonChannelService : IAmazonChannelService
    {
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        public AmazonChannelService(AppDbContext context, IConfiguration iconfiguration)
        {
            this._iconfiguration = iconfiguration;
            this.context = context;
        }
        public AmazonChannelService()
        {
            this.context = new AppDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<AppDbContext>());
        }
        public string GetEndDateTime()
        {
            List<StagingOrders> OrderList = context.StagingOrders.ToList();
            if (OrderList.Count() > 0)
            {
                var result = OrderList.OrderByDescending(x => x.EndDate).Take(1).Select(x => x.EndDate);
                if (result != null)
                    return ((DateTime)result.ElementAt(0)).ToString();
            }
            return null;
        }

        public string SaveOrders(DateTime StartDate, DateTime EndDate, string OrdersXML)
        {
            try
            {
                StagingOrders OrdersObj = new StagingOrders();
                OrdersObj.EndDate = EndDate;
                OrdersObj.StartDate = StartDate;
                OrdersObj.JSON = OrdersXML;
                OrdersObj.Status = false;
                context.StagingOrders.Add(OrdersObj);
                context.SaveChanges();
                return _iconfiguration["SUCCESS"];
            }
            catch (Exception Ex) { return "ERROR_" + Ex.Message; }
        }
    }
}
