using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    internal class RequestInfo
    {
        public string Method { get; set; }

        public string Path { get; set; }

        public string Body { get; set; }

        public string Query { get; set; }

        public RequestInfo(string method, string path, string body, string query)
        {
            Method = method;
            Path = path;
            Body = body;
            Query = query;
        }
    }
}
