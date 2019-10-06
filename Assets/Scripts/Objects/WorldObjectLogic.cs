using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectLogic : Singleton<WorldObjectLogic>
{   
    public void CreateObjectAtLocation(GameObject objectPrefab, TileScript location)
    {
        GameObject newWorldObject = Instantiate(objectPrefab, location.transform);
        WorldObject newWorldObjectScript = newWorldObject.GetComponent<WorldObject>();
        newWorldObject.transform.position = location.WorldPosition;
        // set tree object a little higher to prevent low hanging into tiles below
        if(newWorldObjectScript.objectType == WorldObject.ObjectType.Tree)
        {
            newWorldObject.transform.position = new Vector3(location.WorldPosition.x, location.WorldPosition.y + 0.3f, 0);
        }
        
        location.SetTileAsOccupiedByObject(newWorldObjectScript);
        newWorldObjectScript.myTile = location;
        TileSpriteManager.Instance.DetermineAndSetWorldObjectSprite(newWorldObjectScript);
    }
}
