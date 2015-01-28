using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// 
/// author Kravchenko Stas. Patches team ©
///

namespace aAcREST
{
    class centerAuth
    {
        /*
             ALM12
             Authorization: Basic c3Rhc1Rlc3Q6dGVzdA== (base64)
            http://myd-vm04486.hpswlabs.adapps.hp.com:8080/qcbin/authentication-point/alm-authenticate
         */

        public static int Login(string login, string pass)
        {
            int status = 0;
            string url = "";
            string xml = "";

            url = "http://mydtqc02.isr.hp.com:8080/qcbin/authentication-point/alm-authenticate";
            xml = "<?xml version='1.0' encoding='utf-8'?><alm-authentication><user>" + login + "</user><password>" + pass + "</password></alm-authentication>";

            Request.centerLogin(url, xml);
            return status;
        }

        public static string Sync(string email)
        {
            string username = "";
            string url = "";
            
            url = "http://mydtqc02.isr.hp.com:8080/qcbin/rest/domains/MERCURY/projects/QC/customization/users";

            Request.GetUserByName(url, email);
            return username;
        }
    }
}
