using Robot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace robot_head
{
    public class GlobalData
    {
        public static readonly string TELEPRESENCE_URL
                = @"https://telepresence-np.firebaseapp.com";

        public static readonly string ListeningModeImg = "wf.gif";
        public static readonly string TeleModeImg = "telemode.png";
        public static readonly string RoboticsAndAIImg = "RoboticsAndAI.png";
        public static readonly string ShortCoursesImg = "ShortCourses.png";
        public static readonly string ChatBotActivationImg = "Activate.png";

        public static readonly string RobotName = ConfigurationManager.AppSettings["RobotName"];

        public static readonly string ROS_IP = "192.168.31.200:9090";
       
    }
}
