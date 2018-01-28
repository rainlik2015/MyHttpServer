using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyHttpServer;

namespace GuiServer
{
    public partial class AddNewSite : Form
    {
        public event Action<WebSiteConfig> SiteAddedEvent;

        public AddNewSite()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WebSiteConfig cfg = new WebSiteConfig { SiteName = siteName.Text.Trim(), Port = port.Text.Trim(), PhysicalPath = physicalPath.Text.Trim() };
            if (cfg.Save())
            {
                if (SiteAddedEvent != null)
                    SiteAddedEvent(cfg);
                this.DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                physicalPath.Text = fbd.SelectedPath;
            }
        }
    }
}
