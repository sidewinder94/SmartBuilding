using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var chr = GetComponent<CharacterController>();
        chr.Move(new Vector3(0.0f, 0.0f, 0.1f));
    }

}
