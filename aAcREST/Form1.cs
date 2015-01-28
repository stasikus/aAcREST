using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// 
/// author Kravchenko Stas. Patches team ©
///

namespace aAcREST
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void atlantaLogin_btn_Click(object sender, EventArgs e)
        {
            int intstatus;
            intstatus = atlantaAuth.Login(atlantaLogin_txt.Text, atlantaPass_txt.Text);

            if (myMethod.statusAtlanta == "OK")
                atlantaStatus_lbl.ForeColor = System.Drawing.Color.Green;
            else
                atlantaStatus_lbl.ForeColor = System.Drawing.Color.Red;

            atlantaStatus_lbl.Text = myMethod.statusAtlanta;
        }

        private void centerLogin_btn_Click(object sender, EventArgs e)
        {
            int intstatus;
            intstatus = centerAuth.Login(centerLogin_txt.Text, centerPass_txt.Text);

            if (myMethod.statusCenter == "OK")
                centerStatus_lbl.ForeColor = System.Drawing.Color.Green;
            else
                centerStatus_lbl.ForeColor = System.Drawing.Color.Red;

            centerStatus_lbl.Text = myMethod.statusCenter;
        }

        private void lookingFor_btn_Click(object sender, EventArgs e)
        {
            myMethod.atlantaUser = "N/A";
            centerAuth.Sync(email_txt.Text);
            atlantaUser_txt.Text = myMethod.atlantaUser;
            mailName_txt.Text = myMethod.username;
        }
    }
}
