using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class vmAccountProfile
{
    [Display(Name = "登入帳號")]
    public string AccountNo { get; set; }
    [Display(Name = "登入名稱")]
    public string AccountName { get; set; }

    [Display(Name = "性別")]
    [Required(ErrorMessage = "性別不可空白!!")]
    public string GenderCode { get; set; }

    [Display(Name = "性別")]
    public string GenderName { get { return GetGenderCodeList().Find(m => m.Value == GenderCode).Text; } }




    [Display(Name = "出生日期")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
    public Nullable<System.DateTime> BirthDate { get; set; }
    [Display(Name = "部門代號")]
    public string DepartmentNo { get; set; }
    [Display(Name = "部門名稱")]
    public string DepartmentName { get; set; }
    [Display(Name = "職稱代號")]
    public string TitleNo { get; set; }
    [Display(Name = "職稱")]
    public string TitleName { get; set; }
    [Display(Name = "電子信箱")]
    [Required(ErrorMessage = "電子信箱不可空白!!")]
    [EmailAddress(ErrorMessage = "電子信箱格式不正確!!")]
    public string ContactEmail { get; set; }
    [Display(Name = "連絡電話")]
    public string ContactPhone { get; set; }
    [Display(Name = "郵遞區號")]
    public string ContactZip { get; set; }
    [Display(Name = "連絡地址")]
    public string ContactAddress { get; set; }

    public List<SelectListItem> GetGenderCodeList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "男", Value = "M" },
            new SelectListItem() { Text = "女", Value = "F" }
        };
    }
}
