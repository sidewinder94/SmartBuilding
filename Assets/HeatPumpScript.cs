﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using JetBrains.Annotations;
using Object = UnityEngine.Object;

public class HeatPumpScript : MonoBehaviour, INotifyPropertyChanged
{

    public double TotalPower = 9.0f;

    private readonly Mutex _mutex = new Mutex();
    private readonly Dictionary<Object, double> _allocatedPower = new Dictionary<Object, double>();
    private double _usedPower;

    public HeatPumpScript()
    {
        UsedPower = 0.0f;
    }

    public double UsedPower
    {
        get { return _usedPower; }
        private set
        {
            if (_usedPower == value) return;
            _usedPower = value;
            OnPropertyChanged("UsedPower");
        }
    }

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
        if (used == 0.0) return;

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
            _allocatedPower[sender] = val;

            if (val == 0.0)
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
