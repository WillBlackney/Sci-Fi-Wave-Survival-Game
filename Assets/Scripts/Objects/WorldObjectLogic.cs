using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObjectLogic : Singleton<WorldObjectLogic>
{
    public List<WorldObject> allWorldObjects = new List<WorldObject>();
    public void CreateObjectAtLocation(GameObject objectPrefab, TileScript location, bool setSprite = false)
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
        allWorldObjects.Add(newWorldObjectScript);
        if (setSprite == true)
        {
            TileSpriteManager.Instance.DetermineAndSetWorldObjectSprite(newWorldObjectScript);
        }
        
        
    }
}
