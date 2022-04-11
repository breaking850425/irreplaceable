using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdTitles))]
    public partial class Titles
    {
        private class mdTitles
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "職稱代號")]
            [Required(ErrorMessage = "職稱代號不可空白!!")]
            public string title_no { get; set; }
            [Display(Name = "職稱")]
            [Required(ErrorMessage = "職稱不可空白!!")]
            public string title_name { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}