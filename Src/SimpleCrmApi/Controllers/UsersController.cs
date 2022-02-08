using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Common;
using DataAccess;
using DataAccess.Entities;
using Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SimpleCrmApi.Models;
using SimpleCrmApi.Services;

namespace SimpleCrmApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly NLog.Logger _logger;
        private DataStore _db;
        //private UserService _userService;
        private UserLogic _userLogic;


        public UsersController(DataStore context)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _db = context;
            //_userService = new UserService(context);
            _userLogic = new UserLogic(context);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public string Test()
        {
            return "Test complete";
        }

        //
        //TODO: доработать проверку на брутфорс и паузу между попытками
        //
        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Token([FromBody] AuthData data)
        {
            var result = new
            {
                ErrorHappened = false,
                Message = "",
            };

            try
            {
                var identityResult = GetIdentity(data.Login, data.Password);
                if (identityResult.identity == null)
                {
                    return BadRequest(new { errorText = "Invalid username or password." });
                }

                var now = DateTime.UtcNow;
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identityResult.identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                var response = new UserToken
                {
                    UserEmail = identityResult.identity.Name,
                    //UserEmail = identityResult.user?.UserEmail,
                    FirstName = identityResult.user?.FirstName,
                    SecondName = identityResult.user?.SecondName,
                    Expires = now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    AccessToken = encodedJwt
                };

                return Json(response);
            }
            catch (Exception e)
            {
                _logger.Error(e);

                result = new
                {
                    ErrorHappened = true,
                    Message = "Ошибка в процессе авторизации"
                };
            }


            return Json(result);
        }

        //TODO: доделать реализацию пароля
        private (ClaimsIdentity identity, CrmUser user) GetIdentity(string useremail, string password)
        {
            var us = new UserLogic(_db);

            var user = _db.CrmUsers
                .Include(c => c.UserRole)
                .FirstOrDefault(x => x.CrmUserEmail == useremail && x.CrmUserPassword==us.HashPassword(password));

            if (user != null)
            {
                //var role = _db.CrmUsersRoles.FirstOrDefault(r => r.Id == user.UserRole);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.CrmUserEmail),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.UserRole?.Code),
                    new Claim("CrmAccountId", user.CrmAccountId.ToString()),
                    new Claim("CrmUserId", user.CrmUserId.ToString())
                };

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return (claimsIdentity, user);
            }

            // если пользователя не найден
            return (null, null);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> RegUser([FromBody] RegData data)
        {
            var result = new
            {
                ErrorHappened = false,
                Message = ""
            };

            try
            {
                await _userLogic.RegUser(data);
            }
            catch(Exception ex)
            {
                var m = "Ошибка регистрации пользователя. Попробуйте позже";

                _logger.Error(m + ex);
                result = new
                {
                    ErrorHappened = true,
                    Message = m
                };
            }



            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("[action]")]
        public void CreateTestUser()
        {
            _userLogic.CreateTestUser();
        }
    }
}
