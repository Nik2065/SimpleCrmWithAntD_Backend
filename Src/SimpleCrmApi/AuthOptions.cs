using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCrmApi
{
    public class AuthOptions
    {
        public const string ISSUER = "SimpleSrmServer"; // издатель токена
        public const string AUDIENCE = "SimpleCrmClient"; // потребитель токена
        const string KEY = "06389F9E-EBC0-4552-A329-E7254C510EDD";   // ключ для шифрации
        public const int LIFETIME = 60; // время жизни токена - 60 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
