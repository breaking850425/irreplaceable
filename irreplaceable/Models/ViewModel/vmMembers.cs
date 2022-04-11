using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;



public class vmMembers
{
    
    [Display(Name = "會員編號")]
    public string MemberNo { get; set; }

    [Display(Name = "會員姓名")]
    [Required(ErrorMessage = "會員姓名不可空白!!")]
    public string MemberName { get; set; }

    [Display(Name = "會員密碼")]
    public string MemberPassword { get; set; }

    [Display(Name = "會員性別")]
    public string GenderCode { get; set; }

    //[Display(Name = "性別中文")]
    //public string GenderName { get { return GetGenderCodeList().Find(m => m.Value == GenderCode).Text; } }

    [Display(Name = "會員生日")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
    public Nullable<System.DateTime> BirthDate { get; set; }

    [Display(Name = "電子信箱")]
    [Required(ErrorMessage = "電子信箱不可空白!!")]
    [EmailAddress(ErrorMessage = "電子信箱格式不正確!!")]
    [DataType(DataType.EmailAddress)]
    public string MemberEmail { get; set; }

    [Display(Name = "聯絡電話")]
    public string MemberPhone { get; set; }

    [Display(Name = "郵遞區號")]
    public string MemberZip { get; set; }

    [Display(Name = "通訊地址")]
    public string MemberAddress { get; set; }

    [Display(Name = "備註")]
    public string Remark { get; set; }


}
