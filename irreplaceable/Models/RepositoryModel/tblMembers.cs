using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;
using PagedList;

public class tblMembers : BaseClass
{
    public IRepository<Members> repo;
    public tblMembers()
    {
        repo = new EFGenericRepository<Members>(new web110b_07Entities());

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
                                 (m.member_no == accountNo || m.member_email == accountNo) && m.member_password == "");

            if (data != null)
            {
                data.member_password = cryp.SHA256Encode(accountNo);
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
                 (m.member_no == accountNo || m.member_email == accountNo) &&
                 m.member_password == passWord &&
                 m.is_validate == true);
        if (data != null)
        {
            SessionService.AccountNo = data.member_no;
            SessionService.AccountName = data.member_name;
            SessionService.RoleNo = "Member";
            SessionService.RoleName = "會員";
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
        var data = repo.ReadSingle(m => (m.member_no == accountNo || m.member_email == accountNo) && m.member_password == passWord && m.is_validate == false);
        if (data != null) return true; return false;
    }


    /// <summary>
    /// 檢查電子信箱重複註冊
    /// </summary>
    /// 
    public bool CheckAccountRegister(vmRegister model)
    {
        var data = repo.ReadSingle(m => m.member_email == model.ContactEmail);
        if (data != null) return true; return false;

    }

    /// <summary>
    /// 註冊-新增未驗證信箱的會員資料
    /// </summary>
    /// 
    public void AddAccountRegister(vmRegister model, string str_validate_code)
    {
        using (Cryptographys cryp = new Cryptographys())
        {


            string str_password = cryp.SHA256Encode(model.Password);

            //新增為驗證信箱的會員資料
            Members newData = new Members();
            newData.member_email = model.ContactEmail;
            newData.member_password = str_password;
            newData.member_name = model.AccountName;
            newData.gender_code = model.GenderCode;
            newData.birth_date = model.Birthday;
            newData.member_phone = model.ContactPhone;
            newData.member_address = model.ContactAddress;
            newData.is_validate = false;
            newData.validate_code = str_validate_code;
            repo.Create(newData);
            repo.SaveChanges();
        }
    }


    /// <summary>
    /// 信箱驗證為正式會員
    /// </summary>

    /// <summary>
    /// 檢查驗證碼不存在
    /// </summary>
    public bool EmptyValidateCode(string id)
    {
        var data = repo.ReadSingle(m => m.validate_code == id);
        if (data == null) return true; return false;
    }

    /// <summary>
    /// 檢查驗證碼重複
    /// </summary>
    public bool RepeatValidateCode(string id)
    {
        var data = repo.ReadSingle(m => m.validate_code == id);
        bool bln_validate = (data.is_validate == null) ? false : data.is_validate.GetValueOrDefault();
        if (bln_validate) return true; return false;
    }

    /// <summary>
    /// 修改驗證狀態
    /// </summary>
    public bool UpdateValidateCode(string id)
    {
        var data = repo.ReadSingle(m => m.validate_code == id);
        bool bln_validate = (data.is_validate == null) ? false : data.is_validate.GetValueOrDefault();
        if (bln_validate == false)
        {
            data.is_validate = true;
            repo.SaveChanges();
            return true;
        }
        return false;
    }


