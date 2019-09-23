using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class AILogic 
{
    public static TileScript GetBestValidMoveLocationBetweenMeAndTarget(LivingEntity characterActing, LivingEntity target, int rangeFromTarget, int movePoints)
    {
        TileScript tile = LevelManager.Instance.GetClosestValidTile(LevelManager.Instance.GetTilesWithinRange(rangeFromTarget, target.TileCurrentlyOn), characterActing.TileCurrentlyOn);
        Stack<Node> pathFromMeToIdealTile = AStar.GetPath(characterActing.TileCurrentlyOn.GridPosition, tile.GridPosition);        

        Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() generated a path with " +
            pathFromMeToIdealTile.Count.ToString() + " tiles on it"
            );

        // if there is no possible path to the target
        if (pathFromMeToIdealTile.Count == 0)
        {
            // to do in future: in order to prevent enemies being trapped and unable to draw a path,
            // if this method detects pathing is impossible, the enemy should attack the nearest obstacle in an attempt to make a path
            Debug.Log("GetBestValidMoveLocationBetweenMeAndTarget() detected that there is no possible path between " + characterActing.name + " and " + target.name);
            return null;
        }

        if(pathFromMeToIdealTile.Count < movePoints)
        {
            return pathFromMeToIdealTile.ElementAt(pathFromMeToIdealTile.Count - 1).TileRef;
        }
        else
        {
            return pathFromMeToIdealTile.ElementAt(movePoints - 1).TileRef;
        }
    }

    public static bool IsEngagedInMelee(LivingEntity enemyConsidered)
    {
        List<TileScript> tilesInMyMeleeRange = LevelManager.Instance.GetTilesWithinRange(1, enemyConsidered.TileCurrentlyOn);
        bool inMelee = false;

        foreach (LivingEntity entity in LivingEntityManager.Instance.allLivingEntities)
        {
            if (tilesInMyMeleeRange.Contains(entity.TileCurrentlyOn) && CombatLogic.Instance.IsTargetFriendly(enemyConsidered, entity) == false)
            {
                inMelee = true;
            }
        }

        return inMelee;
    }


}
