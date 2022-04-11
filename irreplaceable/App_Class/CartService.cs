using irreplaceable.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class CartService
{
    #region 公開屬性
    /// <summary>
    /// 訂單編號
    /// </summary>
    public static string OrderNo { get; set; }

    /// <summary>
    /// 購物批號 LotNo
    /// </summary>
    public static string LotNo
    {
        get { return GetLotNo(); }
        set { HttpContext.Current.Session["CartLotNo"] = value; }
    }
    /// <summary>
    /// 購物批號建立時間
    /// </summary>
    public static DateTime LotCreateTime
    {
        get { return GetLotCreateTime(); }
        set { HttpContext.Current.Session["CartCreateTime"] = value; }
    }
    /// <summary>
    /// 購物車筆數
    /// </summary>
    public static int Counts { get { return GetCartCount(); } }

    /// <summary>
    /// 購物車合計
    /// </summary>
    public static int Totals { get { return GetCartTotals(); } }
    #endregion
    #region 公用函數
    /// <summary>
    /// 更新購物批號
    /// </summary>
    /// <returns></returns>
    public static string NewLotNo()
    {
        string str_lot_no = "";
        //if (!SessionService.IsLogined)
        str_lot_no = Guid.NewGuid().ToString().Substring(0, 15).ToUpper();
        LotNo = str_lot_no;
        LotCreateTime = DateTime.Now;
        return str_lot_no;
    }
    #endregion
    #region 公用事件
    /// <summary>
    /// 登入時將現有遊客的購物車加入客戶的購物車
    /// </summary>
    public static void LoginCart()
    {
        if (!string.IsNullOrEmpty(LotNo))
        {
            UpdateMemberCartLotNo();
            int int_qty = 0;
            using (tblCarts carts = new tblCarts())
            {
                var datas = carts.repo.ReadAll(m => m.lot_no == LotNo && m.user_no != SessionService.AccountNo);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        int_qty = item.qty.GetValueOrDefault();
                        AddCart(item.product_no, item.product_spec, int_qty);
                        carts.repo.Delete(item);
                    }
                    carts.repo.SaveChanges();
                }
            }
        }
        else
        {
            NewLotNo();
            UpdateMemberCartLotNo();
        }
    }


    public static void UpdateMemberCartLotNo()
    {
        if (SessionService.IsLogined)
        {
            using (tblCarts carts = new tblCarts())
            {
                var datas = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
                if (datas != null)
                {
                    foreach (var item in datas)
                    {
                        var data = carts.repo.ReadSingle(m => m.rowid == item.rowid);
                        if (data != null)
                        {
                            data.lot_no = LotNo;
                            carts.repo.Update(data);
                        }
                    }
                    carts.repo.SaveChanges();
                }
            }
        }
    }


    /// <summary>
    /// 加入購物車
    /// </summary>
    /// <param name="productNo">商品編號</param>
    public static void AddCart(string productNo)
    {
        AddCart(productNo, "", 1);
    }

    /// <summary>
    /// 加入購物車
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <param name="buyQty">數量</param>
    public static void AddCart(string productNo, int buyQty)
    {
        AddCart(productNo, "", buyQty);
    }

    /// <summary>
    /// 加入購物車
    /// </summary>
    /// <param name="productNo">商品編號</param>
    /// <param name="prod_Spec">商品規格</param>
    /// <param name="buyQty">數量</param>
    public static void AddCart(string productNo, string prod_Spec, int buyQty)
    {
        using (tblCarts carts = new tblCarts())
        {
            carts.AddCart(productNo, prod_Spec, buyQty);
        }
    }

    /// <summary>
    /// 更新購物車
    /// </summary>
    /// <param name="rowID">row ID</param>
    /// <param name="qty">數量</param>
    public static void UpdateCart(int rowID, int qty)
    {
        using (tblCarts carts = new tblCarts())
        {
            carts.UpdateCart(rowID, qty);
        }
    }

    /// <summary>
    /// 刪除購物車
    /// </summary>
    /// <param name="rowID">row ID</param>
    public static void DeleteCart(int rowID)
    {
        using (tblCarts carts = new tblCarts())
        {
            carts.DeleteCart(rowID);
        }
    }

    /// <summary>
    /// 消費者付款
    /// </summary>
    public static int CartPayment(vmOrders model)
    {
        int int_order_id = 0;
        OrderNo = CreateNewOrderNo(model);
        using (tblCarts carts = new tblCarts())
        {
            using (tblOrders orders = new tblOrders())
            {
                using (tblOrdersDetail ordersDetail = new tblOrdersDetail())
                {
                    using (tblProducts products = new tblProducts())
                    {
                        var datas = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
                        if (datas != null)
                        {
                            int int_amount = datas.Sum(m => m.amount).GetValueOrDefault();
                            decimal dec_tax = 0;
                            if (int_amount > 0)
                            {
                                dec_tax = Math.Round((decimal)(int_amount * 0 / 100), 0);
                            }
                            int int_total = int_amount + (int)dec_tax;

                            var data = orders.repo.ReadSingle(m => m.order_no == OrderNo);
                            if (data != null)
                            {
                                data.amounts = 0;
                                data.taxs = (int)dec_tax;
                                data.totals = int_total;
                                data.order_validate = 0; // 未付款
                                orders.repo.Update(data);
                                orders.repo.SaveChanges();
                            }

                            foreach (var item in datas)
                            {
                                OrdersDetail detail = new OrdersDetail();
                                detail.order_no = OrderNo;
                                detail.product_no = item.product_no;
                                detail.product_name = item.product_name;
                                detail.vendor_no = "";
                                detail.category_name = products.GetProductCategoryName(item.product_no);
                                detail.product_spec = item.product_spec;
                                detail.qty = item.qty;
                                detail.price = item.price;
                                detail.amount = item.amount;
                                detail.remark = "";

                                ordersDetail.repo.Create(detail);
                                ordersDetail.repo.SaveChanges();
                            }
                        }
                    }
                }
            }
        }
        return int_order_id;
    }
    /// <summary>
    /// 清除購物車
    /// </summary>
    public static void ClearCart()
    {
        using (tblCarts carts = new tblCarts())
        {
            carts.ClearCart();
        }
    }

    #endregion
    #region 私有函數
    /// <summary>
    /// 取得購物批號建立時間
    /// </summary>
    /// <returns></returns>
    private static DateTime GetLotCreateTime()
    {
        object obj_time = HttpContext.Current.Session["CartCreateTime"];
        return (obj_time == null) ? DateTime.Now : DateTime.Parse(obj_time.ToString());
    }

    /// <summary>
    /// 取得購物批號
    /// </summary>
    /// <returns></returns>
    private static string GetLotNo()
    {
        return (HttpContext.Current.Session["CartLotNo"] == null) ? NewLotNo() : HttpContext.Current.Session["CartLotNo"].ToString();
    }

    /// <summary>
    /// 取得目前購物車筆數
    /// </summary>
    /// <returns></returns>
    private static int GetCartCount()
    {
        int int_count = 0;
        int total_qty = 0;
        using (tblCarts carts = new tblCarts())
        {
            if (SessionService.IsLogined)
            {
                var data1 = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
                foreach (var item in data1)
                {
                    total_qty += (int)item.qty;
                }
                if (data1 != null) int_count = total_qty;
            }
            else
            {
                var data2 = carts.repo.ReadAll(m => m.lot_no == LotNo);
                foreach (var item in data2)
                {
                    total_qty += (int)item.qty;
                }
                if (data2 != null) int_count = total_qty;
            }
        }
        return int_count;
    }

    private static int GetCartTotals()
    {
        int? int_totals = 0;
        using (tblCarts carts = new tblCarts())
        {
            if (SessionService.IsLogined)
            {
                var data1 = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
                if (data1 != null) int_totals = data1.Sum(m => m.amount);
            }
            else
            {
                var data2 = carts.repo.ReadAll(m => m.lot_no == LotNo);
                if (data2 != null) int_totals = data2.Sum(m => m.amount);
            }
        }
        if (int_totals == null) int_totals = 0;
        return int_totals.GetValueOrDefault();
    }

    private static string CreateNewOrderNo(vmOrders model)
    {
        ShopService.OrderID = 0;
        ShopService.OrderNo = "0";
        string str_order_no = "";
        string str_guid = Guid.NewGuid().ToString().Substring(0, 25).ToUpper();
        using (tblOrders orders = new tblOrders())
        {
            Orders newOrders = new Orders();
            newOrders.order_closed = 0;
            newOrders.order_validate = 0;
            newOrders.order_no = "";
            newOrders.order_date = DateTime.Now;
            newOrders.user_no = SessionService.AccountNo;
            newOrders.order_status = "ON";
            newOrders.order_guid = str_guid;
            newOrders.payment_no = model.payment_no;
            newOrders.shipping_no = model.shipping_no;
            newOrders.receive_name = model.receive_name;
            newOrders.receive_email = model.receive_email;
            newOrders.receive_address = model.receive_address;
            newOrders.remark = "";

            orders.repo.Create(newOrders);
            orders.repo.SaveChanges();

            var neword = orders.repo.ReadSingle(m => m.order_guid == str_guid);
            if (neword != null)
            {
                str_order_no = neword.rowid.ToString().PadLeft(8, '0');
                neword.order_no = str_order_no;
                orders.repo.Update(neword);
                orders.repo.SaveChanges();

                ShopService.OrderID = neword.rowid;
                ShopService.OrderNo = str_order_no;
            }
        }
        return str_order_no;
    }
    #endregion
}