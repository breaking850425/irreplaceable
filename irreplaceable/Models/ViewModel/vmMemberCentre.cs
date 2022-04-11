using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;



public class vmMemberCentre
{

    public vmMembers Members { get; set; }
    public vmResetPassword ResetPassword { get; set; }
    public Orders OrderSingle { get; set; }
    public List<Orders> Orders { get; set; }
    public List<OrdersDetail> OrdersDetail { get; set; }
    

    public List<SelectListItem> GetGenderCodeList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "請選擇性別",  Disabled = true , Selected =true },
            new SelectListItem() { Text = "男", Value = "M" },
            new SelectListItem() { Text = "女", Value = "F" }
        };
    }
}
