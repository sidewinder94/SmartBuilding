using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

public class HeatPumpScript : MonoBehaviour
{

    public float AvailablePower = 9.0f;

    private readonly Mutex _mutex = new Mutex();
    private readonly Dictionary<Object, float> _allocatedPower = new Dictionary<Object, float>();

    public HeatPumpScript()
    {
        UsedPower = 0.0f;
    }

    public float UsedPower { get; private set; }


    public bool AskPower(float required, Object sender)
    {
        var wasAvailable = false;
        _mutex.WaitOne();
        if (UsedPower + required <= AvailablePower)
        {
            if (!_allocatedPower.ContainsKey(sender))
            {
                _allocatedPower[sender] = required;
            }
            else
            {
                _allocatedPower[sender] += required;
            }

            UsedPower += required;
            wasAvailable = true;
        }
        _mutex.ReleaseMutex();
        return wasAvailable;
    }

    public void ReleasePower(float used, Object sender)
    {
        _mutex.WaitOne();




    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
