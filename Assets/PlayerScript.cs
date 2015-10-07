using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class PlayerScript : MonoBehaviour, INotifyPropertyChanged
{
    public double Temperature = 37.5f;
    private RoomScript _currentRoom;

    public RoomScript CurrentRoom
    {
        get { return _currentRoom; }
        private set
        {
            if (_currentRoom == value) return;
            _currentRoom = value;
            OnPropertyChanged("CurrentRoom");
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log(name + " " + collision.name);

        GameObject roomObject = null;

        if (collision.attachedRigidbody != null) //Composed room
        {
            roomObject = collision.attachedRigidbody.gameObject;
        }
        else//Simple Room
        {
            roomObject = collision.gameObject;
        }

        CurrentRoom = roomObject.GetComponent<RoomScript>();

    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    }
}
