using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCoverHover : Singleton<TileCoverHover>
{
    [Header("Component References")]
    public GameObject visualParent;
    public GameObject northHalfCoverIcon;
    public GameObject northFullCoverIcon;
    public GameObject southHalfCoverIcon;
    public GameObject southFullCoverIcon;
    public GameObject eastHalfCoverIcon;
    public GameObject eastFullCoverIcon;
    public GameObject westHalfCoverIcon;
    public GameObject westFullCoverIcon;

    [Header("Component References")]
    public bool isActive;

    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {        
        if (LevelManager.Instance.mousedOverTile != null)
        {
            transform.position = LevelManager.Instance.mousedOverTile.WorldPosition;
        }     
    }

    public void SetVisibility(bool onOrOff)
    {
        visualParent.SetActive(onOrOff);
        isActive = onOrOff;
    }

    public void DisableAllIcons()
    {
        northHalfCoverIcon.SetActive(false);
        northFullCoverIcon.SetActive(false);
        southHalfCoverIcon.SetActive(false);
        southFullCoverIcon.SetActive(false);
        eastHalfCoverIcon.SetActive(false);
        eastFullCoverIcon.SetActive(false);
        westHalfCoverIcon.SetActive(false);
        westFullCoverIcon.SetActive(false);
    }

    public void OnNewTileMousedOver(TileScript tileMousedOver)
    {
        if (isActive)
        {
            DisableAllIcons();

            // North Tile
            if (PositionLogic.Instance.GetAdjacentNorthernTile(tileMousedOver).ProvidesHalfCover())
            {
                northHalfCoverIcon.SetActive(true);
            }
            else if (PositionLogic.Instance.GetAdjacentNorthernTile(tileMousedOver).ProvidesFullCover())
            {
                northFullCoverIcon.SetActive(true);
            }

            // South Tile
            if (PositionLogic.Instance.GetAdjacentSouthernTile(tileMousedOver).ProvidesHalfCover())
            {
                southHalfCoverIcon.SetActive(true);
            }
            else if (PositionLogic.Instance.GetAdjacentSouthernTile(tileMousedOver).ProvidesFullCover())
            {
                southFullCoverIcon.SetActive(true);
            }

            // East Tile
            if (PositionLogic.Instance.GetAdjacentEasternTile(tileMousedOver).ProvidesHalfCover())
            {
                eastHalfCoverIcon.SetActive(true);
            }
            else if (PositionLogic.Instance.GetAdjacentEasternTile(tileMousedOver).ProvidesFullCover())
            {
                eastFullCoverIcon.SetActive(true);
            }

            // West Tile
            if (PositionLogic.Instance.GetAdjacentWesternTile(tileMousedOver).ProvidesHalfCover())
            {
                westHalfCoverIcon.SetActive(true);
            }
            else if (PositionLogic.Instance.GetAdjacentWesternTile(tileMousedOver).ProvidesFullCover())
            {
                westFullCoverIcon.SetActive(true);
            }
        }
        

    }
}
