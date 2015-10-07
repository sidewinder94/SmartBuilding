using UnityEngine;
using System.Collections;
using System.ComponentModel;
using JetBrains.Annotations;

public class OutsideScript : MonoBehaviour, INotifyPropertyChanged
{


    public float Temperature = 10.0f;
    public int Pressure = 101325;
    public float Humidity = 0.76f;

    // Use this for initialization
    void Start()
    {

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
