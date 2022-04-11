using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdProgramTypes))]
    public partial class ProgramTypes
    {
        private class mdProgramTypes
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "分類代號")]
            [Required(ErrorMessage = "分類代號不可空白!!")]
            public string type_no { get; set; }
            [Display(Name = "分類名稱")]
            [Required(ErrorMessage = "分類名稱不可空白!!")]
            public string type_name { get; set; }
            [Display(Name = "圖示名稱")]
            public string icon_name { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}