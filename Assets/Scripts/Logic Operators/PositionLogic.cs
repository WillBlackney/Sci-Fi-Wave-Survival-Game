using System.Collections.Generic;
using UnityEngine;

public class PositionLogic : Singleton<PositionLogic>
{
    // Direction / Orientation Booleans
    #region
    public bool IsTargetLocationNorth(TileScript originTile, TileScript targetTile)
    {
        if (targetTile.GridPosition.Y < originTile.GridPosition.Y)
        {
            Debug.Log("Tile " + targetTile.GridPosition.X + ", " + targetTile.GridPosition.Y + " is north of Tile " + originTile.GridPosition.X + ", " + originTile.GridPosition.Y);
            Debug.Log("Target is north of attacker");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTargetLocationSouth(TileScript originTile, TileScript targetTile)
    {
        if (targetTile.GridPosition.Y > originTile.GridPosition.Y)
        {
            Debug.Log("Tile " + targetTile.GridPosition.X + ", " + targetTile.GridPosition.Y + " is south of Tile " + originTile.GridPosition.X + ", " + originTile.GridPosition.Y);
            Debug.Log("Target is south of attacker");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTargetLocationEast(TileScript originTile, TileScript targetTile)
    {
        if (targetTile.GridPosition.X > originTile.GridPosition.X)
        {
            Debug.Log("Tile " + targetTile.GridPosition.X + ", " + targetTile.GridPosition.Y + " is east of Tile " + originTile.GridPosition.X + ", " + originTile.GridPosition.Y);
            Debug.Log("Target is east of attacker");
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTargetLocationWest(TileScript originTile, TileScript targetTile)
    {
        if (targetTile.GridPosition.X < originTile.GridPosition.X)
        {
            Debug.Log("Tile " + targetTile.GridPosition.X + ", " + targetTile.GridPosition.Y + " is west of Tile " + originTile.GridPosition.X + ", " + originTile.GridPosition.Y);
            Debug.Log("Target is west of attacker");
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Cover logic + related
    #region
    public bool IsTargetInHalfCover(LivingEntity attacker, LivingEntity target)
    {
        if (IsTargetLocationNorth(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentSouthernTile(target.TileCurrentlyOn).ProvidesHalfCover())
        {
            return true;
        }
        else if (IsTargetLocationSouth(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentNorthernTile(target.TileCurrentlyOn).ProvidesHalfCover())
        {
            return true;
        }
        else if (IsTargetLocationEast(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentWesternTile(target.TileCurrentlyOn).ProvidesHalfCover())
        {
            return true;
        }
        else if (IsTargetLocationWest(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentEasternTile(target.TileCurrentlyOn).ProvidesHalfCover())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool IsTargetInFullCover(LivingEntity attacker, LivingEntity target)
    {
        if (IsTargetLocationNorth(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentSouthernTile(target.TileCurrentlyOn).ProvidesFullCover())
        {
            return true;
        }
        else if (IsTargetLocationSouth(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentNorthernTile(target.TileCurrentlyOn).ProvidesFullCover())
        {
            return true;
        }
        else if (IsTargetLocationEast(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentWesternTile(target.TileCurrentlyOn).ProvidesFullCover())
        {
            return true;
        }
        else if (IsTargetLocationWest(attacker.TileCurrentlyOn, target.TileCurrentlyOn) &&
            GetAdjacentEasternTile(target.TileCurrentlyOn).ProvidesFullCover())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    // Get adjacent NSEW tile logic
    #region
    public TileScript GetAdjacentNorthernTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentSouthernTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentEasternTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X == location.GridPosition.X + 1 &&
                tile.GridPosition.Y == location.GridPosition.Y)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentWesternTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.X == location.GridPosition.X - 1 &&
                tile.GridPosition.Y == location.GridPosition.Y)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

    // Get adjacent NE, NW, SE, SW tile logic
    #region
    public TileScript GetAdjacentNorthEastTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X + 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentNorthWestTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y - 1 &&
                tile.GridPosition.X == location.GridPosition.X - 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentSouthEastTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X + 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    public TileScript GetAdjacentSouthWestTile(TileScript location)
    {
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        TileScript tileReturned = null;

        foreach (TileScript tile in allTiles)
        {
            if (tile.GridPosition.Y == location.GridPosition.Y + 1 &&
                tile.GridPosition.X == location.GridPosition.X - 1)
            {
                tileReturned = tile;
                break;
            }
        }

        if (tileReturned == null)
        {
            Debug.Log("PositionLogic.GetAdjacentTile() did not find a matching tile, returning null...");
        }

        return tileReturned;
    }
    #endregion

    // Direction changing
    #region
    public void FlipCharacterSprite(LivingEntity character, bool faceRight)
    {
        if (character.spriteImportedFacingRight == true)
        {
            if (faceRight == true)
            {
                character.mySpriteRenderer.flipX = false;
            }

            else
            {
                character.mySpriteRenderer.flipX = true;
            }
        }

        else if (character.spriteImportedFacingRight == false)
        {
            if (faceRight == true)
            {
                character.mySpriteRenderer.flipX = true;
            }

            else
            {
                character.mySpriteRenderer.flipX = false;
            }
        }

    }
    public void SetDirection(LivingEntity character, string leftOrRight)
    {
        if (leftOrRight == "Left")
        {
            FlipCharacterSprite(character, false);
            //character.FlipMySprite(false);
            character.facingRight = false;
        }
        else if (leftOrRight == "Right")
        {
            FlipCharacterSprite(character, true);
            //character.FlipMySprite(true);
            character.facingRight = true;
        }
    }
    public void CalculateWhichDirectionToFace(LivingEntity character, TileScript tileToFace)
    {
        // flip the sprite's x axis depending on the direction of movement
        if (LevelManager.Instance.IsDestinationTileToTheRight(character.TileCurrentlyOn, tileToFace))
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the right...");
            SetDirection(character, "Right");
        }

        else if (LevelManager.Instance.IsDestinationTileToTheRight(character.TileCurrentlyOn, tileToFace) == false)
        {
            Debug.Log("CalculateWhichDirectionToFace() facing character towards the left...");
            SetDirection(character, "Left");
        }

    }
    #endregion

    // Line of Sight Logic
    #region
    public bool IsThereLosFromAtoB(TileScript a, TileScript b)
    {
        bool hasLoS = true;        

        // first ray cast and check from the tile the attacker is on        
        RaycastHit2D[] hits = Physics2D.LinecastAll(a.WorldPosition, b.WorldPosition);
        //Debug.Log("Raycast detected and hit " + hits.Length.ToString() + " tiles between A and B tiles");

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.GetComponent<TileScript>().CanBeSeenThrough() == false)
            {
                hasLoS = false;
                break;
            }
        }

        return hasLoS;
    }
    #endregion

    // Overwatch + Camoflage + On tile moved on checking
    public void CheckForCamoflage(LivingEntity entity)
    {
        if(entity.TileCurrentlyOn.myTileType == TileScript.TileType.Grass && entity.isCamoflaged == false)
        {
            entity.ApplyCamoflage();
        }
        else if (entity.TileCurrentlyOn.myTileType != TileScript.TileType.Grass && entity.isCamoflaged)
        {
            entity.RemoveCamoflage();
        }
    }
    // Legacy Methods
    #region
    public List<TileScript> GetTargetsFrontArcTiles(LivingEntity character)
    {
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.TileCurrentlyOn);
        List<TileScript> frontArcTiles = new List<TileScript>();

        if (character.facingRight)
        {
            foreach(TileScript tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X + 1 || 
                    tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) || 
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y +1) || 
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))                    
                {
                    frontArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (
                    (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X - 1 ||
                    tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X) &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    frontArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsFrontArcTiles() returned " + frontArcTiles.Count.ToString() + " tiles");
        return frontArcTiles;
    }
    public List<TileScript> GetTargetsBackArcTiles(LivingEntity character)
    {
        List<TileScript> adjacentTiles = LevelManager.Instance.GetTilesWithinRange(1, character.TileCurrentlyOn);
        List<TileScript> backArcTiles = new List<TileScript>();

        if (character.facingRight)
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X - 1 &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }
        else
        {
            foreach (TileScript tile in adjacentTiles)
            {
                if (tile.GridPosition.X == character.TileCurrentlyOn.GridPosition.X + 1 &&
                    ((tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y + 1) ||
                    (tile.GridPosition.Y == character.TileCurrentlyOn.GridPosition.Y - 1)
                    ))
                {
                    backArcTiles.Add(tile);
                }
            }
        }

        Debug.Log("GetTargetsBackArcTiles() returned " + backArcTiles.Count.ToString() + " tiles");
        return backArcTiles;
    }
    public bool CanAttackerHitTargetsBackArc(LivingEntity attacker, LivingEntity target)
    {
        TileScript attackerPos = attacker.TileCurrentlyOn;
        TileScript targetPos = target.TileCurrentlyOn;

        if(target.facingRight && attackerPos.GridPosition.X < targetPos.GridPosition.X)
        {
            return true;
        }
        else if(!target.facingRight && attacker.GridPosition.X > targetPos.GridPosition.X)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void CheckForFlanking()
    {
        /*
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            int enemiesInMyFrontArc = 0;
            int alliesInMyFrontArc = 0;

            foreach (LivingEntity entity2 in LivingEntityManager.Instance.allLivingEntities)
            {
                if (GetTargetsFrontArcTiles(entity).Contains(entity2.TileCurrentlyOn) && !CombatLogic.Instance.IsTargetFriendly(entity, entity2))
                {
                    enemiesInMyFrontArc++;
                }
                else if (GetTargetsFrontArcTiles(entity).Contains(entity2.TileCurrentlyOn) && CombatLogic.Instance.IsTargetFriendly(entity, entity2))
                {
                    alliesInMyFrontArc++;
                }
            }

            // if this is found to be flanked, and not already considered flanked, apply flanked
            if(enemiesInMyFrontArc >= 2 && alliesInMyFrontArc == 0 && !entity.myPassiveManager.Flanked)
            {
                entity.myPassiveManager.ModifyFlanked(1);
            }

            // if this is found to be not flanked, but currently has the flanked debuff, remove flanked
            else if(entity.myPassiveManager.Flanked &&
                ((enemiesInMyFrontArc < 2) || (enemiesInMyFrontArc >= 2 && alliesInMyFrontArc >= 1))
                )
            {
                entity.myPassiveManager.ModifyFlanked(-entity.myPassiveManager.flankedStacks);
            }

            
        }
        */
    }
    public bool HasLosFromLocationToTarget3(TileScript a, TileScript b)
    {
        /*
        bool hasLoS = true;
        bool hasLosFromATile = true;
        
        List<TileScript> peekPositions = GetCornerPeekPositions(a);

        // first ray cast and check from the tile the attacker is on        
        RaycastHit2D[] hitsFromAPos = Physics2D.LinecastAll(a.WorldPosition, b.WorldPosition);
        Debug.Log("Raycast detected and hit " + hitsFromAPos.Length.ToString() + " tiles between A and B tiles");

        foreach (RaycastHit2D hit in hitsFromAPos)
        {
            if (hit.transform.GetComponent<TileScript>().CanBeSeenThrough() == false)
            {
                hasLosFromATile = false;
                break;
            }
        }

        // if we already made LoS from the A tile, dont bother checking the peek positions
        if(hasLosFromATile == true)
        {
            Debug.Log("Raycast detected LoS from the origin point, not checking peek positions...");
            return true;
        }

        /*
        // second, raycast and check from the peek position tiles of the attacker to find LoS
        foreach (TileScript peekTile in peekPositions)
        {
            List<TileScript> banList = new List<TileScript>();
            bool hasLosFromThisTile = true;
            RaycastHit2D[] hits = Physics2D.LinecastAll(peekTile.WorldPosition, b.WorldPosition);
            Debug.Log("Raycast detected and hit " + hits.Length.ToString() + " tiles between peek position tile and B tile");

            foreach (RaycastHit2D hit in hits)
            {
                // Add tiles to the ban list first

                // West peek position from origin
                if (GetAdjacentEasternTile(peekTile) == a &&
                    IsTargetLocationEast(peekTile, hit.transform.GetComponent<TileScript>()))
                {
                    banList.Add(hit.transform.GetComponent<TileScript>());
                }

                // East peek position from origin
                else if (GetAdjacentWesternTile(peekTile) == a &&
                    IsTargetLocationWest(peekTile, hit.transform.GetComponent<TileScript>()))
                {
                    banList.Add(hit.transform.GetComponent<TileScript>());
                }

                // North peek position from origin
                else if (GetAdjacentNorthernTile(peekTile) == a &&
                    IsTargetLocationNorth(peekTile, hit.transform.GetComponent<TileScript>()))
                {
                    banList.Add(hit.transform.GetComponent<TileScript>());
                }

                // South peek position from origin
                else if (GetAdjacentSouthernTile(peekTile) == a &&
                    IsTargetLocationSouth(peekTile, hit.transform.GetComponent<TileScript>()))
                {
                    banList.Add(hit.transform.GetComponent<TileScript>());
                }

                Debug.Log("Added " + banList.Count.ToString() + " tiles to the ban list");

                // check if the tile hit by the raycast blocks LoS or is in the ban list
                if (
                     hit.transform.GetComponent<TileScript>().CanBeSeenThrough() == false ||
                     banList.Contains(hit.transform.GetComponent<TileScript>())
                    )
                {
                    hasLosFromThisTile = false;
                    break;
                    
                }
            }

            if (hasLosFromThisTile == true)
            {
                hasLoS = true;
                break;
            }
        }

        

        return hasLoS;
        */

        bool hasLoS = false;
        List<TileScript> tilesToCheck = GetCornerPeekPositions(a);
        tilesToCheck.Add(a);
        // first ray cast and check from the tile the attacker is on
        // second, raycast and check from NSEW tiles of the attacker if these tiles do not block LOS

        foreach (TileScript tile in tilesToCheck)
        {
            bool hasLosFromThisTile = true;
            RaycastHit2D[] hits = Physics2D.LinecastAll(tile.WorldPosition, b.WorldPosition);
            Debug.Log("Raycast detected and hit " + hits.Length.ToString() + " tiles between A and B tiles");

            foreach (RaycastHit2D hit in hits)
            {
                if (//hit.transform.GetComponent<TileScript>() != b &&
                    //hit.transform.GetComponent<TileScript>() != tile &&
                     hit.transform.GetComponent<TileScript>().CanBeSeenThrough() == false
                    )
                {
                    hasLosFromThisTile = false;
                    break;
                }
            }

            if (hasLosFromThisTile == true)
            {
                hasLoS = true;
                break;
            }
        }

        return hasLoS;



    }
    public bool HasLosFromLocationToTarget2(TileScript a, TileScript b)
    {
        bool hasLoS = true;
        List<TileScript> exceptionTiles = new List<TileScript>();
        //List<TileScript> bannedList = new List<TileScript>();

        // Check for exception tiles first. This allows characters to draw LoS by peeking around corners slightly

        // North East
        /*
        if (IsTargetLocationNorth(a, b) && IsTargetLocationEast(a, b)) 
        {
            if (
                (GetAdjacentNorthWestTile(a) != null && GetAdjacentNorthWestTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthWestTile(b) != null && GetAdjacentSouthWestTile(b).CanBeSeenThrough()) ||
                (GetAdjacentNorthEastTile(a) != null && GetAdjacentNorthEastTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthEastTile(b) != null && GetAdjacentSouthEastTile(b).CanBeSeenThrough())
                )
            {
                exceptionTiles.Add(GetAdjacentNorthernTile(a));
                exceptionTiles.Add(GetAdjacentSouthernTile(b));
            }

            if (IsTargetLocationEast(a, b))
            {
                if (
                    (GetAdjacentNorthEastTile(a) != null && GetAdjacentNorthEastTile(a).CanBeSeenThrough() &&
                     GetAdjacentNorthWestTile(b) != null && GetAdjacentNorthWestTile(b).CanBeSeenThrough()) ||
                    (GetAdjacentSouthEastTile(a) != null && GetAdjacentSouthEastTile(a).CanBeSeenThrough() &&
                    GetAdjacentSouthWestTile(b) != null && GetAdjacentSouthWestTile(b).CanBeSeenThrough())
                    )
                {
                    exceptionTiles.Add(GetAdjacentEasternTile(a));
                    exceptionTiles.Add(GetAdjacentWesternTile(b));
                }
            }
        }
        */

        // North
        if (IsTargetLocationNorth(a, b))
        {
            if (
                (GetAdjacentNorthWestTile(a) != null && GetAdjacentNorthWestTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthWestTile(b) != null && GetAdjacentSouthWestTile(b).CanBeSeenThrough()) ||
                (GetAdjacentNorthEastTile(a) != null && GetAdjacentNorthEastTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthEastTile(b) != null && GetAdjacentSouthEastTile(b).CanBeSeenThrough())
                )
            {
                exceptionTiles.Add(GetAdjacentNorthernTile(a));
                exceptionTiles.Add(GetAdjacentSouthernTile(b));
            }
        }
        // South
        else if (IsTargetLocationSouth(a, b))
        {
            if (
                (GetAdjacentSouthWestTile(a) != null && GetAdjacentSouthWestTile(a).CanBeSeenThrough() &&
                 GetAdjacentNorthWestTile(b) != null && GetAdjacentNorthWestTile(b).CanBeSeenThrough()) ||
                (GetAdjacentSouthEastTile(a) != null && GetAdjacentSouthEastTile(a).CanBeSeenThrough() &&
                GetAdjacentNorthEastTile(b) != null && GetAdjacentNorthEastTile(b).CanBeSeenThrough())
                )
            {
                exceptionTiles.Add(GetAdjacentSouthernTile(a));
                exceptionTiles.Add(GetAdjacentNorthernTile(b));
            }
        }
        // East
        if (IsTargetLocationEast(a, b))
        {
            if (
                (GetAdjacentNorthEastTile(a) != null && GetAdjacentNorthEastTile(a).CanBeSeenThrough() &&
                 GetAdjacentNorthWestTile(b) != null && GetAdjacentNorthWestTile(b).CanBeSeenThrough()) ||
                (GetAdjacentSouthEastTile(a) != null && GetAdjacentSouthEastTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthWestTile(b) != null && GetAdjacentSouthWestTile(b).CanBeSeenThrough())
                )
            {
                exceptionTiles.Add(GetAdjacentEasternTile(a));
                exceptionTiles.Add(GetAdjacentWesternTile(b));
            }
        }
        // West
        else if (IsTargetLocationWest(a, b))
        {
            if (
                (GetAdjacentNorthWestTile(a) != null && GetAdjacentNorthWestTile(a).CanBeSeenThrough() &&
                 GetAdjacentNorthEastTile(b) != null && GetAdjacentNorthEastTile(b).CanBeSeenThrough()) ||
                (GetAdjacentSouthWestTile(a) != null && GetAdjacentSouthWestTile(a).CanBeSeenThrough() &&
                GetAdjacentSouthEastTile(b) != null && GetAdjacentSouthEastTile(b).CanBeSeenThrough())
                )
            {
                exceptionTiles.Add(GetAdjacentWesternTile(a));
                exceptionTiles.Add(GetAdjacentEasternTile(b));
            }
        }

        RaycastHit2D[] hits = Physics2D.LinecastAll(a.WorldPosition, b.WorldPosition);
        Debug.Log("Raycast detected and hit " + hits.Length.ToString() + " tiles between A and B tiles");

        foreach (RaycastHit2D hit in hits)
        {
            if (//hit.transform.GetComponent<TileScript>() != b &&
                //hit.transform.GetComponent<TileScript>() != tile &&
                 hit.transform.GetComponent<TileScript>().CanBeSeenThrough() == false &&
                 exceptionTiles.Contains(hit.transform.GetComponent<TileScript>()) == false
                )
            {
                hasLoS = false;
            }
        }

        return hasLoS;
    }
    public bool IsThereLosFromAtoB2(TileScript a, TileScript b)
    {
        // check NE, NW, SE, SW directions first

        // North East
        if (IsTargetLocationNorth(a, b) && IsTargetLocationEast(a, b))
        {
            if (GetAdjacentSouthWestTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // North West
        else if (IsTargetLocationNorth(a, b) && IsTargetLocationWest(a, b))
        {
            if (GetAdjacentSouthEastTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // South East
        else if (IsTargetLocationSouth(a, b) && IsTargetLocationEast(a, b))
        {
            if (GetAdjacentNorthWestTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // South West
        else if (IsTargetLocationSouth(a, b) && IsTargetLocationWest(a, b))
        {
            if (GetAdjacentNorthEastTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // North
        else if (IsTargetLocationNorth(a, b))
        {
            if (GetAdjacentSouthernTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // South
        else if (IsTargetLocationSouth(a, b))
        {
            if (GetAdjacentNorthernTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // East
        else if (IsTargetLocationEast(a, b))
        {
            if (GetAdjacentWesternTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // West
        else if (IsTargetLocationWest(a, b))
        {
            if (GetAdjacentEasternTile(b).CanBeSeenThrough())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // obligatory catch all false statment
        else
        {
            Debug.Log("IsThereLosFromAtoB() could not determine the direction from tile A to tile B, returning false...");
            return false;
        }
    }
    public List<TileScript> GetCornerPeekPositions(TileScript origin)
    {
        List<TileScript> tilesReturned = new List<TileScript>();

        // NSEW

        // East cover
        if (GetAdjacentEasternTile(origin) != null && GetAdjacentEasternTile(origin).CanBeSeenThrough() == false)
        {
            TileScript obstructionTile = GetAdjacentEasternTile(origin);

            // North peek position
            if (GetAdjacentNorthernTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentNorthernTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentNorthernTile(origin));
            }
            // South peek position
            if (GetAdjacentSouthernTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentSouthernTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentSouthernTile(origin));
            }

        }

        // West cover
        if (GetAdjacentWesternTile(origin) != null && GetAdjacentWesternTile(origin).CanBeSeenThrough() == false)
        {
            TileScript obstructionTile = GetAdjacentWesternTile(origin);

            // North peek position
            if (GetAdjacentNorthernTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentNorthernTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentNorthernTile(origin));
            }
            // South peek position
            if (GetAdjacentSouthernTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentSouthernTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentSouthernTile(origin));
            }
        }

        // North cover
        if (GetAdjacentNorthernTile(origin) != null && GetAdjacentNorthernTile(origin).CanBeSeenThrough() == false)
        {
            TileScript obstructionTile = GetAdjacentNorthernTile(origin);

            // West peek position
            if (GetAdjacentWesternTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentWesternTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentWesternTile(origin));
            }
            // East peek position
            if (GetAdjacentEasternTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentEasternTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentEasternTile(origin));
            }
        }
        // South cover
        if (GetAdjacentSouthernTile(origin) != null && GetAdjacentSouthernTile(origin).CanBeSeenThrough() == false)
        {
            TileScript obstructionTile = GetAdjacentSouthernTile(origin);

            // West peek position
            if (GetAdjacentWesternTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentWesternTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentWesternTile(origin));
            }
            // East peek position
            if (GetAdjacentEasternTile(obstructionTile).CanBeSeenThrough() &&
                GetAdjacentEasternTile(origin).CanBeSeenThrough())
            {
                tilesReturned.Add(GetAdjacentEasternTile(origin));
            }
        }

        Debug.Log("GetCornerPeekPositions() found a returned " + tilesReturned.Count.ToString() + " peek position tiles");
        return tilesReturned;
    }
    #endregion

}
