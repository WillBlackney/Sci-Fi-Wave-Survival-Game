using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    public GameObject blankTile;
    private SpriteRenderer spriteRenderer;
	
	void Start ()
    {
        //gameObject.SetActive(false);
        spriteRenderer = GetComponent<SpriteRenderer>();
        Deactivate();        
	}	
	
	void Update ()
    {
        FollowMouse();
	}

    private void FollowMouse()
    {
            if(LevelManager.Instance.mousedOverTile != null)
            {
                transform.position = LevelManager.Instance.mousedOverTile.WorldPosition;
            }               
    }

    public void SetBlankTileHoverVisibility(bool onOrOff)
    {
        blankTile.SetActive(onOrOff);
    }

    public void Activate(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;        
    }

    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        DefenderPanelManager.Instance.ClickedDefender = null;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
