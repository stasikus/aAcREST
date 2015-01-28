using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Diagnostics;
using System.Security.Cryptography;

/// 
/// author Kravchenko Stas. Patches team ©
///

namespace aAcREST
{
    class Request
    {

        public static void atlantaLogin(string url, string xml)
        {
            try
            {
                string cookie = "";
                string cookie1 = "";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("http://qc2d.atlanta.hp.com/qcbin/rest?login-form-required=y");
                req.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; WOW64; Trident/6.0)";
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("Accept-Encoding: ");
                req.Headers.Add("Accept-Language: en-US,en;q=0.8");
                req.Headers.Add("Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3");
                req.AllowAutoRedirect = false;
                req.KeepAlive = false;
                req.Method = "GET";
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                cookie = res.Headers.Get("Set-Cookie");


                HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(xml.ToString());
                req1.Method = "POST";
                req1.ContentType = "application/x-www-form-urlencoded";
                req1.Accept = "application/x-ms-application, image/jpeg, application/xaml+xml, image/gif, image/pjpeg, application/x-ms-xbap, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword";
                req1.Referer = "https://qc2d.atlanta.hp.com/qcbin/authentication-point/login.jsp?redirect-url=http%3A%2F%2Fqc2d.atlanta.hp.com%2Fqcbin%2Frest%3Flogin-form-required%3Dy";
                req1.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/5.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E; InfoPath.3)";
                req1.Host = "qc2d.atlanta.hp.com";
                req1.Headers.Add("Cookie", cookie);
                req1.ContentLength = requestBytes.Length;
                req1.KeepAlive = true;
                req1.AllowAutoRedirect = false;
                Stream requestStream = req1.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res1 = (HttpWebResponse)req1.GetResponse();
                cookie1 = res1.Headers.Get("Set-Cookie");

                myMethod.statusAtlanta = "OK";
                myMethod.cookieAtlanta = cookie1;
            }
            catch (Exception)
            {
                myMethod.statusAtlanta = "NOK";
                 //throw;
            }
            
        }

        public static string centerLogin(string url, string xml)
        {
            try
            {
                string LWSSO_COOKIE_KEY = "";
                string QCSessionCookie = "";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                byte[] requestBytes = System.Text.Encoding.UTF8.GetBytes(xml.ToString());
                req.Method = "POST";
                req.ContentType = "application/xml";
                req.Accept = "application/xml";
                req.KeepAlive = true;
                req.AllowAutoRedirect = true;
                req.ContentLength = requestBytes.Length;
                Stream requestStream = req.GetRequestStream();
                requestStream.Write(requestBytes, 0, requestBytes.Length);
                requestStream.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);
                LWSSO_COOKIE_KEY = res.Headers.Get("Set-Cookie");
                sr.Close();
                res.Close();

                string urlCreateSession = "http://mydtqc02.isr.hp.com:8080/qcbin/rest/site-session";
                HttpWebRequest reqCreateSess = (HttpWebRequest)WebRequest.Create(urlCreateSession);
                reqCreateSess.Method = "POST";
                reqCreateSess.KeepAlive = true;
                reqCreateSess.AllowAutoRedirect = true;
                reqCreateSess.Headers.Add("Cookie", LWSSO_COOKIE_KEY);
                HttpWebResponse res1 = (HttpWebResponse)reqCreateSess.GetResponse();
               
                string[] pieces = res1.GetResponseHeader("Set-Cookie").Split('=');
                QCSessionCookie = pieces[1];
                QCSessionCookie = QCSessionCookie.Remove(QCSessionCookie.Length - 5);

                res1.Close();

                LWSSO_COOKIE_KEY = "QCSession=" + QCSessionCookie + ";" + LWSSO_COOKIE_KEY;
                myMethod.cookieCenter = LWSSO_COOKIE_KEY;

                myMethod.statusCenter = "OK";

                return LWSSO_COOKIE_KEY;
            }
            catch (Exception)
            {
                myMethod.statusCenter = "NOK";
                return null;
                //throw;
            }
            
        }

        public static string GetUserByName(string url, string nameSearch)
        {
            string email = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.Accept = "application/xml";
                req.KeepAlive = true;
                req.AllowAutoRedirect = true;
                req.Headers.Add("Cookie", myMethod.cookieCenter);
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();

                try
                {
                    XDocument xDoc = XDocument.Load(res.GetResponseStream());
                    XElement xmlNode = xDoc.Element("Users").Elements("User").First(e => e.Attribute("Name").Value == nameSearch);
                    if (xmlNode != null)
                    {
                        email = xmlNode.Element("email").Value;
                        myMethod.username = xmlNode.FirstAttribute.Value;
                        GetAtlantaUserName(email);
                    }
                    else
                    {
                        email = "N/A";
                        myMethod.username = "N/A";
                    }

                    res.Close();

                }
                catch (Exception)
                {

                    MessageBox.Show("User doesn't exist");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Some thing going wrong... mb, you are not logined.");
            }
           
            return email;
        }

        public static void GetAtlantaUserName(string email)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://qc2d.atlanta.hp.com/qcbin/rest/domains/BTO/projects/ALM/customization/users");
            req.Method = "GET";
            req.Accept = "application/xml";
            req.KeepAlive = true;
            req.AllowAutoRedirect = true;
            req.Headers.Add("Cookie", myMethod.cookieAtlanta);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();

            XDocument xDoc = XDocument.Load(res.GetResponseStream());
            XElement xmlNode = xDoc.Element("Users").Elements("User").First(e => e.Element("email").Value == email);

            if (xmlNode != null)
            {
                myMethod.atlantaUser = xmlNode.FirstAttribute.Value;
            }
            else
            {
                myMethod.atlantaUser = "N/A";
                myMethod.username = "N/A";
            }
        }
    }
}
