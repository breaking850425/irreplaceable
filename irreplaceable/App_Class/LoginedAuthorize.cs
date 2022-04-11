using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/// <summary>
/// 自定義權限 Filter
/// </summary>
public class LoginedAuthorize : AuthorizeAttribute
{
    /// <summary>
    /// 覆寫 Authorize 設定
    /// </summary>
    /// <param name="httpContext">httpContext</param>
    /// <returns>驗證結果</returns>
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        return SessionService.IsLogined;
    }

    // 驗證失敗要做什麼
    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        // 設定要執行的Result
        filterContext.Result = new RedirectResult("/Account/Login");
    }
}