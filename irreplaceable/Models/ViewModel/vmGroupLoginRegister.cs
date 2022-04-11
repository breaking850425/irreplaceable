using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class vmGroupLoginRegister
{
    public vmLogin Login { get; set; }
    public vmRegister Register { get; set; }
   

    public List<SelectListItem> GetGenderCodeList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "Gender 請選擇性別",  Disabled = true , Selected =true },
            new SelectListItem() { Text = "男", Value = "M" },
            new SelectListItem() { Text = "女", Value = "F" }
        };
    }


}


