using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SimpleCrmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CustomersController : Controller
    {
        NLog.Logger _logger;
        DataStore _db;

        public CustomersController(DataStore context)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _db = context;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetCustomersList()
        {
            var result = new {
                ErrorHappened = false,
                Message = "",
                Customers = new List<CrmCustomer>()
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == "CrmAccountId")?.Value;

                if (!string.IsNullOrEmpty(accountId))
                {
                    var g = new Guid(accountId);
                    var o = _db.CrmCustomers.Where(item => item.CrmAccountId == g).ToList();

                    result = new
                    {
                        ErrorHappened = false,
                        Message = "",
                        Customers = o
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при получении списка",
                    Customers = new List<CrmCustomer>()
                };
            }


            return Ok(result);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> AddCustomer(AddCustomerRequest data)
        {
            var result = new
            {
                ErrorHappened = false,
                Message = "",
                Customer = new CrmCustomer()
            };

            try
            {
                var accountId = User.Claims.FirstOrDefault(r => r.Type == ClaimsEnum.CrmAccountId)?.Value;
                var userId = User.Claims.FirstOrDefault(r => r.Type == ClaimsEnum.CrmUserId)?.Value;

                if (!string.IsNullOrEmpty(accountId) && !string.IsNullOrEmpty(userId))
                {
                    var customer = new CrmCustomer();
                    customer.Created = DateTime.Now;
                    customer.AuthorCrmUserId = new Guid(userId);
                    customer.CrmAccountId = new Guid(accountId);

                    customer.CustomerFirstName = data.FirstName;
                    customer.CustomerPhone = data.Phone;
                    customer.CustomerEmail = data.Email;

                    _db.CrmCustomers.Add(customer);
                    await _db.SaveChangesAsync();
                    result = new
                    {
                        ErrorHappened = false,
                        Message = "Клиент добавлен",
                        Customer = customer
                    };
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка при добавлении клиента",
                    Customer = new CrmCustomer()
                };

            }



            return Ok(result);
        }

    }

    public class AddCustomerRequest
    {
        public string FirstName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
