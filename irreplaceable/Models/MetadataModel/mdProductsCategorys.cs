using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdProductsCategorys))]
    public partial class ProductsCategorys
    {
        public int product_counts { get; set; }

        private class mdProductsCategorys
        {     
            [Key]
            public int rowid { get; set; }
            public bool is_enabled { get; set; }
            public string category_parent_no { get; set; }
            public string category_no { get; set; }
            public string category_name { get; set; }
            public string remark { get; set; }
        }

    }
}


