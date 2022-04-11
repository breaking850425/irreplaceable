using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// 購物商城頁專用類別
/// </summary>
public static class ShopService
{
    /// <summary>
    /// 分類方式
    /// </summary>
    public static string CategoryNo { get { return GetSessionValue("ShopCategoryNo", ""); } set { HttpContext.Current.Session["ShopCategoryNo"] = value; } }
    /// <summary>
    /// 最低售價
    /// </summary>
    public static int PriceLow { get { return GetSessionIntegerValue("ShopPriceLow", 0); } set { HttpContext.Current.Session["ShopPriceLow"] = value; } }
    /// <summary>
    /// 最高售價
    /// </summary>
    public static int PriceHigh { get { return GetSessionIntegerValue("ShopPriceHigh", 1000); } set { HttpContext.Current.Session["ShopPriceHigh"] = value; } }
    public static int OrderID { get; set; }
    public static string OrderNo { get; set; }
    /// <summary>
    /// 排序方式
    /// </summary>
    public static string SortNo { get { return GetSessionValue("ShopSortNo", "NewArrival"); } set { HttpContext.Current.Session["ShopSortNo"] = value; } }
    /// <summary>
    /// 搜尋文字
    /// </summary>
    public static string SearchText { get { return GetSessionValue("ShopSearchText", ""); } set { HttpContext.Current.Session["ShopSearchText"] = value; } }
    /// <summary>
    /// 目前頁數
    /// </summary>
    public static int Page { get { return GetSessionIntegerValue("ShopPage", 1); } set { HttpContext.Current.Session["ShopPage"] = value; } }
    /// <summary>
    /// 總頁數
    /// </summary>
    public static int Pages
    {
        get
        {
            int int_page_count = PageRowCount / PageSize;
            if (PageRowCount % PageSize > 0) int_page_count++;
            return int_page_count;
        }
    }
    /// <summary>
    /// 每頁筆數
    /// </summary>
    public static int PageSize { get { return GetSessionIntegerValue("ShopPageSize", 10); } set { HttpContext.Current.Session["ShopPageSize"] = value; } }
    /// <summary>
    /// 每頁顯示頁數
    /// </summary>
    public static int PageCount { get { return GetSessionIntegerValue("ShopPageCount", 10); } set { HttpContext.Current.Session["ShopPageCount"] = value; } }
    /// <summary>
    /// 總筆數
    /// </summary>
    public static int PageRowCount { get { return GetSessionIntegerValue("ShopRowCount", 0); } set { HttpContext.Current.Session["ShopRowCount"] = value; } }
    /// <summary>
    /// 開始頁數
    /// </summary>
    public static int PageStart
    {
        get
        {
            int int_start = 1;
            if (Page > PageCount)
            {
                int int_count = Page / PageCount;
                if (Page % PageCount == 0) int_count--;
                int_start = (int_count * PageCount) + 1;
            }
            return int_start;
        }
    }
    /// <summary>
    /// 結束頁數
    /// </summary>
    public static int PageEnd
    {
        get
        {
            int int_page = PageStart;
            int int_row_count = PageRowCount;
            if (PageStart > PageCount)
            {
                int_row_count -= (PageSize * (PageStart - 1));
            }
            if (int_row_count > 0)
            {
                int int_count = PageSize / int_row_count;
                if (PageSize % int_row_count > 0) int_count++;
                if (int_count > PageCount) int_count = PageCount;
                int_page += (PageCount - 1);
                if (int_page > Pages) int_page = Pages;
            }
            return int_page;
        }
    }

    /// <summary>
    /// 往前頁數
    /// </summary>
    public static int PriorPage()
    {
        return (PageStart - 1);
    }
    /// <summary>
    /// 往後頁數
    /// </summary>
    public static int NextPage()
    {
        return (PageEnd + 1);
    }

