using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

public class RoomScript : MonoBehaviour, INotifyPropertyChanged
{
    public double TargetTemperature = 20.0f;
    public double CurrentTemperature = 15.0f;
    public double AllowedDeltaTemp = 0.2f;
    public double RoomSize = 0.0f;

    private List<GameObject> _roomLights;
    private PlayerScript _player = null;
    private HeatPumpScript _heatPump = null;
    private OutsideScript _outside = null;
    private List<GameObject> _heaters;
    private Boolean _lighted = false;
    private double _neededHeatPower;
    private double _allocatedPower;
    private double _powerLoss;


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
        _heaters = GameObject.FindGameObjectsWithTag(tag).Where(o => o.layer == 9).ToList();
        _outside = GameObject.FindGameObjectWithTag("Terrain").GetComponent<OutsideScript>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void FixedUpdate()
    {


        if (_allocatedPower > 0)
        {
            //Fin de calcul du round précédent => calcul de la vitesse de changement de temp et application de la nouvelle temp.
            double neededTime = _neededHeatPower / _allocatedPower;
            double heatingSpeed = (TargetTemperature - CurrentTemperature) / neededTime;
            double diffTemp = heatingSpeed * Time.fixedDeltaTime;
            CurrentTemperature += diffTemp;
        }
        else
        {

        }


        //Calcul de la déperdition de chaleur
        double powerLoss = RoomSize * 0.75f * (CurrentTemperature - _outside.Temperature);
        double neededPower = 0.0f;

        //Si on est dans la fourchette autorisée de la température requise
        if (TargetTemperature - CurrentTemperature < AllowedDeltaTemp)
        {
            //On ne demande que la puissance requise pour compense la déperdition
            neededPower = powerLoss;
        }
        else
        {
            //Sinon et si on doit chauffer (Target > Current)
            if (powerLoss > 0)
            {
                //Calcul de la masse volumique de l'air
                double rho = (1.0f / (287.06f * (CurrentTemperature + 273.15f))) * (_outside.Pressure - 230.617 * _outside.Humidity * Math.Exp((17.503 * CurrentTemperature) / (241.2 + CurrentTemperature)));
                //Calcul de la masse d'air dans la pièce 
                double airMass = RoomSize * rho;
                //Calcul de la puissance nécessaire pour chauffer le volume d'air
                neededPower = airMass * (TargetTemperature - CurrentTemperature);
                //On ajoute la déperdition de chaleur
                neededPower += powerLoss;
                //On définit la puissance requise pour monter a la température voulue pour le calcul au prochain round
                _neededHeatPower = neededPower;
            }
            else
            {
                //Si on a trop chaud, on relache toutes nos ressources pour tenter de profiter de la déperdition de chaleur pour faire baisser la température
                _heatPump.ReleasePower(_allocatedPower, this);
                _allocatedPower = 0.0f;
            }
        }

        //Si on a plus de ressources que nécessaire, on les relâches
        if (_allocatedPower > neededPower)
        {
            _heatPump.ReleasePower(_allocatedPower - neededPower, this);
        }
        else
        {
            //Sinon on demande ce qui nous manque à la pompe (elle donne ce qu'elle peut)
            _allocatedPower += _heatPump.AskPower(neededPower - _allocatedPower, this);
        }
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
