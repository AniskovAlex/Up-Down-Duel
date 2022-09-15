using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletField;
    public Action<GameObject> ScoredAction;

    protected void Shoot()
    {
        Instantiate(bullet, transform.position + (transform.up * 0.6f), transform.rotation, bulletField.transform);
    }

    public void Scored()
    {
        ScoredAction(gameObject);
    }
}
