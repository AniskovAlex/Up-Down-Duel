using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Shootable
{
    public float rotationSpeed = 1f;
    public float moveSpeed = 1f;
    Rigidbody2D rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float rotation = Input.GetAxis("Horizontal");
        if (rotation != 0)
        {
            transform.Rotate(-Vector3.forward, rotation * rotationSpeed);
        }

        float move = Input.GetAxis("Vertical");

        rigidbody.velocity = gameObject.transform.up * move * moveSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            Shoot();
        }
    }
}
