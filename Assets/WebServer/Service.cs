using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using UnityEngine;

namespace Assets.WebServer
{
    public class Service
    {
        #region Singleton Implementation

        public static Service Instance
        {
            get { return Holder.Instance; }
        }


        private class Holder
        {
            internal static readonly Service Instance = new Service();
        }

        #endregion

        private ServiceHost _host;

        public CommunicationState ServiceState
        {
            get { return _host.State; }
        }

        private Service()
        {


            _host = new ServiceHost(typeof(HomeAutomationService), new Uri("http://0.0.0.0:8001/"));
            ServiceMetadataBehavior smb = _host.Description.Behaviors.Find<ServiceMetadataBehavior>() ??
                                          new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            _host.Description.Behaviors.Add(smb);


            _host.AddServiceEndpoint(typeof(IHomeAutomationService), new BasicHttpBinding(), "");
            _host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName,
                MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

            _host.Faulted += HostOnFaulted;
            _host.Open();

        }

        private void HostOnFaulted(object sender, EventArgs eventArgs)
        {
            Debug.Log("Host Faulted");
        }
    }
}
