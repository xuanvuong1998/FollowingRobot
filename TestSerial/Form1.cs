using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Robot;

namespace TestSerial
{
    public partial class Form1 : Form
    {
        int count = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void ContextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LattePandaCommunication.Initalise();
            LattePandaCommunication.Start();
            LattePandaCommunication.onLatteDataReceived += LattePandaCommunication_onLatteDataReceived;
        }
        private void LattePandaCommunication_onLatteDataReceived(object sender, LatteDataReceivedEventArgs e)
        {
            MessageBox.Show(e.Message);
            //if (!string.IsNullOrWhiteSpace(e.Message) && e.Message != "\n")
            //Debug.WriteLine(++count + ": " + e.Message );
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            for(int i = 1; i<= 1; i++)
            {
                GlobalFlowControl.SendToBase("MESSAGE FROM HEAD");
                //GlobalFlowControl.SendToBase("Telepresence", "StartTele");
                //Thread.Sleep(10);
            }

            //MessageBox.Show("Finished");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
