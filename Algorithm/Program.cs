using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net;
using System.Runtime.InteropServices;
using Fiddler;
using System.Threading;


using System.Security;
using Microsoft.SharePoint.Client;

namespace Algorithm
{
    class Program
    {
        public static void Main()
        {
            #region 获取一条链接的 Response Headers 参数

            //Dictionary<string, string> Headers = GetHTTPResponseHeaders("http://www.sharejs.com");
            //GetHtmlPage("http://cosnews.cosco.com/search?channelid=12168&searchword=%u62A5%u540D=%D6%D0%B9%FA%D4%B6%D1%F3%BA%A3%D4%CB%B1%A8*%u65E5%u671F=2018.01.05*%u7248%u6B212=A01");
            //foreach (string HeaderKey in Headers.Keys)
            //    Console.WriteLine("{0}: {1}", HeaderKey, Headers[HeaderKey]);
            #endregion

            #region 委托的实现案例
            //技术方案实现，一般有三种实现法，分别是：一般是宪法、用接口实现、用委托实现
            //委托的实现案例
            //一般实现法—控制台实现
            //Console.WriteLine(GetGreetingContens("小王", "Chinese"));
            //Console.WriteLine(GetGreetingContens("Alan_beijing", "English"));
            //Console.WriteLine(GetGreetingContens("Linda", "Russian"));
            //Console.ReadKey();
            #endregion

            #region 测试获取cookie方法
            //string url = string.Format("http://www.sxncb.com/");
            //string cookie = GetCookieStrings(url);
            #endregion

            FiddlerDemo2();
            Console.ReadKey();
        }


        #region 既然IE的API都可以这么调用，那么fiddler的如果有找到API照理说也是可以调用的

        /// <summary>
        /// demo 来源：http://blog.csdn.net/zhang116868/article/details/49406599
        /// </summary>
        public static void FiddlerDemo()
        {
            //设置一个监听接口
            Fiddler.FiddlerApplication.Startup(8877, true, true);

            //这是别名，具体不知道干嘛
            Fiddler.FiddlerApplication.SetAppDisplayName("FiddlerCoreDemoApp");

            //Starfaction
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;

            //定义端口
            int iPort = 8877;

            //启动代理程序，开始监听HTTP请求
            //端口是否使用Windows系统代理，如果为true,系统所以的HTTP访问都会使用该代理
            Fiddler.FiddlerApplication.Startup(iPort, true, false, true);

            //我们还要创建一个HTTPS监听器，当FiddlerCore被伪装成HTTPS服务器有用
            //而不是作为一个正常的CERN样式代理服务器，PS：CERN是什么鬼？我至今不明白笔者这个简写名称
            
           
        }

        /// <summary>
        /// demo 来源：http://www.knowsky.com/898235.html
        /// 该类跑完异常后浏览器就自动打开了代理导致本地在不手动修改去掉那个代理选项就不能正常的浏览网页，具体原因不明白
        /// </summary>
        public static void FiddlerDemo2()
        {
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;

            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            FiddlerApplication.Startup(9898, FiddlerCoreStartupFlags.Default | FiddlerCoreStartupFlags.RegisterAsSystemProxy);
            try
            {
                ClientContext context = new ClientContext("http://www.knowsky.com/898235.html");
                //string  context = string.Format("http://www.knowsky.com/898235.html");

                SecureString se = new SecureString();
                foreach (var cc in "passWord")
                {
                    se.AppendChar(cc);
                }

                //var cre = new SharePointOnlineCredentials("user@domain.onmicrosoft.com", se);
                var cre = new SharePointOnlineCredentials("user@domain.onmicrosoft.com", se);
                var cookie = cre.GetAuthenticationCookie(new Uri("http://www.knowsky.com/898235.html"));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            FiddlerApplication.Shutdown();
            Console.ReadLine();
        }


        static void FiddlerApplication_BeforeResponse(Session oSession)
        {
            //想如何改写Response信息在这里随意发挥了
            Console.WriteLine("BeforeResponse: {0}", oSession.responseCode);
        }

        static void FiddlerApplication_BeforeRequest(Session oSession)
        {
            //想如何改写Request信息在这里随意发挥了
            Console.WriteLine("BeforeRequest: {0}, {1}", oSession.fullUrl, oSession.responseCode);
        }

        #endregion


        #region 获取cookie的方法

        //调用IE的API
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref int pcchCookieData, int dwFlags, IntPtr lpReserved);

        //这个是前辈给的，我没有直接用，只是把最后那个参数的类型从object改成了IntPtr
        //static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);




