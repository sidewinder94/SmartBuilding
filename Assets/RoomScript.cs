using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;

public class RoomScript : MonoBehaviour, INotifyPropertyChanged
{
    public float TargetTemperature = 20.0f;
    public float CurrentTemperature = 15.0f;
    public float RoomSize = 0.0f;

    private List<GameObject> _roomLights;
    private PlayerScript _player = null;
    private HeatPumpScript _heatPump = null;
    private OutsideScript _outside = null;
    private Boolean _lighted = false;

    public Boolean Lighted
    {
        get { return _lighted; }
        set
        {
            if (value == _lighted) return;
            _lighted = value;
            _roomLights.ForEach(rl => rl.GetComponent<Light>().intensity = value ? 8.0f : 0.0f);
        }
    }

    // Use this for initialization
    void Start()
    {
        _heatPump = GameObject.FindGameObjectWithTag("HeatPump").GetComponent<HeatPumpScript>();
        _roomLights = GameObject.FindGameObjectsWithTag(tag).Where(o => o.layer == 8).ToList();
        _outside = GameObject.FindGameObjectWithTag("Terrain").GetComponent<OutsideScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " collided with me");
        var script = collision.gameObject.GetComponent<PlayerScript>();
        _player = script;
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " is within my boundaries");
        Lighted = true;

    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " exited my boundaries");
        _player = null;
        Lighted = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
