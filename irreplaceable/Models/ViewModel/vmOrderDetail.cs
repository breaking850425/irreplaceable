using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using irreplaceable.Models;

public class vmOrderDetail
{
    public Orders orders { get; set; }
    public List<OrdersDetail> ordersDetail { get; set; }
}