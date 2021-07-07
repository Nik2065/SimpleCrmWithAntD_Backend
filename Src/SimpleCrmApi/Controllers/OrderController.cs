using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTOLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SimpleCrmApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [Route("[action]")]
        public ActionResult GetOrders()
        {
            var result = new List<DtoCrmOrder>();

            {
                var o1 = new DtoCrmOrder();
                o1.Id = new Guid("DE3C7BBE-E3FE-4192-AD81-A2D617EC0888");
                o1.Created = new DateTime(2021, 1, 1);
                o1.Content = "Заказ №1 и его описание";
                result.Add(o1);
            }
            //{
            //    var o2 = new DtoCrmOrder();
            //    o2.Id = new Guid("DE3C7BBE-E3FE-4192-AD81-A2D617EC0119");
            //    o2.Created = new DateTime(2021, 1, 1);
            //    o2.Content = "Заказ №2 и его описание";
            //    result.Add(o2);
            //}
            //{
            //    var o3 = new DtoCrmOrder();
            //    o3.Id = new Guid("DE3C7BBE-E3FE-4192-AD81-A2D617EC0000");
            //    o3.Created = new DateTime(2021, 1, 1);
            //    o3.Content = "Заказ №3 и его описание";
            //    result.Add(o3);
            //}



            return Ok(result);
        }
    }
}
