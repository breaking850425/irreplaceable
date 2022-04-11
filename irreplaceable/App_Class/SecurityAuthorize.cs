using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 程式權限設定
/// </summary>
public class SecurityAuthorize : AuthorizeAttribute
{
    /// <summary>
    /// 權限設定模式
    /// </summary>
    public enumSecuritys SecurityMode { get; set; } = enumSecuritys.Browse;

    /// <summary>
    /// 覆寫 Authorize 設定
    /// </summary>
    /// <param name="httpContext">httpContext</param>
    /// <returns>驗證結果</returns>
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        if (SecurityMode == enumSecuritys.Add) return SessionService.PrgSecuritys.Add;
        if (SecurityMode == enumSecuritys.Browse) return SessionService.PrgSecuritys.IsSecurity;
        if (SecurityMode == enumSecuritys.Confirm) return SessionService.PrgSecuritys.Confirm;
        if (SecurityMode == enumSecuritys.Delete) return SessionService.PrgSecuritys.Delete;
        if (SecurityMode == enumSecuritys.Download) return SessionService.PrgSecuritys.Download;
        if (SecurityMode == enumSecuritys.Edit) return SessionService.PrgSecuritys.Edit;
        if (SecurityMode == enumSecuritys.Print) return SessionService.PrgSecuritys.Print;
        if (SecurityMode == enumSecuritys.Undo) return SessionService.PrgSecuritys.Undo;
        if (SecurityMode == enumSecuritys.Upload) return SessionService.PrgSecuritys.Upload;
        return false;
    }

    // 驗證失敗重新開啟程式一次
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        // 設定要執行的Result
        string str_url = string.Format("/{0}/{1}", SessionService.ControllerName, SessionService.ActionName);
        if (SecurityMode == enumSecuritys.Browse) str_url = "/Admin/Index";
        filterContext.Result = new RedirectResult(str_url);
    }
}
