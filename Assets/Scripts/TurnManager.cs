using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnManager : Singleton<TurnManager>
{
    [Header("Component References")]
    public TextMeshProUGUI whoseTurnText;
    public TextMeshProUGUI playerCurrentTurnText;
    public CanvasGroup visualParentCG;
    public GameObject startPos;
    public GameObject endPos;
    public GameObject middlePos;

    [Header("Properties")]
    public bool currentlyPlayersTurn;
    public int playerTurnCount = 0;
    public int enemyTurnCount = 0;

    public void ModifyPlayerTurnCount(int turnCountGainedOrLost)
    {
        playerTurnCount += turnCountGainedOrLost;
        UpdateCurrentPlayerTurnText(playerTurnCount);
    }
    public void UpdateCurrentPlayerTurnText(int newTurnCount)
    {
        playerCurrentTurnText.text = newTurnCount.ToString();
    }

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
        ModifyPlayerTurnCount(1);
        PlayerDataManager.Instance.GenerateIncomeOnPlayerTurnStart();

        // Spawn a new enemy wave every 3 turns        
        Action waveSpawnAction = EnemySpawner.Instance.SpawnNextWave();
        yield return new WaitUntil(() => waveSpawnAction.ActionResolved() == true);

        // Reduce all lootbox timers
        Action lootBoxCountDownReductions = LootBoxManager.Instance.ReduceAllLootBoxCountdowns();
        yield return new WaitUntil(() => lootBoxCountDownReductions.ActionResolved() == true);

        // Spawn a loot box every 2 turns
        if (LootBoxManager.Instance.lootSpawnTurns.Contains(playerTurnCount))
        {
            Action lootBoxSpawn = LootBoxManager.Instance.StartNewLootBoxCreatedEvent();
            yield return new WaitUntil(() => lootBoxSpawn.ActionResolved() == true);
            yield return new WaitForSeconds(2f);
        }        

        // Move camera view to spaceship on every turn start
        CameraManager.Instance.SetCameraLookAtTarget(LevelManager.Instance.GetWorldCentreTile().gameObject);
        yield return new WaitUntil(() => CameraManager.Instance.IsCameraWithinRangeOfTarget(LevelManager.Instance.GetWorldCentreTile().gameObject) == true);

        // Display turn number overlay event
        StartCoroutine(DisplayTurnChangeNotification(true));
        yield return new WaitUntil(() => NotificationComplete() == true);
        
        // Run all defender on turn start events
        currentlyPlayersTurn = true;
        foreach(Defender defender in DefenderManager.Instance.allDefenders)
        {
            StartCoroutine(defender.OnTurnStart());
            defender.myOnTurnEndEffectsFinished = false;
        }

        // Re-enable UI elements
        UIManager.Instance.EnableEndTurnButton();

        // Re-enable camera control
        CameraManager.Instance.mainCamera.GetComponent<CinemachineCameraController>().SetCameraControl(true);
        
    }

    public void EndPlayerTurn()
    {
        // disable defender spell bars to stop player doing stuff during enemy turn
        DefenderManager.Instance.ClearSelectedDefender();
        UIManager.Instance.DisableEndTurnButton();
        currentlyPlayersTurn = false;
        CameraManager.Instance.mainCamera.GetComponent<CinemachineCameraController>().SetCameraControl(false);
        StartCoroutine(EndPlayerTurnCoroutine());        
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
        ModifyPlayerTurnCount(-playerTurnCount);
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
