using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDevKit.Net
{
    public class Http
    {
        HttpClient client;
        public Http(Uri baseAddress)
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public void Get(string requestUri)
        {
            client.GetStringAsync(requestUri);
        }
    }
}
