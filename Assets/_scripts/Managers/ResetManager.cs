using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetManager : MonoBehaviour
{
    [SerializeField] GameObject bulletField;
    PlayerController player;
    BotController bot;
    Vector3 playerStartPosition;
    Quaternion playerStartRotation;
    Vector3 botStartPosition;
    Quaternion botStartRotation;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        player.ScoredAction += Reset;
        bot = FindObjectOfType<BotController>();
        bot.ScoredAction += Reset;
        playerStartPosition = player.transform.position;
        playerStartRotation = player.transform.rotation;
        botStartPosition = bot.transform.position;
        botStartRotation = bot.transform.rotation;
    }

    public void Reset(GameObject gameObject)
    {
        player.transform.position = playerStartPosition;
        player.transform.rotation = playerStartRotation;
        bot.transform.position = botStartPosition;
        bot.transform.rotation = botStartRotation;
        BulletController[] bullets = bulletField.GetComponentsInChildren<BulletController>();
        foreach (BulletController x in bullets)
            Destroy(x.gameObject);
    }


}
