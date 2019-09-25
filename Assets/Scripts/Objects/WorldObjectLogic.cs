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
        location.SetTileAsOccupiedByObject(newWorldObjectScript);
        newWorldObjectScript.myTile = location;       
    }
}
