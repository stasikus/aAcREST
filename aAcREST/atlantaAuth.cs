using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// 
/// author Kravchenko Stas. Patches team ©
///

namespace aAcREST
{
    class atlantaAuth
    {

         /*
             ALM11
             Authorization: Basic c3Rhc1Rlc3Q6dGVzdA== (base64)
             http://myd-vm04228.hpswlabs.adapps.hp.com:8080/qcbin/authentication-point/authenticate
         */

        public static int Login(string login, string pass)
        {
            int status = 0;
            string url = "";
            string xml = "";

            url = "https://qc2d.atlanta.hp.com/qcbin/authentication-point/j_spring_security_check";
            xml = "j_username=" + login + "&j_password=" + pass + "&redirect-url=http%253A%252F%252Fqc2d.atlanta.hp.com%252Fqcbin%252Frest%253Flogin-form-required%253Dy";


            Request.atlantaLogin(url, xml);
            return status;
        }

    }
}
