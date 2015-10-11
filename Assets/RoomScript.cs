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
    [SerializeField]
    private double _targetTemperature = 20.0;
    [SerializeField]
    private double _currentTemperature = 15.0;
    public double AllowedDeltaTemp = 0.2;
    public double RoomSize = 0.0;
    public double InsulationConstant = 0.25;

    public double TargetTemperature
    {
        get { return _targetTemperature; }
        set
        {
            if (_targetTemperature == value) return;
            _targetTemperature = value;
            OnPropertyChanged("TargetTemperature");
        }
    }


    public double HeatingSpeed
    {
        get { return _heatingSpeed; }
        set
        {
            if (_heatingSpeed == value) return;
            OnPropertyChanged("HeatingSpeed");
            _heatingSpeed = value;

        }
    }

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

    public double CurrentTemperature
    {
        get { return _currentTemperature; }
        set
        {
            if (_currentTemperature == value) return;
            _currentTemperature = value;
            OnPropertyChanged("CurrentTemperature");
        }
    }

    private List<GameObject> _roomLights;
    private PlayerScript _player = null;
    private HeatPumpScript _heatPump = null;
    private OutsideScript _outside = null;
    private List<GameObject> _heaters;
    private Boolean _lighted = false;
    private double _neededHeatPower;
    private double _allocatedPower;
    private double _powerLoss;
    private double _heatingSpeed;

    private double _maxPower
    {
        get { return _heaters.Sum(h => h.GetComponent<HeaterScript>().Power); }
    }

    public double AllocatedPower
    {
        get { return _allocatedPower; }
        set
        {
            if (_allocatedPower == value) return;
            _allocatedPower = value;
            OnPropertyChanged("AllocatedPower");

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
        if (AllocatedPower > 0)
        {
            //Fin de calcul du round précédent => calcul de la vitesse de changement de temp et application de la nouvelle temp.
            double neededTime = _neededHeatPower / AllocatedPower;
            HeatingSpeed = (_targetTemperature - _currentTemperature) / neededTime;
            double diffTemp = HeatingSpeed * Time.fixedDeltaTime;
            CurrentTemperature += diffTemp;
        }
        else
        {

        }


        //Calcul de la déperdition de chaleur
        double powerLoss = RoomSize * InsulationConstant * (_currentTemperature - _outside.Temperature) / 1000;
        double neededPower = 0.0f;

        //Si on est dans la fourchette autorisée de la température requise
        if (_targetTemperature - _currentTemperature < AllowedDeltaTemp)
        {
            //On ne demande que la puissance requise pour compense la déperdition
            neededPower = powerLoss;
        }
        else
        {
            //Sinon et si on doit chauffer (Target > Current)
            if (_targetTemperature - _currentTemperature > AllowedDeltaTemp)// Replace by Target and Current
            {
                //Calcul de la masse volumique de l'air
                double rho = (1.0f / (287.06f * (_currentTemperature + 273.15f))) * (_outside.Pressure - 230.617 * _outside.Humidity * Math.Exp((17.503 * _currentTemperature) / (241.2 + _currentTemperature)));
                //Calcul de la masse d'air dans la pièce 
                double airMass = RoomSize * rho;
                //Calcul de la puissance nécessaire pour chauffer le volume d'air
                neededPower = airMass * (_targetTemperature - _currentTemperature);
                //On ajoute la déperdition de chaleur
                neededPower += powerLoss;
                //On définit la puissance requise pour monter a la température voulue pour le calcul au prochain round
                _neededHeatPower = neededPower;
            }
            else
            {
                //Si on a trop chaud, on relache toutes nos ressources pour tenter de profiter de la déperdition de chaleur pour faire baisser la température
                _heatPump.ReleasePower(AllocatedPower, gameObject);
                AllocatedPower = 0.0f;
            }
        }

        //Si on a plus de ressources que nécessaire, on les relâches
        if (AllocatedPower > neededPower)
        {
            _heatPump.ReleasePower(AllocatedPower - neededPower, gameObject);
            AllocatedPower -= (AllocatedPower - neededPower);
        }
        else
        {
            //Sinon on demande ce qui nous manque à la pompe (elle donne ce qu'elle peut), en se limitant à la capacité thermique des radiateus de la salle
            var requested = Math.Min(neededPower - AllocatedPower, _maxPower - AllocatedPower);
            AllocatedPower += _heatPump.AskPower(requested, gameObject);

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
        //Debug.Log(name + " " + collision.name + " is within my boundaries");
        Lighted = true;

    }

    void OnTriggerExit(Collider collision)
    {
        //Debug.Log(name + " " + collision.name + " exited my boundaries");
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
