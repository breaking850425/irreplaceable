using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;

namespace irreplaceable.Controllers
{
    public class ProgramTypesController : Controller
    {
        //[SecurityAuthorize(SecurityMode = enumSecuritys.Browse)]
        [LoginedAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10 , string searchText = "")
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("資料列表");
            using (tblProgramTypes models = new tblProgramTypes())
            {
                var model = models.GetModelList(0 , page , pageSize , searchText);
                return View(model);
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="id">欄位名稱</param>
        /// <returns></returns>
        [LoginedAuthorize()]
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
        public ActionResult Create()
        {
            ProgramTypes model = new ProgramTypes();
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
        public ActionResult Create(ProgramTypes model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblProgramTypes models = new tblProgramTypes())
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
            using (tblProgramTypes models = new tblProgramTypes())
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
        /// <param name="model">ProgramTypes 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Edit(ProgramTypes model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblProgramTypes models = new tblProgramTypes())
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
            using (tblProgramTypes models = new tblProgramTypes())
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
            return RedirectToAction("Index" , "ProgramTypes" , new { searchText  = str_text });
        }
    }
}
