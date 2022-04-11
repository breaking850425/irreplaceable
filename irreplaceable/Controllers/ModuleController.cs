using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;
using PagedList;

namespace bookstore.Controllers
{
    public class ModuleController : Controller
    {

        [LoginedAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10, string searchText = "")
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("資料列表");
            using (tblModules models = new tblModules())
            {
                var model = models.GetModelList(0, page, pageSize, searchText);
                return View(model);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">欄位名稱</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ColumnSort(string id)
        {
            int int_index = 0;
            var sort = SessionService.GetColumnSort(int_index);
            if (sort.SortColumn == id)
            {
                if (sort.SortDirection == enumSortDirection.Asc)
                    SessionService.SetColumnSort(int_index, sort.Page, id, enumSortDirection.Desc);
                else
                    SessionService.SetColumnSort(int_index, sort.Page, id, enumSortDirection.Asc);
            }
            else
            {
                SessionService.SetColumnSort(int_index, sort.Page, id, enumSortDirection.Asc);
            }
            var sortData = SessionService.GetColumnSort(int_index);
            return RedirectToAction("Index", new { page = sortData.Page });
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Add()
        {
            Modules model = new Modules();
            ViewBag.PanelWidth = SessionService.SetPrgInfo("新增", 6);
            return View(model);
        }

        /// <summary>
        /// 新增存檔
        /// </summary>
        /// <param name="model">Module 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Add(Modules model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblModules models = new tblModules())
            {
                models.repo.Create(model);
                models.repo.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            using (tblModules models = new tblModules())
            {
                var model = models.repo.ReadSingle(m => m.rowid == id);
                if (model == null) return RedirectToAction("Index");
                ViewBag.PanelWidth = SessionService.SetPrgInfo("修改", 6);
                return View(model);
            }
        }

        /// <summary>
        /// 修改存檔
        /// </summary>
        /// <param name="model">Module 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Edit(Modules model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblModules models = new tblModules())
            {
                models.repo.Update(model);
                models.repo.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id">記錄的 ID</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (tblModules models = new tblModules())
            {
                var model = models.repo.ReadSingle(m => m.rowid == id);
                if (model != null)
                {
                    models.repo.Delete(model);
                    models.repo.SaveChanges();
                }
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Search()
        {
            string str_text = Request.Form["search_text"].ToString();
            return RedirectToAction("Index", "Module", new { searchText = str_text });
        }


        /// <summary>
        /// 全選不選
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Index(FormCollection model)
        {
            if (model != null)
            {
                using (ApplicationService app = new ApplicationService())
                {
                    using (tblModules modules = new tblModules())
                    {
                        int int_rowid = 0;
                        string str_module_no = "";
                        bool bln_enabled = false;
                        List<string> rowidList = model["item.rowid"].Split(',').ToList();
                        List<string> noList = model["item.module_no"].Split(',').ToList();
                        List<bool> enabledList = app.GetCheckBoxListValue(model["item.is_enabled"]);
                        for (int i = 0; i < rowidList.Count; i++)
                        {
                            int_rowid = int.Parse(rowidList[i]);
                            str_module_no = noList[i];
                            bln_enabled = enabledList[i];
                            var data = modules.repo.ReadSingle(m => m.rowid == int_rowid);
                            if (data != null)
                            {
                                data.is_enabled = bln_enabled;
                                data.module_name = (data.module_name == null) ? "無" : data.module_name;
                                modules.repo.Update(data);
                                modules.repo.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}