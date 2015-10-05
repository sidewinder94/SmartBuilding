using UnityEngine;
using System.Collections;

public class RoomScript : MonoBehaviour
{
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
        Debug.Log(collision.name + " collided with me");
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log(collision.name + " is within my boundaries");
    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision.name + " exited my boundaries");
    }
}
