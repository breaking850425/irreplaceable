using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;
using PagedList;

public class tblProgramTypes : BaseClass
{
    public IRepository<ProgramTypes> repo;
    public tblProgramTypes()
    {
        repo = new EFGenericRepository<ProgramTypes>(new web110b_07Entities());
    }
    /// <summary>
    /// 取得類別圖示名稱
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetIconName(string typeNo)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.type_no == typeNo);
        if (data != null) str_value = data.icon_name;
        return str_value;
    }
    /// <summary>
    /// 取得類別名稱
    /// </summary>
    /// <param name="typeNo">類別代號</param>
    /// <returns></returns>
    public string GetTypeName(string typeNo)
    {
        string str_value = "";
        var data = repo.ReadSingle(m => m.type_no == typeNo);
        if (data != null) str_value = data.type_name;
        return str_value;
    }
    /// <summary>
    /// 取得程式分類下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetSelectList()
    {
        List<SelectListItem> dataList = new List<SelectListItem>();
        var data = repo.ReadAll().OrderBy(m => m.type_no);
        if (data != null)
        {
            foreach (var item in data)
            {
                dataList.Add(new SelectListItem() { Text = string.Format("{0}.{1}", item.type_no, item.type_name), Value = item.type_no });
            }
        }
        return dataList;
    }

    /// <summary>
    /// 取得模組程式類別資料集
    /// </summary>
    /// <param name="index">陣列索引</param>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    public IPagedList<ProgramTypes> GetModelList(int index, int page, int pageSize, string searchText)
    {
        var model = repo.ReadAll();
        var dataSort = SessionService.GetColumnSort(index);
        if (!string.IsNullOrEmpty(searchText))
        {
            model = model.Where(m =>
            m.type_no.Contains(searchText) ||
            m.type_name.Contains(searchText) ||
            m.icon_name.Contains(searchText) ||
            m.remark.Contains(searchText));
        }
        if (model != null)
        {
            if (string.IsNullOrEmpty(dataSort.SortColumn)) dataSort.SortColumn = "type_no";
            if (dataSort.SortColumn == "type_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.type_no);
            if (dataSort.SortColumn == "type_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.type_no);
            if (dataSort.SortColumn == "type_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.type_name);
            if (dataSort.SortColumn == "type_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.type_name);
            if (dataSort.SortColumn == "icon_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.icon_name);
            if (dataSort.SortColumn == "icon_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.icon_name);
        }

        var datas = model.ToPagedList(page, pageSize);
        SessionService.SetCurrentPage(index, page, searchText, model.ToList().Count, datas.PageCount);
        return datas;
    }
}