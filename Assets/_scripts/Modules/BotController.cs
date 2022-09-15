using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Shootable
{
    public float rotationSpeed = 1;
    public float moveSpeed = 1;
    Rigidbody2D rigidbody;

    GameObject player = null;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player!= null)
        {
            Vector3 vectorToTarget = player.transform.position - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * rotationSpeed);

        }
    }
}
