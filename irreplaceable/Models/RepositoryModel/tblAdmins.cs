using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class tblAdmins : BaseClass
{
    public IRepository<Admins> repo;
    public tblAdmins()
    {
        repo = new EFGenericRepository<Admins>(new web110b_07Entities());
    }

    /// <summary>
    /// //空白密碼加密
    /// </summary>
    /// <param name="accountNo">帳號</param>
    /// <param name="passWord">密碼</param>
    public void EncodeEmptyPassword(string accountNo, string passWord)
    {
        using (Cryptographys cryp = new Cryptographys())
        {
            var data = repo.ReadSingle(m =>
                   (m.admin_no == accountNo || m.admin_email == accountNo) && m.admin_password == "");
            if (data != null)
            {
                data.admin_password = cryp.SHA256Encode(accountNo);
                repo.Update(data);
                repo.SaveChanges();
            }
        }
    }

    /// <summary>
    /// 檢查是否帳號登入
    /// </summary>
    /// <param name="accountNo">帳號</param>
    /// <param name="passWord">密碼</param>
    /// <returns></returns>
    public bool CheckAccountLogin(string accountNo, string passWord)
    {
        SessionService.AccountNo = "";
        SessionService.AccountName = "";
        SessionService.RoleNo = "";
        SessionService.RoleName = "";
        SessionService.IsLogined = false;


        var data = repo.ReadSingle(m =>
                 (m.admin_no == accountNo || m.admin_email == accountNo) &&
                 m.admin_password == passWord &&
                 m.is_validate == true);
        if (data != null)
        {
            SessionService.AccountNo = data.admin_no;
            SessionService.AccountName = data.admin_name;
            SessionService.RoleNo = "Admin";
            SessionService.RoleName = "管理者";
            SessionService.IsLogined = true;
            return true;

        } 
            
            
        return false;
    }

    /// <summary>
    /// 尚未驗證，產生信箱尚未驗證訊息
    /// </summary>
    /// 
    public bool EmailValidateFalse(string accountNo, string passWord)
    {
        var data = repo.ReadSingle(m => (m.admin_no == accountNo || m.admin_email == accountNo) && m.admin_password == passWord && m.is_validate == false);
        if (data != null) return true; return false;
    }



    /// <summary>
    /// 檢查電子郵件是否存在
    /// </summary>
    /// <param name="emailAddress">電子郵件</param>
    /// <param name="validateCode">驗證碼</param>
    /// <param name="userName">名稱</param>
    /// <returns></returns>
    public bool CheckEmailExists(string emailAddress, string validateCode , out string userName)
    {
        var data = repo.ReadSingle(m => m.admin_email == emailAddress);
        if (data != null)
        {
            
            userName = data.admin_name;
            data.validate_code = validateCode;
            repo.Update(data);
            repo.SaveChanges();
            return true;
        }
        
        userName = "";
        return false;
    }

    /// <summary>
    /// 依據驗證碼重設新密碼
    /// </summary>
    /// <param name="validateCode"></param>
    /// <returns></returns>
    public string ForgetPasswordReset(string validateCode)
    {
        string str_password = "";
        var data = repo.ReadSingle(m => m.validate_code == validateCode);
        if (data != null)
        {
            using (Cryptographys cryp = new Cryptographys())
            {
                //亂數產生一組8位數的密碼
                str_password = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();
                //寫回資料庫中
                data.admin_password = cryp.SHA256Encode(str_password);
                data.validate_code = "";
                repo.Update(data);
                repo.SaveChanges();
            }
        }
        return str_password;
    }

    /// <summary>
    /// 取得管理者 Profile 資訊
    /// </summary>
    /// <returns></returns>
    public vmAccountProfile GetAccountProfile()
    {
        vmAccountProfile model = new vmAccountProfile();
        var data = repo.ReadSingle(m => m.admin_no == SessionService.AccountNo);
        if (data != null)
        {
            model.AccountNo = data.admin_no;
            model.AccountName = data.admin_name;
            model.ContactEmail = data.admin_email;
        }
        return model;
    }

    /// <summary>
    /// 更新管理者 Profile 資訊
    /// </summary>
    /// <param name="model">管理者 Profile 資訊</param>
    public void UpdateAccountProfile(vmAccountProfile model)
    {
        var data = repo.ReadSingle(m => m.admin_no == SessionService.AccountNo);
        if (data != null)
        {
            data.admin_email = model.ContactEmail;
            repo.Update(data);
            repo.SaveChanges();
        }
    }

   
    /// <summary>
    /// 檢查密碼是否正確
    /// </summary>
    /// <param name="passWord">密碼</param>
    /// <returns></returns>
    public bool ValidAccountPassword(string passWord)
    {
        bool bln_valid = false;
        var data = repo.ReadSingle(m => m.admin_no == SessionService.AccountNo);
        if (data != null) { if (data.admin_password == passWord) bln_valid = true; }
        return bln_valid;
    }

    /// <summary>
    /// 變更帳號密碼
    /// </summary>
    /// <param name=""></param>
    public void ResetAccountPassword(vmResetPassword model)
    {
        using (Cryptographys cryp = new Cryptographys())
        {
            string str_password = cryp.SHA256Encode(model.NewPassword);
            var data = repo.ReadSingle(m => m.admin_no == SessionService.AccountNo);
            if (data != null)
            {
                data.admin_password = str_password;
                repo.Update(data);
                repo.SaveChanges();
            }
        }
    }


}





