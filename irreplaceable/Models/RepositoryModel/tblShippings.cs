using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;


public class tblShippings : BaseClass
{
    public IRepository<Shippings> repo;
    public tblShippings()
    {
        repo = new EFGenericRepository<Shippings>(new web110b_07Entities());
    }

    public List<Shippings> ShippingsList()
    {
        return repo.ReadAll().OrderBy(m => m.ship_no).ToList();
    }

    /// <summary>
    /// 取得運送中文名稱
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetShippingName(string shipping_no)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.ship_no == shipping_no);
        if (data != null) str_value = data.ship_name;
        return str_value;
    }

    /// <summary>
    /// 取得運送下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetShippingNoList()
    {
        List<SelectListItem> dataList = new List<SelectListItem>();
        var data = repo.ReadAll().OrderBy(m => m.ship_no);
        if (data != null)
        {
            foreach (var item in data)
            {
                dataList.Add(new SelectListItem() { Text = string.Format("{0}", item.ship_name), Value = item.ship_no });
            }
        }
        return dataList;
    }
}