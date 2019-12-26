using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Management;
namespace CPU_Detect
{
    public partial class FrmSystemManagement : Form
    {
        public FrmSystemManagement()
        {
            InitializeComponent();
        }
        Process[] MyProcesses;
        Thread td;
        private void myUser()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_Processor");
            int iCPU = 0;
            int calcCPU = 0;
            int perCPU = 0;
            foreach (ManagementObject myobject in searcher.Get())
            {

                iCPU++;
                calcCPU = Convert.ToInt32(myobject["LoadPercentage"]);
                perCPU += calcCPU;
            }
            perCPU = perCPU / iCPU;
            lblCPU.Text = perCPU + " %";
            mheight = Convert.ToInt32(perCPU);
            if (mheight == 100)
                panel3.Height = 100;
            CreateImage();
            Memory();
        }

        private void Memory()
        {
            Microsoft.VisualBasic.Devices.Computer myInfo = new Microsoft.VisualBasic.Devices.Computer();
            //获取物理内存总量
            pbMemorySum.Maximum = Convert.ToInt32(myInfo.Info.TotalPhysicalMemory /1024 / 1024);
            pbMemorySum.Value = Convert.ToInt32(myInfo.Info.TotalPhysicalMemory / 1024 / 1024);
            lblSum.Text = (myInfo.Info.TotalPhysicalMemory / 1024 / 1024).ToString("###,###");
            //获取可用物理内存总量
            pbMemoryUse.Maximum = Convert.ToInt32(myInfo.Info.TotalPhysicalMemory / 1024 / 1024);
            pbMemoryUse.Value = Convert.ToInt32(myInfo.Info.AvailablePhysicalMemory / 1024 / 1024);
            lblMuse.Text = (myInfo.Info.AvailablePhysicalMemory / 1024 / 1024).ToString("###,###");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            MyProcesses = Process.GetProcesses();
            myUser();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            MyProcesses = Process.GetProcesses();
            td = new Thread(new ThreadStart(myUser));
            td.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (td != null)
            {
                td.Abort();
            }
        }
        int mheight = 0;
        private void CreateImage()
        {
            Bitmap image = new Bitmap(panel3.Width,panel3.Height);
            //创建Graphics类对象
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Green);
            SolidBrush mybrush = new SolidBrush(Color.Red);
            g.FillRectangle(mybrush,0,panel3.Height*(1- mheight * 1.0f / 100),26, panel3.Height * (mheight * 1.0f / 100));
            panel3.BackgroundImage = image;
        }
    }
}
