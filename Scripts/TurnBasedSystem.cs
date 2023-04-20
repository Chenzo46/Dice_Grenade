using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnBasedSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText;

    public static TurnBasedSystem insance;

    public delegate void TurnBasedEvent(string currentTurn, int rollSize);

    private List<TurnBasedEvent> eventList = new List<TurnBasedEvent>();
        
    public static event TurnBasedEvent onTurnFinished;
    public static event TurnBasedEvent onTurnStarted;
    public static event TurnBasedEvent onChoiceStarted;

    private string currentTurn;
    private int rollSize;

    float time;
    int seconds = 0;

    private bool timerOn = false;

    private bool botRolled = false;

    private void OnEnable()
    {
        eventList.Add(onTurnFinished);
        eventList.Add(onTurnStarted);
        eventList.Add(onChoiceStarted);
    }

    private void Awake()
    {
        insance = this;
    }

    private void Update()
    {
        timeText.text = seconds.ToString();

        if (currentTurn != null && currentTurn.Equals("bot"))
        {
            CameraMovement.instance.changeTarget(EnemyScript.instance.transform);
        }
        else if(currentTurn != null && currentTurn.Equals("player"))
        {
            CameraMovement.instance.changeTarget(PlayerController.instance.transform);
        }


        if (Input.GetKeyDown(KeyCode.F)  && currentTurn == null)
        {
            int botRoll = Random.Range(2,8);
            int plrRoll = Random.Range(2, 8);

            if(botRoll >= plrRoll)
            {
                currentTurn = "bot";
            }
            else
            {
                currentTurn = "player";
            }

            Debug.Log(currentTurn);
        }
        else
        {
            if (timerOn)
            {
                time -= Time.deltaTime;

                seconds = (int)(time % 60);

                if (seconds <= 0)
                {
                    //Debug.Log("Timer ran out and stradegy time started");
                    onChoiceStarted.Invoke(currentTurn, rollSize);
                    timerOn = false;
                }
            }


            if (Input.GetKeyDown(KeyCode.F) && currentTurn == "player")
            {
                RollDice();
            }
            else if(currentTurn == "bot" && !botRolled)
            {
                RollDice();
                botRolled = true;
            }
        }

        
    }

    private void clearAllEvents()
    {
        for(int i  = 0; i < eventList.Count; i++)
        {
            TurnBasedEvent ev = eventList[i];
            System.Delegate[] delegateArr = ev.GetInvocationList();
            foreach(System.Delegate d in delegateArr)
            {
                ev -= (d as TurnBasedEvent); 
            }
        }
    }

    private void RollDice()
    {
        timerOn = true;
        onTurnStarted.Invoke(currentTurn,rollSize);
        time = Random.Range(2,8);
    }

    public void endTurn(string current)
    {
        if (current.Equals("bot"))
        {
            currentTurn = "player";
        }
        else
        {
            currentTurn = "bot";
        }
    }
}
