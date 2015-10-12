using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    private OutsideScript _outside;
    private PlayerScript _player;
    private RoomScript _currentRoom;
    private HeatPumpScript _heatPump;

    public RoomScript PlayerCurrentRoom
    {
        get { return _currentRoom; }
        private set
        {
            if (_currentRoom == value) return;
            if (_currentRoom != null) _currentRoom.PropertyChanged -= CurrentRoomOnPropertyChanged;
            if (value != null) value.PropertyChanged += CurrentRoomOnPropertyChanged;
            _currentRoom = value;

        }
    }

    // Use this for initialization
    void Start()
    {
        _outside = GameObject.FindGameObjectWithTag("Terrain").GetComponent<OutsideScript>();
        _outside.PropertyChanged += OutsideOnPropertyChanged;
        OutsideOnPropertyChanged(_outside, new PropertyChangedEventArgs("Temperature"));

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        _player.PropertyChanged += PlayerOnPropertyChanged;

        _heatPump = GameObject.FindGameObjectWithTag("HeatPump").GetComponent<HeatPumpScript>();
        _heatPump.PropertyChanged += HeatPumpOnPropertyChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OutsideOnPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        GameObject.Find("extTempText").GetComponent<Text>().text = ((OutsideScript)sender).Temperature + " °C";
    }

    private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        PlayerCurrentRoom = ((PlayerScript)sender).CurrentRoom;
        CurrentRoomOnPropertyChanged(sender, propertyChangedEventArgs);
    }

    private void CurrentRoomOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {

        if (propertyChangedEventArgs.PropertyName == "CurrentRoom")
        {
            sender = PlayerCurrentRoom;
        }

        if (sender == null) return;

        GameObject.Find("targetTempText").GetComponent<Text>().text = ((RoomScript)sender).TargetTemperature + " °C";
        GameObject.Find("currentTempText").GetComponent<Text>().text = ((RoomScript)sender).CurrentTemperature + " °C";
        GameObject.Find("heatingSpeedText").GetComponent<Text>().text = ((RoomScript)sender).HeatingSpeed + " °C/s";
        GameObject.Find("currentRoomPowerText").GetComponent<Text>().text = ((RoomScript)sender).AllocatedPower + " kW";

    }

    private void HeatPumpOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
        GameObject.Find("pumpCapacityText").GetComponent<Text>().text = String.Format("{0}/{1} kW", _heatPump.ActualPower,
            _heatPump.TotalPower);
    }

}
