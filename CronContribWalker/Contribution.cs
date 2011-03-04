using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.XPath;

namespace CronContribWalker
{
    public partial class Contribution : UserControl
    {
        public Contribution()
        {
            InitializeComponent();
        }

        internal static Contribution NewFromId(int r)
        {
            string url =
                "http://en.wikipedia.org/w/api.php?action=query&format=xml&prop=revisions&rvdiffto=prev&revids=" + r;

            Contribution c = new Contribution();

            HttpWebRequest hwrq = (HttpWebRequest)WebRequest.Create(url);
            hwrq.UserAgent = Form1.userAgent;
            HttpWebResponse resp = (HttpWebResponse)hwrq.GetResponse();

            XPathDocument xpd = new XPathDocument(resp.GetResponseStream());
            XPathNavigator xpn = xpd.CreateNavigator();

            c.revid = r;

            XPathNodeIterator xpni;

            xpni = xpn.Select("//page");
            xpni.MoveNext();
            XPathNavigator page = xpni.Current;

            xpni = xpn.Select("//rev");
            xpni.MoveNext();
            XPathNavigator rev = xpni.Current;

            xpni = xpn.Select("//diff");
            xpni.MoveNext();
            XPathNavigator diff = xpni.Current;

            c.title = c.labelTitle.Text = page.GetAttribute("title", "");
            c.user = rev.GetAttribute("user", "");
            c.comment = rev.GetAttribute("comment", "");
            c.diff = diff.Value;
            c.timestamp = DateTime.Parse(rev.GetAttribute("timestamp", ""));
            c.labelDate.Text = c.timestamp.ToString("yyyy/MM/dd hh:mm:ss");
            c.pagenamespace = int.Parse(page.GetAttribute("ns", ""));

            switch (c.pagenamespace)
            {
                case 0:
                    c.labelColour.BackColor = Color.FromArgb(255, 255, 255);
                    break;
                case 1:
                    c.labelColour.BackColor = Color.FromArgb(232, 232, 232);
                    break;
                case 2:
                    c.labelColour.BackColor = Color.FromArgb(255, 0, 0);
                    break;
                case 3:
                    c.labelColour.BackColor = Color.FromArgb(130, 0, 0);
                    break;
                case 4:
                    c.labelColour.BackColor = Color.FromArgb(255, 255, 0);
                    break;
                case 5:
                    c.labelColour.BackColor = Color.FromArgb(171, 171, 0);
                    break;
                case 6:
                    c.labelColour.BackColor = Color.FromArgb(0, 255, 0);
                    break;
                case 7:
                    c.labelColour.BackColor = Color.FromArgb(0, 157, 0);
                    break;
                case 8:
                    c.labelColour.BackColor = Color.FromArgb(255, 0, 255);
                    break;
                case 9:
                    c.labelColour.BackColor = Color.FromArgb(144, 0, 144);
                    break;
                case 10:
                    c.labelColour.BackColor = Color.FromArgb(0, 0, 255);
                    break;
                case 11:
                    c.labelColour.BackColor = Color.FromArgb(0, 0, 117);
                    break;
                case 12:
                    c.labelColour.BackColor = Color.FromArgb(0, 255, 255);
                    break;
                case 13:
                    c.labelColour.BackColor = Color.FromArgb(0, 155, 155);
                    break;
                case 14:
                    c.labelColour.BackColor = Color.FromArgb(149, 149, 149);
                    break;
                case 15:
                    c.labelColour.BackColor = Color.FromArgb(106, 106, 106);
                    break;
                case 100:
                    c.labelColour.BackColor = Color.FromArgb(255, 102, 0);
                    break;
                case 101:
                    c.labelColour.BackColor = Color.FromArgb(155, 62, 0);
                    break;
                case 108:
                    c.labelColour.BackColor = Color.FromArgb(80, 45, 22);
                    break;
                case 109:
                    c.labelColour.BackColor = Color.FromArgb(40, 23, 11);
                    break;
                default:
                    c.labelColour.BackColor = Color.Transparent;
                    break;
            }


            return c;
        }

        private string title, user, comment, diff;
        private DateTime timestamp;
        private int pagenamespace, revid;


        public string GetDiff()
        {
            return this.diff;
        }
    }
}
