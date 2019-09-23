using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenManager : Singleton<BlackScreenManager>
{
    [Header("Component References")]
    public GameObject visualParent;
    public CanvasGroup canvasGroup;
    public Canvas canvas;

    [Header("Properties")]    
    public int currentSortingLayer;
    public int aboveEverything;
    public int behindEverything;


    // Property Modifiers

    public void SetSortingLayer(int newLayer)
    {
        canvas.sortingOrder = newLayer;
        currentSortingLayer = newLayer;
    }

    public void SetActive(bool onOrOff)
    {
        if(onOrOff == true)
        {
            visualParent.SetActive(true);
        }
        else
        {
            visualParent.SetActive(false);
        }
    }

    // Fade effects

    public Action FadeIn(int speed = 2)
    {
        Action action = new Action();
        StartCoroutine(FadeInCoroutine(speed, action));
        return action;
    }
    public IEnumerator FadeInCoroutine(int speed, Action action)
    {
        SetActive(true);
        SetSortingLayer(aboveEverything);

        Debug.Log("FadeInCoroutine() started...");
        canvasGroup.alpha = 1;

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 0.02f * speed;
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("FadeInCoroutine() finished...");
        SetSortingLayer(behindEverything); SetActive(true);
        SetActive(false);
        action.actionResolved = true;

        

    }

    public Action FadeOut(int speed = 2)
    {
        Action action = new Action();
        StartCoroutine(FadeOutCoroutine(speed, action));
        return action;
    }
    public IEnumerator FadeOutCoroutine(int speed, Action action)
    {
        SetActive(true);
        SetSortingLayer(aboveEverything);
        canvasGroup.alpha = 0;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 0.02f * speed;
            yield return new WaitForEndOfFrame();
        }
        SetSortingLayer(behindEverything);
        SetActive(false);
        action.actionResolved = true;        
    }

}
