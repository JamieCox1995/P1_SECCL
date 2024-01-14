using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P1_SECCL_API.Classes
{
    public class Authentication
    {
        public class AuthenticationToken
        {
            public string Token { get; }
            public string UserName { get; }

            public AuthenticationToken(string _Token, string _UserName)
            {
                Token = _Token;
                UserName = _UserName;
            }
        }

        public struct AuthData
        {
            public string Token { get; set; }
            public string UserName { get; set; }
        }

        public struct AuthenticationObject
        {
            public AuthData Data { get; set; }
        }
    }
}
