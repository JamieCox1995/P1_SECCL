using Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.API
{
    public interface IAPIService
    {
        public APIResponse GetAPIResponse();

        public APIResponse GetResponse(string _APIRoute, string _EndPoint);

        public APIResponse GetResponse(string _APIRoute, string _EndPoint, Dictionary<string, string> _Headers);

        public APIResponse PostResponse(string _APIRoute, string _EndPoint, string _Body);

        public APIResponse PostResponse(string _APIRoute, string _EndPoint, string _Body, Dictionary<string, string> _Headers);
    }
}
