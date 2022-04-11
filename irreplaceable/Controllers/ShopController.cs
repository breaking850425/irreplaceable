using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace irreplaceable.Controllers
{
    public class ShopController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 6)
        {
            using (tblProducts products = new tblProducts())
            {
                using (tblProductsCategorys categorys = new tblProductsCategorys())
                {
                    ShopService.PriceLow = 0;
                    ShopService.PriceHigh = 1000;
                    ShopService.SortNo = "NewArrival";
                    ShopService.PageCount = 10;
                    vmShopIndex model = new vmShopIndex();
                    model.ProductList = products.GetShopProductsList(page, pageSize);
                    model.CategoryList = categorys.GetShopCategoryList();
                    return View(model);
                }
            }
        }

        /// <summary>
        /// 有查詢資料沒重設頁面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Select(int page = 1, int pageSize = 6)
        {
            using (tblProducts products = new tblProducts())
            {
                using (tblProductsCategorys categorys = new tblProductsCategorys())
                {
                    ShopService.PageCount = 10;
                    vmShopIndex model = new vmShopIndex();
                    model.ProductList = products.GetShopProductsList(page, pageSize);
                    model.CategoryList = categorys.GetShopCategoryList();
                    return View(model);
                }
            }
        }


        /// <summary>
        /// 商品排序
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sort(string id)
        {
            ShopService.SortNo = id;
            return RedirectToAction("Select", "Shop");
        }

        /// <summary>
        /// 商品分類
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Category(string id)
        {
            ShopService.CategoryNo = id;
            ShopService.SearchText = "";
            ShopService.PriceLow = 0;
            ShopService.PriceHigh = 1000;
            ShopService.SortNo = "NewArrival";
            return RedirectToAction("Index", "Shop");
        }

        /// <summary>
        /// 商品搜尋
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult Search(FormCollection formCollection)
        {
            ShopService.CategoryNo = "";
            object obj_text = formCollection["SearchText"];
            string str_text = (obj_text == null) ? "" : obj_text.ToString();
            ShopService.SearchText = str_text;
            return RedirectToAction("Index", "Shop");
        }

        /// <summary>
        /// 價格區間
        /// </summary>
        /// <param name="formCollection"></param>
        /// <returns></returns>
        public ActionResult Price(FormCollection formCollection)
        {
            int int_low = 0;
            int int_high = 0;
            object obj_low = formCollection["price_low"];
            object obj_high = formCollection["price_high"];
            string str_low = (obj_low == null) ? "0" : obj_low.ToString();
            string str_high = (obj_high == null) ? "99999" : obj_high.ToString();
            int.TryParse(str_low, out int_low);
            int.TryParse(str_high, out int_high);
            ShopService.PriceLow = int_low;
            ShopService.PriceHigh = int_high;
            return RedirectToAction("Select", "Shop");
        }

        /// <summary>
        /// 商品明細
        /// </summary>
        /// <param name="id">商品編號</param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            using (tblProducts products = new tblProducts())
            {
                var model = products.repo.ReadSingle(m => m.product_no == id);
                return View(model);
            }
        }
        /// <summary>
        /// 商品明細-加入購物車
        /// </summary>
        /// <param name="model">Products Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddToCart(FormCollection collection)
        {
            string str_product_no = collection["product_no"];
            string str_qty = collection["Quantity"];
            int int_qty = 1;
            int.TryParse(str_qty, out int_qty);
            CartService.AddCart(str_product_no, int_qty);
            return RedirectToAction("Detail", "Shop", new { id = str_product_no });
        }

        /// <summary>
        /// 商品總攬-加入購物車
        /// </summary>
        /// <param name="model">Products Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IndexAddToCart(FormCollection collection)
        {
            string str_product_no = collection[1];
            int int_qty = 1;
            CartService.AddCart(str_product_no, int_qty);
            return RedirectToAction("Index", "Shop");
        }

        /// <summary>
        /// 更新購物車
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateCart(FormCollection collection)
        {
            int int_rowid = 0;
            int int_qty = 0;
            for (int i = 1; i < collection.Count; i += 2)
            {
                int_rowid = int.Parse(collection[i].ToString());
                int_qty = int.Parse(collection[i + 1].ToString());
                CartService.UpdateCart(int_rowid, int_qty);
            }
            return RedirectToAction("Cart", "Shop");
        }
        /// <summary>
        /// 刪除購物車
        /// </summary>
        /// <param name="id">row ID</param>
        /// <returns></returns>
        public ActionResult DeleteCart(int id)
        {
            CartService.DeleteCart(id);
            return RedirectToAction("Cart", "Shop");
        }
        /// <summary>
        /// 購物車
        /// </summary>
        /// <returns></returns>
        public ActionResult Cart()
        {
            using (tblCarts carts = new tblCarts())
            {
                if (SessionService.IsLogined)
                {
                    var data1 = carts.repo.ReadAll(m => m.user_no == SessionService.AccountNo);
                    return View(data1);
                }
                var data2 = carts.repo.ReadAll(m => m.lot_no == CartService.LotNo);
                return View(data2);
            }
        }

        /// <summary>
        /// 結帳付款
        /// </summary>
        /// <returns></returns>
        
        public ActionResult Payment()
        {
            if (!SessionService.IsLogined) return RedirectToAction("Login", "Account");
            if (CartService.Counts <= 0) return RedirectToAction("Index", "Shop");

            using (tblPayments payments = new tblPayments())
            {
                using (tblShippings shippings = new tblShippings())
                {
                    using (tblCarts carts = new tblCarts())
                    {
                        vmOrders models = new vmOrders()
                        {
                            receive_name = "",
                            receive_email = "",
                            receive_address = "",
                            payment_no = "01",
                            shipping_no = "01",
                            remark = "",
                            PaymentsList = payments.PaymentsList(),
                            ShippingsList = shippings.ShippingsList(),
                            CartsList = carts.CartList()
                        };
                        return View(models);
                    }
                }
            }
        }
        /// <summary>
        /// 結帳付款成訂單
        /// </summary>
        /// <returns></returns>
        [LoginedAuthorize()]
        [HttpPost]
        public ActionResult Payment(vmOrders model)
        {
            if (!ModelState.IsValid)
            {
                using (tblPayments payments = new tblPayments())
                {
                    using (tblShippings shippings = new tblShippings())
                    {
                        using (tblCarts carts = new tblCarts())
                        {
                            if (model.PaymentsList == null) model.PaymentsList = payments.PaymentsList();
                            if (model.ShippingsList == null) model.ShippingsList = shippings.ShippingsList();
                            if (model.CartsList == null) model.CartsList = carts.CartList();
                            return View(model);
                        }
                    }
                }
            }
            CartService.CartPayment(model);
            CartService.ClearCart();
            CartService.NewLotNo();
            return Redirect("~/ECPayment.aspx");
        }

        public ActionResult PaymentReport()
        {
            return View();
        }
        /// <summary>
        /// 金流付款返回
        /// </summary>
        /// <returns></returns>
        
        public ActionResult CheckoutReport()
        {
            using (SendMailService sendMail = new SendMailService())
            {
                //將訂單改為已付款
                ShopService.SetOrderPayed();
                //寄出訂單付款通知信
                sendMail.OrderPayment();
                return View();
            }
        }
    }
}