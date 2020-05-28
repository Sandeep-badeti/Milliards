using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Milliards.DTO;
using Milliards.Services;

namespace Milliards.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [TypeFilter(typeof(Interceptor))]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        [ActionName("list")]
        public ActionResult<Object> GetProductList(int pageno, int recordsize, string columnsort, string ordersort , string searchValue)
        {
            var result = productService.GetProductList(pageno, recordsize, columnsort, ordersort, searchValue);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("types")]
        public ActionResult<Object> GetTypeList()
        {
            var result = productService.GetTypeList();
            return Ok(result);
        }
        [HttpPost]
        [ActionName("save")]
        public ActionResult<Object> AddEditProduct(ProductDetailsDTO product)
        {
            var result = productService.AddEditProduct(product);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("details")]
        public ActionResult<Object> GetProductDetails(int ProductId)
        {
            var result = productService.GetProductDetails(ProductId);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("view")]
        public ActionResult<Object> GetProductView(int ProductId)
        {
            var result = productService.GetProductView(ProductId);
            return Ok(result);
        }
        [HttpGet]
        [ActionName("validate")]
        public ActionResult<Object> CheckDuplicates(string type, string name)
        {
            var result = productService.CheckDuplicates(type, name);
            return Ok(result);
        }
    }
}