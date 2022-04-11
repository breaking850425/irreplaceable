using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using irreplaceable.Models;


public class SendMailService : BaseClass
{
    ///<summary>
    ///會員註冊寄發驗證的電子郵件
    ///</summary>
    /// <param name="validateCode">驗證碼</param>
    /// <returns></returns>
    public string MemberRegister(string validateCode)
    {

        using (tblMembers members = new tblMembers())
        {
            using (GmailService gmail = new GmailService())
            {
                //驗證
                var memberData = members.repo.ReadSingle(m => m.validate_code == validateCode);
                if (memberData == null) { return "查無驗證碼!!"; }
                bool bln_validate = (memberData.is_validate == null) ? false : memberData.is_validate.GetValueOrDefault();
                if (bln_validate) { return "此驗證碼已通過驗證!!"; }
                if (string.IsNullOrEmpty(memberData.member_email)) { return "此會員未輸入電子信箱!!"; }
                if (string.IsNullOrEmpty(SessionService.WebSiteUrl)) { return "Web.config 未設定 WebSiteUrl 參數!!"; }

                //變數
                string str_member_no = memberData.member_no;
                string str_member_name = memberData.member_name;
                string str_member_email = memberData.member_email;
                string str_reg_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                string str_url = SessionService.WebSiteUrl;
                string str_validate_url = string.Format("{0}/Account/ValidateCode/{1}", str_url, validateCode);
                string str_gender = "";
                if (memberData.gender_code == "F") { str_gender = "小姐"; }
                else { str_gender = "先生"; }




                //接收信件的會員信箱
                gmail.ReceiveEmail = str_member_email;
                //信件主旨
                gmail.Subject = string.Format("{0} 會員註冊驗證通知信", SessionService.CompName);
                //信件內容
                //< !--container-- >
                gmail.Body = "<div style=\"background-color:#f7f7f7; margin:0; padding:30px 0; width:100%; \">";
                gmail.Body += "<table border=\"0\" width =\"100%\" cellspacing =\"0\" cellpadding=\"0\">";
                gmail.Body += "<tbody>";
                gmail.Body += "<tr>";
                gmail.Body += "<td align=\"center\" valign=\"top\">";
                //<!-- LOGO -->
                gmail.Body += "<div>";
                gmail.Body += string.Format("<p style=\"margin-top:0;\"><img src=\"https://i.imgur.com/6cXPHlY.png\" alt=\"{0}\" /</p>",SessionService.CompName);
                gmail.Body += "</div>";
                gmail.Body += "<table style=\"background-color:#ffffff; border: 1px solid #dedede; box-shadow:0 1px 4px rgba(0, 0, 0, 0.1;\" border-radius: 3px; border=\"0\"  width=\"600\" cellspacing=\"0\" cellpadding=\"0\">";
                gmail.Body += "<tbody>";
                gmail.Body += "<tr>";
                gmail.Body += "<td align=\"center\" valign=\"top\">";
                gmail.Body += "</td>";
                gmail.Body += "</tr>";


                //<!-- Body -->
                gmail.Body += "<tr>";
                gmail.Body += "<td align=\"center\" valign=\"top\">";
                gmail.Body += "<table  border=\"0\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\">";
                gmail.Body += "<tbody>";
                gmail.Body += "<tr>";
                gmail.Body += "<td style=\"background-color: #ffffff;\" valign=\"top\">";
                //<!-- Content -->
                gmail.Body += "<table border=\"0\"  width=\"100%\" cellspacing=\"0\" cellpadding=\"20\">";
                gmail.Body += "<tbody>";
                //<!-- Content Header -->
                gmail.Body += "<tr>";
                gmail.Body += "<td style=\"padding:36px 48px; display:block; border: 1px solid #dedede;  border-bottom:0px; background-color:#67a1d4; \">";
                gmail.Body += "<h1 style=\"font-size:30px; font-weight:300; line-height:150%; margin:0px; text-shadow:#85b4dd 0px 1px 0px; color:#ffffff;  text-align:center;\">歡迎加入！</h1>";
                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                //<!-- Content Body -->
                gmail.Body += "<tr>";
                gmail.Body += "<td style=\"padding:25px 15px; border: 1px solid #dedede;\" valign=\"top\"> ";
                gmail.Body += string.Format("敬愛的會員，{0} {1} 您好： <br/><br/>", str_member_name, str_gender);
                gmail.Body += string.Format("歡迎成為{0}的會員<br/>", SessionService.AppName);
                gmail.Body += string.Format("您於 {0} 在我們網站註冊了會員帳號<br/>", str_reg_date);
                gmail.Body += string.Format("您的會員帳號為：{0}<br/><br/>", str_member_no);
                gmail.Body += "請您點擊以下連結進行帳號電子郵件驗證<br/>";
                gmail.Body += string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a><br/><br/>", str_validate_url, str_validate_url);
                gmail.Body += "本信件為系統自動寄出,請勿回覆!!<br/><br/>";
                gmail.Body += "<div style=\"margin-top: 40px; margin-bottom: 10px; padding: 20px 0px;  text-align: center; font-size:12px;\">如果您有任何問題，請<a href=\"mailto:breaking850425@gmail.com\" style=\"color:#3ba1da; text-decoration:none;\">聯繫我們</a></div>";
                gmail.Body += "<div style=\"text-align:center;\">Thank you!</div>";
                gmail.Body += string.Format("<div style=\"text-align:center;\"><a style=\"color:#3ba1da; text-decoration:none;\" href=\"{0}\">The {1} Team</a></div>", str_url, SessionService.AppName);
                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                gmail.Body += "</tbody>";
                gmail.Body += "</table>";
                //< !--End Content-- >

                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                gmail.Body += "</tbody>";
                gmail.Body += "</table>";
                gmail.Body += "</table>";
                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                gmail.Body += "</tbody>";
                gmail.Body += "</table>";
                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                gmail.Body += "</tbody>";
                gmail.Body += "</table>";
                gmail.Body += "</div>";
                //< !--End Body-- >

                //< !--Footer-- >
                gmail.Body += "<table style=\"background-color: #eeeeee;\"  width =\"100%\" cellspacing=\"0\" cellpadding=\"10\" >";
                gmail.Body += "<tbody>";
                gmail.Body += "<tr>";
                gmail.Body += "<td style=\" border:0; color:#8a8a8a; font-size:12px; line-height:150%; text-align:center; padding:24px 0;\" colspan=\"1\" valign=\"middle\" >";
                gmail.Body += "<p>關注我們</p>";
                gmail.Body += string.Format("<p><a href =\"{0}\"><img src=\"https://i.imgur.com/gCYb7RD.png\" alt=\"\"/></a>｜<a href=\"https://www.facebook.com/irreplaceable.handmade.accessory/\"><img src=\"https://i.imgur.com/hAlcvNp.png\" alt=\"\"/></a>｜<a href=\"https://www.instagram.com/irreplaceable.handmade/\"><img src=\"https://i.imgur.com/Xdl7O89.png\" alt =\"\"/></a ></p>", str_url);
                gmail.Body += string.Format("<p style=\"margin: 0 0 16px;\">&copy; {0}<a style=\"color:#67a1d4; font-weight:normal; text-decoration:underline;\" href =\"{1}\"> {2}</a> All Rights Reserved</p>", SessionService.CopyRight, str_url,SessionService.CompName);
                gmail.Body += "<p style=\"margin: 0 0 16px;\"><img style=\"border:none; display:inline-block; font-size:14px; font-weight:bold; height:auto; outline:none; text-decoration:none; text-transform:capitalize; vertical-align:middle; margin-right:10px; max-width:100%;\" src=\"https://i.imgur.com/iKg9Vyi.png\" alt=\"\" width=\"13\" height=\"13\"/><a style=\"color:#67a1d4; font-weight:normal; text-decoration:underline;\" href=\"mailto:breaking850425@gmail.com\"><span style=\"color:red;\"><u>breaking850425@gmail.com</u></span></a></p>";
                gmail.Body += "</td>";
                gmail.Body += "</tr>";
                gmail.Body += "</tbody>";
                gmail.Body += "</table>";
                //<!-- End Footer -->
                gmail.Body += "<p>&nbsp;</p>";

                //寄信
                gmail.Send();

                return gmail.MessageText;

            }

        }

    }



