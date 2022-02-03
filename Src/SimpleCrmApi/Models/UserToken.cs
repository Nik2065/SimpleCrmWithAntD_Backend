using Newtonsoft.Json;
using System;

namespace SimpleCrmApi.Models
{
    public class UserToken
    {
        public string UserEmail { get; set; }

        public string FirstName { get; set; }
        //Фамилия
        public string SecondName { get; set; }
        //Отчество
        public string ThirdName { get; set; }
        //конец действия токена
        public DateTime Expires { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}

