using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;
using System.IO;



namespace irreplaceable.Controllers
{
    public class ProductsController : Controller
    {
        //[SecurityAuthorize(SecurityMode = enumSecuritys.Browse)]
        [LoginedAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10, string searchText = "")
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("資料列表");
            using (tblProducts models = new tblProducts())
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
        /// 新增
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Create()
        {
            using (tblProducts products = new tblProducts()) {
                var datas = products.repo.ReadAll();
                var data = datas.OrderByDescending(m => m.rowid).FirstOrDefault();
                int number_length = 3;
                SessionService.ProductID = (data.rowid + 1 ).ToString();
                

                string str_number = data.product_no.Substring(data.product_no.Length- number_length, number_length);
                int int_number = 0;
                int.TryParse(str_number, out int_number);
                str_number = "000"+(int_number + 1).ToString();
                string new_product_no = "P" + str_number.Substring(str_number.Length - number_length, number_length); ;

                SessionService.ProductNo = new_product_no;

                Products model = new Products();
                ViewBag.PanelWidth = SessionService.SetPrgInfo("新增", 6);
                return View(model);

            }
          
        }

        /// <summary>
        /// 新增存檔
        /// </summary>
        /// <param name="model">Module 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Create(Products model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblProducts models = new tblProducts())
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
            using (tblProducts models = new tblProducts())
            {                    
                var model = models.repo.ReadSingle(m => m.rowid == id);               
                if (model == null) return RedirectToAction("Create");
                SessionService.ProductNo = model.product_no;
                SessionService.ProductID = model.rowid.ToString();
                ViewBag.PanelWidth = SessionService.SetPrgInfo("修改", 6);
                return View(model);
            }
        }

        /// <summary>
        /// 修改存檔
        /// </summary>
        /// <param name="model">Products 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Edit(Products model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblProducts models = new tblProducts())
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
            using (tblProducts models = new tblProducts())
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
            return RedirectToAction("Index", "Products", new { searchText = str_text });
        }




        /// <summary>
        /// 上傳多組圖像
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        //[LoginedAuthorize()]
        //[HttpPost]
        //public ActionResult MulProductImgUploaded(IEnumerable<HttpPostedFileBase> files)
        //{
        //    var i = 0;

        //    foreach (var file in files)
        //    {


        //        if (file != null)
        //        {

        //            if (file.ContentLength > 0)
        //            {

        //                if (i == 0)
        //                {

        //                    var fileName = SessionService.ProductNo + ".jpg";
        //                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
        //                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        //                    file.SaveAs(path);
        //                    i += 1;

        //                }
        //                else
        //                {
        //                    var fileName = SessionService.ProductNo + "-" + i + ".jpg";
        //                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
        //                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        //                    file.SaveAs(path);
        //                    i += 1;

        //                }


        //            }
        //        }
        //    }


        //    int int_product_id = 0;
        //    int.TryParse(SessionService.ProductID, out int_product_id);
        //    int productid = int_product_id;
        //    return RedirectToAction("Edit", "Products", new { id = productid });


        //}

        /// <summary>
        /// 上傳圖像0
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        //[LoginedAuthorize()]
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProductImgUploaded(HttpPostedFileBase file)
        {

            if (file != null)
            {
                if (file.ContentLength > 0)
                {

                    var fileName = SessionService.ProductNo + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }

            int int_product_id = 0;
            int.TryParse(SessionService.ProductID, out int_product_id);
            int productid = int_product_id;

            return RedirectToAction("Edit", "Products", new { id = productid });


        }


        /// <summary>
        /// 上傳圖像1
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        //[LoginedAuthorize()]
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProductImg1Uploaded(HttpPostedFileBase file1)
        {

            if (file1 != null)
            {
                if (file1.ContentLength > 0)
                {

                    var fileName = SessionService.ProductNo + "-1.jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file1.SaveAs(path);
                }
            }

            int int_product_id = 0;
            int.TryParse(SessionService.ProductID, out int_product_id);
            int productid = int_product_id;
            return RedirectToAction("Edit", "Products", new { id = productid });


        }


        /// <summary>
        /// 上傳圖像2
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProductImg2Uploaded(HttpPostedFileBase file2)
        {

            if (file2 != null)
            {
                if (file2.ContentLength > 0)
                {

                    var fileName = SessionService.ProductNo + "-2.jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file2.SaveAs(path);
                }
            }

            int int_product_id = 0;
            int.TryParse(SessionService.ProductID, out int_product_id);
            int productid = int_product_id;
            return RedirectToAction("Edit", "Products", new { id = productid });


        }


        /// <summary>
        /// 上傳圖像3
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProductImg3Uploaded(HttpPostedFileBase file3)
        {

            if (file3 != null)
            {
                if (file3.ContentLength > 0)
                {

                    var fileName = SessionService.ProductNo + "-3.jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file3.SaveAs(path);
                }
            }

            int int_product_id = 0;
            int.TryParse(SessionService.ProductID, out int_product_id);
            int productid = int_product_id;
            return RedirectToAction("Edit", "Products", new { id = productid });


        }


        /// <summary>
        /// 上傳圖像4
        /// </summary>
        /// <param name="files">指定上傳的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProductImg4Uploaded(HttpPostedFileBase file4)
        {

            if (file4 != null)
            {
                if (file4.ContentLength > 0)
                {

                    var fileName = SessionService.ProductNo + "-4.jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file4.SaveAs(path);
                }
            }

            int int_product_id = 0;
            int.TryParse(SessionService.ProductID, out int_product_id);
            int productid = int_product_id;
            return RedirectToAction("Edit", "Products", new { id = productid });


        }


        /// <summary>
        /// 刪除圖像
        /// </summary>
        /// <param name="files">指定刪除的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult ProductImgDelFile(int id)
        {
            using (tblProducts model = new tblProducts())
            {
                var data = model.repo.ReadSingle(m => m.product_no == SessionService.ProductNo);
                string product_id;
                if (id == 0)
                {
                    product_id = SessionService.ProductNo;

                }
                else
                {
                    product_id = SessionService.ProductNo + "-" + id;
                }


                string fileName = string.Format("{0}.jpg", product_id);
                var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                System.IO.FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                return RedirectToAction("Edit", "Products", new { id = data.rowid });
            }


        }


        /// <summary>
        /// 新增頁-刪除圖像
        /// </summary>
        /// <param name="files">指定刪除的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult ProductCreateImgDelFile(int id)
        {
            using (tblProducts model = new tblProducts())
            {
                var data = model.repo.ReadSingle(m => m.product_no == SessionService.ProductNo);
                string product_id;
                if (id == 0)
                {
                    product_id = SessionService.ProductNo;

                }
                else
                {
                    product_id = SessionService.ProductNo + "-" + id;
                }


                string fileName = string.Format("{0}.jpg", product_id);
                var path = Path.Combine(Server.MapPath("~/Images/Products"), fileName);
                System.IO.FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }

                return RedirectToAction("Create", "Products");
            }


        }


    }



}
