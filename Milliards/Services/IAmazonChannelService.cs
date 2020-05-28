using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Milliards.Services
{
   public interface IAmazonChannelService
    {
        string SaveOrders(DateTime StartDate, DateTime EndDate, string OrdersXML);
        string GetEndDateTime();
    }
}
