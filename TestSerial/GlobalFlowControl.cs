using Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSerial
{
    class GlobalFlowControl
    {
      

        public static SynchronisationData LatestData = null;

        public static void SendToBase(string header, string msg)
        {
            SynchronisationData syncData = SynchronisationData.PackDataSingle(header, msg);
            LatestData = syncData;
            LattePandaCommunication.SendObjectAsJson(syncData);
        }

        public static void SendToBase(string msg)
        {
            LattePandaCommunication.Send(msg);
        }
     
        public static void SendToBaseAgain()
        {
            if (LatestData != null)
            {
                LattePandaCommunication.SendObjectAsJson(LatestData);
            }            
        }

        public static void SendToBase(SynchronisationData data)
        {

            LattePandaCommunication.SendObjectAsJson(data);

        }
    }
}
