using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Assets.WebServer
{
    [ServiceContract]
    interface IHomeAutomationService
    {
        [OperationContract]
        List<int> GetRoomList();


        [OperationContract]
        String GetRoomName(int roomId);

        [OperationContract]
        List<string> GetAvailableProperties(int roomId);

        [OperationContract]
        bool SetRoomProperty(int roomId, string property, string value);
    }
}
