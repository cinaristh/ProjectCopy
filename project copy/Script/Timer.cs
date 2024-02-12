using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Mirror;

public class Timer : NetworkBehaviour
{
   [SyncVar]
    public float waitSec;
    private int waitSecInt; //for text
    public TextMeshProUGUI text; 
    public GameManager gameManager;
    [SyncVar]
    private bool timerStarted = false;


     private void FixedUpdate()
    { 
    
        if (timerStarted && waitSec > 0)
        {
            waitSec -= Time.fixedDeltaTime;
            waitSecInt = (int)waitSec;
            text.text = waitSecInt.ToString();
        }
        else
        {
            if (timerStarted)
            {
                gameManager.daySpriteRenderer.enabled = true;
            }
        }
    }

    public void StartTimer()
    {
        timerStarted = true;
    }
}