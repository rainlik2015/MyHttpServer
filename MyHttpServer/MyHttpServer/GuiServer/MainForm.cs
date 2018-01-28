using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyHttpServer;
using MyHttpServer.Log;
using System.Configuration;
using MyHttpServer.Listener;

namespace GuiServer
{
    public partial class MainForm : Form
    {
        private const string STATE_RUNNING = "【Running】";
        private const string STATE_STOPPED = "【Stopped】";

        private BaseLog log = null;
        private List<HttpServer> servers = new List<HttpServer>();

        //HttpServer对象和树节点的映射字典
        private Dictionary<HttpServer, TreeNode> dic = new Dictionary<HttpServer, TreeNode>();

        public MainForm()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InitLogger();
            InitServers();
            InitTreeAndStartAllServers();
        }
        private void InitLogger()
        {
            log = new DelegateLog(LOGLEVEL.INFO, str =>
            {
                //跨线程调用
                listBox1.Invoke(new Action<string>(s =>
                {
                    listBox1.Items.Add(s);
                }), str);
            });
        }
        
        private void InitServers()
        {
            var sites = WebSiteConfig.GetAllSites();
            foreach (var site in sites)
            {
                var server = new HttpServer(site, log);
                servers.Add(server);
            }
        }
        private void SwitchNodeState(TreeNode node)
        {
            if (node.Text.Contains(STATE_RUNNING))
                node.Text = node.Text.Replace(STATE_RUNNING, STATE_STOPPED);
            else if (node.Text.Contains(STATE_STOPPED))
                node.Text = node.Text.Replace(STATE_STOPPED, STATE_RUNNING);
        }
        private void InitTreeAndStartAllServers()
        {
            foreach (var server in servers)
            {
                AddServerNode(server);
            }
        }

        private void AddServerNode(HttpServer server)
        {
            string nodeText = string.Format("{0}:{1}{2}", server.Config.SiteName,server.Config.Port, STATE_STOPPED);
            TreeNode node = new TreeNode(nodeText);
            node.Tag = server;

            ContextMenu menu = new System.Windows.Forms.ContextMenu();

            MenuItem itemStart = new MenuItem("Start");
            itemStart.Tag = server;
            itemStart.Click += new EventHandler(itemStart_Click);
            menu.MenuItems.Add(itemStart);

            MenuItem itemStop = new MenuItem("Stop");
            itemStop.Tag = server;
            itemStop.Click += new EventHandler(itemStop_Click);
            menu.MenuItems.Add(itemStop);

            MenuItem itemDelServer = new MenuItem("Delete");
            itemDelServer.Tag = server;
            itemDelServer.Click += new EventHandler(itemDelServer_Click);
            menu.MenuItems.Add(itemDelServer);

            menu.Tag = node;
            node.ContextMenu = menu;

            treeView1.Nodes.Add(node);

            dic.Add(server, node);

            server.StartServer();
            SwitchNodeState(node);
        }

        void itemDelServer_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除该网站？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var item = sender as MenuItem;
                var node = item.Parent.Tag as TreeNode;

                #region 此处应该用事务进行处理，因为以下删除操作要么全部成功，要么全部失败
                //从树状结构中删除
                treeView1.Nodes.Remove(node);
                var server = node.Tag as HttpServer;

                //从字典中删除
                dic.Remove(server);

                //停止网站并删除配置文件对应项
                server.StopServer();
                server.DelConfig();
                #endregion
            }
        }

        void itemStop_Click(object sender, EventArgs e)
        {
            var item = sender as MenuItem;
            var server = item.Tag as HttpServer;
            if (server == null)
                return;
            server.StopServer();

            var node = item.Parent.Tag as TreeNode;
            SwitchNodeState(node);
        }

        void itemStart_Click(object sender, EventArgs e)
        {
            var item = sender as MenuItem;
            var server = item.Tag as HttpServer;
            if (server == null)
                return;
            server.StartServer();

            var node = item.Parent.Tag as TreeNode;
            SwitchNodeState(node);
        }

        private void 清空日志ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewSite addSite = new AddNewSite();
            addSite.SiteAddedEvent += new Action<WebSiteConfig>(addSite_SiteAddedEvent);
            addSite.ShowDialog();
        }

        void addSite_SiteAddedEvent(WebSiteConfig obj)
        {
            var x = dic.Keys.Where(s => s.Config.Port == obj.Port).FirstOrDefault();
            if (x != null)
            {
                MessageBox.Show("端口号已占用！");
                return;
            }
            HttpServer server = new HttpServer(obj, log);
            AddServerNode(server);
        }
    }
}
