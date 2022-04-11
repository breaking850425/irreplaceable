using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;
using System.ComponentModel.DataAnnotations;


public class vmMemberOrders
{
    [Display(Name = "訂單編號")]
    public string order_no { get; set; }

    [Display(Name = "訂單日期")]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
    public DateTime order_date { get; set; }

    [Display(Name = "訂單狀態")]
    public string order_status { get; set; }

    [Display(Name = "使用者編號")]
    public string user_no { get; set; }


    [Display(Name = "收件人姓名")]
    [Required(ErrorMessage = "收件人姓名不可空白")]
    public string receive_name { get; set; }

    [Display(Name = "收件人電話")]
    [Required(ErrorMessage = "收件人電話不可空白")]
    public string receive_phone { get; set; }

    [Display(Name = "收件人信箱")]
    [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
    public string receive_email { get; set; }

    [Display(Name = "收件人地址")]
    [Required(ErrorMessage = "收件人地址不可空白")]
    public string receive_address { get; set; }

    [Display(Name = "付款方式")]
    [Required(ErrorMessage = "付款方式不可空白")]
    public string payment_no { get; set; }

    [Display(Name = "運送方式")]
    [Required(ErrorMessage = "運送方式不可空白")]
    public string shipping_no { get; set; }

    [Display(Name = "消費金額")]
    public int amounts { get; set; }

    [Display(Name = "稅額")]
    public int taxs { get; set; }

    [Display(Name = "消費總額(含稅)")]
    public int totals { get; set; }

    [Display(Name = "訂單備註")]
    public string remark { get; set; }

    public List<Payments> PaymentsList { get; set; }
    public List<Shippings> ShippingsList { get; set; }
    public List<Carts> CartsList { get; set; }
}
