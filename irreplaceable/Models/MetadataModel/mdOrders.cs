using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdOrders))]
    public partial class Orders
    {

        [Display(Name = "訂購人姓名")]
        public string order_user_name { get { using (tblMembers model = new tblMembers()) { return model.GetOrderUserName(user_no); } } }
        [Display(Name = "訂購人電話")]
        public string order_user_phone { get { using (tblMembers model = new tblMembers()) { return model.GetOrderUserPhone(user_no); } } }
        [Display(Name = "付款方式")]
        public string payment_name { get { using (tblPayments model = new tblPayments()) { return model.GetPaymentName(payment_no); } } }
        [Display(Name = "運送方式")]
        public string shipping_name { get { using (tblShippings model = new tblShippings()) { return model.GetShippingName(shipping_no); } } }

        [Display(Name = "訂單狀態")]
        public string order_status_name { get { using (tblOrderStatus model = new tblOrderStatus()) { return model.GetOrderStatusName(order_status); } } }

        public List<SelectListItem> GetOrderStatusList { get { using (tblOrders model = new tblOrders()) { return model.GetOrderStatusList(); } } }

        public List<SelectListItem> GetPaymentNoList { get { using (tblPayments model = new tblPayments()) { return model.GetPaymentNoList(); } } }

        public List<SelectListItem> GetShippingNoList { get { using (tblShippings model = new tblShippings()) { return model.GetShippingNoList(); } } }

        private class mdOrders
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "訂單編號")]
            public string order_no { get; set; }

            [Display(Name = "訂單日期")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime order_date { get; set; }

            [Display(Name = "訂單狀態")]
            public string order_status { get; set; }

            [Display(Name = "會員編號")]
            public string user_no { get; set; }

            [Display(Name = "付款編號")]
            public string payment_no { get; set; }

            [Display(Name = "運送編號")]
            public string shipping_no { get; set; }

            [Display(Name = "收件人姓名")]
            public string receive_name { get; set; }

            [Display(Name = "收件人電話")]
            public string receive_phone { get; set; }

            [Display(Name = "收件人信箱")]
            public string receive_email { get; set; }

            [Display(Name = "收件人地址")]
            public string receive_address { get; set; }

            [Display(Name = "消費金額")]
            public int amounts { get; set; }

            [Display(Name = "稅額")]
            public int taxs { get; set; }

            [Display(Name = "消費總額(含稅)")]
            public int totals { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

       
            public string order_guid { get; set; }

       
            public Nullable<int> order_closed { get; set; }

      
            public Nullable<int> order_validate { get; set; }
        }
    }
}