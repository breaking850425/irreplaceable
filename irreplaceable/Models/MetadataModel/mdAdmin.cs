using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdAdmins))]
    public partial class Admins
    {
        public class mdAdmins 
        {



            [Key]
            public int rowid { get; set; }

            [Display(Name = "管理員驗證")]
            public bool is_validate { get; set; }


            [Display(Name = "管理員編號")]
            [Required(ErrorMessage = "管理員編號不可空白!!")]
            public string admin_no { get; set; }

            [Display(Name = "管理員姓名")]
            [Required(ErrorMessage = "管理員姓名不可空白!!")]
            public string admin_name { get; set; }

            [Display(Name = "管理員密碼")]
            [Required(ErrorMessage = "密碼不可空白!!")]
            [DataType(DataType.Password)]
            public string admin_password { get; set; }

            [Display(Name = "管理員信箱")]
            [Required(ErrorMessage = "電子信箱不可空白!!")]
            [DataType(DataType.EmailAddress)]
            public string admin_email { get; set; }

            [Display(Name = "驗證碼")]
            public string validate_code { get; set; }

            [Display(Name = "備註說明")]
            public string remark { get; set; }
        }
        
    }
}
