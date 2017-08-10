using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ValoremBot.Helpers
{
    public class WebRequestHelper
    {
        public string Request { get; set; }
        public string PostData { get; set; }
        public RequestMethod Method { get; set; }
        public bool AcceptHeaderRequired { get; set; }
        public ContentType PostContentType;
        public Dictionary<string, object> Parameters { get; set; }
        public Dictionary<string, string> Headers { get; set; }       

        public delegate void DownloadFinishedEventHandler(object sender, DownloadFinishedEventArgs de);
        public event DownloadFinishedEventHandler DownloadFinished;

        public WebRequestHelper()
        {
            Method = RequestMethod.GET;
        }

        public WebRequestHelper(RequestMethod method)
        {
            Method = method;
            if (method == RequestMethod.POST)
            {
                PostContentType = ContentType.FormData;
            }
        }

        public WebRequestHelper(RequestMethod method, ContentType postContentType)
        {
            Method = method;
            PostContentType = postContentType;
        }        

        private HttpWebRequest GetRequest()
        {

            var request = (HttpWebRequest)HttpWebRequest.Create(Request);
            if (AcceptHeaderRequired)
                request.Accept = "application/json";
            if ((Headers != null) && (Headers.Count > 0))
            {
                foreach (var item in Headers)
                {
                    request.Headers[item.Key] = item.Value.ToString();
                }
            }

            if (Method != RequestMethod.GET)
            {
                switch (Method)
                {
                    case RequestMethod.POST:
                        request.Method = "POST";
                        break;
                    case RequestMethod.PUT:
                        request.Method = "PUT";
                        break;
                    case RequestMethod.DELETE:
                        request.Method = "DELETE";
                        break;
                }
                if (PostContentType == ContentType.XML)
                    request.ContentType = "application/atom+xml";
                else if (PostContentType == ContentType.FormData)
                    request.ContentType = "application/x-www-form-urlencoded";
                else
                    request.ContentType = "application/json";
                request.ContentLength = PostData.Length;
            }
            return request;
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            // End the stream request operation            
            Stream postStream = webRequest.EndGetRequestStream(asynchronousResult);
            WriteMultipartObject(postStream, Parameters);
            // Start the web request            
            webRequest.BeginGetResponse(new AsyncCallback(ReadCallback), webRequest);
        }

        private void WriteMultipartObject(Stream stream, object data)
        {
            try
            {
                StreamWriter writer = new StreamWriter(stream);
                if (data != null)
                {
                    foreach (var entry in data as Dictionary<string, object>)
                    {
                        writer.Write(entry.Key + "=" + entry.Value);
                    }
                }

            }
            catch
            {
            }
            byte[] byteArray = Encoding.UTF8.GetBytes(PostData);
            // Add the post data to the web request            
            stream.Write(byteArray, 0, byteArray.Length);
#if NETFX_CORE
                stream.Flush();
#else
            stream.Close();
#endif
        }

        private string ReadResponse(IAsyncResult asynchronousResult, WebResponse response)
        {
            string result = "";
            string error = "";
            try
            {
                if (response == null)
                {
                    HttpWebRequest request = asynchronousResult.AsyncState as HttpWebRequest;
                    response = request.EndGetResponse(asynchronousResult);
                }
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException we)
            {
                if (we.Response == null) return "";

                if (!string.IsNullOrEmpty(we.Response.ContentType))
                {
                    using (Stream stream = we.Response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (SecurityException se)
            {
                error = se.Message;
                if (error == "")
                    error = se.InnerException.Message;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                if (error == "")
                    error = ex.InnerException.Message;
            }
            if (DownloadFinished != null)
                DownloadFinished(this, new DownloadFinishedEventArgs() { Result = result, Error = error });
            return result;
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            ReadResponse(asynchronousResult, null);
        }

        public virtual void CallWebService()
        {
            var request = GetRequest();
            if (Method == RequestMethod.GET)
            {
                request.BeginGetResponse(new AsyncCallback(ReadCallback), request);
            }
            else
            {
                request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
            }
        }

        public async Task<string> CallWebServiceAsync()
        {
            var request = GetRequest();
            WebResponse response;
            if (Method == RequestMethod.GET)
            {
                response = await request.GetResponseAsync();
            }
            else
            {
                var stream = await request.GetRequestStreamAsync();
                WriteMultipartObject(stream, Parameters);
                response = await request.GetResponseAsync();
            }

            return ReadResponse(null, response);
        }

    }

    public enum RequestMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    }

    public enum ContentType
    {
        XML,
        FormData,
        Json
    }

    public class DownloadFinishedEventArgs : EventArgs
    {
        public string Result { get; set; }
        public string Error { get; set; }
    }
}
