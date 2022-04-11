using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdModules))]
    public partial class Modules
    {
        private class mdModules
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "啟用")]
            public bool is_enabled { get; set; }
            [Display(Name = "模組代號")]
            [Required(ErrorMessage = "模組代號不可空白!!")]
            public string module_no { get; set; }
            [Display(Name = "模組名稱")]
            [Required(ErrorMessage = "模組名稱不可空白!!")]
            public string module_name { get; set; }
            [Display(Name = "模組圖示")]
            public string icon_name { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}