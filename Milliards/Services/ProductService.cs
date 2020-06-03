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
    public class ProductService : IProductService
    {
        private readonly AppDbContext context;
        private IConfiguration _iconfiguration;
        public ProductService(AppDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this._iconfiguration = configuration;
        }
        //product list based on pagination with sorting and searching
        public Object GetProductList(int pageno, int recordsize, string columnsort, string ordersort, string searchValue)
        {
            try
            {
                List<ProductDTO> productList = new List<ProductDTO>();
                SqlParameter pagenumber = new SqlParameter(_iconfiguration["PAGENO"], pageno);
                SqlParameter pagesize = new SqlParameter(_iconfiguration["PAGESIZE"], recordsize);
                SqlParameter sortcolumn = new SqlParameter(_iconfiguration["SORTCOLUMN"], columnsort == null ? _iconfiguration["ORDERID"] : columnsort);
                SqlParameter sortorder = new SqlParameter(_iconfiguration["SORTORDER"], ordersort == null ? _iconfiguration["DESC"] : ordersort.ToUpper());
                SqlParameter searchvalue = new SqlParameter(_iconfiguration["SEARCHVALUE"], searchValue == null ? "" : searchValue);
                productList = context.ProductList.FromSqlRaw("EXEC [dbo].[usp_ProductList] @PageNo,@PageSize,@SortColumn,@SortOrder,@SearchValue", pagenumber, pagesize, sortcolumn, sortorder, searchvalue).ToList();
                var RecordCount = productList.Select(s => s.TotalRecs).FirstOrDefault();
                return new { TotalRecordsCount = RecordCount, Data = productList };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //find out tables data for Dropdown list purpose(in the form of Key,value pair)
        public Object GetTypeList()
        {
            try
            {
                var boxList = context.Box.Select(x => new { x.BoxId, x.Name, x.Length, x.Width, x.Height, x.DimensionUnitId }).ToList();
                var dimensionUnitList = context.DimensionUnit.Select(x => new { x.DimensionUnitId, x.Name, x.CoversionRate }).ToList();
                var weightUnitList = context.WeightUnit.Select(x => new { x.WeightUnitId, x.Name, x.CoversionRate }).ToList();
                var productStatusList = context.ProductStatus.Select(x => new { x.ProductStatusId, x.Name }).ToList();
                var productSKUStatusList = context.ProductSKUStatus.Select(x => new { x.ProductSKUStatusId, x.Name }).ToList();
                var warehouseList = context.Warehouse.Select(x => new { x.WarehouseId, x.Name, x.WHStatusId, x.TimezoneId, x.ShipFromAddressId }).ToList();
                var productInventoryStatusList = context.ProductInventoryStatus.Select(x => new { x.ProductInventoryStatusId, x.Name }).ToList();
                var manufacturerList = context.Manufacturer.Select(x => new { x.ManufacturerId, x.Name }).ToList();
                var conditionList = context.Condition.Select(x => new { x.ConditionId, x.Name }).ToList();
                var colorList = context.Color.Select(x => new { x.ColorId, x.Name }).ToList();
                var errorReasonList = context.ErrorReason.Select(x => new { x.ErrorReasonId, x.Name }).ToList();
                var OnHoldReasonList = context.OnHoldReason.Select(x => new { x.OnHoldReasonId, x.Name }).ToList();
                var productTagList = context.ProductTag.Select(x => new { x.ProductId, x.TagId }).ToList();
                var productCategoryList = context.ProductCategory.Select(x => new { x.ProductCategoryId, x.Name }).ToList();
                var tagList = context.Tag.Select(x => new { x.TagId, x.Name, x.TagTypeId }).ToList();
                var modeList = (from fm in context.FulfillMode
                                select new Modes { modeId = fm.FulfillModeId, modeName = fm.Name })
                                          .OrderBy(x => x.modeId).ToList();
                var carrierList = context.Carrier.Select(c => new FulfillCarrier { carrierId = c.CarrierId, name = c.Name }).ToList();
                return new
                {
                    boxList,
                    dimensionUnitList,
                    weightUnitList,
                    productStatusList,
                    productSKUStatusList,
                    warehouseList,
                    productInventoryStatusList,
                    manufacturerList,
                    conditionList,
                    colorList,
                    productTagList,
                    productCategoryList,
                    tagList,
                    modeList,
                    carrierList,
                    errorReasonList,
                    OnHoldReasonList
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // creation of the produt  && edit the produt
        public Object AddEditProduct(ProductDetailsDTO product)
        {
            try
            {
                bool isSuccess;
                string result = string.Empty;
                int? productId;
                bool isNew = product.ProductId != 0 ? false : true;
                var addProduct = isNew ? new Product { } : context.Product.Find(product.ProductId);
                addProduct.Name = product.Name == null ? null : product.Name;
                addProduct.Description = product.Description == null ? null : product.Description;
                addProduct.UPC = product.UPC == null ? null : product.UPC;
                addProduct.BoxId = product.BoxId == null ? null : product.BoxId;
                addProduct.StatusId = product.StatusId;
                addProduct.Length = product.Length;
                addProduct.Width = product.Width;
                addProduct.Height = product.Height;
                addProduct.Weight = product.Weight;
                addProduct.DimensionUnitId = product.DimensionUnitId;
                addProduct.WeightUnitId = product.WeightUnitId;
                addProduct.ShipsAlone_FLG = product.ShipsAlone_FLG;
                addProduct.ManufacturerId = product.ManufacturerId;
                addProduct.ConditionId = product.ConditionId;
                addProduct.CategoryId = product.CategoryId;
                addProduct.ColorId = product.ColorId;
                addProduct.Ref1 = product.Ref1;
                addProduct.Ref2 = product.Ref2;
                if (isNew)
                {
                    //sku
                    var addProductSku = new List<ProductSKU>();
                    foreach (var item in product.ProductSku)
                    {
                        ProductSKU addsku = new ProductSKU();
                        addsku.ProductId = item.ProductId;
                        addsku.SKU = item.SKU == null ? null : item.SKU;
                        addsku.StatusId = item.StatusId;
                        addsku.Description = item.Description == null ? null : item.Description;
                        addsku.SKUTypeId = item.SKUTypeId;
                        addProductSku.Add(addsku);
                    }
                    addProduct.ProductSKUs = addProductSku;
                    //inventory
                    var addProductInven = new List<ProductInventory>();
                    foreach (var item in product.ProductInventory)
                    {
                        ProductInventory addInventory = new ProductInventory();
                        addInventory.ProductVersionId = item.ProductVersionId;
                        addInventory.WarehouseId = item.WarehouseId;
                        addInventory.Quantity = item.Quantity;
                        addInventory.StatusId = item.StatusId;
                        addProductInven.Add(addInventory);
                    }
                    //versions
                    var addProductVersion = new List<ProductVersion>();
                    foreach (var item in product.ProductVersion)
                    {
                        ProductVersion addversion = new ProductVersion();
                        var matchedInventory = new List<ProductInventory>();
                        matchedInventory = addProductInven.FindAll(x => x.ProductVersionId == item.ProductVersionId);
                        addversion.ProductInventories = matchedInventory;
                        addversion.ProductId = item.ProductId;
                        addversion.Description = item.Description;
                        addversion.StatusId = item.StatusId;
                        addProductVersion.Add(addversion);
                    }
                    addProduct.ProductVersions = addProductVersion;
                    //tags
                    var addProductTag = new List<ProductTag>();
                    if (product.ProductTag != null)
                    {
                        foreach (var item in product.ProductTag)
                        {
                            ProductTag addTag = new ProductTag();
                            addTag.ProductId = item.ProductId;
                            addTag.TagId = item.TagId;
                            addProductTag.Add(addTag);
                        }
                        addProduct.ProductTags = addProductTag;
                    }
                    context.Product.Add(addProduct);
                    context.SaveChanges();
                    result = _iconfiguration["PRODUCT_ADDED_SUCCESSFUL"];
                    productId = addProduct.ProductId;
                    isSuccess = true;
                }
                else
                {
                    List<ProductSKU> existingProductSku = context.ProductSKU.Where(x => x.ProductId == product.ProductId).ToList();
                    List<ProductVersion> existingProductVersion = context.ProductVersion.Where(x => x.ProductId == product.ProductId).ToList();
                    List<ProductTag> existingProductTag = context.ProductTag.Where(x => x.ProductId == product.ProductId).ToList();
                    var existingInventory = new List<ProductInventory>();
                    foreach (var item in existingProductVersion)
                    {
                        var existingmatchedInventory = new List<ProductInventory>();
                        existingmatchedInventory = context.ProductInventory.Where(x => x.ProductVersionId == item.ProductVersionId).ToList();
                        if (existingmatchedInventory.Count > 0)
                        {
                            existingInventory.AddRange(existingmatchedInventory);
                        }
                    }
                    //sku
                    var newProductSku = new List<ProductSKU>();
                    foreach (var item in product.ProductSku)
                    {
                        ProductSKU addsku = new ProductSKU();
                        addsku.ProductId = item.ProductId;
                        addsku.SKU = item.SKU == null ? null : item.SKU;
                        addsku.StatusId = item.StatusId;
                        addsku.Description = item.Description == null ? null : item.Description;
                        addsku.SKUTypeId = item.SKUTypeId;
                        newProductSku.Add(addsku);
                    }
                    //inventory
                    var newProductInven = new List<ProductInventory>();
                    foreach (var item in product.ProductInventory)
                    {
                        ProductInventory addInventory = new ProductInventory();
                        addInventory.ProductVersionId = item.ProductVersionId;
                        addInventory.WarehouseId = item.WarehouseId;
                        addInventory.Quantity = item.Quantity;
                        addInventory.StatusId = item.StatusId;
                        newProductInven.Add(addInventory);
                    }
                    //versions
                    var newProductVersion = new List<ProductVersion>();
                    foreach (var item in product.ProductVersion)
                    {
                        ProductVersion addversion = new ProductVersion();
                        var matchedInventory = new List<ProductInventory>();
                        matchedInventory = newProductInven.FindAll(x => x.ProductVersionId == item.ProductVersionId);
                        addversion.ProductInventories = matchedInventory;
                        addversion.ProductId = item.ProductId;
                        addversion.Description = item.Description;
                        addversion.StatusId = item.StatusId;
                        newProductVersion.Add(addversion);
                    }
                    //tags
                    var newProductTag = new List<ProductTag>();
                    if (product.ProductTag != null)
                    {
                        foreach (var item in product.ProductTag)
                        {
                            ProductTag addTag = new ProductTag();
                            addTag.ProductId = item.ProductId;
                            addTag.TagId = item.TagId;
                            newProductTag.Add(addTag);
                        }
                    }
                    addProduct.ProductSKUs = new List<ProductSKU>();
                    addProduct.ProductVersions = new List<ProductVersion>();
                    addProduct.ProductTags = new List<ProductTag>();
                    if (existingProductSku.Count > 0)
                    {
                        context.ProductSKU.RemoveRange(existingProductSku);
                    }
                    if (existingInventory.Count > 0)
                    {
                        context.ProductInventory.RemoveRange(existingInventory);
                    }
                    if (existingProductVersion.Count > 0)
                    {
                        context.ProductVersion.RemoveRange(existingProductVersion);
                    }
                    if (existingProductTag.Count > 0)
                    {
                        context.ProductTag.RemoveRange(existingProductTag);
                    }
                    context.ProductSKU.AddRange(newProductSku);
                    context.ProductVersion.AddRange(newProductVersion);
                    if (newProductTag.Count > 0)
                    {
                        context.ProductTag.AddRange(newProductTag);
                    }
                    context.Product.Update(addProduct);
                    context.SaveChanges();
                    context.Dispose();
                    result = _iconfiguration["PRODUCT_EDITED_SUCCESSFUL"];
                    productId = addProduct.ProductId;
                    isSuccess = true;
                }
                return new { status = isSuccess, productID = productId, message = result };
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return new { status = false, productID = product.ProductId, message = ex.InnerException.Message };
                }
                else
                {
                    return new { status = false, productID = product.ProductId, message = ex.Message };
                }
                throw ex;
            }
        }

        public object GetProductDetails(int ProductId)
        {
            try
            {
                var versionsResult = (from pr in context.Product.Where(s => s.ProductId == ProductId)
                                      join pv in context.ProductVersion on pr.ProductId equals pv.ProductId into pv1
                                      from pv in pv1.DefaultIfEmpty()
                                      select new ProductVersion
                                      {
                                          ProductId = pv.ProductId,
                                          ProductVersionId = pv.ProductVersionId,
                                          Version = pv.Version,
                                          Description = pv.Description,
                                          StatusId = pv.StatusId
                                      }).ToList();

                var inventoryResult = (from pd in context.Product.Where(s => s.ProductId == ProductId)
                                       join pv in context.ProductVersion on pd.ProductId equals pv.ProductId into pv1
                                       from pv in pv1.DefaultIfEmpty()
                                       join pin in context.ProductInventory on pv.ProductVersionId equals pin.ProductVersionId into pin1
                                       from pin in pin1.DefaultIfEmpty()
                                       select new ProductInventory
                                       {
                                           ProductInventoryId = pin.WarehouseId,
                                           ProductVersionId = pin.ProductVersionId,
                                           WarehouseId = pin.WarehouseId,
                                           Quantity = pin.Quantity,
                                           StatusId = pin.StatusId
                                       }).ToList();

                var tagsResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)

                                  join pt in context.ProductTag on prd.ProductId equals pt.ProductId into pt1
                                  from pt in pt1
                                  select new ProductTag
                                  {
                                      TagId = pt.TagId,
                                      ProductId = pt.ProductId
                                  }).ToList();

                var virtualSkuResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)
                                        join ps in context.ProductSKU on prd.ProductId equals ps.ProductId into ps1
                                        from ps in ps1.DefaultIfEmpty()
                                        where ps.SKUTypeId == 1
                                        select new ProductSKU
                                        {
                                            ProductSKUId = ps.ProductSKUId,
                                            ProductId = ps.ProductId,
                                            SKU = ps.SKU,
                                            Description = ps.Description,
                                            SKUTypeId = ps.SKUTypeId,
                                            StatusId = ps.StatusId
                                        }).ToList();

                var mainSkuResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)
                                     join ps in context.ProductSKU on prd.ProductId equals ps.ProductId into ps1
                                     from ps in ps1.DefaultIfEmpty()
                                     where ps.SKUTypeId == 2
                                     select new ProductSKU
                                     {
                                         ProductSKUId = ps.ProductSKUId,
                                         ProductId = ps.ProductId,
                                         SKU = ps.SKU,
                                         Description = ps.Description,
                                         SKUTypeId = ps.SKUTypeId,
                                         StatusId = ps.StatusId
                                     }).ToList();

                var result = (from p in context.Product.Where(s => s.ProductId == ProductId)
                              join pk in context.ProductSKU on p.ProductId equals pk.ProductId into p1
                              from pk in p1.DefaultIfEmpty()
                              join d in context.DimensionUnit on p.DimensionUnitId equals d.DimensionUnitId into d1
                              from d in d1.DefaultIfEmpty()
                              join w in context.WeightUnit on p.WeightUnitId equals w.WeightUnitId into w1
                              from w in w1.DefaultIfEmpty()
                              join b in context.Box on p.BoxId equals b.BoxId into b1
                              from b in b1.DefaultIfEmpty()
                              join pst in context.ProductStatus on p.StatusId equals pst.ProductStatusId into pst1
                              from pst in pst1.DefaultIfEmpty()
                              join vst in context.ProductSKU on p.ProductId equals vst.ProductId into vst1
                              from vst in vst1.DefaultIfEmpty()
                              join pver in context.ProductVersion on p.ProductId equals pver.ProductId into pver1
                              from pver in pver1.DefaultIfEmpty()
                              join pinv in context.ProductInventory on pver.ProductVersionId equals pinv.ProductVersionId into pinv1
                              from pinv in pinv1.DefaultIfEmpty()
                              join pinst in context.ProductInventoryStatus on pinv.StatusId equals pinst.ProductInventoryStatusId into pinst1
                              from pinst in pinst1.DefaultIfEmpty()
                              join ptag in context.ProductTag on p.ProductId equals ptag.ProductId into ptag1
                              from ptag in ptag1.DefaultIfEmpty()
                              join tag in context.Tag on ptag.TagId equals tag.TagId into tag1
                              from tag in tag1.DefaultIfEmpty()
                              join manu in context.Manufacturer on p.ManufacturerId equals manu.ManufacturerId into manu1
                              from manu in manu1.DefaultIfEmpty()
                              join con in context.Condition on p.ConditionId equals con.ConditionId into con1
                              from con in con1.DefaultIfEmpty()
                              join cat in context.ProductCategory on p.CategoryId equals cat.ProductCategoryId into cat1
                              from cat in cat1.DefaultIfEmpty()
                              join sksta in context.ProductSKUStatus on vst.StatusId equals sksta.ProductSKUStatusId into sksta1
                              from sksta in sksta1.DefaultIfEmpty()
                              join col in context.Color on p.ColorId equals col.ColorId into col1
                              from col in col1.DefaultIfEmpty()
                              join ware in context.Warehouse on pinv.WarehouseId equals ware.WarehouseId into ware1
                              from ware in ware1.DefaultIfEmpty()
                              select new ProductDetailsDTO
                              {
                                  ProductId = p.ProductId,
                                  Name = p.Name,
                                  Description = p.Description,
                                  UPC = p.UPC,
                                  BoxId = p.BoxId,
                                  StatusId = p.StatusId,
                                  Length = p.Length,
                                  Width = p.Width,
                                  Height = p.Height,
                                  Weight = p.Weight,
                                  DimensionUnitId = p.DimensionUnitId,
                                  WeightUnitId = p.WeightUnitId,
                                  ShipsAlone_FLG = p.ShipsAlone_FLG,
                                  ManufacturerId = p.ManufacturerId,
                                  ConditionId = p.ConditionId,
                                  CategoryId = p.CategoryId,
                                  ColorId = p.ColorId,
                                  Ref1 = p.Ref1,
                                  Ref2 = p.Ref2,
                                  MainSKU = mainSkuResult.Count > 0 ? mainSkuResult[0].SKU : null,
                                  ProductSku = virtualSkuResult,
                                  ProductInventory = inventoryResult,
                                  ProductTag = tagsResult,
                                  ProductVersion = versionsResult
                              }).FirstOrDefault();
                return new { Data = result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetProductView(int ProductId)
        {
            try
            {
                var versionsResult = (from pr in context.Product.Where(s => s.ProductId == ProductId)
                                      join pv in context.ProductVersion on pr.ProductId equals pv.ProductId into pv1
                                      from pv in pv1.DefaultIfEmpty()
                                      select new ProductVersionDTO
                                      {
                                          ProductId = pv.ProductId,
                                          ProductVersionId = pv.ProductVersionId,
                                          Version = pv.Version,
                                          Description = pv.Description,
                                          StatusId = pv.StatusId
                                      }).ToList();

                var inventoryResult = (from pd in context.Product.Where(s => s.ProductId == ProductId)
                                       join pv in context.ProductVersion on pd.ProductId equals pv.ProductId into pv1
                                       from pv in pv1.DefaultIfEmpty()
                                       join pin in context.ProductInventory on pv.ProductVersionId equals pin.ProductVersionId into pin1
                                       from pin in pin1.DefaultIfEmpty()
                                       join ware in context.Warehouse on pin.WarehouseId equals ware.WarehouseId into ware1
                                       from ware in ware1.DefaultIfEmpty()
                                       join pinst in context.ProductInventoryStatus on pin.StatusId equals pinst.ProductInventoryStatusId into pinst1
                                       from pinst in pinst1.DefaultIfEmpty()
                                       select new ProductInventoryDTO
                                       {
                                           ProductInventoryId = pin.WarehouseId,
                                           ProductVersionId = pin.ProductVersionId,
                                           WarehouseId = pin.WarehouseId,
                                           WarehouseName = ware.Name,
                                           Quantity = pin.Quantity,
                                           StatusId = pin.StatusId,
                                           StatusName = pinst.Name
                                       }).ToList();

                var tagsResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)
                                  join pt in context.ProductTag on prd.ProductId equals pt.ProductId into pt1
                                  from pt in pt1.DefaultIfEmpty()
                                  join tag in context.Tag on pt.TagId equals tag.TagId into tag1
                                  from tag in tag1.DefaultIfEmpty()
                                  select new ProductTagDTO
                                  {
                                      TagId = pt.TagId,
                                      TagName = tag.Name,
                                      ProductId = pt.ProductId
                                  }).ToList();

                var virtualSkuResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)
                                        join ps in context.ProductSKU on prd.ProductId equals ps.ProductId into ps1
                                        from ps in ps1.DefaultIfEmpty()
                                        join ss in context.ProductSKUStatus on ps.StatusId equals ss.ProductSKUStatusId into ss1
                                        from ss in ss1.DefaultIfEmpty()
                                        where ps.SKUTypeId == 1
                                        select new ProductSkuDTO
                                        {
                                            ProductSKUId = ps.ProductSKUId,
                                            ProductId = ps.ProductId,
                                            SKU = ps.SKU,
                                            Description = ps.Description,
                                            SKUTypeId = ps.SKUTypeId,
                                            StatusId = ps.StatusId,
                                            StatusName = ss.Name
                                        }).ToList();

                var mainSkuResult = (from prd in context.Product.Where(s => s.ProductId == ProductId)
                                     join ps in context.ProductSKU on prd.ProductId equals ps.ProductId into ps1
                                     from ps in ps1.DefaultIfEmpty()
                                     where ps.SKUTypeId == 2
                                     select new ProductSKU
                                     {
                                         ProductSKUId = ps.ProductSKUId,
                                         ProductId = ps.ProductId,
                                         SKU = ps.SKU,
                                         Description = ps.Description,
                                         SKUTypeId = ps.SKUTypeId,
                                         StatusId = ps.StatusId
                                     }).ToList();

                var result = (from p in context.Product.Where(s => s.ProductId == ProductId)
                              join pk in context.ProductSKU on p.ProductId equals pk.ProductId into p1
                              from pk in p1.DefaultIfEmpty()
                              join d in context.DimensionUnit on p.DimensionUnitId equals d.DimensionUnitId into d1
                              from d in d1.DefaultIfEmpty()
                              join w in context.WeightUnit on p.WeightUnitId equals w.WeightUnitId into w1
                              from w in w1.DefaultIfEmpty()
                              join b in context.Box on p.BoxId equals b.BoxId into b1
                              from b in b1.DefaultIfEmpty()
                              join pst in context.ProductStatus on p.StatusId equals pst.ProductStatusId into pst1
                              from pst in pst1.DefaultIfEmpty()
                              join vst in context.ProductSKU on p.ProductId equals vst.ProductId into vst1
                              from vst in vst1.DefaultIfEmpty()
                              join pver in context.ProductVersion on p.ProductId equals pver.ProductId into pver1
                              from pver in pver1.DefaultIfEmpty()
                              join pinv in context.ProductInventory on pver.ProductVersionId equals pinv.ProductVersionId into pinv1
                              from pinv in pinv1.DefaultIfEmpty()
                              join pinst in context.ProductInventoryStatus on pinv.StatusId equals pinst.ProductInventoryStatusId into pinst1
                              from pinst in pinst1.DefaultIfEmpty()
                              join ptag in context.ProductTag on p.ProductId equals ptag.ProductId into ptag1
                              from ptag in ptag1.DefaultIfEmpty()
                              join tag in context.Tag on ptag.TagId equals tag.TagId into tag1
                              from tag in tag1.DefaultIfEmpty()
                              join manu in context.Manufacturer on p.ManufacturerId equals manu.ManufacturerId into manu1
                              from manu in manu1.DefaultIfEmpty()
                              join con in context.Condition on p.ConditionId equals con.ConditionId into con1
                              from con in con1.DefaultIfEmpty()
                              join cat in context.ProductCategory on p.CategoryId equals cat.ProductCategoryId into cat1
                              from cat in cat1.DefaultIfEmpty()
                              join sksta in context.ProductSKUStatus on vst.StatusId equals sksta.ProductSKUStatusId into sksta1
                              from sksta in sksta1.DefaultIfEmpty()
                              join col in context.Color on p.ColorId equals col.ColorId into col1
                              from col in col1.DefaultIfEmpty()
                              join ware in context.Warehouse on pinv.WarehouseId equals ware.WarehouseId into ware1
                              from ware in ware1.DefaultIfEmpty()
                              join m in context.Manufacturer on p.ManufacturerId equals m.ManufacturerId into m1
                              from m in m1.DefaultIfEmpty()
                              join c in context.Condition on p.ConditionId equals c.ConditionId into c1
                              from c in c1.DefaultIfEmpty()
                              join pc in context.ProductCategory on p.CategoryId equals pc.ProductCategoryId into pc1
                              from pc in pc1.DefaultIfEmpty()
                              join pt in context.ProductTag on p.ProductId equals pt.ProductId into pt1
                              from pt in pt1.DefaultIfEmpty()
                              select new ProductViewDTO
                              {
                                  ProductId = p.ProductId,
                                  Name = p.Name,
                                  Description = p.Description,
                                  UPC = p.UPC,
                                  BoxId = p.BoxId,
                                  BoxName = b.Name,
                                  StatusName = pst.Name,
                                  DimensionName = d.Name,
                                  WeightUnitName = w.Name,
                                  StatusId = p.StatusId,
                                  Length = p.Length,
                                  Width = p.Width,
                                  Height = p.Height,
                                  Weight = p.Weight,
                                  DimensionUnitId = p.DimensionUnitId,
                                  WeightUnitId = p.WeightUnitId,
                                  ShipsAlone_FLG = p.ShipsAlone_FLG,
                                  ManufacturerId = p.ManufacturerId,
                                  ManufacturerName = m.Name,
                                  ConditionId = p.ConditionId,
                                  ConditionName = c.Name,
                                  CategoryId = p.CategoryId,
                                  CategoryName = pc.Name,
                                  ColorId = p.ColorId,
                                  ColorName = col.Name,
                                  Ref1 = p.Ref1,
                                  Ref2 = p.Ref2,
                                  MainSKU = mainSkuResult.Count > 0 ? mainSkuResult[0].SKU : null,
                                  ProductSku = virtualSkuResult,
                                  ProductInventory = inventoryResult,
                                  ProductTag = tagsResult,
                                  ProductVersion = versionsResult
                              }).FirstOrDefault();
                return new { Data = result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object CheckDuplicates(string type, string name)
        {
            try
            {
                int ProductId = 0;
                bool Status = false;
                if (type == _iconfiguration["SKU"])
                {
                    var validateSKU = context.ProductSKU.Where(a => a.SKU == name).ToList();
                    if (validateSKU.Count > 0)
                    {
                        ProductId = validateSKU[0].ProductId;
                        Status = true;
                    }
                }
                else if (type == _iconfiguration["UPC"])
                {
                    var validateUPC = context.Product.Where(a => a.UPC == name).ToList();
                    if (validateUPC.Count > 0)
                    {
                        ProductId = validateUPC[0].ProductId;
                        Status = true;
                    }
                }
                return new { status = Status, productID = ProductId };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
