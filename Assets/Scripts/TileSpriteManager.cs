using System.Collections.Generic;
using UnityEngine;

public class TileSpriteManager : Singleton<TileSpriteManager>
{
    [Header("Component References")]
    public GameObject edgePrefabGO;

    [Header("Water Tiles")]    
    public List<Sprite> waterCentreTiles;    

    [Header("Dirt Tiles")]    
    public List<Sprite> dirtCentreTiles;

    [Header("Object Sprite Lists")]
    public List<Sprite> rubbleSprites;
    public List<Sprite> treeSprites;
    public List<Sprite> rockWallSprites;

    [Header("Tile Edge Sprites")]
    public Sprite northEdge;
    public Sprite southEdge;
    public Sprite westEdge;
    public Sprite eastEdge;
    public Sprite northEastEdge;
    public Sprite northWestEdge;
    public Sprite southEastEdge;
    public Sprite southWestEdge;    



    // Edge + position checks
    #region
    public bool NorthEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType)            
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EastEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WestEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthEastEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthWestEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthWestEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthEastEdge(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Get + Create edge sprites
    #region
    public void DetermineAndSetEdgeSprites(TileScript tile)
    {
        // Set centre tile image
        if (tile.myTileType == TileScript.TileType.Dirt)
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(dirtCentreTiles);
        }

        else if (tile.myTileType == TileScript.TileType.Water)
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterCentreTiles);
        }

        // Set edge sprites
        if (NorthEdge(tile))
        {
            CreateEdgeSprite(tile, northEdge);
        }
        if (SouthEdge(tile))
        {
            CreateEdgeSprite(tile, southEdge);
        }
        if (WestEdge(tile))
        {
            CreateEdgeSprite(tile, westEdge);
        }
        if (EastEdge(tile))
        {
            CreateEdgeSprite(tile, eastEdge);
        }
        if (NorthEastEdge(tile))
        {
            CreateEdgeSprite(tile, northEastEdge);
        }
        if (SouthEastEdge(tile))
        {
            CreateEdgeSprite(tile, southEastEdge);
        }
        if (NorthWestEdge(tile))
        {
            CreateEdgeSprite(tile, northWestEdge);
        }
        if (SouthWestEdge(tile))
        {
            CreateEdgeSprite(tile, southWestEdge);
        }
    }
    public void CreateEdgeSprite(TileScript location, Sprite edgeSprite)
    {
        GameObject newEdgeSprite = Instantiate(edgePrefabGO);
        edgePrefabGO.transform.position = location.transform.position;
        edgePrefabGO.GetComponent<SpriteRenderer>().sprite = edgeSprite;        
        
    }
    public Sprite GetRandomSpriteFromList(List<Sprite> spriteList)
    {
        int randomIndex = Random.Range(0, spriteList.Count);
        return spriteList[randomIndex];
    }
    public void DetermineAndSetWorldObjectSprite(WorldObject worldObject)
    {
        if(worldObject.objectType == WorldObject.ObjectType.Rubble)
        {
            worldObject.spriteRenderer.sprite = GetRandomSpriteFromList(rubbleSprites);
        }
        else if (worldObject.objectType == WorldObject.ObjectType.Tree)
        {
            worldObject.spriteRenderer.sprite = GetRandomSpriteFromList(treeSprites);
        }
        else if (worldObject.objectType == WorldObject.ObjectType.RockWall)
        {
            worldObject.spriteRenderer.sprite = GetRandomSpriteFromList(rockWallSprites);
        }
       
    }
    #endregion

    // Legacy methods
    /*
    public bool IsolatedTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool SurroundedTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool SurroundedNoNorthOrSouth(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SurroundedNoEastOrWest(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
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
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthTipTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthTipTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WestTipTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EastTipTile(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SurroundedButNoSouthWestTile(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SurroundedButNoSouthEastTile(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SurroundedButNoNorthEastTile(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SurroundedButNoNorthWestTile(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoNEorNW(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoNWorSW(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoSWorSE(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoSEorNE(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoNEorSW(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType != tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoNWorSE(TileScript tile)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthWestTile(tile).myTileType == tile.myTileType
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthNoSE(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthEastTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool SouthNoNW(TileScript tile)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(tile).myTileType != tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthEastTile(tile).myTileType == tile.myTileType &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile) != null &&
            PositionLogic.Instance.GetAdjacentNorthWestTile(tile).myTileType != tile.myTileType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */
}

