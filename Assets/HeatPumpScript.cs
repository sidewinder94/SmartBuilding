using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Object = UnityEngine.Object;

public class HeatPumpScript : MonoBehaviour
{

    public double TotalPower = 9.0f;

    private readonly Mutex _mutex = new Mutex();
    private readonly Dictionary<Object, double> _allocatedPower = new Dictionary<Object, double>();

    public HeatPumpScript()
    {
        UsedPower = 0.0f;
    }

    public double UsedPower { get; private set; }
    public double ActualPower { get { return TotalPower - UsedPower; } }

    public double AskPower(double required, Object sender)
    {
        double allocated;
        _mutex.WaitOne();
        if (UsedPower + required <= TotalPower)
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
            allocated = required;
        }
        else
        {
            allocated = TotalPower - UsedPower;
            UsedPower = TotalPower;
        }
        _mutex.ReleaseMutex();
        return allocated;
    }

    public void ReleasePower(double used, Object sender)
    {
        _mutex.WaitOne();
        if (_allocatedPower.ContainsKey(sender))
        {
            double val = _allocatedPower[sender];
            if (used > val)
            {
                throw new UnauthorizedAccessException();
            }

            val -= used;
            UsedPower -= used;

            if (val == 0)
            {
                _allocatedPower.Remove(sender);
            }
        }
        else
        {
            throw new UnauthorizedAccessException();
        }

        _mutex.ReleaseMutex();

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
