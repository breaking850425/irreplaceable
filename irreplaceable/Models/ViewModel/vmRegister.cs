using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public class vmRegister
{
    [Display(Name = "會員姓名")]
    [Required(ErrorMessage = "姓名不可空白!!")]
    public string AccountName { get; set; }

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "密碼不可空白!!")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "密碼不可小於 6 個字!!")]
    public string Password { get; set; }

    [Display(Name = "確認密碼")]
    [Required(ErrorMessage = "確認密碼不可空白!!")]
    [DataType(DataType.Password)]
    [StringLength(50, MinimumLength = 6, ErrorMessage = "密碼不可小於 6 個字!!")]
    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "確認密碼不正確!!")]
    public string ConfirmPassword { get; set; }


    [Display(Name = "會員性別")]
    public string GenderCode { get; set; }

    [Display(Name = "出生日期")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
    public DateTime Birthday { get; set; }

    [Display(Name = "電子信箱")]
    [Required(ErrorMessage = "電子信箱不可空白!!")]
    [DataType(DataType.EmailAddress)]
    public string ContactEmail { get; set; }

    [Display(Name = "聯絡電話")]
    public string ContactPhone { get; set; }

    [Display(Name = "聯絡地址")]
    public string ContactAddress { get; set; }


    public List<SelectListItem> GetGenderCodeList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "男", Value = "M" },
            new SelectListItem() { Text = "女", Value = "F" }
        };
    }
}
