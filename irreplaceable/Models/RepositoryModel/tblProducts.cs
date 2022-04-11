using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using irreplaceable.Models;
using PagedList;

public class tblProducts : BaseClass
{
    public IRepository<Products> repo;
    public tblProducts()
    {
        repo = new EFGenericRepository<Products>(new web110b_07Entities());
    }

    public string GetProductName(string productNo)
    {
        return repo.ReadSingle(m => m.product_no == productNo).product_name;
    }

    public string GetProductCategoryNo(string productNo)
    {
        return repo.ReadSingle(m => m.product_no == productNo).category_no;
    }


    public string GetProductCategoryName(string productNo)
    {
        using (tblProductsCategorys categorys = new tblProductsCategorys())
        {
            string str_category_no = repo.ReadSingle(m => m.product_no == productNo).category_no;
            return categorys.GetProductCategoryName(str_category_no);

        }

    }
    public int GetProductPrice(string productNo)
    {
        return repo.ReadSingle(m => m.product_no == productNo).price;
    }

    public List<Products> GetShopProductsList(int page, int pageSize)
    {

        int int_low = ShopService.PriceLow;
        int int_high = ShopService.PriceHigh;
        var productModel = repo.ReadAll(m => m.price >= int_low && m.price <= int_high).ToList();
        if (string.IsNullOrEmpty(ShopService.SearchText))
        {
            if (!string.IsNullOrEmpty(ShopService.CategoryNo))
            {
                productModel = productModel.Where(m => m.category_no == ShopService.CategoryNo).ToList();
            }
        }

        if (!string.IsNullOrEmpty(ShopService.SearchText))
        {
            productModel = productModel.Where(m =>
            m.product_no.Contains(ShopService.SearchText) ||
            m.product_name.Contains(ShopService.SearchText) ||
            m.product_material.Contains(ShopService.SearchText) ||
            m.product_length.Contains(ShopService.SearchText)
            ).ToList();
        }
        if (ShopService.SortNo == "NewArrival") productModel = productModel.OrderByDescending(m => m.rowid).ToList();
        if (ShopService.SortNo == "NameAsc") productModel = productModel.OrderBy(m => m.product_name).ToList();
        if (ShopService.SortNo == "NameDesc") productModel = productModel.OrderByDescending(m => m.product_name).ToList();
        if (ShopService.SortNo == "PriceAsc") productModel = productModel.OrderBy(m => m.price).ToList();
        if (ShopService.SortNo == "PriceDesc") productModel = productModel.OrderByDescending(m => m.price).ToList();

        ShopService.Page = (page == -1) ? ShopService.PriorPage() : (page == -2) ? ShopService.NextPage() : page;
        ShopService.PageSize = pageSize;
        ShopService.PageRowCount = productModel.Count();

        productModel = productModel.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return productModel;

    }



    /// <summary>
    /// 取得商品資料集
    /// </summary>
    /// <param name="index">陣列索引</param>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    public IPagedList<Products> GetModelList(int index, int page, int pageSize, string searchText)
    {
        var model = repo.ReadAll();
        var dataSort = SessionService.GetColumnSort(index);
        if (!string.IsNullOrEmpty(searchText))
        {
            model = model.Where(m =>
            m.product_no.Contains(searchText) ||
            m.product_name.Contains(searchText) ||
            m.category_no.Contains(searchText) ||
            m.price.ToString().Contains(searchText) ||
            m.content_text.ToString().Contains(searchText) ||
            m.product_length.ToString().Contains(searchText) ||
            m.product_material.ToString().Contains(searchText) ||
            m.remark.Contains(searchText));
        }
        if (model != null)
        {
            if (string.IsNullOrEmpty(dataSort.SortColumn))
            {
                dataSort.SortColumn = "product_no";
                dataSort.SortDirection = enumSortDirection.Desc;
                model = model.OrderByDescending(m => m.product_no);
            } 
            if (dataSort.SortColumn == "product_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.product_no);
            if (dataSort.SortColumn == "product_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.product_no);
            if (dataSort.SortColumn == "product_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.product_name);
            if (dataSort.SortColumn == "product_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.product_name);
            if (dataSort.SortColumn == "category_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.category_no);
            if (dataSort.SortColumn == "category_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.category_no);
            if (dataSort.SortColumn == "price" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.price);
            if (dataSort.SortColumn == "price" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.price);
            if (dataSort.SortColumn == "content_text" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.content_text);
            if (dataSort.SortColumn == "content_text" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.content_text);
            if (dataSort.SortColumn == "product_length" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.product_length);
            if (dataSort.SortColumn == "product_length" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.product_length);
            if (dataSort.SortColumn == "product_material" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.product_material);
            if (dataSort.SortColumn == "product_material" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.product_material);
        }

        var datas = model.ToPagedList(page, pageSize);
        SessionService.SetCurrentPage(index, page, searchText, model.ToList().Count, datas.PageCount);
        return datas;
    }



    /// <summary>
    /// 前六項熱門商品
    /// </summary>
    /// <returns></returns>
    public List<Products> ProductsDataList()
    {
        using (tblProducts products = new tblProducts())
        {
            var data = repo.ReadAll().OrderByDescending(m => m.sold_qty).Take(6).ToList();


            return data;
        }

    }

    /// <summary>
    /// 取得商品編號下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetProductsSelectList()
    {
        List<SelectListItem> dataList = new List<SelectListItem>();
        var data = repo.ReadAll().OrderBy(m => m.product_no);
        if (data != null)
        {
            foreach (var item in data)
            {
                dataList.Add(new SelectListItem() { Text = string.Format("{0}.{1}", item.product_no, item.product_name), Value = item.product_no });
            }
        }
        return dataList;
    }


}