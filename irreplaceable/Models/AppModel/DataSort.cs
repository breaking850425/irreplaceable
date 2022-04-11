using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 欄位排序類別
/// </summary>
public class DataSort
{
    /// <summary>
    /// 排序代號
    /// </summary>
    public int SortID { get; set; } = 1;
    /// <summary>
    /// 目前頁數
    /// </summary>
    public int Page { get; set; } = 1;
    /// <summary>
    /// 排序欄位
    /// </summary>
    public string SortColumn { get; set; } = "";
    /// <summary>
    /// 排序方式
    /// </summary>
    public enumSortDirection SortDirection { get; set; } = enumSortDirection.Asc;
    /// <summary>
    /// 查詢文字
    /// </summary>
    public string SearchText { get; set; }
    /// <summary>
    /// 總記錄筆數
    /// </summary>
    public int RecordCount { get; set; }
    /// <summary>
    /// 總頁數
    /// </summary>
    public int PageCount { get; set; }
}