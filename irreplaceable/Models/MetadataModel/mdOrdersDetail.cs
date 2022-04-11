using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace irreplaceable.Models
{

    [MetadataType(typeof(mdOrdersDetail))]
    public partial class OrdersDetail
    {

        public List<SelectListItem> ProductTypesList { get { using (tblProducts model = new tblProducts()) { return model.GetProductsSelectList(); } } }

      
     

        private class mdOrdersDetail
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "訂單編號")]
            public string order_no { get; set; }

            [Display(Name = "庫存編號")]
            public string vendor_no { get; set; }

            [Display(Name = "類別")]
            public string category_name { get; set; }

            [Display(Name = "商品編號")]
            public string product_no { get; set; }

            [Display(Name = "商品名稱")]
            public string product_name { get; set;  }

            [Display(Name = "商品規格")]
            public string product_spec { get; set; }

            [Display(Name = "價格")]
            public int price { get; set; }

            [Display(Name = "數量")]
            public int qty { get; set; }

            [Display(Name = "小計")]
            public int amount { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}