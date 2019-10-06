using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    public enum ObjectType { Tree, Rubble, RockWall, SandBag };
    [Header("Component References")]
    public SpriteRenderer spriteRenderer;
    public Animator myAnimator;
    public TileScript myTile;

    [Header("Properties")]
    public ObjectType objectType;
    public bool canBeMovedThrough;
    public bool preventsOccupation;
    public bool blocksLOS;
    public bool providesHalfCover;
    public bool providesFullCover;
        
}