    /// <summary>
    /// 檢查電子郵件是否存在
    /// </summary>
    /// <param name="emailAddress">電子郵件</param>
    /// <param name="validateCode">驗證碼</param>
    /// <param name="userName">名稱</param>
    /// <returns></returns>
    public bool CheckEmailExists(string emailAddress, string validateCode, out string userName)
    {
        var data = repo.ReadSingle(m => m.member_email == emailAddress);
        if (data != null)
        {
            userName = data.member_name;
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
                data.member_password = cryp.SHA256Encode(str_password);
                data.validate_code = "";
                repo.Update(data);
                repo.SaveChanges();
            }
        }
        return str_password;
    }


    /// <summary>
    /// 取得會員 Profile 資訊
    /// </summary>
    /// <returns></returns>
    public vmMembers GetAccountProfile()
    {
        vmMembers model = new vmMembers();
        var data = repo.ReadSingle(m => m.member_no == SessionService.AccountNo);
        if (data != null)
        {
            model.MemberNo = data.member_no;
            model.MemberName = data.member_name;
            model.GenderCode = data.gender_code;
            model.BirthDate = data.birth_date;
            model.MemberEmail = data.member_email;
            model.MemberPhone = data.member_phone;
            model.MemberZip = data.member_zip;
            model.MemberAddress = data.member_address;
        }
        return model;
    }


    /// <summary>
    /// 更新會員 Profile 資訊
    /// </summary>
    /// <param name="model">會員 Profile 資訊</param>
    public void UpdateAccountProfile(vmMembers model)
    {
        var data = repo.ReadSingle(m => m.member_no == SessionService.AccountNo);
        if (data != null)
        {
            data.member_name = model.MemberName;
            data.gender_code = model.GenderCode;
            data.birth_date = model.BirthDate;
            data.member_email = model.MemberEmail;
            data.member_phone = model.MemberPhone;
            data.member_zip = model.MemberZip;
            data.member_address = model.MemberAddress;
            repo.Update(data);
            repo.SaveChanges();
        }
    }

    /// <summary>
    /// 變更會員帳號密碼
    /// </summary>
    /// <param name=""></param>
    public void ResetAccountPassword(vmResetPassword model)
    {
        using (Cryptographys cryp = new Cryptographys())
        {
            string str_password = cryp.SHA256Encode(model.NewPassword);
            var data = repo.ReadSingle(m => m.member_no == SessionService.AccountNo);
            if (data != null)
            {
                data.member_password = str_password;
                repo.Update(data);
                repo.SaveChanges();
            }
        }
    }

    /// <summary>
    /// 檢查會員密碼是否正確
    /// </summary>
    /// <param name="passWord">密碼</param>
    /// <returns></returns>
    public bool ValidAccountPassword(string passWord)
    {
        bool bln_valid = false;
        var data = repo.ReadSingle(m => m.member_no == SessionService.AccountNo);
        if (data != null) { if (data.member_password == passWord) bln_valid = true; }
        return bln_valid;
    }


    /// <summary>
    /// 取得會員資料集
    /// </summary>
    /// <param name="index">陣列索引</param>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    public IPagedList<Members> GetModelList(int index, int page, int pageSize, string searchText)
    {
        var model = repo.ReadAll();
        var dataSort = SessionService.GetColumnSort(index);
        if (!string.IsNullOrEmpty(searchText))
        {
            model = model.Where(m =>
            m.member_no.Contains(searchText) ||
            m.member_name.Contains(searchText) ||
            m.gender_code.Contains(searchText) ||
            m.birth_date.ToString().Contains(searchText) ||
            m.member_email.Contains(searchText) ||
            m.member_phone.Contains(searchText) ||
            m.member_zip.Contains(searchText) ||
            m.member_address.Contains(searchText) ||
            m.remark.Contains(searchText));
        }
        if (model != null)
        {
            if (string.IsNullOrEmpty(dataSort.SortColumn))
            {
                dataSort.SortColumn = "member_no";
                dataSort.SortDirection = enumSortDirection.Desc;
                model.OrderByDescending(m => m.member_no);
            }
            if (dataSort.SortColumn == "is_validate" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.is_validate);
            if (dataSort.SortColumn == "is_validate" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.is_validate);
            if (dataSort.SortColumn == "member_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_no);
            if (dataSort.SortColumn == "member_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_no);
            if (dataSort.SortColumn == "member_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_name);
            if (dataSort.SortColumn == "member_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_name);
            if (dataSort.SortColumn == "gender_code" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.gender_code);
            if (dataSort.SortColumn == "gender_code" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.gender_code);
            if (dataSort.SortColumn == "birth_date" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.birth_date);
            if (dataSort.SortColumn == "birth_date" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.birth_date);
            if (dataSort.SortColumn == "member_email" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_email);
            if (dataSort.SortColumn == "member_email" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_email);
            if (dataSort.SortColumn == "member_phone" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_phone);
            if (dataSort.SortColumn == "member_phone" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_phone);
            if (dataSort.SortColumn == "member_zip" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_zip);
            if (dataSort.SortColumn == "member_zip" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_zip);
            if (dataSort.SortColumn == "member_address" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.member_address);
            if (dataSort.SortColumn == "member_address" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.member_address);

        }

        var datas = model.ToPagedList(page, pageSize);
        SessionService.SetCurrentPage(index, page, searchText, model.ToList().Count, datas.PageCount);
        return datas;
    }


    /// <summary>
    /// 取得會員性別下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetGenderCodeList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "男", Value = "M" },
            new SelectListItem() { Text = "女", Value = "F" }
        };
    }

    /// <summary>
    /// 取得訂購人姓名
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetOrderUserName(string user_no)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.member_no == user_no);
        if (data != null) str_value = data.member_name;
        return str_value;
    }

    /// <summary>
    /// 取得訂購人電話
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetOrderUserPhone(string user_no)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.member_no == user_no);
        if (data != null) str_value = data.member_phone;
        return str_value;
    }

}

