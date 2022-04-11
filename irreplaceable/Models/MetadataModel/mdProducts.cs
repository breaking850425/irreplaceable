using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdProducts))]
    public partial class Products
    {
        [Display(Name = "商品圖片")]
        public string product_img { get; set; }

        public List<SelectListItem> GetCategoryList { get { using (tblProductsCategorys products = new tblProductsCategorys()) { return products.GetCategoryList(); } } }


        private class mdProducts
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "商品編號")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            public string product_name { get; set; }
            [Display(Name = "分類")]
            public string category_no { get; set; }
            [Display(Name = "銷售單價")]
            public int price { get; set; }
            [Display(Name = "商品摘要")]
            public string content_text { get; set; }
            [Display(Name = "商品尺寸")]
            public string product_length { get; set; }
            [Display(Name = "商品材質")]
            public string product_material { get; set; }
            [Display(Name = "商品詳細說明")]
            public string detail_text { get; set; }
            [Display(Name = "銷售數量")]
            public int sold_qty { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }

}


