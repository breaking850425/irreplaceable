using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace irreplaceable.Models
{

    [MetadataType(typeof(mdMembers))]
    public partial class Members
    {


        public List<SelectListItem> GetGenderCodeList { get { using (tblMembers model = new tblMembers()) { return model.GetGenderCodeList(); } } }

        private class mdMembers
        {
            [Key]
            public int rowid { get; set; }

            [Display(Name = "會員啟用狀態")]
            public bool is_validate { get; set; }

            [Display(Name = "會員編號")]
            public string member_no { get; set; }

            [Display(Name = "會員姓名")]
            public string member_name { get; set; }

            [Display(Name = "會員密碼")]      
            public string member_password { get; set; }

            [Display(Name = "性別")]
            public string gender_code { get; set; }

            [Display(Name = "生日")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
            public Nullable<System.DateTime> birth_date { get; set; }

            [Display(Name = "電子信箱")]
            [DataType(DataType.EmailAddress)]
            public string member_email { get; set; }

            [Display(Name = "電話")]
            public string member_phone { get; set; }

            [Display(Name = "郵遞區號")]
            public string member_zip { get; set; }

            [Display(Name = "通訊地址")]
            public string member_address { get; set; }

            [Display(Name = "驗證碼")]
            public string validate_code { get; set; }

            [Display(Name = "備註")]
            public string remark { get; set; }

   
        }



    }
}
