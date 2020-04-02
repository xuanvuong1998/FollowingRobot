using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace robot_head
{
    class FollowingPerson
    {
        const string outputFile = @"C:\RobotReID\output\output_robot.txt";
        const double D = 120, X = 330;
        const double MIN_LINEAR = 0.15, MAX_LINEAR = 0.7;
        const double MIN_ANGULAR = 0.4, MAX_ANGULAR = 1.2;

        const double P_Linear = 0.015, P_Angular = 0.006; // propotion
        public static void FollowTarget(double d, double x)
        {
            double dDelta = d - D; // Positive: turn right, negative: turn left
            double xDelta = x - X;

            double linearSpeed = dDelta * P_Linear;
            double angularSpeed = xDelta * P_Angular;

            int flag = angularSpeed > 0 ? -1 : 1;

            if (angularSpeed < 0) angularSpeed *= -1;

            //linearSpeed = Math.Max(linearSpeed, MIN_LINEAR);
            linearSpeed = Math.Min(linearSpeed, MAX_LINEAR);

            //angularSpeed = Math.Max(angularSpeed, MIN_ANGULAR);
            angularSpeed = Math.Min(angularSpeed, MAX_ANGULAR);

            if (linearSpeed < MIN_LINEAR) linearSpeed = 0;
            if (angularSpeed < MIN_ANGULAR) angularSpeed = 0;

            Debug.WriteLine("Linear : " + linearSpeed + ", " +
                    "Angular: " + angularSpeed * flag);

            BaseHelper.Move(linearSpeed, angularSpeed * flag);
            // -1: in ROS, negative means left, positive means right
        }


        private static void Wait(int miliSec)
        {
            Thread.Sleep(miliSec);
        }

        private static string ReadData()
        {
            string result;

            try
            {
                using (var stream = File.Open(outputFile, FileMode.Open,
                    FileAccess.Read, FileShare.ReadWrite))
                {
                    result = File.ReadAllText(outputFile);
                }
            }
            catch (Exception)
            {

                return null;
            }

            return result;
        }

        public static void ReadChanges()
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                GlobalFlowControl.Robot.IsFollowing = true;
                do
                {
                    string data = ReadData();

                    if (data == null)
                    {
                        Debug.Write("Permission denied");
                        continue;
                    }

                    double d = double.Parse(data.Split('%')[1]); // meter
                    double x = double.Parse(data.Split('%')[0].Substring(1));

                    if (d == -1) d = D; // stop linear
                    else d *= 100;

                    if (x == -1) x = X; // stop angular

                    Debug.WriteLine("d : " + d + ", x : " + x);

                    FollowTarget(d, x); // meter to cm
                    
                    Wait(300);
                } while (GlobalFlowControl.Robot.IsFollowing);
            }));

            thread.Start();
        }
    }
}
