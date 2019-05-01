using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityScript : MonoBehaviour
{
    private float WalkSpeed = 15f;
    private float TurnSpeed = 240f;
    private Rigidbody rb;

    private LineRenderer _lines;
    
    void Start()
    {
        rb = gameObject.AddComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        _lines = gameObject.AddComponent<LineRenderer>();
        _lines.positionCount = 2;
    }

    void Update()
    {
        float speed = WalkSpeed * Time.deltaTime;
        float rotSpeed = TurnSpeed * Time.deltaTime;
        float x = Input.GetAxis("Horizontal") * rotSpeed;
        float z = Input.GetAxis("Vertical") * speed;

        _lines.SetPosition(0, transform.position);

        _lines.SetPosition(1, (transform.position + (transform.forward * 4f)));

        transform.Translate(0f, 0f, z);
        transform.Rotate(0f, x, 0f);
    }
}
