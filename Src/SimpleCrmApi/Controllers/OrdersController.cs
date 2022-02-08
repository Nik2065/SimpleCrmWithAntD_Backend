using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Entities;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SimpleCrmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly NLog.Logger _logger;
        private readonly DataStore _db;
        private readonly OrderLogic _orderLogic;

        public OrdersController(DataStore context)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _db = context;

        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetOrdersList()
        {
            var result = new {
                ErrorHappened = false,
                Message = "",
                Orders = new List<CrmOrder>()
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == "CrmAccountId")?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    var accountIdGuid = new Guid(accountId);

                    var condition = new CrmOrdersCondition();
                    var o = _orderLogic.GetCrmOrders(condition);



                    result = new
                    {
                        ErrorHappened = false,
                        Message = "",
                        Orders = o
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при получении списка заказов",
                    Orders = new List<CrmOrder>()
                };
            }


            return Ok(result);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetOrder(Guid orderId)
        {
            var result = new
            {
                ErrorHappened = false,
                Message = "",
                Order = new CrmOrder()
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == "CrmAccountId")?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    var g = new Guid(accountId);
                    var o = _db.CrmOrders.FirstOrDefault(item => item.CrmAccountId == g && item.Id == orderId);

                    result = new
                    {
                        ErrorHappened = false,
                        Message = "",
                        Order = o
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при получении заказа",
                    Order = new CrmOrder()
                };
            }


            return Ok(result);
        }

        [HttpDelete]
        [Route("[action]")]
        public IActionResult DeleteOrder([FromBody]DeleteOrderRequest request)
        {
            var context = HttpContext.Request;

            var result = new
            {
                ErrorHappened = false,
                Message = "",
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == "CrmAccountId")?.Value;
                

                if (!string.IsNullOrEmpty(accountId)
                    && !string.IsNullOrEmpty(request.OrderId))
                {
                    var accountIdGuid = new Guid(accountId);
                    var orderIdGuid = new Guid(request.OrderId);

                    var o = _db.CrmOrders
                        .FirstOrDefault(item => item.CrmAccountId == accountIdGuid 
                        && item.Id == orderIdGuid);
                    if (o != null)
                    {
                        _db.CrmOrders.Remove(o);
                        _db.SaveChanges();

                        result = new
                        {
                            ErrorHappened = false,
                            Message = "Заказ удален",
                        };
                    }
                    else
                    {
                        result = new
                        {
                            ErrorHappened = false,
                            Message = "Заказ не найден",
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при удалении заказа",
                };
            }


            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderRequest request)
        {
            var result = new
            {
                ErrorHappened = false,
                Message = "",
                Order = new CrmOrder()
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == Common.ClaimsEnum.CrmAccountId)?.Value;
                var userId = User.Claims.FirstOrDefault(r => r.Type == Common.ClaimsEnum.CrmUserId)?.Value;

                if (!string.IsNullOrEmpty(accountId)
                    && !string.IsNullOrEmpty(request.CustomerId)
                    && !string.IsNullOrEmpty(userId))
                {
                    var accountIdGuid = new Guid(accountId);
                    var userIdGuid = new Guid(userId);
                    var customerIdGuild = new Guid(request.CustomerId);

                    var o = new CrmOrder();
                    o.LocalId = await _db.GenerateLocatOrderId();
                    o.CrmOrderCreated = DateTime.Now;
                    o.CrmAccountId = accountIdGuid;
                    o.AuthorCrmUserId = userIdGuid;
                    o.ResponsibleCrmUserId = userIdGuid;
                    o.CustomerId = customerIdGuild;
                    o.Description = request.Description;

                    _db.CrmOrders.Add(o);
                    await _db.SaveChangesAsync();


                    //TODO:
                    //добавлять запись о событиях в лог

                    result = new
                    {
                        ErrorHappened = false,
                        Message = "Заказ создан",
                        Order = o
                    };
                }
                else
                {
                    result = new
                    {
                        ErrorHappened = true,
                        Message = "Ошибка при создании заказа",
                        Order = new CrmOrder()
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при получении заказа",
                    Order = new CrmOrder()
                };
            }

            await Task.Delay(5000);

            return Ok(result);
        }

    }


    public class AddOrderRequest
    {
        //описание заказа
        public string Description { get; set; }
        public string CustomerId { get; set; }
    }

    public class DeleteOrderRequest
    {
        public string OrderId { get; set; }
    }
}
