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
    public class MembersController : Controller
    {
        //[SecurityAuthorize(SecurityMode = enumSecuritys.Browse)]
        [LoginedAuthorize()]
        public ActionResult Index(int page = 1, int pageSize = 10, string searchText = "")
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("資料列表");
            using (tblMembers models = new tblMembers())
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
        public ActionResult Create()
        {
            Members model = new Members();
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
        public ActionResult Create(Members model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblMembers models = new tblMembers())
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
            using (tblMembers models = new tblMembers())
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
        /// <param name="model">Members 資料物件</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Edit(Members model)
        {
            if (!ModelState.IsValid) return View(model);
            using (tblMembers models = new tblMembers())
            {
                using (Cryptographys cryp = new Cryptographys())
                {

                    model.member_password = cryp.SHA256Encode(model.member_password);
                    models.repo.Update(model);
                    models.repo.SaveChanges();
                    return RedirectToAction("Index");
                }
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
            using (tblMembers models = new tblMembers())
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
            return RedirectToAction("Index", "Members", new { searchText = str_text });
        }

        /// <summary>
        /// 全選不選
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Index(FormCollection model)
        {
            if (model != null)
            {
                using (ApplicationService app = new ApplicationService())
                {
                    using (tblMembers modules = new tblMembers())
                    {
                        int int_rowid = 0;
                        string str_module_no = "";
                        bool bln_enabled = false;
                        List<string> rowidList = model["item.rowid"].Split(',').ToList();
                        List<string> noList = model["item.member_no"].Split(',').ToList();
                        List<bool> enabledList = app.GetCheckBoxListValue(model["item.is_validate"]);
                        for (int i = 0; i < rowidList.Count; i++)
                        {
                            int_rowid = int.Parse(rowidList[i]);
                            str_module_no = noList[i];
                            bln_enabled = enabledList[i];
                            var data = modules.repo.ReadSingle(m => m.rowid == int_rowid);
                            if (data != null)
                            {
                                data.is_validate = bln_enabled;
                                data.member_name = (data.member_name == null) ? "無" : data.member_name;
                                modules.repo.Update(data);
                                modules.repo.SaveChanges();
                            }
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 會員帳號管理頁面
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult Centre()
        {
            using (AccountService account = new AccountService())
            {
                using (tblOrders orders = new tblOrders())
                {

                    using (tblOrdersDetail detail = new tblOrdersDetail())
                    {
                        vmMemberCentre model = new vmMemberCentre();
                        model.Members = account.GetMemberAccountProfile();
                        model.Orders = orders.repo.ReadAll(m => m.user_no == @SessionService.AccountNo).OrderByDescending(m => m.order_no).ToList();


                        if (model.OrderSingle == null)
                        {
                            SessionService.OrderNo = null;
                        }

                        return View(model);

                    }
                }

            }
        }

        /// <summary>
        /// 我的帳號-個人資料-修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Info([Bind(Prefix = "Members")] vmMembers model)
        {
            if (!ModelState.IsValid) return View(model);
            using (AccountService account = new AccountService())
            {
                account.UpdateMemberAccountProfile(model);
                return RedirectToAction("Centre");
            }
        }


        /// <summary>
        /// 變更密碼頁面
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult ResetPassword()
        {
            using (AccountService account = new AccountService())
            {
                using (tblOrders orders = new tblOrders())
                {

                    using (tblOrdersDetail detail = new tblOrdersDetail())
                    {
                        vmMemberCentre model = new vmMemberCentre();
                        model.Members = account.GetMemberAccountProfile();
                        model.Orders = orders.repo.ReadAll(m => m.user_no == @SessionService.AccountNo).ToList();

                        return View(model);
                    }
                }
            }
        }


        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ResetPassword(vmMemberCentre model)
        {
            if (!ModelState.IsValid) return View(model);
            using (AccountService account = new AccountService())
            {

                using (tblOrders orders = new tblOrders())
                {

                    using (tblOrdersDetail detail = new tblOrdersDetail())
                    {
                        if (!account.ValidAccountPassword(model.ResetPassword.OldPassword))
                        {
                            ModelState.AddModelError("ResetPassword.OldPassword", "現在密碼輸入錯誤!!");
                            model.Members = account.GetMemberAccountProfile();
                            model.Orders = orders.repo.ReadAll(m => m.user_no == @SessionService.AccountNo).OrderByDescending(m => m.order_no).ToList();
                            return View(model);
                        }
                        account.ResetAccountPassword(model.ResetPassword);
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
        }



        /// <summary>
        /// 會員訂單頁面
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult Orders()
        {
            using (AccountService account = new AccountService())
            {
                using (tblOrders orders = new tblOrders())
                {

                    using (tblOrdersDetail detail = new tblOrdersDetail())
                    {
                        vmMemberCentre model = new vmMemberCentre();
                        model.Members = account.GetMemberAccountProfile();
                        model.Orders = orders.repo.ReadAll(m => m.user_no == @SessionService.AccountNo).OrderByDescending(m => m.order_no).ToList();

                        return View(model);
                    }
                }
            }
        }




        /// <summary>
        /// 會員訂單明細
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult OrdersDetail(string id)
        {
            using (AccountService account = new AccountService())
            {
                using (tblOrders orders = new tblOrders())
                {

                    using (tblOrdersDetail detail = new tblOrdersDetail())
                    {

                        SessionService.FormID = id;
                        var data = orders.repo.ReadSingle(m => m.order_no == id);
                        vmMemberCentre model = new vmMemberCentre();
                        model.Members = account.GetMemberAccountProfile();
                        model.OrderSingle = data;
                        model.OrdersDetail = detail.repo.ReadAll(m => m.order_no == model.OrderSingle.order_no).ToList();
                        return View(model);

                    }
                }

            }
        }









        /// <summary>
        /// 會員訂單查詢
        /// </summary>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult OrderSearch()
        //{
        //    string str_text = Request.Form["search_text"].ToString();
        //    return RedirectToAction("Orders", "Members", new { searchText = str_text });
        //}







    }
}
