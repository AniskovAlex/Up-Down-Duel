using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Shootable
{
    public float rotationSpeed = 1f;
    public float moveSpeed = 1f;
    public float reloadTime = 1f;
    float currentReloadTime = 0f;
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
        if (player != null)
        {
            Vector3 direction = GetTargetDirection();
            if (direction == Vector3.zero)
                return;

            if (!Aim(direction))
                return;

            if (currentReloadTime <= 0)
            {
                Shoot();
                currentReloadTime = reloadTime;
            }
            else
                currentReloadTime -= Time.deltaTime;
        }
    }

    bool Aim(Vector3 target)
    {
        float angle = (Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg) - 90;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, rotationSpeed);
        if (transform.rotation == q) return true;
        return false;
    }

    Vector3 GetTargetDirection()
    {
        Vector3 posDirection;
        Vector3 negDirection;
        for (int i = 0; i < 180; i++)
        {
            posDirection = new Vector3(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad), 0);
            negDirection = new Vector3(Mathf.Cos(-i * Mathf.Deg2Rad), Mathf.Sin(-i * Mathf.Deg2Rad), 0);
            if (HitChecker(posDirection))
                return posDirection;
            if (HitChecker(negDirection))
                return negDirection;

        }
        return Vector3.zero;
    }

    bool HitChecker(Vector3 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (direction * 0.6f), direction);
        if (hit == true)
        {
            if (hit.collider.gameObject == player)
                return true;
            if (hit.collider.gameObject.GetComponent<Wall>() != null)
            {
                Vector3 possible = direction;
                if (RicochetHit(hit, direction, 1))
                    return true;
            }

        }
        return false;
    }

    bool RicochetHit(RaycastHit2D oldHit, Vector3 oldDirection, int count)
    {
        Vector3 newDirection = Vector3.Reflect(oldDirection, oldHit.normal);
        RaycastHit2D hit = Physics2D.Raycast(oldHit.point, newDirection);
        if (count > 5)
            return false;
        if (hit == false)
            return false;
        if (hit.collider.gameObject == player)
            return true;
        else
            return RicochetHit(hit, newDirection, count + 1);
    }
}
