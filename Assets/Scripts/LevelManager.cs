﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    // Properties / Variables
    #region
    public TileScript selectedTile;
    public TileScript mousedOverTile;
    public GameObject tileParent;
    private Point mapSize;

    [SerializeField] private GameObject[] tilePrefabs;

    //[SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private Transform TileParent;

    [SerializeField] private List <TextAsset> mapTextFiles;         

    public Dictionary<Point, TileScript> Tiles { get; set; }

    public List<TileScript> HighlightedTiles = new List<TileScript>();

    public List<TileScript> spaceShipControlZone;
    public float TileSize
    {
        get { return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }
    #endregion

    // Initialization / Level Generation
    #region
    public void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero;

        //Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));
        Vector3 worldStart = new Vector3( 0,0,0);

        for (int y = 0; y < mapY; y++) // the y positions
        {
            char[] newTiles = mapData[y].ToCharArray();

            for (int x = 0; x < mapX; x++) // the x positions
            {
                PlaceTile(newTiles[x].ToString(), x,y, worldStart);
            }
        }

        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        Debug.Log("Tiles currently in existence = " + FindObjectsOfType<TileScript>().Length);
        // create a new combat scene BG GO from prefab
        // GameObject bg = Instantiate(combatBGPrefab);
        // bg.transform.position = GetWorldCentreTile().WorldPosition;
        // position camera on centre tile
        //ToggleLevelBackgroundView(true);
        //Camera.main.transform.position = new Vector3(GetWorldCentreTile().WorldPosition.x, GetWorldCentreTile().WorldPosition.y, -10);
        //FindObjectOfType<CameraMovement>().cinemachineCamera.transform.position = new Vector3(GetWorldCentreTile().WorldPosition.x, GetWorldCentreTile().WorldPosition.y, -10);

    }	
    public void CreateSpaceShipControlZone()
    {
        spaceShipControlZone = GetTilesWithinRange(GlobalSettings.Instance.spaceShipControlZoneSize, GetWorldCentreTile());
    }
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        TileScript newTile = Instantiate((tilePrefabs[tileIndex]).GetComponent<TileScript>(),tileParent.transform);        

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), TileParent);
        newTile.gameObject.name = "Tile " + x.ToString() + ", " + y.ToString();

    }
    private string[] ReadLevelText()
    {
        // TextAsset bindData = Resources.Load("Level 2") as TextAsset;
        TextAsset bindData = mapTextFiles[UnityEngine.Random.Range(0, mapTextFiles.Count)];
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }
    public bool InBounds(Point position)
    {
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }
    public void DestroyCurrentLevel()
    {
        Debug.Log("Destroying all tiles...");

        List<TileScript> tilesToDestroy = new List<TileScript>();

        if (Tiles != null)
        {
            tilesToDestroy.AddRange(GetAllTilesFromCurrentLevelDictionary());
            Tiles.Clear();
        }

        foreach (TileScript tile in tilesToDestroy)
        {
            Destroy(tile.gameObject);
            Destroy(tile);
        }

        // Destroy(currentLevelBG);

    }
    #endregion

    // Get Tile Lists and Data Methods
    #region
    public List<TileScript> GetAllTilesFromCurrentLevelDictionary()
    {

        List<TileScript> listReturned = new List<TileScript>();

        foreach (KeyValuePair<Point, TileScript> kvp in Tiles)
        {
            listReturned.Add(kvp.Value);
        }

        return listReturned;
    }
    public List<TileScript> GetTilesWithinRange(int range, TileScript tileFrom, bool removeTileFrom = true)
    {
        // iterate through every tile, and add those within range to the temp list        
        List<TileScript> allTiles = GetAllTilesFromCurrentLevelDictionary();
        List<TileScript> allTilesWithinXPosRange = new List<TileScript>();
        List<TileScript> allTilesWithinRange = new List<TileScript>();

        // first, filter in all tiles with an X grid position within movement range
        foreach (TileScript tile in allTiles)
        {
            int myXPos = tile.GridPosition.X;

            if (
                (myXPos >= tileFrom.GridPosition.X && (myXPos <= tileFrom.GridPosition.X + range)) ||
                (myXPos <= tileFrom.GridPosition.X && (myXPos >= tileFrom.GridPosition.X - range))
                )
            {
                allTilesWithinXPosRange.Add(tile);
            }

        }

        // second, filter out all tiles outside of Y range, then add the remainding tiles to the final list.
        foreach (TileScript Xtile in allTilesWithinXPosRange)
        {
            int myYPos = Xtile.GridPosition.Y;

            if (
                (myYPos >= tileFrom.GridPosition.Y && myYPos <= tileFrom.GridPosition.Y + range) ||
                (myYPos <= tileFrom.GridPosition.Y && (myYPos >= tileFrom.GridPosition.Y - range))
                )
            {
                allTilesWithinRange.Add(Xtile);
            }
        }

        // third, remove the 'fromTile' from the list
        if(removeTileFrom == true)
        {
            allTilesWithinRange.Remove(tileFrom);
        }
        
        
        //Debug.Log("Tiles within movement range: " + allTilesWithinRange.Count);
        return allTilesWithinRange;
    }   
    public List<TileScript> GetValidMoveableTilesWithinRange(int range, TileScript tileFrom)
    {
        // iterate through every tile, and add those within range to the temp list        
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> allTilesWithinXPosRange = new List<TileScript>();
        List<TileScript> allTilesWithinRange = new List<TileScript>();
        List<TileScript> allTilesWithinMobilityRange = new List<TileScript>();

        // first, filter in all tiles with an X grid position within movement range
        foreach (TileScript tile in allTiles)
        {
            int myXPos = tile.GridPosition.X;

            if (
                (myXPos >= tileFrom.GridPosition.X && (myXPos <= tileFrom.GridPosition.X + range)) ||
                (myXPos <= tileFrom.GridPosition.X && (myXPos >= tileFrom.GridPosition.X - range))
                )
            {
                //only add tiles to the list if they are walkable and unoccupied
                if (tile.CanBeMovedThrough() && tile.CanBeOccupied())
                {
                    allTilesWithinXPosRange.Add(tile);
                }
                /*
                if (tile.isEmpty == true && tile.isWalkable == true)
                {
                    allTilesWithinXPosRange.Add(tile);
                }     
                */
            }
        }

        // second, filter out all tiles outside of Y range, then add the remainding tiles to the final list.
        foreach (TileScript Xtile in allTilesWithinXPosRange)
        {
            int myYPos = Xtile.GridPosition.Y;

            if (
                (myYPos >= tileFrom.GridPosition.Y && myYPos <= tileFrom.GridPosition.Y + range) ||
                (myYPos <= tileFrom.GridPosition.Y && (myYPos >= tileFrom.GridPosition.Y - range))
                )
            {
                allTilesWithinRange.Add(Xtile);
            }
        }

        // third, remove the 'fromTile' from the list
        allTilesWithinRange.Remove(tileFrom);

        // fourth, draw a path to each tile in the list, filtering the ones within mobility range
        foreach (TileScript tile in allTilesWithinRange)
        {
            Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, tile.GridPosition);
            if (path.Count <= range)
            {
                allTilesWithinMobilityRange.Add(tile);
            }
        }

        //Debug.Log("Tiles within range: " + allTilesWithinRange.Count);
        return allTilesWithinMobilityRange;
    }
    public List<TileScript> GetTilesWithinMovementRange(int range, TileScript tileFrom)
    {
        // iterate through every tile, and add those within range to the temp list        
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> allTilesWithinXPosRange = new List<TileScript>();
        List<TileScript> allTilesWithinRange = new List<TileScript>();
        List<TileScript> allTilesWithinMobilityRange = new List<TileScript>();

        // first, filter in all tiles with an X grid position within movement range
        foreach (TileScript tile in allTiles)
        {
            int myXPos = tile.GridPosition.X;

            if (
                (myXPos >= tileFrom.GridPosition.X && (myXPos <= tileFrom.GridPosition.X + range)) ||
                (myXPos <= tileFrom.GridPosition.X && (myXPos >= tileFrom.GridPosition.X - range))
                )
            {
                //only add tiles to the list if they are walkable and unoccupied
                if (tile.CanBeMovedThrough() && tile.CanBeOccupied())
                {
                    allTilesWithinXPosRange.Add(tile);
                }
            }
        }

        // second, filter out all tiles outside of Y range, then add the remainding tiles to the final list.
        foreach (TileScript Xtile in allTilesWithinXPosRange)
        {
            int myYPos = Xtile.GridPosition.Y;

            if (
                (myYPos >= tileFrom.GridPosition.Y && myYPos <= tileFrom.GridPosition.Y + range) ||
                (myYPos <= tileFrom.GridPosition.Y && (myYPos >= tileFrom.GridPosition.Y - range))
                )
            {
                allTilesWithinRange.Add(Xtile);
            }
        }

        // third, remove the 'fromTile' from the list
        allTilesWithinRange.Remove(tileFrom);

        // fourth, draw a path to each tile in the list, filtering the ones within mobility range
        foreach(TileScript tile in allTilesWithinRange)
        {
            Stack<Node> path = AStar.GetPath(tileFrom.GridPosition, tile.GridPosition);
            if(path.Count <= range)
            {
                allTilesWithinMobilityRange.Add(tile);
            }
        }

        //Debug.Log("Tiles within range: " + allTilesWithinRange.Count);
        return allTilesWithinMobilityRange;
    }
    public List<TileScript> GetTilesWithinRangeAndLoS(int range, TileScript origin)
    {
        List<TileScript> tilesWithinRange = GetTilesWithinRange(range, origin, true);
        List<TileScript> tilesWithLoS = new List<TileScript>();

        for (int loopCount = 0; loopCount < tilesWithinRange.Count; loopCount++)
        {
            if (PositionLogic.Instance.IsThereLosFromAtoB(origin, tilesWithinRange[loopCount]))
            {
                tilesWithLoS.Add(tilesWithinRange[loopCount]);
            }
        }

        /*
        foreach (TileScript tile in tilesWithinRange)
        {
            if (PositionLogic.Instance.IsThereLosFromAtoB(origin, tile))
            {
                tilesWithLoS.Add(tile);
            }
        }
        */

        return tilesWithLoS;
    }    
    public List<TileScript> GetTilesOnMapEdges()
    {
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> tilesOnMapEdge = new List<TileScript>();

        int maxXGridPos = 0;
        int maxYGridPos = 0;

        // Calculate the furthest x position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X > maxXGridPos)
            {
                maxXGridPos = tile.GridPosition.X;
            }
        }

        // Calculate the furthest y position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y > maxYGridPos)
            {
                maxYGridPos = tile.GridPosition.Y;
            }
        }

        // If a tile's X or Y position is on the map edge, add to the final list
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X == maxXGridPos || tile.GridPosition.X == 0)
            {
                tilesOnMapEdge.Add(tile);
            }

            if ((tile.GridPosition.Y == maxYGridPos || tile.GridPosition.Y == 0) && !tilesOnMapEdge.Contains(tile))
            {
                tilesOnMapEdge.Add(tile);
            }
        }

        //Debug.Log("Max x, y positions: " + maxXGridPos + ", " + maxYGridPos);
        //Debug.Log("Tiles on map edge: " + tilesOnMapEdge.Count);
        //HighlightTiles(tilesOnMapEdge);

        return tilesOnMapEdge;

    }
    public List<TileScript> GetTilesOnMapCorners()
    {
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> tilesOnMapCorners = new List<TileScript>();

        int maxXGridPos = 0;
        int minXGridPos = 0;
        int maxYGridPos = 0;
        int minYGridPos = 0;

        // Calculate the furthest x position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X > maxXGridPos)
            {
                maxXGridPos = tile.GridPosition.X;
            }
        }

        // Calculate the furthest y position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y > maxYGridPos)
            {
                maxYGridPos = tile.GridPosition.Y;
            }
        }

        // Find the corner tiles
        foreach (TileScript tile in allTiles)
        {

            if (
                // Get NW corner tile
                (tile.GridPosition.X == minXGridPos && tile.GridPosition.Y == minYGridPos) ||
                // Get NE corner tile
                (tile.GridPosition.X == maxXGridPos && tile.GridPosition.Y == minYGridPos) ||
                // Get SW corner tile
                (tile.GridPosition.X == minXGridPos && tile.GridPosition.Y == maxYGridPos) ||
                // Get SE corner tile
                (tile.GridPosition.X == maxXGridPos && tile.GridPosition.Y == maxYGridPos)
                )
            {
                tilesOnMapCorners.Add(tile);
            }
        }

        // return corner tiles back
        return tilesOnMapCorners;


    }
    public List<TileScript> GetTilesOnNSEW()
    {
        TileScript[] allTiles = FindObjectsOfType<TileScript>();
        List<TileScript> tilesOnNSEW = new List<TileScript>();

        int maxXGridPos = 0;
        int middleXGridPos = 0;
        int maxYGridPos = 0;
        int middleYGridPos = 0;

        // Calculate the furthest x position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X > maxXGridPos)
            {
                maxXGridPos = tile.GridPosition.X;
            }
        }

        // Calculate the furthest y position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y > maxYGridPos)
            {
                maxYGridPos = tile.GridPosition.Y;
            }
        }

        middleXGridPos = maxXGridPos / 2;
        middleYGridPos = maxYGridPos / 2;

        // Find the NSEW tiles
        foreach (TileScript tile in allTiles)
        {

            if (
                // Get N tile
                (tile.GridPosition.X == middleXGridPos && tile.GridPosition.Y == 0) ||
                // Get S tile
                (tile.GridPosition.X == middleXGridPos && tile.GridPosition.Y == maxYGridPos) ||
                // Get E corner tile
                (tile.GridPosition.X == maxXGridPos && tile.GridPosition.Y == middleYGridPos) ||
                // Get W corner tile
                (tile.GridPosition.X == 0 && tile.GridPosition.Y == middleYGridPos)
                )
            {
                tilesOnNSEW.Add(tile);
            }
        }

        return tilesOnNSEW;
    }
    public List<TileScript> GetDefenderSpawnLocations()
    {
        List<TileScript> tilesReturned = new List<TileScript>();
        TileScript centreTile = GetWorldCentreTile();

        foreach(TileScript tile in GetAllTilesFromCurrentLevelDictionary())
        {
            
            if(
                // North West
                (tile.GridPosition.X == centreTile.GridPosition.X - 2 && tile.GridPosition.Y == centreTile.GridPosition.Y - 2) ||
                // North East
                (tile.GridPosition.X == centreTile.GridPosition.X + 2 && tile.GridPosition.Y == centreTile.GridPosition.Y - 2) ||
                // South West
                (tile.GridPosition.X == centreTile.GridPosition.X - 2 && tile.GridPosition.Y == centreTile.GridPosition.Y + 2) ||
                // South East
                (tile.GridPosition.X == centreTile.GridPosition.X + 2 && tile.GridPosition.Y == centreTile.GridPosition.Y + 2)
                )
            {
                tilesReturned.Add(tile);
            }
        }

        return tilesReturned;
    }
    public List<TileScript> GetSpaceShipControlZoneTile()
    {
        return spaceShipControlZone;
    }
    #endregion

    // Get Tile Data Methods
    #region
    public TileScript GetClosestValidTile(List<TileScript> tiles, TileScript tileFrom)
    {
        TileScript closestTile = null;
        float minimumDistance = Mathf.Infinity;

        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (TileScript tile in tiles)
        {
            float distancefromTileFrom = Vector2.Distance(tile.gameObject.transform.position, transform.position);
            if (distancefromTileFrom < minimumDistance && tile.CanBeOccupied())
            {
                closestTile = tile;
                minimumDistance = distancefromTileFrom;
            }
        }

        if (closestTile == null)
        {
            Debug.Log("GetClosestValidTile() could not find any valid tiles from the supplied list, return NULL...");
        }
        return closestTile;
    }
    public TileScript GetFurthestTileFromTargetFromList(List<TileScript> tiles, TileScript tileFrom)
    {
        TileScript furthestTile = null;
        float minimumDistance = 0;

        // Declare new temp list for storing defender 
        //List<Defender> defenders = new List<Defender>();

        // Add all active defenders to the temp list


        // Iterate throught the temp list to find the closest defender to this enemy
        foreach (TileScript tile in tiles)
        {
            float distancefromTileFrom = Vector2.Distance(tile.gameObject.transform.position, transform.position);
            if (distancefromTileFrom > minimumDistance && tile.CanBeOccupied())
            {
                furthestTile = tile;
                minimumDistance = distancefromTileFrom;
            }
        }

        if (furthestTile == null)
        {
            Debug.Log("GetClosestValidTile() could not find any valid tiles from the supplied list, return NULL...");
        }
        return furthestTile;
    }
    public TileScript GetWorldCentreTile()
    {
        List<TileScript> allTiles = GetAllTilesFromCurrentLevelDictionary();
        TileScript centreTile = null;
        int maxXGridPos = 0;
        int maxYGridPos = 0;

        int centreTileX = 0;
        int centreTileY = 0;

        // Calculate the furthest x position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X > maxXGridPos)
            {
                maxXGridPos = tile.GridPosition.X;
            }
        }

        // Calculate the furthest y position on the map
        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y > maxYGridPos)
            {
                maxYGridPos = tile.GridPosition.Y;
            }
        }

        centreTileX = maxXGridPos / 2;
        centreTileY = maxYGridPos / 2;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X == centreTileX &&
               tile.GridPosition.Y == centreTileY)
            {
                centreTile = tile;
            }
        }

        return centreTile;
    }
    public TileScript GetRandomTileFromList(List<TileScript> tiles)
    {
        int randomIndex = UnityEngine.Random.Range(0, tiles.Count);
        return tiles[randomIndex];
    }
    public TileScript GetRandomValidMoveableTileFromList(List<TileScript> tiles)
    {
        List<TileScript> validTiles = new List<TileScript>();

        foreach (TileScript tile in tiles)
        {
            if (tile.CanBeOccupied() &&
                tile.CanBeMovedThrough())
            {
                validTiles.Add(tile);
            }
        }

        Debug.Log("Valid tiles found: " + validTiles.Count);

        int randomIndex = UnityEngine.Random.Range(0, validTiles.Count);
        return validTiles[randomIndex];
    }
    public TileScript GetTileFromPointReference(Point point)
    {
        return Tiles[point];
    }
    public Point GetPointFromTileReference(TileScript tile)
    {
        return tile.GridPosition;
    }
    #endregion

    // Boolean / Conditional Check Logic
    #region
    public bool IsDestinationTileToTheRight(TileScript tileFrom, TileScript destination)
    {
        if (tileFrom.GridPosition.X < destination.GridPosition.X)
        {
            return true;
        }
        else if (tileFrom.GridPosition.X > destination.GridPosition.X)
        {
            return false;
        }

        else
        {
            return false;
        }
    }
    public bool IsTileWithinMovementRange(TileScript targetTile, TileScript tileFrom, int mobility)
    {
        Stack <Node> path = AStar.GetPath(tileFrom.GridPosition, targetTile.GridPosition);
        if(path.Count <= mobility)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Tile Highlighting / Visuals
    #region
    public void HighlightTiles(List<TileScript> tilesToHighlight)
    {
        Debug.Log("Highlighting tiles");
        foreach (TileScript tile in tilesToHighlight)
        {
            //tile.ColorTile(tile.highlightedColor);
            HighlightTile(tile);
        }
    }
    public void HighlightTile(TileScript tile)
    {
        HighlightedTiles.Add(tile);
        //tile.ColorTile(tile.highlightedColor);
        tile.myAnimator.SetBool("Highlight", true);
    }      
    public void UnhighlightTile(TileScript tile)
    {
        //tile.ColorTile(tile.originalColor);
        tile.myAnimator.SetBool("Highlight", false);
        if (HighlightedTiles.Contains(tile))
        {
            HighlightedTiles.Remove(tile);
        }
    }    
    public void UnhighlightAllTiles()
    {
        foreach(TileScript tile in HighlightedTiles)
        {
            tile.myAnimator.SetBool("Highlight", false);
        }

        HighlightedTiles.Clear();
    }
    #endregion

    // Set Tile State
    #region
        /*
    public void SetTileAsOccupiedByEntity(TileScript tile, LivingEntity entity)
    {
        Debug.Log("Tile " + tile.GridPosition.X + ", " + tile.GridPosition.Y + " is now occupied");
        tile.myEntity = entity;
        //tile.isEmpty = false;
    }
    public void SetTileAsUnoccupiedByEntity(TileScript tile)
    {
        Debug.Log("Tile " + tile.GridPosition.X + ", " + tile.GridPosition.Y + " is now unoccupied");
        tile.myEntity = null;
        //tile.isEmpty = true;
    }
    */
    #endregion



    

    


}
