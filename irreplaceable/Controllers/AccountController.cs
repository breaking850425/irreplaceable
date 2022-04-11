using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;



namespace irreplaceable.Controllers
{
    public class AccountController : Controller
    {


        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {

            SessionService.Logout();
            vmGroupLoginRegister model = new vmGroupLoginRegister();


            return View(model);

        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([Bind(Prefix = "Login")] vmLogin model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Login", "Account");
            using (tblAdmins admins = new tblAdmins())
            {
                using (tblUsers users = new tblUsers())
                {
                    using (tblMembers members = new tblMembers())
                    {
                        using (Cryptographys cryp = new Cryptographys())
                        {
                            //空白密碼加密
                            admins.EncodeEmptyPassword(model.AccountNo, model.Password);
                            members.EncodeEmptyPassword(model.AccountNo, model.Password);
                            users.EncodeEmptyPassword(model.AccountNo, model.Password);


                            //檢查登入角色
                            string str_password = cryp.SHA256Encode(model.Password);
                            if (admins.CheckAccountLogin(model.AccountNo, str_password)) return RedirectToAction("Index", "Admin");
                            if (members.CheckAccountLogin(model.AccountNo, str_password))
                            {
                                //登入時將現有遊客的購物車加入客戶的購物車
                                CartService.LoginCart();
                                return RedirectToAction("Index", "Home");
                            }
                            if (users.CheckAccountLogin(model.AccountNo, str_password)) return RedirectToAction("Index", "Admin");

                            //尚未驗證，產生信箱尚未驗證訊息
                            if (admins.EmailValidateFalse(model.AccountNo, str_password)) return RedirectToAction("ValidateError", "Account");
                            if (members.EmailValidateFalse(model.AccountNo, str_password)) return RedirectToAction("ValidateError", "Account");
                            if (users.EmailValidateFalse(model.AccountNo, str_password)) return RedirectToAction("ValidateError", "Account");

                            //導入網頁並引發一段錯誤訊息
                            return RedirectToAction("LoginError", "Account");
                        }
                    }
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            vmGroupLoginRegister model = new vmGroupLoginRegister();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([Bind(Prefix = "Register")] vmRegister model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Login", "Account");
            using (tblMembers members = new tblMembers())
            {

                string str_validate_code = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20);

                //檢查電子信箱重複註冊
                if (members.CheckAccountRegister(model)) return RedirectToAction("RegisterError", "Account");
                //新增未驗證信箱的會員資料
                else members.AddAccountRegister(model, str_validate_code);


                //寄出電子信箱驗證信
                using (SendMailService sendMail = new SendMailService())
                {
                    sendMail.MemberRegister(str_validate_code);
                }

                //顯示註冊完成並提示收信資訊
                return RedirectToAction("Registered", "Account");

            }

        }




        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginOutToHome()
        {
            SessionService.Logout();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult LoginError()
        {
            vmGroupLoginRegister model = new vmGroupLoginRegister();
            ModelState.AddModelError("Login.Password", "帳號或密碼輸入不正確!!");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RegisterError()
        {
            vmGroupLoginRegister model = new vmGroupLoginRegister();
            ModelState.AddModelError("Register.ContactEmail", "此電子郵件已有註冊紀錄!!");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Registered()
        {
            vmGroupLoginRegister model = new vmGroupLoginRegister();
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ValidateCode(string id)
        {
            ViewBag.MessageText = "";
            if (string.IsNullOrEmpty(id)) { ViewBag.MessageText = "驗證碼空白!!"; return View(); }

            using (tblMembers members = new tblMembers())
            {
                //檢查驗證碼不存在
                if (members.EmptyValidateCode(id)) { ViewBag.MessageText = "驗證碼不存在!!"; return View(); }
                //檢查驗證碼重複              
                if (members.RepeatValidateCode(id)) { ViewBag.MessageText = "會員已驗證，不可重覆驗證!!"; return View(); }
                //修改驗證狀態
                if (members.UpdateValidateCode(id)) { ViewBag.MessageText = "會員電子郵件已驗證成功，您可以至登入頁面進行登入!!"; return View(); }
                //顯示訊息畫面
                return View();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult ValidateError()
        {
            vmGroupLoginRegister model = new vmGroupLoginRegister();
            ModelState.AddModelError("Login.AccountNo", "電子郵件尚未驗證，請至註冊信箱收件並驗證!!");
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Forget()
        {
            vmForget model = new vmForget();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Forget(vmForget model)
        {
            if (!ModelState.IsValid) return View(model);
            string str_validate_code = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 20).ToUpper();
            string str_user_name = "";
            bool bln_exists = false;

            //檢查電子郵件是否存在
            using (tblAdmins admins = new tblAdmins())
            { bln_exists = admins.CheckEmailExists(model.AccountEmail, str_validate_code, out str_user_name); }
            if (!bln_exists)
            {
                using (tblMembers members = new tblMembers())
                { bln_exists = members.CheckEmailExists(model.AccountEmail, str_validate_code, out str_user_name); }
            }
            if (!bln_exists)
            {
                using (tblUsers users = new tblUsers())
                { bln_exists = users.CheckEmailExists(model.AccountEmail, str_validate_code, out str_user_name); }
            }
            if (!bln_exists)
            {
                ModelState.AddModelError("AccountEmail", "電子信箱不存在!!");
                return View(model);
            }

            //寄出電子信箱驗證信
            using (SendMailService sendMail = new SendMailService())
            {
                sendMail.AccountForget(model.AccountEmail, str_validate_code, str_user_name);
            }

            //提示收信資訊
            return RedirectToAction("Forgeted");

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Forgeted()
        {
            ViewBag.MessageText = "您的忘記密碼需求已核准，";
            ViewBag.MessageText += "請至您註冊的電子信箱中收信，並使用新密碼進行會員登入";
            ViewBag.MessageText += "，登入後請立即變更會員密碼，以維護您資訊安全的權益!!";
            return View();
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ValidateForgetCode(string id)
        {
            ViewBag.MessageText = "";
            if (string.IsNullOrEmpty(id)) { ViewBag.MessageText = "驗證碼空白!!"; return View(); }
            string str_password = "";
            using (tblAdmins admins = new tblAdmins()) { str_password = admins.ForgetPasswordReset(id); }
            if (string.IsNullOrEmpty(str_password))
                using (tblMembers members = new tblMembers()) { str_password = members.ForgetPasswordReset(id); }
            if (string.IsNullOrEmpty(str_password))
                using (tblUsers users = new tblUsers()) { str_password = users.ForgetPasswordReset(id); }

            if (!string.IsNullOrEmpty(str_password))
                ViewBag.MessageText = string.Format("您的密碼已重新設定成功，新的密碼為：{0}  (請牢記並登入，登入後請立即變更會員密碼，以維護您資訊安全的權益!!)", str_password);
            else
                ViewBag.MessageText = "密碼已失效，請重新至忘記密碼頁面設定!!";
            //顯示訊息畫面
            return View();
        }



        /// <summary>
        /// 我的帳號
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult AccountProfile()
        {

            using (AccountService account = new AccountService())
            {
                ViewBag.PanelWidth = SessionService.SetPrgInfo("", "我的帳號", "帳號資訊", 6);
                vmAccountProfile model = new vmAccountProfile();
                model = account.GetAccountProfile();
                return View(model);
            }
        }

        /// <summary>
        /// 我的帳號-修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpGet]
        public ActionResult ProfileEdit()
        {
            using (AccountService account = new AccountService())
            {
                ViewBag.PanelWidth = SessionService.SetPrgInfo("", "我的帳號", "修改", 6);
                vmAccountProfile model = new vmAccountProfile();
                model = account.GetAccountProfile();
                return View(model);
            }
        }


        /// <summary>
        /// 我的帳號-修改
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProfileEdit(vmAccountProfile model)
        {
            using (AccountService account = new AccountService())
            {
                account.UpdateAccountProfile(model);
                return RedirectToAction("AccountProfile");
            }
        }


        /// <summary>
        /// 上傳圖像
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult ProfileUpload()
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("", "我的帳號", "上傳圖像", 6);
            return View();
        }

        /// <summary>
        /// 上傳圖像
        /// </summary>
        /// <param name="file">指定上傳的檔案</param>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ProfileUploaded(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    var fileName = SessionService.AccountNo + ".jpg";
                    var path = Path.Combine(Server.MapPath("~/Images/Account"), fileName);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("AccountProfile");
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        public ActionResult ResetPassword()
        {
            ViewBag.PanelWidth = SessionService.SetPrgInfo("", "我的帳號", "變更密碼", 6);
            vmResetPassword model = new vmResetPassword();
            return View(model);
        }

        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult ResetPassword(vmResetPassword model)
        {
            if (!ModelState.IsValid) return View(model);
            using (AccountService account = new AccountService())
            {
                if (!account.ValidAccountPassword(model.OldPassword))
                {
                    ViewBag.PanelWidth = SessionService.SetPrgInfo("", "我的帳號", "變更密碼", 6);

                    ModelState.AddModelError("OldPassword", "現在密碼輸入錯誤!!");
                    return View(model);
                }
                account.ResetAccountPassword(model);
                return RedirectToAction("Index", "Admin");
            }
        }
    }
}