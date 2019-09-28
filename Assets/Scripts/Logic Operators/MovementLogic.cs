﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementLogic : Singleton<MovementLogic>
{

    // Path generation
    public Stack<Node> GeneratePath(Point start, Point end)
    {
        return AStar.GetPath(start, end);
    }
    public void SetPath(LivingEntity characterMoved, Stack<Node> path)
    {
        characterMoved.path = path;
        characterMoved.GridPosition = path.Peek().GridPosition;
        characterMoved.destination = path.Peek().WorldPosition;
    }


    // Conditional checks
    public bool IsLocationMoveable(TileScript destination, LivingEntity characterMoved, int range)
    {
        List<TileScript> validTilesWithinMovementRange = LevelManager.Instance.GetValidMoveableTilesWithinRange(range, LevelManager.Instance.Tiles[characterMoved.GridPosition]);

        if (validTilesWithinMovementRange.Contains(destination) &&
            destination.CanBeOccupied()
            )
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Coroutines + movement operators
    
    // Movement
    public Action MoveEntity(LivingEntity characterMoved, TileScript destination, float speed = 3)
    {
        Action action = new Action();
        StartCoroutine(MoveEntityCoroutine(characterMoved, destination, action, speed));
        return action;
    }
    public IEnumerator MoveEntityCoroutine(LivingEntity characterMoved, TileScript destination, Action action, float speed = 3)
    {
        Debug.Log("Running MovementLogic.MoveEntityCoroutine() coroutine...");

        // Set properties
        float originalSpeed = characterMoved.speed;
        float speedOfThisMovement = speed;
        bool hasCompletedMovement = false;

        // Set path + destination
        SetPath(characterMoved, GeneratePath(characterMoved.GridPosition, destination.GridPosition));

        // Play movement animation
        characterMoved.myAnimator.SetTrigger("Move");

        // flip the sprite's x axis depending on the direction of movement
        PositionLogic.Instance.CalculateWhichDirectionToFace(characterMoved, destination);

        // remove entrenched if the character has it
        if (characterMoved.myPassiveManager.Entrenched)
        {
            characterMoved.myPassiveManager.ModifyEntrenched(-characterMoved.myPassiveManager.entrenchedStacks);
        }

        // Commence movement
        while (hasCompletedMovement == false)
        {
            // prevent bugs caused by moving after dying from an overwatch shot
            if (characterMoved.inDeathProcess)
            {
                action.actionResolved = true;
                break;
            }
            characterMoved.transform.position = Vector2.MoveTowards(characterMoved.transform.position, characterMoved.destination, speedOfThisMovement * Time.deltaTime);

            if (characterMoved.transform.position == characterMoved.destination)
            {
                // if we have reached the next tile in our path
                if (characterMoved.path != null && characterMoved.path.Count > 0)
                {
                    // Get the grid pos point reference of the tile we just moved on
                    characterMoved.GridPosition = characterMoved.path.Peek().GridPosition;
                    // Free up the tile we were previously standing                
                    characterMoved.TileCurrentlyOn.SetTileAsUnoccupiedByEntity();
                    // Set our 'TileCurrentlyOn' to our new location (from the grid pos we just stored)
                    characterMoved.TileCurrentlyOn = LevelManager.Instance.GetTileFromPointReference(characterMoved.GridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.                    
                    characterMoved.TileCurrentlyOn.SetTileAsOccupiedByEntity(characterMoved);
                    // Check for events related to moving to a new location(overwatch, camoflage, etc)
                    Action movedToNewLocation = OnLocationMovedTo(characterMoved);
                    // wait until these events (if any) are resolved
                    yield return new WaitUntil(() => movedToNewLocation.ActionResolved() == true);
                    // Set our destination to the next element in the path stack
                    characterMoved.destination = characterMoved.path.Pop().WorldPosition;
                }

                // if we have reached the final destination
                else if (characterMoved.path != null && characterMoved.path.Count == 0)
                {
                    characterMoved.TileCurrentlyOn.SetTileAsUnoccupiedByEntity();
                    // Set our 'TileCurrentlyOn' to our new location (from the grid pos we just stored)
                    characterMoved.TileCurrentlyOn = LevelManager.Instance.GetTileFromPointReference(characterMoved.GridPosition);
                    // Set our current tile to be occupied, so other characters cant stack ontop of it.                    
                    characterMoved.TileCurrentlyOn.SetTileAsOccupiedByEntity(characterMoved);
                    // Check for events related to moving to a new location(overwatch, camoflage, etc)
                    Action movedToNewLocation = OnLocationMovedTo(characterMoved);
                    // wait until these events (if any) are resolved
                    yield return new WaitUntil(() => movedToNewLocation.ActionResolved() == true);
                    Debug.Log("Final point reached, movement finished");
                    characterMoved.myAnimator.SetTrigger("Idle");
                    hasCompletedMovement = true;
                    action.actionResolved = true;
                }
            }

            // Reset speed
            characterMoved.speed = originalSpeed;
            yield return null;
        }

        action.actionResolved = true;

    }

    // Teleport
    public void TeleportEntity(LivingEntity target, TileScript destination)
    {
        StartCoroutine(TeleportEntityCoroutine(target, destination));
    }
    public IEnumerator TeleportEntityCoroutine(LivingEntity target, TileScript destination)
    {
        target.TileCurrentlyOn.SetTileAsUnoccupiedByEntity();
        //LevelManager.Instance.SetTileAsUnoccupiedByEntity(target.TileCurrentlyOn);
        target.GridPosition = destination.GridPosition;
        target.TileCurrentlyOn = destination;
        target.transform.position = destination.WorldPosition;
        //LevelManager.Instance.SetTileAsOccupiedByEntity(destination);
        destination.SetTileAsOccupiedByEntity(target);
        yield return null;
    }

    // Knock back
    public void KnockBackEntity(LivingEntity attacker, LivingEntity target, int pushBackDistance)
    {
        Debug.Log("CreateKnockBackEvent() called, starting new knockback event...");
        // First, deal the initial bolt damage
        // HandleDamage(CalculateDamage(damageAmount, target, attacker), attacker);

        TileScript TileCurrentlyOn = attacker.TileCurrentlyOn;
        string direction = "unassigned";
        TileScript targetTile = target.TileCurrentlyOn;
        TileScript finalDestination = targetTile;
        List<TileScript> tilesOnPath = new List<TileScript>();
        List<TileScript> allTiles = LevelManager.Instance.GetAllTilesFromCurrentLevelDictionary();
        LivingEntity characterKnockedInto = null;

        // Second, calculate which direction the target will be moved towards when shot

        // South
        if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
            TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South";
            Debug.Log("Knockback target is south...");
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (TileScript tile in allTiles)
            {
                if (
                    tile.GridPosition.X == targetTile.GridPosition.X &&
                    tile.GridPosition.Y > targetTile.GridPosition.Y &&
                    tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in SortedList)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North
        else if (TileCurrentlyOn.GridPosition.X == targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North";

            foreach (TileScript tile in allTiles)
            {
                if (
                    tile.GridPosition.X == targetTile.GridPosition.X &&
                    tile.GridPosition.Y < targetTile.GridPosition.Y &&
                    tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.Y).ToList();
            SortedList.Reverse();

            foreach (TileScript tile in SortedList)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }


            //LevelManager.Instance.SetTileAsUnoccupiedByEntity(target.TileCurrentlyOn);
            target.TileCurrentlyOn.SetTileAsUnoccupiedByEntity();
;           target.GridPosition = finalDestination.GridPosition;
            target.TileCurrentlyOn = finalDestination;
            target.transform.position = finalDestination.WorldPosition;
            //LevelManager.Instance.SetTileAsOccupiedByEntity(finalDestination);
            finalDestination.SetTileAsOccupiedByEntity(target);
        }

        // East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
        {
            direction = "East";
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (TileScript tile in allTiles)
            {
                if (
                    tile.GridPosition.Y == targetTile.GridPosition.Y &&
                    tile.GridPosition.X > targetTile.GridPosition.X &&
                    tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance
                    )
                {
                    tilesOnPath.Add(tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in SortedList)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y == targetTile.GridPosition.Y)
        {
            direction = "West";
            // Find all tiles that the target will move over during the knock back, then add them to a list
            foreach (TileScript tile in allTiles)
            {
                if (
                    tile.GridPosition.Y == targetTile.GridPosition.Y &&
                    tile.GridPosition.X < targetTile.GridPosition.X &&
                    tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance
                    )
                {
                    tilesOnPath.Insert(0, tile);
                    Debug.Log("Tile " + tile.GridPosition.X.ToString() + ", " + tile.GridPosition.Y.ToString());
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            //List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();
            //tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in tilesOnPath)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // South West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South West";
            List<TileScript> tempList = new List<TileScript>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (TileScript tile in allTiles)
            {
                if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                    tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (TileScript tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in tilesOnPath)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // South East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y < targetTile.GridPosition.Y)
        {
            direction = "South East";
            List<TileScript> tempList = new List<TileScript>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (TileScript tile in allTiles)
            {
                if (tile.GridPosition.Y > targetTile.GridPosition.Y && tile.GridPosition.Y <= targetTile.GridPosition.Y + pushBackDistance &&
                    tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (TileScript tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y + yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in tilesOnPath)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North West
        else if (TileCurrentlyOn.GridPosition.X > targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North West";
            List<TileScript> tempList = new List<TileScript>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (TileScript tile in allTiles)
            {
                if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                    tile.GridPosition.X < targetTile.GridPosition.X && tile.GridPosition.X >= targetTile.GridPosition.X - pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (TileScript tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X - xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in tilesOnPath)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        // North East
        else if (TileCurrentlyOn.GridPosition.X < targetTile.GridPosition.X &&
                 TileCurrentlyOn.GridPosition.Y > targetTile.GridPosition.Y)
        {
            direction = "North East";
            List<TileScript> tempList = new List<TileScript>();
            int xPos = 1;
            int yPos = 1;
            // Find all tiles that the target will move over during the knock back, then add them to a list

            foreach (TileScript tile in allTiles)
            {
                if (tile.GridPosition.Y < targetTile.GridPosition.Y && tile.GridPosition.Y >= targetTile.GridPosition.Y - pushBackDistance &&
                    tile.GridPosition.X > targetTile.GridPosition.X && tile.GridPosition.X <= targetTile.GridPosition.X + pushBackDistance)
                {
                    tempList.Add(tile);
                }
            }

            Loop:
            foreach (TileScript tile in tempList)
            {
                if (tile.GridPosition.X == targetTile.GridPosition.X + xPos)
                {
                    if (tile.GridPosition.Y == targetTile.GridPosition.Y - yPos)
                    {
                        tilesOnPath.Add(tile);
                        xPos++;
                        yPos++;
                        goto Loop;
                    }
                }
            }

            // Order the list of tiles, so that the tiles closest to the target are at the start of the list
            List<TileScript> SortedList = tilesOnPath.OrderBy(o => o.GridPosition.X).ToList();

            // Check each tile in the sorted list. Determine which tile will become the final destination of the knockback.
            // This is determined looking at the next tile, and checking if it contains an enemy on it already.
            foreach (TileScript tile in tilesOnPath)
            {
                if (tile.CanBeOccupied())
                {
                    finalDestination = tile;
                }
                else
                {
                    foreach (Enemy enemy in EnemyManager.Instance.allEnemies)
                    {
                        if (enemy.TileCurrentlyOn == tile && enemy != target)
                        {
                            characterKnockedInto = enemy;
                            break;
                        }
                    }
                }
            }
        }

        //LevelManager.Instance.SetTileAsUnoccupiedByEntity(target.TileCurrentlyOn);
        target.TileCurrentlyOn.SetTileAsUnoccupiedByEntity();
        target.GridPosition = finalDestination.GridPosition;
        target.TileCurrentlyOn = finalDestination;
        //target.transform.position = finalDestination.WorldPosition;
        StartCoroutine(KnockBackEntityCoroutine(target, finalDestination.WorldPosition));
        //LevelManager.Instance.SetTileAsOccupiedByEntity(finalDestination);
        finalDestination.SetTileAsOccupiedByEntity(target);


        Debug.Log("Tiles on path: " + tilesOnPath.Count.ToString());
        Debug.Log("Target is " + direction + " of the the attacker");

    }
    public IEnumerator KnockBackEntityCoroutine(LivingEntity entityMoved, Vector3 destination)
    {
        Debug.Log("KnockBackMove() called by CombatLogic.cs....");
        bool movementCompleted = false;

        while (movementCompleted == false)
        {
            Debug.Log("KnockBackMove() moving entity...");
            entityMoved.transform.position = Vector2.MoveTowards(entityMoved.transform.position, destination, 8f * Time.deltaTime);
            if (entityMoved.transform.position == destination)
            {
                movementCompleted = true;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public Action OnLocationMovedTo(LivingEntity characterMoved)
    {
        Debug.Log("OnLocationMovedToCalled() called....");
        Action action = new Action();
        StartCoroutine(OnLocationMovedToCoroutine(characterMoved, action));
        return action;
    }

    public IEnumerator OnLocationMovedToCoroutine(LivingEntity characterMoved, Action action)
    {
        Debug.Log("OnLocationMovedToCalledCoroutine() called....");        
        PositionLogic.Instance.CheckForCamoflage(characterMoved);
        Action overwatchCheck = CheckAndResolveOverwatch(characterMoved);
        yield return new WaitUntil(() => overwatchCheck.ActionResolved() == true);
        action.actionResolved = true;           
        
    }


    public Action ResolveFreeStrikes(LivingEntity characterMoved, TileScript previousLocation, TileScript newLocation)
    {
        Debug.Log("ResolveFreeStrikes() called....");
        Action action = new Action();
        StartCoroutine(ResolveFreeStrikesCoroutine(characterMoved, action, previousLocation, newLocation));
        return action;
    }

    public Action CheckAndResolveOverwatch(LivingEntity characterMoved)
    {
        Action action = new Action();
        StartCoroutine(CheckAndResolveOverwatchCoroutine(characterMoved, action));
        return action;
    }

    public IEnumerator CheckAndResolveOverwatchCoroutine(LivingEntity characterMoved, Action action)
    {
        List<LivingEntity> enemiesWithOverwatch = new List<LivingEntity>();

        // Filter a list of enemies
        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(characterMoved, entity) == false &&
                entity.myPassiveManager.Overwatch)
            {
                enemiesWithOverwatch.Add(entity);
            }
        }

        foreach (LivingEntity entity in enemiesWithOverwatch)
        {
            // if there is LoS AND the attacker actually has a ranged weapon AND the character moving is in range of the attackers ranged weapon
            if (PositionLogic.Instance.IsThereLosFromAtoB(entity.TileCurrentlyOn, characterMoved.TileCurrentlyOn) &&
                entity.myRangedWeapon != null &&
                entity.IsTargetInRange(characterMoved, entity.myRangedWeapon.weaponRange) &&
                ((entity.myPassiveManager.HeavyWeapon && entity.myPassiveManager.Entrenched) ||
                  entity.myPassiveManager.HeavyWeapon == false)
                    )
            {
                    // All conditions for an overwatch attack have been met, perform an overwatch attack
                    // pause the character moving's movement animation so they dont appear to walk on the spot
                    characterMoved.myAnimator.enabled = false;
                    Action overwatchAction = AbilityLogic.Instance.PerformOverwatchShot(entity, characterMoved);
                    yield return new WaitUntil(() => overwatchAction.ActionResolved() == true);
                    characterMoved.myAnimator.enabled = true;
            }            
        }

        action.actionResolved = true;
    }

    public IEnumerator ResolveFreeStrikesCoroutine(LivingEntity characterMoved, Action action, TileScript previousLocation, TileScript newLocation)
    {
        /*
        Debug.Log("ResolveFreeStrikesCoroutine() called....");
        List<LivingEntity> unfriendlyEntities = new List<LivingEntity>();

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (CombatLogic.Instance.IsTargetFriendly(characterMoved, entity) == false)
            {
                unfriendlyEntities.Add(entity);
            }
        }

        foreach (LivingEntity entity in unfriendlyEntities)
        {
            if (PositionLogic.Instance.GetTargetsFrontArcTiles(entity).Contains(previousLocation) &&
                PositionLogic.Instance.GetTargetsFrontArcTiles(entity).Contains(newLocation) == false)
            {
                Debug.Log("ResolveFreeStrikesCoroutine() detected that " + characterMoved.name + " triggered a free strike from " + entity.name);
                Action freeStrikeAction = AbilityLogic.Instance.PerformFreeStrike(entity, characterMoved);
                yield return new WaitUntil(() => freeStrikeAction.ActionResolved() == true);
            }
        }
        */
        yield return null;

        action.actionResolved = true;
    }

    
   
}
