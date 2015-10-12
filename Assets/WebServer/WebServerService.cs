using UnityEngine;
using System.Collections;
using System.ServiceModel;
using Assets.WebServer;

public class WebServerService : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        CommunicationState state = Service.Instance.ServiceState;
        Debug.Log(state);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
