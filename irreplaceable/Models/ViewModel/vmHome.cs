using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class vmHome:BaseClass
{

    /// <summary>
    /// 應用程式相關資料
    /// </summary>
    //public Applications ApplicationsData { get; set; }

    /// <summary>
    /// 促銷商品資料
    /// </summary>
    public List<BigSales> BigSalesData { get; set; }
    /// <summary>
    /// 商品分類資料
    /// </summary>
    public List<ProductsCategorys> ProductsCategorysData { get; set; }

    /// <summary>
    /// 商品資料
    /// </summary>
    public List<Products> ProductsData { get; set; }



    public vmHome()
    {
        //using (tblApplications applications = new tblApplications())
        //{ ApplicationsData = applications.GetApplicationsData(); }

        using (tblBigSales bigSales = new tblBigSales())
        { BigSalesData = bigSales.GetBigSalesDataList(); }

        using (tblProductsCategorys product_categorys = new tblProductsCategorys())
        { ProductsCategorysData = product_categorys.GetCategoryDataList(); }


        using (tblProducts products = new tblProducts())
        { ProductsData = products.ProductsDataList(); }
    }
}
