using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Milliards.Models;
using Milliards.Services;

namespace Milliards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : ControllerBase
    {
        private IAmazonChannelService _channelService;
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        public ChannelController(AppDbContext context, IAmazonChannelService channelService, IConfiguration iconfiguration)
        {
            this.context = context;
            _channelService = channelService;
            _iconfiguration = iconfiguration;
        }

        [HttpGet]
        [Route("GetEndDate")]
        public ActionResult<string> GetDate() {
           return  _channelService.GetEndDateTime();
        }

        [HttpPost]
        [Route("SaveOrders")]
        public ActionResult<string> SaveOrders(DateTime StartDate, DateTime EndDate, string OrdersXML)
        {
            string result = string.Empty;
            result = _channelService.SaveOrders(StartDate, EndDate, OrdersXML);
            return result;
        }
    }
}