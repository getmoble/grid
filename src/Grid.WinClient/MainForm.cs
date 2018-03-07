using Grid.Clients.ITSync.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Grid.WinClient.Properties;

namespace Grid.WinClient
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        string GetActiveWindowTitle()
        {
            try
            {
                const int nChars = 256;
                var buff = new StringBuilder(nChars);
                var handle = GetForegroundWindow();

                if (GetWindowText(handle, buff, nChars) > 0)
                {
                    return buff.ToString();
                }

                return "";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        string GetActiveProcessFileName()
        {
            try
            {
                var hwnd = GetForegroundWindow();
                uint pid;
                GetWindowThreadProcessId(hwnd, out pid);
                var p = Process.GetProcessById((int)pid);
                return p.ProcessName;
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon.Visible = true;
            }  
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void syncSoftwaresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var assetId = Properties.Settings.Default.AssetId;
            var token = Properties.Settings.Default.Token;

            if (!string.IsNullOrEmpty(assetId) && !string.IsNullOrEmpty(token))
            {
                var systemData = new SystemInfo
                {
                    AssetId = assetId,
                    Softwares = SoftwareInfo.GetInstalledApps(),
                    Hardware = HardwareInfo.GetSystemDetails(),
                    RanOn = DateTime.Now
                };

                var snapShot = JsonConvert.SerializeObject(systemData);
                var response = DataManager.SendData(snapShot);
                
                MessageBox.Show(response ? "Sync Complete" : "Sync Failed");
            }
            else
            {
                var settingsDialog = new Settings();
                settingsDialog.ShowDialog();
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.AboutMessage);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hide();
            notifyIcon.Visible = true;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var settings = new Settings();
            settings.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }
    }
}