        private static string GetCookieStrings(string url)
        {
            int datasize = 1024;
            StringBuilder cookieData = new StringBuilder((int)datasize);

            try
            {
                // Determine the size of the cookie      

                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                {
                    if (datasize < 0) return null;
                    // Allocate stringbuilder large enough to hold the cookie    
                    cookieData = new StringBuilder((int)datasize);
                    if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                        return null;
                }
               

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return cookieData.ToString();
        }

        //private static string GetCookieString(string url)
        //{
        //    int capacity = 0x100;
        //    StringBuilder pchCookieData = new StringBuilder(capacity);
        //    if (!InternetGetCookieEx(url, null, pchCookieData, ref capacity, 0x2000, null))
        //    {
        //        if (capacity < 0)
        //        {
        //            return null;
        //        }
        //        pchCookieData = new StringBuilder(capacity);
        //        if (!InternetGetCookieEx(url, null, pchCookieData, ref capacity, 0x2000, null))
        //        {
        //            return null;
        //        }
        //    }
        //    return pchCookieData.ToString();
        //}
        #endregion


        #region 根据用户名和语言，获取问候词
        public static string GetGreetingContens(string UserName, string Language)
        {
            //new 一个GetGreeToUser对象
            GreetToUsers greetToUsers = new GreetToUsers();
            //当然，也可以使用switch开发语句来代替如下的if...else...
            if (Language == "Chinese")
            {
                return greetToUsers.ChinesePeople(UserName);
            }
            else if (Language == "English")
            {
                return greetToUsers.EnglishPeople(UserName);
            }
            else
            {
                return "抱歉，当前系统只支持汉语和英语（Sorry,the current system only supports Chinese and English.）";
            }
        }
        #endregion

        #region 获取服务器的返回报文
        public static void GetMain()
        {
            Dictionary<string, string> Headers = GetHTTPResponseHeaders("http://www.sharejs.com");
        }
        public static Dictionary<string, string> GetHTTPResponseHeaders(string Url)
        {
            Dictionary<string, string> HeaderList = new Dictionary<string, string>();

            WebRequest WebRequestObject = WebRequest.Create(Url);
            WebResponse ResponseObject = WebRequestObject.GetResponse();

            foreach (string HeaderKey in ResponseObject.Headers)
                HeaderList.Add(HeaderKey, ResponseObject.Headers[HeaderKey]);

            ResponseObject.Close();

            return HeaderList;
        }
        #endregion


        #region //等同于download的方法
        public static void GetHtmlPage(string strURL)
        {
            try
            {
                String strResult;
                System.Net.WebResponse objResponse;

                System.Net.WebRequest objRequest = System.Net.HttpWebRequest.Create(strURL);

                objResponse = objRequest.GetResponse();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(objResponse.GetResponseStream()))
                {
                    strResult = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        #endregion

        #region cookie 帮助类，测试的，具体功能至今没明白，网上弄下来的
        /// <summary>  
        /// Cookie操作帮助类  
        /// </summary>  
        public static class HttpCookieHelper
        {
            /// <summary>  
            /// 根据字符生成Cookie列表  
            /// </summary>  
            /// <param name="cookie">Cookie字符串</param>  
            /// <returns></returns>  
            public static List<CookieItem> GetCookieList(string cookie)
            {
                List<CookieItem> cookielist = new List<CookieItem>();
                foreach (string item in cookie.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (Regex.IsMatch(item, @"([\s\S]*?)=([\s\S]*?)$"))
                    {
                        Match m = Regex.Match(item, @"([\s\S]*?)=([\s\S]*?)$");
                        cookielist.Add(new CookieItem() { Key = m.Groups[1].Value, Value = m.Groups[2].Value });
                    }
                }
                return cookielist;
            }

            /// <summary>  
            /// 根据Key值得到Cookie值,Key不区分大小写  
            /// </summary>  
            /// <param name="Key">key</param>  
            /// <param name="cookie">字符串Cookie</param>  
            /// <returns></returns>  
            public static string GetCookieValue(string Key, string cookie)
            {
                foreach (CookieItem item in GetCookieList(cookie))
                {
                    if (item.Key == Key)
                        return item.Value;
                }
                return "";
            }
            /// <summary>  
            /// 格式化Cookie为标准格式  
            /// </summary>  
            /// <param name="key">Key值</param>  
            /// <param name="value">Value值</param>  
            /// <returns></returns>  
            public static string CookieFormat(string key, string value)
            {
                return string.Format("{0}={1};", key, value);
            }
        }

        /// <summary>  
        /// Cookie对象  
        /// </summary>  
        public class CookieItem
        {
            /// <summary>  
            /// 键  
            /// </summary>  
            public string Key { get; set; }
            /// <summary>  
            /// 值  
            /// </summary>  
            public string Value { get; set; }
        }
        #endregion

    }

    internal class SharePointOnlineCredentials
    {
        private SecureString se;
        private string v;

        public SharePointOnlineCredentials(string v, SecureString se)
        {
            this.v = v;
            this.se = se;
        }

        internal object GetAuthenticationCookie(Uri uri)
        {
            throw new NotImplementedException();
        }
    }

    #region 类方法
    public class GreetToUsers
    {
        //Chinese People
        public string ChinesePeople(string UserName)
        {
            string GreetToUsers = "您好！" + UserName;
            return GreetToUsers;
        }

        //English People
        public string EnglishPeople(string UserName)
        {
            string GreetToUsers = "Hello!" + UserName;
            return GreetToUsers;
        }
    }
    #endregion

}