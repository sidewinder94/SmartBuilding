using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RoomScript : MonoBehaviour
{
    private List<GameObject> _roomLights;
    public String RoomTag;

    private PlayerScript _player = null;

    // Use this for initialization
    void Start()
    {
        _roomLights = GameObject.FindGameObjectsWithTag(RoomTag).Where(o => o.layer == 8).ToList();
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
        _roomLights.ForEach(rl => rl.GetComponent<Light>().intensity = 8.0f);
    }

    void OnTriggerStay(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " is within my boundaries");

        //See if there is no better method
        //foreach (var li in _roomLights.Select(rl => rl.GetComponent<Light>()).Where(l => l.intensity != 8.0f))
        //{
        //    li.intensity = 8.0f;
        //}

    }

    void OnTriggerExit(Collider collision)
    {
        Debug.Log(name + " " + collision.name + " exited my boundaries");
        _player = null;
        _roomLights.ForEach(rl => rl.GetComponent<Light>().intensity = 0.0f);
    }
}
