using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataShardPort
{
    public partial class showContents : Form
    {
        private DriveInfo shard;
        public showContents(DriveInfo shard, int screenId)
        {
            this.Location = Screen.AllScreens[screenId].WorkingArea.Location;
            this.shard = shard;
            InitializeComponent();
        }

        private void showContents_Load(object sender, EventArgs e)
        {
            try
            {
                webBrowser1.Url = new Uri("file:///" + shard.RootDirectory + "index.html");
            }
            catch { }
        }

        private void iloop_Tick(object sender, EventArgs e)
        {
            if (!Directory.Exists(shard.RootDirectory.ToString()))
                this.Close();
        }
    }
}
