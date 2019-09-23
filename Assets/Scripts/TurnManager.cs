﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : Singleton<TurnManager>
{
    [Header("Turn Notifier Properties + Components")]
    public TextMeshProUGUI whoseTurnText;
    public CanvasGroup visualParentCG;
    public GameObject startPos;
    public GameObject endPos;
    public GameObject middlePos;

    public bool currentlyPlayersTurn = false;

    public int playerTurnCount = 0;
    public int enemyTurnCount = 0;

    public bool playerOnTurnEndEventsResolved;
    public bool PlayerOnTurnEndEventsResolved()
    {
        if(playerOnTurnEndEventsResolved == true)
        {
            Debug.Log("Player 'OnTurnEnd' events resolved and finished...");
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnEndTurnButtonClicked()
    {
        StartCoroutine(OnEndTurnButtonClickedCoroutine());
    }
    public IEnumerator OnEndTurnButtonClickedCoroutine()
    {
        Debug.Log("OnEndTurnButtonClickedCoroutine() started...");
        // TO DO: endplayerturn will need to be a coroutine most likely
        // endplayer turn will trigger all player end turn effects, BEFORE switching to enemy turn
        EndPlayerTurn();
        yield return new WaitUntil(() => PlayerOnTurnEndEventsResolved() == true);
        // reset boolean for future use
        playerOnTurnEndEventsResolved = false;

        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        foreach (Defender defender in DefenderManager.Instance.allDefenders)
        {
            defender.myOnTurnEndEffectsFinished = false;
        }
        Debug.Log("StartEnemyTurn() called...");
        ResetEnemyTurnProperties();
        StartCoroutine(StartEnemyTurnCoroutine());
    }

    public IEnumerator StartEnemyTurnCoroutine()
    {
        enemyTurnCount++;
        StartCoroutine(DisplayTurnChangeNotification(false));
        // notification yield not currently working
        yield return new WaitUntil(() => NotificationComplete() == true);
        //yield return new WaitForSeconds(1.5f);

        EnemyManager.Instance.StartEnemyTurnSequence();
        yield return new WaitUntil(() => EnemyManager.Instance.AllEnemiesHaveActivated() == true);
        StartCoroutine(EndEnemyTurnCoroutine());
    }

    public IEnumerator EndEnemyTurnCoroutine()
    {
        EnemyManager.Instance.StartEnemyTurnEndSequence();
        yield return new WaitUntil(() => EnemyManager.Instance.EnemyTurnFinished() == true);       
        StartCoroutine(StartPlayerTurn());
    }

    public IEnumerator StartPlayerTurn()
    {
        playerTurnCount++;

        // Spawn a new enemy wave every 5 turns
        if(playerTurnCount == 5 ||
            playerTurnCount == 10 ||
            playerTurnCount == 15 ||
            playerTurnCount == 20 ||
            playerTurnCount == 25)
        {
            EnemySpawner.Instance.SpawnEnemyWave();
            yield return new WaitForSeconds(2f);
        }

        StartCoroutine(DisplayTurnChangeNotification(true));
        yield return new WaitUntil(() => NotificationComplete() == true);
        
        currentlyPlayersTurn = true;
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            StartCoroutine(defender.OnTurnStart());
            defender.myOnTurnEndEffectsFinished = false;
        }

        UIManager.Instance.EnableEndTurnButton();
        // turn on controls
        // re-enable end turn button
    }

    public void EndPlayerTurn()
    {
        // disable defender spell bars to stop player doing stuff during enemy turn
        DefenderManager.Instance.ClearSelectedDefender();
        UIManager.Instance.DisableEndTurnButton();
        currentlyPlayersTurn = false;
        StartCoroutine(EndPlayerTurnCoroutine());
        // disable en turn button
        // turn off controls
        // trigger on turn effects(bleed, poison etc)
    }

    public IEnumerator EndPlayerTurnCoroutine()
    {
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            defender.OnTurnEnd();
            yield return new WaitUntil(() => defender.MyOnTurnEndEffectsFinished() == true);
        }

        playerOnTurnEndEventsResolved = true;
    }

    public void ResetTurnManagerPropertiesOnCombatStart()
    {
        playerTurnCount = 0;
        enemyTurnCount = 0;
    }

    public void ResetEnemyTurnProperties()
    {
        EnemyManager.Instance.allEnemiesHaveActivated = false;

        foreach(Enemy enemy in EnemyManager.Instance.allEnemies)
        {
            enemy.myOnTurnEndEffectsFinished = false;
        }
    }

    public IEnumerator DisplayTurnChangeNotification(bool playerTurn = true)
    {
        bool reachedMiddlePos = false;
        bool reachedEndPos = false;

        visualParentCG.gameObject.SetActive(true);
        whoseTurnText.gameObject.transform.position = startPos.transform.position;
        visualParentCG.alpha = 0;

        if (playerTurn)
        {
            whoseTurnText.text = "Player Turn " + playerTurnCount;
        }

        else if (playerTurn == false)
        {
            whoseTurnText.text = "Enemy Turn " + enemyTurnCount;
        }

        while(reachedMiddlePos == false)
        {
            visualParentCG.alpha += 0.08f;
            whoseTurnText.gameObject.transform.position = Vector2.MoveTowards(whoseTurnText.gameObject.transform.position, middlePos.transform.position, 1500 * Time.deltaTime);
            if(whoseTurnText.gameObject.transform.position == middlePos.transform.position)
            {
                Debug.Log("reached Middle pos");
                reachedMiddlePos = true;
            }
            yield return new WaitForEndOfFrame();
        }

        visualParentCG.alpha = 1;

        // brief pause while text in centred on screen
        yield return new WaitForSeconds(0.5f);

        while (reachedEndPos == false)
        {
            visualParentCG.alpha -= 0.08f;
            whoseTurnText.gameObject.transform.position = Vector2.MoveTowards(whoseTurnText.gameObject.transform.position, endPos.transform.position, 1500 * Time.deltaTime);
            if (whoseTurnText.gameObject.transform.position == endPos.transform.position)
            {
                Debug.Log("reached end pos");
                reachedEndPos = true;
            }
            yield return new WaitForEndOfFrame();
        }

        

        visualParentCG.alpha = 0;
        visualParentCG.gameObject.SetActive(false);
        notificationComplete = true;
    }

    public bool notificationComplete = false;
    public bool NotificationComplete()
    {
        if(notificationComplete == true)
        {
            notificationComplete = false;
            return true;
        }
        else
        {
            return false;
        }
    }
}
