using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
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

    public void Activate(Sprite sprite)
    {
        this.spriteRenderer.sprite = sprite;
        this.spriteRenderer.enabled = true;        
    }

    public void Deactivate()
    {
        this.spriteRenderer.enabled = false;
        DefenderPanelManager.Instance.ClickedDefender = null;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
