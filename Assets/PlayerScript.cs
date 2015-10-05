using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public double Temperature = 37.5f;

    public float Speed = 6.0f;
    public float JumpSpeed = 8.0f;
    public float Gravity = 20.0f;

    private Vector3 _moveDirection = Vector3.zero;


    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var chr = GetComponent<CharacterController>();
        var rb = GetComponent<Rigidbody>();
        //chr.Move(rb.position.z > 10 ? new Vector3(0.0f, 0.0f, -20.0f) : new Vector3(0.0f, 0.0f, 0.1f));
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _moveDirection = transform.TransformDirection(_moveDirection);
        _moveDirection *= Speed;
        chr.Move(_moveDirection * Time.deltaTime);


    }

}
