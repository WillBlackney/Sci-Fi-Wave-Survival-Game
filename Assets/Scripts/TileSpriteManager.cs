using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpriteManager : Singleton<TileSpriteManager>
{
    [Header("Water Tiles")]
    public List<Sprite> waterNorthTiles;
    public List<Sprite> waterSouthTiles;
    public List<Sprite> waterEastTiles;
    public List<Sprite> waterWestTiles;
    public List<Sprite> waterNorthEastTiles;
    public List<Sprite> waterNorthWestTiles;
    public List<Sprite> waterSouthEastTiles;
    public List<Sprite> waterSouthWestTiles;

    public Sprite GetRandomSpriteFromList(List<Sprite> spriteList)
    {
        int randomIndex = Random.Range(0, spriteList.Count);
        return spriteList[randomIndex];
    }

    public bool NorthTile(TileScript tile)
    {
        if(PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SouthTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool EastTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool WestTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NorthEastTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType)            
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool NorthWestTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SouthWestTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool SouthEastTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void DetermineAndSetSprite(TileScript tile)
    {
        // North East
        if (NorthEastTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterNorthEastTiles);
        }
        // North West
        else if (NorthWestTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterNorthWestTiles);
        }
        // South East
        else if (SouthEastTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterSouthEastTiles);
        }
        // South West
        else if (SouthWestTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterSouthWestTiles);
        }

        // North
        else if (NorthTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterNorthTiles);
        }
        // South
        else if (SouthTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterSouthTiles);
        }
        // East
        else if (EastTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterEastTiles);
        }
        //West
        else if (WestTile(tile))
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterWestTiles);
        }
    }
}
