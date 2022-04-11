using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;


public class tblOrderStatus : BaseClass
{
    public IRepository<OrderStatus> repo;
    public tblOrderStatus()
    {
        repo = new EFGenericRepository<OrderStatus>(new web110b_07Entities());
    }


    /// <summary>
    /// 取得訂單狀態中文名稱
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetOrderStatusName(string status_no)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.status_no == status_no);
        if (data != null) str_value = data.status_name;
        return str_value;
    }
}


