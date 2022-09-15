using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed = 1;
    Vector3 currentVelocity;
    Rigidbody2D rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        currentVelocity = transform.up* bulletSpeed;
        rigidbody.velocity = currentVelocity;
        Collider2D sdfsdf;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Shootable>()!= null)
        {
            collision.gameObject.GetComponent<Shootable>().Scored();
        }
        else
        {
            rigidbody.velocity = Vector3.Reflect(currentVelocity, collision.contacts[0].normal);
            currentVelocity = rigidbody.velocity;
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
