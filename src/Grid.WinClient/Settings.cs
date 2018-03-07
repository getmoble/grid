using System;
using System.Windows.Forms;

namespace Grid.WinClient
{
    public partial class Settings : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            txt_assetId.Text = Properties.Settings.Default.AssetId;
            txt_token.Text = Properties.Settings.Default.Token;
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AssetId = txt_assetId.Text;
            Properties.Settings.Default.Token = txt_token.Text;
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
