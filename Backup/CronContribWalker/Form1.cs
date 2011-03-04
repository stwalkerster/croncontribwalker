using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml.XPath;

namespace CronContribWalker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string user = "Dusti";

        string uccontinue = "";

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        string getApiUrl()
        {
            string url = "http://en.wikipedia.org/w/api.php?action=query&list=usercontribs&ucuser=" + user + "&uclimit=500&ucprop=ids|title|timestamp|comment&format=xml";
            if (uccontinue != "")
                url += "&ucstart=" + uccontinue;

            return url;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string url = getApiUrl();

            HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(url);
            hwrq.UserAgent = "CronContribWalker/0.1 ( User:Stwalkerster )";
            HttpWebResponse resp = (HttpWebResponse)hwrq.GetResponse();

            Stream response = resp.GetResponseStream();

            
            XPathDocument xpd = new XPathDocument(response);

            XPathNavigator xpnav = xpd.CreateNavigator();
            XPathNodeIterator xni = xpnav.Select("//usercontribs/item");

            foreach (XPathNavigator nav in xni)
            {
                nav.GetAttribute
            }
            
        }
    }
}
