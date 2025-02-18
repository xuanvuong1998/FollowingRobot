﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Robot;
using Timer = System.Timers.Timer;

namespace robot_head
{
    public partial class MainForm : Form
    {

        private static Timer savingTimer = new Timer();
        
        public void InitUI()
        {
          //  FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;            
            // Top Most : Don't use TopMost property. It will freeze your UI
        }
        public void ActivateChatBot()
        {
            if (GlobalFlowControl.moduleActivated) return;            
            Task.Run(() => ChatModule.Start()).ConfigureAwait(false);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadAnnc()
        {
            TelepresenceControlHandler.LoadDailyAnnouncement();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //DisplayWebFace();            
            RobotFaceBrowser.ChooseRobotIDAutoimatically();
            //SpeechGeneration.SetUp(System.Speech.Synthesis.VoiceGender.Female, System.Speech.Synthesis.VoiceAge.Child);
            InitUI();            
            //ChatModule.Init();
            //LoadAnnc();

            //SetUpBodyLink();
            //InitExcelHelper();
            //ChatModule.Init();
            //ChatModule.Start();

            BaseHelper.Connect();

            FollowingPerson.ReadChanges();
        }

        private void InitExcelHelper()
        {
            ExcelHelper.CreateTable();
            savingTimer.Interval = 1000 * 60 * 60;
            savingTimer.AutoReset = true;
            savingTimer.Elapsed += SavingTimer_Elapsed;
            savingTimer.Start();
            
        }

        private void SavingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ExcelHelper.table.Rows.Count >= 10)
            {
                ExcelHelper.ExportToFile();
            }
        }

        private void DisplayWebFace()
        {
            RobotFaceBrowser.Load(GlobalData.TELEPRESENCE_URL, "winformFuncAsync", new TelepresenceControlHandler());

            //Add browser to the form
            this.Controls.Add(RobotFaceBrowser.browser);

            //Make the browser fill the entire form
            RobotFaceBrowser.browser.Dock = DockStyle.Fill;

        }

        
      
       
        private void LattePandaCommunication_onLatteDataReceived(object sender, LatteDataReceivedEventArgs e)
        {
            dynamic syncData = null;
            string mess = e.Message;
            if (string.IsNullOrWhiteSpace(mess) || mess == "\n") return;
            Debug.WriteLine(mess);
            if (mess.Contains("SendAgain"))
            {
                GlobalFlowControl.SendToBaseAgain();
                return;
            }            
            try
            {
                syncData = JsonConvert.DeserializeObject<SynchronisationData>(mess);
                    
            }
            catch
            {               
                Debug.WriteLine("Error! Requesting to receive again");
                GlobalFlowControl.SendToBase("SendAgain");
                return;
            }            

        }

        private void MainForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            //LattePandaCommunication.Stop();
            
            if (ExcelHelper.table.Rows.Count >= 5)
            {
                MessageBox.Show("PRESS OK TO SAVE ALL THE RECORDS!");
                ExcelHelper.ExportToFile();
            }
            Environment.Exit(0);
        }

        private void RestartApplication()
        {
            Process.Start(Application.ExecutablePath);
            Process.GetCurrentProcess().Kill();
            Environment.Exit(0);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                BaseHelper.Stop();
            }
            if (e.KeyData == Keys.Enter)
            {
                GlobalFlowControl.Robot.IsFollowing = false;
                BaseHelper.Stop();
            }

            if (e.KeyData == Keys.R)
            {
                RestartApplication();
            }
        }
    }
}
