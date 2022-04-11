using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class tblCarts : BaseClass
{
    public IRepository<Carts> repo;
    public tblCarts()
    {
        repo = new EFGenericRepository<Carts>(new web110b_07Entities());
    }

    public List<Carts> CartList()
    {
        return repo.ReadAll(m => m.user_no == SessionService.AccountNo).ToList();
    }

    //加入購物車
    public void AddCart(string productNo, string prod_Spec, int buyQty)
    {
        using (tblProducts products = new tblProducts())
        {
            int int_price = products.GetProductPrice(productNo);
            //int int_amount = (buyQty * int_price);
            var datas = repo.ReadSingle(m =>
              m.lot_no == CartService.LotNo &&
                m.user_no == SessionService.AccountNo &&
               m.product_no == productNo);

            //&&   m.product_spec == prod_Spec);

            //加入購物車，並新增一筆資料
            if (datas == null)
            {
                Carts models = new Carts();
                models.lot_no = CartService.LotNo;
                models.user_no = SessionService.AccountNo;
                models.crete_time = CartService.LotCreateTime;
                models.product_no = productNo;
                models.product_name = products.GetProductName(productNo);
                models.product_spec = prod_Spec;
                models.qty = buyQty;
                //models.amount = int_amount;
                models.price = int_price;

                repo.Create(models);
                repo.SaveChanges();
            }
            //更新購物車數量
            else
            {
                datas.qty += buyQty;
                //datas.amount = buyQty * int_price;

                repo.Update(datas);
                repo.SaveChanges();
            }
        }
    }

    //更新購物車
    public void UpdateCart(int rowID, int qty)
    {
        var data = repo.ReadSingle(m => m.rowid == rowID);
        data.qty = qty;
        //data.amount = qty * data.price;
        repo.Update(data);
        repo.SaveChanges();
    }

    //刪除購物車
    public void DeleteCart(int rowID)
    {
        var data = repo.ReadSingle(m => m.rowid == rowID);
        if (data != null)
        {
            repo.Delete(data);
            repo.SaveChanges();
        }
    }

    /// <summary>
    /// 清除購物車
    /// </summary>
    public void ClearCart()
    {
        var datas = repo.ReadAll(m => m.user_no == SessionService.AccountNo);
        if (datas != null)
        {
            foreach (var item in datas)
            {
                repo.Delete(item);
            }
            repo.SaveChanges();
        }
    }
}