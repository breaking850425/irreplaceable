using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;


public class tblBigSales:BaseClass
{
    public IRepository<BigSales> repo;
    public tblBigSales()
    {
        repo = new EFGenericRepository<BigSales>(new web110b_07Entities());
    }

    public List<BigSales> GetBigSalesDataList() 
    {
        return repo.ReadAll()
            .Where(m => m.start_time <= DateTime.Now)
            .Where(m => m.end_time >= DateTime.Now)
            .OrderBy(m => m.product_no).ToList();
    }

}
