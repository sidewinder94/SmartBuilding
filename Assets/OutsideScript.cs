using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Security.Policy;
using JetBrains.Annotations;

public class OutsideScript : MonoBehaviour, INotifyPropertyChanged
{


    public Material DaySkybox;
    public Material NightSkybox;

    [SerializeField]
    private float _temperature = 10.0f;
    public int Pressure = 101325;
    public float Humidity = 0.76f;
    [SerializeField]
    private float _luminosity = 500.0f;

    private Light _sun;

    public float Temperature
    {
        get { return _temperature; }
        set
        {
            if (_temperature == value) return;
            _temperature = value;
            OnPropertyChanged("Temperature");
        }
    }

    public float Luminosity
    {
        get { return _luminosity; }
        set
        {
            if (_luminosity == value) return;
            _luminosity = value;
            RenderSettings.skybox = _luminosity > 320 ? DaySkybox : NightSkybox;
            OnPropertyChanged("Luminosity");
        }
    }

    public float NormalizedLuminosity
    {
        set
        {
            _sun.intensity = value * 2;
            RenderSettings.ambientIntensity = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        _sun = GameObject.Find("DirectionalSun").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
