using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using irreplaceable.Models;
using PagedList;

public class tblOrders:BaseClass
{
    public IRepository<Orders> repo;
    public tblOrders()
    {
        repo = new EFGenericRepository<Orders>(new web110b_07Entities());
    }


    /// <summary>
    /// 取得訂單資料集
    /// </summary>
    /// <param name="index">陣列索引</param>
    /// <param name="page">目前頁數</param>
    /// <param name="pageSize">每頁筆數</param>
    /// <param name="searchText">查詢文字</param>
    /// <returns></returns>
    public IPagedList<Orders> GetModelList(int index, int page, int pageSize, string searchText)
    {
        var model = repo.ReadAll();
        var dataSort = SessionService.GetColumnSort(index);
        if (!string.IsNullOrEmpty(searchText))
        {
            model = model.Where(m =>
            m.order_no.Contains(searchText) ||
            m.order_date.ToString().Contains(searchText) ||
            m.order_status.Contains(searchText) ||
            m.user_no.Contains(searchText) ||
            m.payment_no.ToString().Contains(searchText) ||
            m.shipping_no.Contains(searchText) ||
            m.totals.ToString().Contains(searchText) ||
            m.receive_name.Contains(searchText) ||
            m.receive_phone.Contains(searchText) ||
            m.receive_address.Contains(searchText) ||
            m.remark.Contains(searchText));
        }
        if (model != null)
        {
            if (string.IsNullOrEmpty(dataSort.SortColumn))
            {
                dataSort.SortColumn = "order_no";
                dataSort.SortDirection = enumSortDirection.Desc;
                model = model.OrderByDescending(m => m.order_no);
            } 
            if (dataSort.SortColumn == "order_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.order_no);
            if (dataSort.SortColumn == "order_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.order_no);
            if (dataSort.SortColumn == "order_date" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.order_date);
            if (dataSort.SortColumn == "order_date" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.order_date);
            if (dataSort.SortColumn == "order_status" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.order_status);
            if (dataSort.SortColumn == "order_status" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.order_status);
            if (dataSort.SortColumn == "user_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.user_no);
            if (dataSort.SortColumn == "user_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.user_no);
            if (dataSort.SortColumn == "payment_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.payment_no);
            if (dataSort.SortColumn == "payment_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.payment_no);
            if (dataSort.SortColumn == "shipping_no" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.shipping_no);
            if (dataSort.SortColumn == "shipping_no" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.shipping_no);
            if (dataSort.SortColumn == "totals" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.totals);
            if (dataSort.SortColumn == "totals" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.totals);
            if (dataSort.SortColumn == "receive_name" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.receive_name);
            if (dataSort.SortColumn == "receive_name" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.receive_name);
            if (dataSort.SortColumn == "receive_phone" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.receive_phone);
            if (dataSort.SortColumn == "receive_phone" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.receive_phone);
            if (dataSort.SortColumn == "receive_address" && dataSort.SortDirection == enumSortDirection.Asc) model = model.OrderBy(m => m.receive_address);
            if (dataSort.SortColumn == "receive_address" && dataSort.SortDirection == enumSortDirection.Desc) model = model.OrderByDescending(m => m.receive_address);


        }

        var datas = model.ToPagedList(page, pageSize);
        SessionService.SetCurrentPage(index, page, searchText, model.ToList().Count, datas.PageCount);
        return datas;
    }


    /// <summary>
    /// 取得訂單狀態下拉式選單集合
    /// </summary>
    /// <returns></returns>
    public List<SelectListItem> GetOrderStatusList()
    {
        return new List<SelectListItem>() {
            new SelectListItem() { Text = "訂單處理中", Value = "ON" },
            new SelectListItem() { Text = "訂單已完成", Value = "END" },
            new SelectListItem() { Text = "訂單已取消", Value = "CANCEL" }
        };
    }


    

}

