using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    [Header("Component References")]
    public SpriteRenderer spriteRenderer;
    public Animator myAnimator;

    [Header("Properties")]
    public TileScript myTile;
    public bool canBeMovedThrough;
    public bool preventsOccupation;
    public bool blocksLOS;
    public bool providesHalfCover;
    public bool providesFullCover;
        
}