    ///<summary>
    ///會員帳號忘記密碼寄發密碼重設的電子郵件
    ///</summary>
    /// <param name="validateCode">驗證碼</param>
    /// <returns></returns>
    public string AccountForget(string emailAddress, string validateCode, string accountName)
    {

        using (GmailService gmail = new GmailService())
        {

            //驗證
            if (string.IsNullOrEmpty(SessionService.WebSiteUrl)) { return "Web.config 未設定 WebSiteUrl 參數!!"; }

            //變數                  
            string str_reg_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            string str_url = SessionService.WebSiteUrl;
            string str_validate_url = string.Format("{0}/Account/ValidateForgetCode/{1}", str_url, validateCode);
            
            




            //接收信件的會員信箱
            gmail.ReceiveEmail = emailAddress;
            //信件主旨
            gmail.Subject = string.Format("{0} 帳號忘記密碼重新設定通知信", SessionService.CompName);
            //信件內容
            //< !--container-- >
            gmail.Body = "<div style=\"background-color:#f7f7f7; margin:0; padding:30px 0; width:100%; \">";
            gmail.Body += "<table border=\"0\" width =\"100%\" cellspacing =\"0\" cellpadding=\"0\">";
            gmail.Body += "<tbody>";
            gmail.Body += "<tr>";
            gmail.Body += "<td align=\"center\" valign=\"top\">";
            //<!-- LOGO -->
            gmail.Body += "<div>";
            gmail.Body += string.Format("<p style=\"margin-top:0;\"><img src=\"https://i.imgur.com/6cXPHlY.png\" alt=\"{0}\" /</p>",SessionService.CompName);
            gmail.Body += "</div>";
            gmail.Body += "<table style=\"background-color:#ffffff; border: 1px solid #dedede; box-shadow:0 1px 4px rgba(0, 0, 0, 0.1;\" border-radius: 3px; border=\"0\"  width=\"600\" cellspacing=\"0\" cellpadding=\"0\">";
            gmail.Body += "<tbody>";
            gmail.Body += "<tr>";
            gmail.Body += "<td align=\"center\" valign=\"top\">";
            gmail.Body += "</td>";
            gmail.Body += "</tr>";


            //<!-- Body -->
            gmail.Body += "<tr>";
            gmail.Body += "<td align=\"center\" valign=\"top\">";
            gmail.Body += "<table  border=\"0\" width=\"600\" cellspacing=\"0\" cellpadding=\"0\">";
            gmail.Body += "<tbody>";
            gmail.Body += "<tr>";
            gmail.Body += "<td style=\"background-color: #ffffff;\" valign=\"top\">";
            //<!-- Content -->
            gmail.Body += "<table border=\"0\"  width=\"100%\" cellspacing=\"0\" cellpadding=\"20\">";
            gmail.Body += "<tbody>";
            //<!-- Content Header -->
            gmail.Body += "<tr>";
            gmail.Body += "<td style=\"padding:36px 48px; display:block; border: 1px solid #dedede;  border-bottom:0px; background-color:#67a1d4; \">";
            gmail.Body += "<h1 style=\"font-size:30px; font-weight:300; line-height:150%; margin:0px; text-shadow:#85b4dd 0px 1px 0px; color:#ffffff;  text-align:center;\">忘記密碼 - 重新設定</h1>";
            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            //<!-- Content Body -->
            gmail.Body += "<tr>";
            gmail.Body += "<td style=\"padding:25px 15px; border: 1px solid #dedede;\" valign=\"top\"> ";
            gmail.Body += string.Format("敬愛的會員，{0} 您好： <br/><br/>", accountName);
            gmail.Body += string.Format("您於 {0} 在我們網站執行了忘記密碼的功能<br/>", str_reg_date);
            gmail.Body += "請您點擊以下連結進行忘記密碼重新設定<br/>";
            gmail.Body += string.Format("<a href=\"{0}\" target=\"_blank\">{1}</a><br/><br/>", str_validate_url, str_validate_url);
            gmail.Body += "本信件為系統自動寄出,請勿回覆!!<br/><br/>";
            gmail.Body += "<div style=\"margin-top: 40px; margin-bottom: 10px; padding: 20px 0px;  text-align: center; font-size:12px;\">如果您有任何問題，請<a href=\"mailto:breaking850425@gmail.com\" style=\"color:#3ba1da; text-decoration:none;\">聯繫我們</a></div>";
            gmail.Body += "<div style=\"text-align:center;\">Thank you!</div>";
            gmail.Body += string.Format("<div style=\"text-align:center;\"><a style=\"color:#3ba1da; text-decoration:none;\" href=\"{0}\">The {1} Team</a></div>", str_url, SessionService.AppName);
            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            gmail.Body += "</tbody>";
            gmail.Body += "</table>";
            //< !--End Content-- >

            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            gmail.Body += "</tbody>";
            gmail.Body += "</table>";
            gmail.Body += "</table>";
            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            gmail.Body += "</tbody>";
            gmail.Body += "</table>";
            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            gmail.Body += "</tbody>";
            gmail.Body += "</table>";
            gmail.Body += "</div>";
            //< !--End Body-- >

            //< !--Footer-- >
            gmail.Body += "<table style=\"background-color: #eeeeee;\"  width =\"100%\" cellspacing=\"0\" cellpadding=\"10\" >";
            gmail.Body += "<tbody>";
            gmail.Body += "<tr>";
            gmail.Body += "<td style=\" border:0; color:#8a8a8a; font-size:12px; line-height:150%; text-align:center; padding:24px 0;\" colspan=\"1\" valign=\"middle\" >";
            gmail.Body += "<p>關注我們</p>";
            gmail.Body += string.Format("<p><a href =\"{0}\"><img src=\"https://i.imgur.com/gCYb7RD.png\" alt=\"\"/></a>｜<a href=\"https://www.facebook.com/irreplaceable.handmade.accessory/\"><img src=\"https://i.imgur.com/hAlcvNp.png\" alt=\"\"/></a>｜<a href=\"https://www.instagram.com/irreplaceable.handmade/\"><img src=\"https://i.imgur.com/Xdl7O89.png\" alt =\"\"/></a ></p>", str_url);
            gmail.Body += string.Format("<p style=\"margin: 0 0 16px;\">&copy; {0}<a style=\"color:#67a1d4; font-weight:normal; text-decoration:underline;\" href =\"{1}\"> {2}</a> All Rights Reserved</p>", SessionService.CopyRight, str_url,SessionService.CompName);
            gmail.Body += "<p style=\"margin: 0 0 16px;\"><img style=\"border:none; display:inline-block; font-size:14px; font-weight:bold; height:auto; outline:none; text-decoration:none; text-transform:capitalize; vertical-align:middle; margin-right:10px; max-width:100%;\" src=\"https://i.imgur.com/iKg9Vyi.png\" alt=\"\" width=\"13\" height=\"13\"/><a style=\"color:#67a1d4; font-weight:normal; text-decoration:underline;\" href=\"mailto:breaking850425@gmail.com\"><span style=\"color:red;\"><u>breaking850425@gmail.com</u></span></a></p>";
            gmail.Body += "</td>";
            gmail.Body += "</tr>";
            gmail.Body += "</tbody>";
            gmail.Body += "</table>";
            //<!-- End Footer -->
            gmail.Body += "<p>&nbsp;</p>";

            //寄信
            gmail.Send();

            return gmail.MessageText;

        }



    }

