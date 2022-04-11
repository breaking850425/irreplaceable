using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;


public class tblOrdersDetail : BaseClass
{
    public IRepository<OrdersDetail> repo;
    public tblOrdersDetail()
    {
        repo = new EFGenericRepository<OrdersDetail>(new web110b_07Entities());
    }

}
