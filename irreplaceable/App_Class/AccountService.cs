using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class AccountService : BaseClass
{

    /// <summary>
    /// 取得 vmAccountProfile 函數
    /// </summary>
    /// <returns></returns>
    public vmAccountProfile GetAccountProfile()
    {
        vmAccountProfile model = new vmAccountProfile();
        if (SessionService.RoleNo == "Admin") { using (tblAdmins admins = new tblAdmins()) { model = admins.GetAccountProfile(); } }
        if (SessionService.RoleNo == "User") { using (tblUsers users = new tblUsers()) { model = users.GetAccountProfile(); } }
        return model;
    }

    /// <summary>
    /// 取得 vmMembers  GetAccountProfile函數
    /// </summary>
    /// <returns></returns>
    public vmMembers GetMemberAccountProfile()
    {
        vmMembers model = new vmMembers();
        if (SessionService.RoleNo == "Member") { using (tblMembers members = new tblMembers()) { model = members.GetAccountProfile(); } }
        return model;
    }

    /// <summary>
    /// 更新帳號相關資料
    /// </summary>
    /// <param name="model"></param>
    public void UpdateAccountProfile(vmAccountProfile model)
    {
        if (SessionService.RoleNo == "Admin") { using (tblAdmins admins = new tblAdmins()) { admins.UpdateAccountProfile(model); } }
        if (SessionService.RoleNo == "User") { using (tblUsers users = new tblUsers()) { users.UpdateAccountProfile(model); } }
    }

    /// <summary>
    /// 更新會員帳號相關資料
    /// </summary>
    /// <param name="model"></param>
    public void UpdateMemberAccountProfile(vmMembers model)
    {
        if (SessionService.RoleNo == "Member") { using (tblMembers members = new tblMembers()) { members.UpdateAccountProfile(model); } }
    }

    /// <summary>
    /// 檢查密碼是否正確
    /// </summary>
    /// <param name="passWord">密碼</param>
    /// <returns></returns>
    public bool ValidAccountPassword(string passWord)
    {
        using (Cryptographys cryp = new Cryptographys())
        {
            bool bln_valid = false;
            string str_password = cryp.SHA256Encode(passWord);
            if (SessionService.RoleNo == "Admin") { using (tblAdmins admins = new tblAdmins()) { bln_valid = admins.ValidAccountPassword(str_password); } }
            if (SessionService.RoleNo == "User") { using (tblUsers users = new tblUsers()) { bln_valid = users.ValidAccountPassword(str_password); } }
            if (SessionService.RoleNo == "Member") { using (tblMembers members = new tblMembers()) { bln_valid = members.ValidAccountPassword(str_password); } }

            return bln_valid;
        }
    }


    /// <summary>
    /// 變更帳號密碼
    /// </summary>
    /// <param name=""></param>
    public void ResetAccountPassword(vmResetPassword model)
    {
        if (SessionService.RoleNo == "Admin") { using (tblAdmins admins = new tblAdmins()) { admins.ResetAccountPassword(model); } }
        if (SessionService.RoleNo == "User") { using (tblUsers users = new tblUsers()) { users.ResetAccountPassword(model); } }
        if (SessionService.RoleNo == "Member") { using (tblMembers members = new tblMembers()) { members.ResetAccountPassword(model); } }

    }




    /// <summary>
    /// 取得帳號的模組列表
    /// </summary>
    /// <returns></returns>
    public static List<Modules> GetAccountModules()
    {
        using (tblSecuritys securitys = new tblSecuritys())
        {
            return securitys.GetAccountModules();
        }
    }
    /// <summary>
    /// 取得帳號的模組程式列表
    /// </summary>
    /// <returns></returns>
    public static List<Programs> GetAccountPrograms(string module_no)
    {
        using (tblSecuritys securitys = new tblSecuritys())
        {
            return securitys.GetAccountPrograms(module_no);
        }
    }





 


}