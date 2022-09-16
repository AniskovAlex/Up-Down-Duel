using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionsManger : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text textObject;
    PlayerController player;

    int shootsCount = 1;
    bool firstShoot = true;

    float timeBetweenShoots = 0.4f;
    float currentTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.ShootAction = OnAction;
    }

    private void Update()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
        {
            if (!firstShoot)
            {
                string actions = "";
                if (Input.GetAxis("Horizontal") != 0)
                    actions += "поворот, ";
                if (Input.GetAxis("Vertical") != 0)
                    actions += "движение, ";
                actions += "выстрелов - " + shootsCount;
                GameObject newSign = Instantiate(textObject.gameObject, panel.transform);
                newSign.GetComponent<Text>().text = actions;
                Destroy(newSign, 5f);
                shootsCount = 1;
                firstShoot = true;
            }
        }
    }

    void OnAction()
    {
        if (firstShoot)
        {
            currentTime = timeBetweenShoots;
            firstShoot = false;
            return;
        }
        if (currentTime > 0)
        {
            shootsCount++;
            currentTime = timeBetweenShoots;
            return;
        }
    }
}
