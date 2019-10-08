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
    public List<Sprite> grassyElementSprites;

    [Header("Tile Edge Sprites")]
    public Sprite northEdge;
    public Sprite southEdge;
    public Sprite westEdge;
    public Sprite eastEdge;
    public Sprite northEastEdge;
    public Sprite northWestEdge;
    public Sprite southEastEdge;
    public Sprite southWestEdge;

    [Header("Sand Bag Sprites")]
    public Sprite sandbagIsolated;
    public Sprite sandbagSurrounded;
    public Sprite sandbagNoEastOrWest;
    public Sprite sandbagNoNorthOrSouth;
    public Sprite sandbagNorthWestCorner;
    public Sprite sandbagNorthEastCorner;
    public Sprite sandbagSouthWestCorner;
    public Sprite sandbagSouthEastCorner;
    public Sprite sandbagSouthTipCorner;
    public Sprite sandbagNorthTipCorner;
    public Sprite sandbagWestTipCorner;
    public Sprite sandbagEastTipCorner;

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

        if (tile.myTileSetupType == TileScript.TileSetupType.Dirt)
        {            
            return;
        }
            /*
            // Set centre tile image
            if (tile.myTileSetupType == TileScript.TileSetupType.Dirt)
            {
                tile.spriteRenderer.sprite = GetRandomSpriteFromList(dirtCentreTiles);
                int randomNumber = Random.Range(0, 100);
                if(randomNumber < 20)
                {
                    CreateSpriteOverTile(tile, GetRandomSpriteFromList(grassyElementSprites));
                }
            }
            */

        if (tile.myTileType == TileScript.TileType.Water)
        {
            tile.spriteRenderer.sprite = GetRandomSpriteFromList(waterCentreTiles);
        }

        // Set edge sprites
        if (NorthEdge(tile))
        {
            CreateSpriteOverTile(tile, northEdge);
        }
        if (SouthEdge(tile))
        {
            CreateSpriteOverTile(tile, southEdge);
        }
        if (WestEdge(tile))
        {
            CreateSpriteOverTile(tile, westEdge);
        }
        if (EastEdge(tile))
        {
            CreateSpriteOverTile(tile, eastEdge);
        }
        if (NorthEastEdge(tile))
        {
            CreateSpriteOverTile(tile, northEastEdge);
        }
        if (SouthEastEdge(tile))
        {
            CreateSpriteOverTile(tile, southEastEdge);
        }
        if (NorthWestEdge(tile))
        {
            CreateSpriteOverTile(tile, northWestEdge);
        }
        if (SouthWestEdge(tile))
        {
            CreateSpriteOverTile(tile, southWestEdge);
        }
    }
    public void CreateSpriteOverTile(TileScript location, Sprite edgeSprite)
    {
        GameObject newEdgeSprite = Instantiate(edgePrefabGO);
        edgePrefabGO.transform.position = location.WorldPosition;
        edgePrefabGO.GetComponent<SpriteRenderer>().sprite = edgeSprite;        
        
    }
    public Sprite GetRandomSpriteFromList(List<Sprite> spriteList)
    {
        int randomIndex = Random.Range(0, spriteList.Count);
        return spriteList[randomIndex];
    }
    public void DetermineAndSetWorldObjectSprite(WorldObject worldObject)
    {
        Debug.Log("DetermineAndSetWorldObjectSprite() called for " + worldObject.name);
        // for testing
        if (worldObject.myTile == null)
        {
            Debug.Log("The myTile property of " + worldObject.name + " is null");
            return;
        }

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
        else if (worldObject.objectType == WorldObject.ObjectType.SandBag)
        {
            if (IsolatedObject(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagIsolated;
            }
            else if (SurroundedObject(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagSurrounded;
            }
            else if (NoMatchingObjectNorthOrSouth(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagNoNorthOrSouth;
            }
            else if (NoMatchingObjectEastOrWest(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagNoEastOrWest;
            }
            else if (NorthWestCorner(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagNorthWestCorner;
            }
            else if (NorthEastCorner(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagNorthEastCorner;
            }
            else if (SouthWestCorner(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagSouthWestCorner;
            }
            else if (SouthEastCorner(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagSouthEastCorner;
            }
            else if (SouthTip(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagSouthTipCorner;
            }
            else if (NorthTip(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagNorthTipCorner;
            }
            else if (EastTip(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagEastTipCorner;
            }
            else if (WestTip(worldObject))
            {
                worldObject.spriteRenderer.sprite = sandbagWestTipCorner;
            }
        }

    }
    #endregion

    // Directional Logic and Calculators for world objects
    #region
    public bool IsolatedObject(WorldObject worldObject)
    {

        if (PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile).myObject.objectType != worldObject.objectType)
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not isolated");
            return false;
        }
    }
    public bool SurroundedObject(WorldObject worldObject)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthWestTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthEastTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthEastTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthWestTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not surrounded");
            return false;
        }
    }
    public bool NoMatchingObjectNorthOrSouth(WorldObject worldObject)
    {
        
        if (/*
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           */
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " has no matching object north or south");
            return false;
        }
    }
    public bool NoMatchingObjectEastOrWest(WorldObject worldObject)
    {
        if (PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType 
           /*
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
           PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType != worldObject.objectType
           */
           )
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " has no matching object east or west");
            return false;
        }
    }
    public bool NorthWestCorner(WorldObject worldObject)
    {
        if(PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null)
        {
            Debug.Log("GetAdjacentSouthernTile(worldObject.myTile) != null is a true statment");
        }
        if(PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null)
        {
            Debug.Log("GetAdjacentSouthernTile(worldObject.myTile).myObject != null is a true statment");
            if (PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
            {
                Debug.Log("GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType is a true statment");
            }
        }
        if (PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null)
        {
            Debug.Log("GetAdjacentEasternTile(worldObject.myTile) != null is a true statment");
        }
        if (PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null)
        {
            Debug.Log("GetAdjacentEasternTile(worldObject.myTile).myObject != null is a true statment");
            if (PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
            {
                Debug.Log("GetAdjacentEasernTile(worldObject.myTile).myObject.objectType == worldObject.objectType is a true statment");
            }
        }

        if (PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType 
            //PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
           // PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
            //PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType != worldObject.objectType &&
            //PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
            //PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
           // PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType != worldObject.objectType
           )
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not a north west corner");
            return false;
        }
    }
    public bool NorthEastCorner(WorldObject worldObject)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&            
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not a north east corner");
            return false;
        }
    }
    public bool SouthWestCorner(WorldObject worldObject)
    {
        if (
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType )
            
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not a south west corner");
            return false;
        }
    }
    public bool SouthEastCorner(WorldObject worldObject)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
        {
            return true;
        }
        else
        {
            Debug.Log(worldObject.name + " is not a south east corner");
            return false;
        }
    }
    public bool SouthTip(WorldObject worldObject)
    {
        if (
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentNorthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType)            
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NorthTip(WorldObject worldObject)
    {
        if (PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentSouthernTile(worldObject.myTile).myObject.objectType == worldObject.objectType )
            
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool EastTip(WorldObject worldObject)
    {
        if (
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentWesternTile(worldObject.myTile).myObject.objectType == worldObject.objectType)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool WestTip(WorldObject worldObject)
    {
        if (PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile) != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject != null &&
            PositionLogic.Instance.GetAdjacentEasternTile(worldObject.myTile).myObject.objectType == worldObject.objectType 
            )
        {
            return true;
        }
        else
        {
            return false;
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

