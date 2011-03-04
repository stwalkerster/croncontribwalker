using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml.XPath;

namespace CronContribWalker
{
    public partial class Form1 : Form
    {
        public const string userAgent = "CronContribWalker/0.1 ( User:Stwalkerster )";

        public Form1()
        {
            InitializeComponent();
        }

        string user = "Dusti";

        string uccontinue = "";

        private Thread t;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        string getApiUrl()
        {
            string url = "http://en.wikipedia.org/w/api.php?action=query&list=usercontribs&uclimit=50&ucprop=ids&format=xml&ucuser=" + user;
            if (uccontinue != "")
                url += "&ucstart=" + uccontinue;

            return url;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (t != null && t.ThreadState == ThreadState.Running)
            {
                MessageBox.Show("Already running");
                return;
            }

            t = new Thread(getContribsWorker);
            t.Start();
        }

        private void getContribsWorker()
        {
            string url = getApiUrl();

            HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(url);
            hwrq.UserAgent = userAgent;
            HttpWebResponse resp = (HttpWebResponse)hwrq.GetResponse();

            Stream response = resp.GetResponseStream();

            
            XPathDocument xpd = new XPathDocument(response);

            XPathNavigator xpnav = xpd.CreateNavigator();
            XPathNodeIterator xni = xpnav.Select("//usercontribs/item");

            List<int> revisions = new List<int>(500);
            revisions.AddRange(from XPathNavigator nav in xni select int.Parse(nav.GetAttribute("revid", "")));

            XPathNodeIterator xpni = xpnav.Select("//query-continue/usercontribs");
            xpni.MoveNext();
            uccontinue = xpni.Current.GetAttribute("ucstart", "");

            foreach (int r in revisions)
            {
                Contribution c = Contribution.NewFromId(r);
                addContribToGui(c);
            }
        }

        private delegate void contribDelegate(Contribution c);
        private void addContribToGui(Contribution c)
        {
            if(this.InvokeRequired)
            {
                contribDelegate dlgt = addContribToGui;
                this.BeginInvoke(dlgt, c);
            }
            else
            {
                flowLayoutPanel1.Controls.Add(c);
            }
        }

        //skip
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                currentContribution = (Contribution) flowLayoutPanel1.Controls[0];
                flowLayoutPanel1.Controls.Remove(currentContribution);
            }
            else if (flowLayoutPanel2.Controls.Count > 0)
            {
                currentContribution = (Contribution) flowLayoutPanel2.Controls[0];
                flowLayoutPanel2.Controls.Remove(currentContribution);
            }

            splitContainer1.Panel2.Controls.Clear();
            webBrowser1 = new WebBrowser();
            webBrowser1.Dock = DockStyle.Fill;
            webBrowser1.DocumentText = Properties.Resources.wikihtml + currentContribution.GetDiff() + "</table></body></html>";
            splitContainer1.Panel2.Controls.Add(webBrowser1);
        }

        //flag
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            flowLayoutPanel2.Controls.Add(currentContribution);
            currentContribution = (Contribution)flowLayoutPanel1.Controls[0];
            flowLayoutPanel1.Controls.Remove(currentContribution);

            splitContainer1.Panel2.Controls.Clear();
            webBrowser1 = new WebBrowser();
            webBrowser1.Dock = DockStyle.Fill;
            webBrowser1.DocumentText = Properties.Resources.wikihtml + currentContribution.GetDiff() + "</table></body></html>";
            splitContainer1.Panel2.Controls.Add(webBrowser1);
        }

        private Contribution currentContribution = null;

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show(webBrowser1.DocumentText);
        }

    }
}
