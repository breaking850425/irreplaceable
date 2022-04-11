using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;


public class tblProductsCategorys : BaseClass
{
    public IRepository<ProductsCategorys> repo;
    public tblProductsCategorys()
    {
        repo = new EFGenericRepository<ProductsCategorys>(new web110b_07Entities());
    }

    public List<ProductsCategorys> GetCategoryDataList()
    {
        return repo.ReadAll(m => m.is_enabled == true && m.category_parent_no == "").OrderBy(m => m.rowid).ToList();
    }

    public string GetProductCategoryName(string categoryNo)
    {

        var data = repo.ReadSingle(m => m.category_no == categoryNo);
        return (data == null) ? "" : data.category_name;
    }
    public List<ProductsCategorys> GetShopCategoryList()
    {
        using (tblProducts products = new tblProducts())
        {
            var model = repo.ReadAll(m => m.is_enabled == true).OrderBy(m => m.category_no).ToList();
            foreach (var item in model)
            {
                item.product_counts = products.repo.ReadAll(m => m.category_no == item.category_no).Count();
            }
            return model;
        }
    }

    /// <summary>
    /// 取得商品分類下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetCategoryList()
    {
        List<SelectListItem> dataList = new List<SelectListItem>();
        var data = repo.ReadAll().OrderBy(m => m.category_no);
        if (data != null)
        {
            foreach (var item in data)
            {
                dataList.Add(new SelectListItem() { Text = string.Format("{0}_{1}", item.category_no, item.category_name), Value = item.category_no });
            }
        }
        return dataList;
    }
}
