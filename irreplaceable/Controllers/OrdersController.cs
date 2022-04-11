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
    public class OrdersController : Controller
    {
        //[SecurityAuthorize(SecurityMode = enumSecuritys.Browse)]
        [LoginedAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10, string searchText = "")
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("資料列表");
            using (tblOrders models = new tblOrders())
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
        /// 明細
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Detail(string id)
        {
            using (tblOrders master = new tblOrders())
            {
                using (tblOrdersDetail detail = new tblOrdersDetail())
                {
                    SessionService.FormID = id;
                    var data = master.repo.ReadSingle(m => m.order_no == id);
                    SessionService.OrderNo = data.order_no;
                    vmOrderDetail model = new vmOrderDetail();
                    model.orders = master.repo.ReadSingle(m => m.order_no == id);
                    model.ordersDetail = detail.repo.ReadAll(m => m.order_no == @SessionService.OrderNo).ToList();
                    return View(model);
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Create()
        {
            Orders model = new Orders();
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
        public ActionResult Create(Orders model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblOrders models = new tblOrders())
            {
                models.repo.Create(model);
                models.repo.SaveChanges();

            }
            using (tblOrders modelsed = new tblOrders())
            {
                var data = modelsed.repo.ReadAll().OrderByDescending(m => m.rowid).Take(1);
                if (data == null) return RedirectToAction("Index");
                SessionService.FormID = data.FirstOrDefault().order_no;
                return RedirectToAction("Detail", "Orders", new { id = SessionService.FormID });

            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            using (tblOrders models = new tblOrders())
            {
                SessionService.FormID = id;
                var model = models.repo.ReadSingle(m => m.order_no == id);
                if (model == null) return RedirectToAction("Index");
                ViewBag.PanelWidth = SessionService.SetPrgInfo("修改", 6);
                return View(model);
            }
        }

        /// <summary>
        /// 修改存檔
        /// </summary>
        /// <param name="model">Orders 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Edit(Orders model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblOrders models = new tblOrders())
            {
                models.repo.Update(model);
                models.repo.SaveChanges();
                return RedirectToAction("Detail", "Orders", new { id = model.order_no });
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
            using (tblOrders models = new tblOrders())
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
            return RedirectToAction("Index", "Orders", new { searchText = str_text });
        }






        /// <summary>
        /// 明細新增
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult DetailCreate()
        {
            OrdersDetail model = new OrdersDetail();
            ViewBag.PanelWidth = SessionService.SetPrgInfo("新增", 6);
            return View(model);
        }

        /// <summary>
        /// 明細新增存檔
        /// </summary>
        /// <param name="model">OrdersDetail 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult DetailCreate(OrdersDetail model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblOrdersDetail models = new tblOrdersDetail())
            {
                using (tblProducts products = new tblProducts())
                {
                    model.order_no = SessionService.FormID;
                    model.product_name = products.GetProductName(model.product_no);
                    models.repo.Create(model);
                    models.repo.SaveChanges();
                    return RedirectToAction("Detail", "Orders", new { id = SessionService.FormID });

                }


            }


        }

        /// <summary>
        /// 明細修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult DetailEdit(int id)
        {
            using (tblOrdersDetail models = new tblOrdersDetail())
            {
                var model = models.repo.ReadSingle(m => m.rowid == id);
                if (model == null) return RedirectToAction("Index");
                ViewBag.PanelWidth = SessionService.SetPrgInfo("修改", 6);
                return View(model);
            }
        }

        /// <summary>
        /// 明細修改存檔
        /// </summary>
        /// <param name="model">OrdersDetail 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult DetailEdit(OrdersDetail model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblOrdersDetail models = new tblOrdersDetail())
            {
                model.amount = model.price * model.qty;
                models.repo.Update(model);
                models.repo.SaveChanges();
                return RedirectToAction("Detail", "Orders", new { id = model.order_no });
            }
        }

        /// <summary>
        /// 明細刪除
        /// </summary>
        /// <param name="id">記錄的 ID</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult DetailDelete(int id)
        {
            using (tblOrdersDetail models = new tblOrdersDetail())
            {
                var model = models.repo.ReadSingle(m => m.rowid == id);
                if (model != null)
                {
                    models.repo.Delete(model);
                    models.repo.SaveChanges();
                }
                return RedirectToAction("Detail", "Orders", new { id = SessionService.FormID });
            }
        }
    }
}

