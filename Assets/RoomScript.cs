using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour
{

    private PlayerScript _player = null;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " exited my boundaries");
        _player = null;
    }
}
