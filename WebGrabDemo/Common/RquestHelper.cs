using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebGrabDemo.Common
{
    public static class RequestHelper
    {
        #region Timeout Check

        public static TResult TimeoutAfter<TResult>(this Func<TResult> func, TimeSpan timeout)
        {
            var task = Task.Run(func);
            return TimeoutAfterAsync(task, timeout).GetAwaiter().GetResult();
        }

        private static async Task<TResult> TimeoutAfterAsync<TResult>(this Task<TResult> task, TimeSpan timeout)
        {
            var result = await Task.WhenAny(task, Task.Delay(timeout));
            if (result == task)
            {
                return task.GetAwaiter().GetResult();
            }
            else
            {
                throw new TimeoutException();
            }
        }

        #endregion

        #region HttpGet

        public static string HttpGet(string url, Encoding encoding, bool exceptionBackToUpper = true)
        {
            WebResponse response = null;
            WebRequest request = null;
            string html = string.Empty;
            try
            {
                request = WebRequest.Create(url);
                //request.ContentType = "text/html; charset=gb2312";
                request.Method = "get";
                request.UseDefaultCredentials = true;
                
                var task = request.GetResponseAsync();
                WebResponse wResp = task.Result;
                var respStream = wResp.GetResponseStream();
                using (StreamReader reader = new StreamReader(respStream, encoding))
                {
                    html =  reader.ReadToEnd();
                }
                return html;
            }
            catch (Exception ex)
            {
                LogHelper.Error("Fetch The Url Exception", ex, new { Url = url });
                if (exceptionBackToUpper)
                    throw ex;
                return string.Empty;
            }
            finally
            {
                if (response != null)
                {
                    response.Dispose();
                }
                if (request != null)
                {
                    request = null;
                }
            }
        }

        public static void HttpGet(string url)
        {
            WebRequest request = null;
            request = WebRequest.Create(url);
            //request.ContentType = "text/html; charset=gb2312";
            request.Method = "get";
            request.UseDefaultCredentials = true;
            request.GetResponseAsync();            
            request = null;
        }


        //public static string HttpGet(string url, Encoding encoding, CookieCollection cookie, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        CookieContainer cookieContainer = new CookieContainer();
        //        cookieContainer.Add(cookie);
        //        request.CookieContainer = cookieContainer;
        //        response = (HttpWebResponse)request.GetResponse();
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpGet(string url, Encoding encoding, CookieContainer cookieContainer, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.CookieContainer = cookieContainer;
        //        response = (HttpWebResponse)request.GetResponse();
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpGet(string url, Encoding encoding, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        request = (HttpWebRequest)HttpWebRequest.Create(url);

        //        request.Proxy = new WebProxy(proxy, true);
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        response = (HttpWebResponse)request.GetResponse();
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception e)
        //    {
        //        if (exceptionBackToUpper)
        //            throw e;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpGet(string url, Encoding encoding, CookieCollection cookie, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.Proxy = new WebProxy(proxy, true);
        //        CookieContainer cookieContainer = new CookieContainer();
        //        cookieContainer.Add(cookie);
        //        request.CookieContainer = cookieContainer;
        //        response = (HttpWebResponse)request.GetResponse();
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpGet(string url, Encoding encoding, CookieContainer cookieContainer, string proxy, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        request = (HttpWebRequest)WebRequest.Create(url);
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.Proxy = new WebProxy(proxy, true);
        //        request.CookieContainer = cookieContainer;
        //        response = (HttpWebResponse)request.GetResponse();
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        #endregion

        #region HttpPost

        //public static string HttpPost(string posturl, string postData, Encoding encoding, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        byte[] data = encoding.GetBytes(postData);
        //        request = (HttpWebRequest)WebRequest.Create(posturl);
        //        request.AllowAutoRedirect = true;
        //        request.Method = "POST";
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = data.Length;
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(data, 0, data.Length);
        //        stream.Close();
        //        response = request.GetResponse() as HttpWebResponse;
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpPost(string posturl, string postData, Encoding encoding, CookieContainer cookieContainer, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        byte[] data = encoding.GetBytes(postData);
        //        request = (HttpWebRequest)WebRequest.Create(posturl);
        //        request.AllowAutoRedirect = true;
        //        request.Method = "POST";
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = data.Length;
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(data, 0, data.Length);
        //        stream.Close();
        //        response = request.GetResponse() as HttpWebResponse;
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        //public static string HttpPost(string posturl, string postData, Encoding encoding, CookieCollection cookie, bool exceptionBackToUpper = true, int timeOut = 20000)
        //{
        //    HttpWebResponse response = null;
        //    StreamReader reader = null;
        //    HttpWebRequest request = null;
        //    string html = string.Empty;
        //    try
        //    {
        //        byte[] data = encoding.GetBytes(postData);
        //        request = (HttpWebRequest)WebRequest.Create(posturl);
        //        request.AllowAutoRedirect = true;
        //        request.Method = "POST";
        //        request.Timeout = timeOut;
        //        request.ReadWriteTimeout = timeOut;
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = data.Length;
        //        Stream stream = request.GetRequestStream();
        //        stream.Write(data, 0, data.Length);
        //        stream.Close();
        //        response = request.GetResponse() as HttpWebResponse;
        //        reader = GetStreamReader(encoding, response);
        //        html = reader.ReadToEnd();
        //        return html;
        //    }
        //    catch (Exception ex)
        //    {
        //        if (exceptionBackToUpper)
        //            throw ex;
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //        }
        //        if (response != null)
        //        {
        //            response.Close();
        //            response.Dispose();
        //        }
        //        if (request != null)
        //        {
        //            request = null;
        //        }
        //    }
        //}

        #endregion

    }
}