    /// <summary>
    /// 訂單已付款通知信
    /// </summary>
    /// <returns></returns>
    public string OrderPayment()
    {
        using (GmailService gmail = new GmailService())
        {
            using (tblOrders orders = new tblOrders())
            {
                using (tblOrdersDetail ordersDetail = new tblOrdersDetail())
                {
                    if (string.IsNullOrEmpty(SessionService.WebSiteUrl)) { return "Web.config 未設定 WebSiteUrl 參數!!"; }
                    var orderModel = orders.repo.ReadSingle(m => m.rowid == ShopService.OrderID);
                    //變數
                    string str_reg_date = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                    string str_url = SessionService.WebSiteUrl;
                    var orderDetailModel = ordersDetail.repo.ReadAll(m => m.order_no == orderModel.order_no);
                    //信件內容
                    gmail.ReceiveEmail = orderModel.receive_email;
                    gmail.Subject = string.Format("{0} 訂單已付款通知信", SessionService.AppName);
                    gmail.Body = string.Format("敬愛的會員 {0} 您好!! <br /><br />", orderModel.receive_name);
                    gmail.Body += string.Format("您於 {0} 在我們網站購買了商品，以下為訂單的資訊，<br />", str_reg_date);
                    gmail.Body += string.Format("訂單日期：{0}<br />", orderModel.order_date.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm"));
                    gmail.Body += string.Format("訂單編號：{0}<br />", orderModel.order_no);
                    gmail.Body += "訂單明細：<br />";
                    gmail.Body += "<table border=\"1\" style=\"border-spacing: 0px; border-collapse: collapse\">";
                    gmail.Body += "<tr>";
                    gmail.Body += "<td>商品編號</td><td>商品名稱</td>";
                    gmail.Body += "<td style=\"text-align: right\">單價</td><td style=\"text-align: right\">數量</td><td style=\"text-align: right\">小計</td>";
                    gmail.Body += "</tr>";
                    foreach (var item in orderDetailModel)
                    {
                        gmail.Body += "<tr>";
                        gmail.Body += string.Format("<td>{0}</td>", item.product_no);
                        gmail.Body += string.Format("<td>{0}</td>", item.product_name);
                        gmail.Body += string.Format("<td style=\"text-align: right\">{0}</td>", item.price);
                        gmail.Body += string.Format("<td style=\"text-align: right\">{0}</td>", item.qty);
                        gmail.Body += string.Format("<td style=\"text-align: right\">{0}</td>", item.amount);
                        gmail.Body += "</tr>";
                    }
                    gmail.Body += "</table><br /><br />";
                    gmail.Body += "本信件為系統自動寄出,請勿回覆!!<br /><br />";
                    gmail.Body += "-------------------------------------------<br />";
                    gmail.Body += string.Format("{0}<br />", SessionService.AppName);
                    gmail.Body += string.Format("{0}<br />", str_url);
                    gmail.Body += "-------------------------------------------<br />";
                    //寄信
                    gmail.Send();
                    return gmail.MessageText;
                }
            }
        }
    }





}
