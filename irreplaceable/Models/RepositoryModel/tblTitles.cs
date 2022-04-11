using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;
using PagedList;

public class tblTitles : BaseClass
{
    public IRepository<Titles> repo;
    public tblTitles()
    {
        repo = new EFGenericRepository<Titles>(new web110b_07Entities());
    }
    /// <summary>
    /// 用職稱代號取得職稱名稱
    /// </summary>
    /// <param name="titleNo">職稱代號</param>
    /// <returns></returns>
    public string GetTitleName(string titleNo)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.title_no == titleNo);
        if (data != null) str_value = data.title_name;
        return str_value;
    }

    /// <summary>
    /// 取得模組資料集
    /// </summary>
    /// <param name="index">陣列索引</param>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    public IPagedList<Titles> GetModelList(int index, int page, int pageSize, string searchText)
    {
        var model = repo.ReadAll();
        var dataSort = SessionService.GetColumnSort(index);
        if (!string.IsNullOrEmpty(searchText))
        {
            model = model.Where(m =>
            m.title_no.Contains(searchText) ||
            m.title_name.Contains(searchText) ||
            m.remark.Contains(searchText));
        }
        if (model != null)
        {
            if (string.IsNullOrEmpty(dataSort.SortColumn)) dataSort.SortColumn = "title_no";
            if (dataSort.SortColumn == "title_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.title_no);
            if (dataSort.SortColumn == "title_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.title_no);
            if (dataSort.SortColumn == "title_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.title_name);
            if (dataSort.SortColumn == "title_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.title_name);
        }

        var datas = model.ToPagedList(page, pageSize);
        SessionService.SetCurrentPage(index, page, searchText, model.ToList().Count, datas.PageCount);
        return datas;
    }
}