using Milliards.DTO;
using System;

namespace Milliards.Services
{
    public interface IProductService
    {
        public Object GetProductList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue);
        public Object GetTypeList();
        public Object AddEditProduct(ProductDetailsDTO product);
        public object GetProductDetails(int ProductId);
        public object GetProductView(int ProductId);
        public object CheckDuplicates(string type, string name);
    }
}
