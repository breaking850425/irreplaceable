using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace irreplaceable.Models
{
    [MetadataType(typeof(mdPrograms))]
    public partial class Programs
    {
        [Display(Name = "模組名稱")]
        public string module_name { get { using (tblModules model = new tblModules()) { return model.GetModuleName(module_no); } } }
        [Display(Name = "類別名稱")]
        public string program_type_name { get { using (tblProgramTypes model = new tblProgramTypes()) { return model.GetTypeName(program_type_no); } } }

        public List<SelectListItem> ModulesList { get { using (tblModules model = new tblModules()) { return model.GetSelectList(); } } }
        public List<SelectListItem> ProgramTypesList { get { using (tblProgramTypes model = new tblProgramTypes()) { return model.GetSelectList(); } } }
        private class mdPrograms
        {
            [Key]
            public int rowid { get; set; }
            [Display(Name = "啟用")]
            public bool is_enabled { get; set; }
            [Display(Name = "模組代號")]
            [Required(ErrorMessage = "模組代號不可空白!!")]
            public string module_no { get; set; }
            [Display(Name = "程式代號")]
            [Required(ErrorMessage = "程式代號不可空白!!")]
            public string program_no { get; set; }
            [Display(Name = "程式名稱")]
            [Required(ErrorMessage = "程式名稱不可空白!!")]
            public string program_name { get; set; }
            [Display(Name = "類別代號")]
            [Required(ErrorMessage = "類別代號不可空白!!")]
            public string program_type_no { get; set; }
            [Display(Name = "區域")]
            public string area_name { get; set; }
            [Display(Name = "控制器")]
            [Required(ErrorMessage = "控制器不可空白!!")]
            public string controller_name { get; set; }
            [Display(Name = "事件")]
            [Required(ErrorMessage = "事件不可空白!!")]
            public string action_name { get; set; }
            [Display(Name = "參數")]
            public string parameter_value { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}