using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;


public class tblPayments : BaseClass
{
    public IRepository<Payments> repo;
    public tblPayments()
    {
        repo = new EFGenericRepository<Payments>(new web110b_07Entities());
    }

    public List<Payments> PaymentsList()
    {
        return repo.ReadAll().OrderBy(m => m.pay_no).ToList();
    }

    /// <summary>
    /// 取得付款中文名稱
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetPaymentName(string payment_no)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.pay_no == payment_no);
        if (data != null) str_value = data.pay_name;
        return str_value;
    }


    /// <summary>
    /// 取得付款下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetPaymentNoList()
    {
        List<SelectListItem> dataList = new List<SelectListItem>();
        var data = repo.ReadAll().OrderBy(m => m.pay_no);
        if (data != null)
        {
            foreach (var item in data)
            {
                dataList.Add(new SelectListItem() { Text = string.Format("{0}", item.pay_name), Value = item.pay_no });
            }
        }
        return dataList;
    }

}