    /// <summary>
    /// 所有商品筆數
    /// </summary>
    public static int GetAllProductsCount()
    {
        using (tblProducts products = new tblProducts())
        {
            return products.repo.ReadAll().Count();
        }
    }
    /// <summary>
    /// 分類名稱
    /// </summary>
    public static string CategoryName
    {
        get
        {
            if (!string.IsNullOrEmpty(SearchText)) return string.Format("搜尋：{0}", SearchText);
            if (string.IsNullOrEmpty(CategoryNo)) return "全部商品";
            using (tblProductsCategorys categorys = new tblProductsCategorys())
            {
                return categorys.GetProductCategoryName(CategoryNo);
            }
        }
    }
    /// <summary>
    /// 排序名稱
    /// </summary>
    public static string SortName
    {
        get
        {
            if (SortNo == "NewArrival") return "最新商品";
            if (SortNo == "NameAsc") return "依名稱,由小到大";
            if (SortNo == "NameDesc") return "依名稱,由大到小";
            if (SortNo == "PriceAsc") return "依價格,由小到大";
            if (SortNo == "PriceDesc") return "依價格,由大到小";
            return "";
        }
    }
    /// <summary>
    /// 取得 Session 值-文字型別
    /// </summary>
    /// <param name="sessionName">Session 名稱</param>
    /// <returns></returns>
    public static string GetSessionValue(string sessionName, string defauleValue)
    {
        return (HttpContext.Current.Session[sessionName] == null) ? defauleValue : HttpContext.Current.Session[sessionName].ToString();
    }
    /// <summary>
    /// 取得 Session 值-數字型別
    /// </summary>
    /// <param name="sessionName">Session 名稱</param>
    /// <returns></returns>
    public static int GetSessionIntegerValue(string sessionName, int defauleValue)
    {
        object obj_value = HttpContext.Current.Session[sessionName];
        if (obj_value == null) return defauleValue;
        string str_value = obj_value.ToString();
        int int_value = 0;
        if (int.TryParse(str_value, out int_value)) return int_value;
        return defauleValue;
    }
    /// <summary>
    /// 取得圖片路徑
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <returns></returns>
    public static string GetProductImageUrl(string productNo)
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string str_url = string.Format("~/Images/Products/{0}.jpg", productNo);
       
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
            str_url = "~/Images/Products/None.jpg";

        str_url += string.Format(@"?t={0}", str_now);
        return str_url;
    }

    /// <summary>
    /// 取得圖片-1路徑
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <returns></returns>
    public static string GetProductImage1Url(string productNo)
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string str_url = string.Format("~/Images/Products/{0}-1.jpg", productNo);
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
            str_url = "#";

        str_url += string.Format(@"?t={0}", str_now);
        return str_url;
    }

    /// <summary>
    /// 取得圖片-2路徑
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <returns></returns>
    public static string GetProductImage2Url(string productNo)
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string str_url = string.Format("~/Images/Products/{0}-2.jpg", productNo);
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
            str_url = "#";

        str_url += string.Format(@"?t={0}", str_now);
        return str_url;
    }
    /// <summary>
    /// 取得圖片-3路徑
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <returns></returns>
    public static string GetProductImage3Url(string productNo)
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string str_url = string.Format("~/Images/Products/{0}-3.jpg", productNo);
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
            str_url = "#";

        str_url += string.Format(@"?t={0}", str_now);
        return str_url;
    }
    /// <summary>
    /// 取得圖片-4路徑
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <returns></returns>
    public static string GetProductImage4Url(string productNo)
    {
        string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
        string str_url = string.Format("~/Images/Products/{0}-4.jpg", productNo);
        if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
            str_url = "#";

        str_url += string.Format(@"?t={0}", str_now);
        return str_url;
    }

    ///// <summary>
    ///// 取得圖片-5路徑
    ///// </summary>
    ///// <param name="productNo">商品編號</param>
    ///// <returns></returns>
    //public static string GetProductImage5Url(string productNo)
    //{
    //    string str_now = DateTime.Now.ToString("yyyyMMddHHmmssffff");
    //    string str_url = string.Format("~/Images/Products/{0}-5.jpg", productNo);
    //    if (!File.Exists(HttpContext.Current.Server.MapPath(str_url)))
    //        str_url = "#";
    //     str_url += string.Format(@"?t={0}", str_now);
    //    return str_url;
    //}

    /// <summary>
    /// 將訂單改為已付款
    /// </summary>
    public static void SetOrderPayed()
    {
        using (tblOrders orders = new tblOrders())
        {
            var data = orders.repo.ReadSingle(m => m.rowid == ShopService.OrderID);
            if (data != null)
            {
                SessionService.AccountNo = data.user_no;
                SessionService.AccountName = data.order_user_name;
                SessionService.RoleNo = "Member";
                SessionService.RoleName = "會員";
                SessionService.IsLogined = true;
                data.order_validate = 1;
                orders.repo.Update(data);
                orders.repo.SaveChanges();
            }
        }
    }
}