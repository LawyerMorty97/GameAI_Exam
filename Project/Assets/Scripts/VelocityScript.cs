using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityScript : MonoBehaviour
{
    public float StartSpeed = 50f;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb;
        if (GetComponent<Rigidbody>() == null)
            rb = gameObject.AddComponent<Rigidbody>();
        else
            rb = gameObject.GetComponent<Rigidbody>();

        rb.velocity = new Vector3(StartSpeed, 0f, StartSpeed);
    }
}
