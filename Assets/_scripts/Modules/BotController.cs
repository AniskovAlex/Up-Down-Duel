using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : Shootable
{
    public float rotationSpeed = 1f;
    public float moveSpeed = 1f;
    public float reloadTime = 1f;
    float currentReloadTime = 0f;
    float saveTime = 0.2f;
    float currentSaveTime = 0f;
    Rigidbody2D rigidbody;
    Collider2D threat = null;
    Collider2D colliderBody;
    ContactFilter2D contactFilter;

    bool evasion = false;
    int evationDirection = 1;
    GameObject player = null;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
        colliderBody = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (evasion)
        {
            rigidbody.velocity = evationDirection * gameObject.transform.right * moveSpeed;
        }
        else
        {
            if (currentSaveTime > 0)
                currentSaveTime -= Time.deltaTime;
            else
            {
                rigidbody.velocity = Vector3.zero;
                currentSaveTime = saveTime;
            }
        }

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (threat != null)
        {
            if (collision.Distance(colliderBody).distance <= threat.Distance(colliderBody).distance)
            {
                if (CheckBulletDirection(collision))
                {
                    threat = collision;
                    evationDirection = GetEavasionDirection(collision.attachedRigidbody.velocity, transform.position - collision.transform.position);
                    evasion = true;
                }
                else
                    if (collision == threat)
                    evasion = false;
            }
        }
        else
        {
            if (CheckBulletDirection(collision))
            {
                threat = collision;
                evationDirection = GetEavasionDirection(collision.attachedRigidbody.velocity, transform.position - collision.transform.position);
                evasion = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == threat)
        {
            threat = null;
            evasion = false;
        }
    }

    int GetEavasionDirection(Vector3 velocity, Vector3 disatnce)
    {
        int direction = -1;
        float angle1 = Vector3.Angle(velocity, transform.up);
        if (angle1 > 90)
            direction *= -1;
        float angle = Vector3.SignedAngle(disatnce, velocity, Vector3.forward);
        if (angle > 0)
            direction *= -1;
        return direction;
    }

    bool CheckBulletDirection(Collider2D collision)
    {
        RaycastHit2D hitBulletRight = Physics2D.Raycast(collision.transform.position + (collision.transform.right * 0.05f), collision.attachedRigidbody.velocity);
        RaycastHit2D hitBulletLeft = Physics2D.Raycast(collision.transform.position + (-collision.transform.right * 0.05f), collision.attachedRigidbody.velocity);
        if (hitBulletRight && hitBulletRight.rigidbody == rigidbody)
            return true;
        if (hitBulletLeft && hitBulletLeft.rigidbody == rigidbody)
            return true;
        return false;
    }
}
