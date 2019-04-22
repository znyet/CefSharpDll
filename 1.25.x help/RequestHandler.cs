using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CefSharp;
using System.IO;

namespace TestCefSharp
{
    public class RequestHandler : IRequestHandler, ICookieVisitor
    {

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
        {
            return false;
        }

        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength, ref IDownloadHandler handler)
        {
            handler = new DownloadHandler(fileName);
            return true;
        }

        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {

            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            var headers = requestResponse.Request.GetHeaders();
            headers.Add("token", "objectid" + DateTime.Now.Second);
            requestResponse.Request.SetHeaders(headers);
            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, System.Net.WebHeaderCollection headers)
        {
            //throw new NotImplementedException();
        }

        public bool Visit(System.Net.Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            return true;
        }
    }




    class DownloadHandler : IDownloadHandler
    {
        private readonly string _path;
        private Stream _stream;

        public DownloadHandler(string fileName)
        {
            _path = Path.Combine(Path.GetTempPath(), fileName);
            _stream = File.Create(_path);
        }

        public bool ReceivedData(byte[] data)
        {
            _stream.Write(data, 0, data.GetLength(0));
            return true;

        }
        public void Complete()
        {
            _stream.Dispose();
            _stream = null;

            Console.WriteLine("Downloaded: {0}", _path);
        }
    }



}
