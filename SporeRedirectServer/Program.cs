using System;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SporeRedirectServer
{
    class Program
    {
        private static TcpListener Listener { get; set; }
        private static TcpClient ServerClient { get; set; }

        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            // who gives a shit?
            return true;
        }

        static void Main(string[] args)
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslp) => true;

            HttpClient client = new HttpClient(handler);
            HttpListener listener = new HttpListener();

           
            // why is this required? *sigh*
            listener.Prefixes.Add("http://127.0.0.1/");

            listener.Start();

            while (true)
            {
                var a = listener.GetContext();

                HttpRequestMessage request = new HttpRequestMessage()
                {
                 //   RequestUri = new Uri("https://159.153.64.91")
                };

                // HttpRequestMessage -> HttpRequestHeaders
               
                foreach (string key in a.Request.Headers.AllKeys)
                {
                    string value = a.Request.Headers.Get(key);
                    request.Headers.TryAddWithoutValidation(key, value);

                    Console.WriteLine($"Client: {key}: {value}");
                }

                Console.WriteLine(a.Request.ContentLength64);

                request.RequestUri = a.Request.Url;

                Console.WriteLine(a.Request.Url.OriginalString);
                Console.WriteLine(request.RequestUri.AbsoluteUri);

                if (!request.RequestUri.AbsoluteUri.Contains("community.spore.com") || request.RequestUri.AbsoluteUri == "http://community.spore.com/community/assetBrowser/home")
                    request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("http", "https"));

                HttpResponseMessage response = client.SendAsync(request).GetAwaiter().GetResult();

                HttpListenerResponse res = a.Response;

                // HttpResponseMessage -> HttpListenerResponse
                res.Headers.Clear();

                Console.WriteLine((int)response.StatusCode);
                res.StatusCode = (int)response.StatusCode;

                foreach (var header in response.Headers)
                {
                    Console.WriteLine($"Server: {header.Key}: {String.Join(' ', header.Value)}");
                    res.AddHeader(header.Key, String.Join(' ', header.Value));
                }


                // copy data aswell..
                if (response.Content.Headers.ContentLength > 0)
                {
                    Console.WriteLine($"Server: {response.Content.Headers.ContentType.ToString()}");
                    res.ContentType = response.Content.Headers.ContentType.ToString();
                    response.Content.ReadAsStreamAsync().GetAwaiter().GetResult().CopyTo(res.OutputStream);
                }

                Console.WriteLine(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());


                res.OutputStream.Close();
            }
        }
    }
}
