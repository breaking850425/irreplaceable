using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class tblSecuritys : BaseClass
{
    public IRepository<Securitys> repo;
    public tblSecuritys()
    {
        repo = new EFGenericRepository<Securitys>(new web110b_07Entities());
    }
    /// <summary>
    /// 取得帳號的模組列表
    /// </summary>
    /// <returns></returns>
    public List<Modules> GetAccountModules()
    {
        using (tblModules modules = new tblModules())
        {
            List<Modules> moduleList = new List<Modules>();
            var datas = repo.ReadAll(m => m.account_no == SessionService.AccountNo)
                .Select(m => m.module_no).Distinct().ToList();
            foreach (string str_module_no in datas)
            {
                var data = modules.repo.ReadSingle(m => m.module_no == str_module_no && m.is_enabled.ToString() == "True");
                if (data != null)
                {
                    if (string.IsNullOrEmpty(data.icon_name))
                        data.icon_name = "nav-icon fas fa-copy";
                    else
                        data.icon_name = string.Format("nav-icon {0}", data.icon_name);
                    moduleList.Add(data);
                }
            }
            return moduleList;
        }
    }
    /// <summary>
    /// 取得帳號的模組程式列表
    /// </summary>
    /// <returns></returns>
    public List<Programs> GetAccountPrograms(string module_no)
    {
        using (tblPrograms programs = new tblPrograms())
        {
            List<Programs> programList = new List<Programs>();
            var datas = repo.
                ReadAll(m => m.account_no == SessionService.AccountNo && m.module_no == module_no)
               .OrderBy(m => m.program_no).ToList();
            foreach (var data in datas)
            {
                var prgData = programs.repo.ReadSingle(m => m.program_no == data.program_no && m.is_enabled.ToString() == "True");
                if (prgData != null)
                {
                    if (string.IsNullOrEmpty(prgData.program_type_no))
                        prgData.program_type_no = "nav-icon far fa-circle";
                    else
                    {
                        string str_icon_name = programs.GetProgramIconName(prgData.program_no);
                        if (string.IsNullOrEmpty(str_icon_name))
                            prgData.program_type_no = "nav-icon far fa-circle";
                        else
                            prgData.program_type_no = string.Format("nav-icon {0}", str_icon_name);
                    }
                    programList.Add(prgData);
                }
            }
            return programList;
        }
    }
    /// <summary>
    /// 取得程式權限
    /// </summary>
    /// <param name="prgNo">程式代號</param>
    public AccountSecurity GetProgramSecurity(string prgNo)
    {
        AccountSecurity securitys = new AccountSecurity()
        { Add = false, Edit = false, Delete = false, Print = false, Upload = false, Download = false, Confirm = false, Undo = false };
        var data = repo.ReadSingle(m => m.program_no == prgNo);
        if (data != null)
        {
            securitys.Add = data.is_add;
            securitys.Edit = data.is_edit;
            securitys.Delete = data.is_delete;
            securitys.Print = data.is_print;
            securitys.Upload = data.is_upload;
            securitys.Download = data.is_download;
            securitys.Confirm = data.is_confirm;
            securitys.Undo = data.is_undo;
        }
        return securitys;
    }
}