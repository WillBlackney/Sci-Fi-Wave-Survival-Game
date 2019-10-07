using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{   
    [Header("Component References")]
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer edgeSpriteRenderer;
    public Animator myAnimator;

    [Header("Properties")]
    public TileType myTileType;
    public TileSetupType myTileSetupType;
    public WorldObject myObject;
    public LivingEntity myEntity;
    public Color32 originalColor;
    public Color32 highlightedColor = Color.white;    
    public bool isWalkable;
    public bool isBuildable;    


    // Properties
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f);
        }
    }
    public Point GridPosition { get; set; }

    // Enum declarations
    public enum TileSetupType { None, Dirt, DirtTree, DirtRock, DirtRubble, DirtGoldNode, DirtSteelNode, Grass, GrassTree, Rock, Water };
    public enum TileType { None, Dirt, Grass, Forest, Rock, Water };


    // Setup Initialization
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //myAnimator = GetComponent<Animator>();
        originalColor = spriteRenderer.color;
    }
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent)
    {        

        if (myTileSetupType == TileSetupType.Dirt)
        {
            RunDirtTileSetup();
        }
        else if (myTileSetupType == TileSetupType.DirtTree)
        {
            RunDirtTreeTileSetup();
        }
        else if (myTileSetupType == TileSetupType.DirtRock)
        {
            RunDirtRockTileSetup();
        }
        else if (myTileSetupType == TileSetupType.DirtRubble)
        {
            RunDirtRubbleTileSetup();
        }
        else if (myTileSetupType == TileSetupType.Grass)
        {
            RunGrassTileSetup();
        }

        else if (myTileSetupType == TileSetupType.Water)
        {
            RunWaterTileSetup();
        }


        GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }


    // Set tile type
    #region
    public void RunDirtTileSetup()
    {
        isWalkable = true;        
    }
    public void RunDirtTreeTileSetup()
    {
        isWalkable = true;        
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.treePrefab, this);
    }
    public void RunDirtRockTileSetup()
    {
        isWalkable = true;        
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.rockWallPrefab, this);
    }
    public void RunDirtRubbleTileSetup()
    {
        isWalkable = true;        
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.rubblePrefab, this);
    }
    public void RunGrassTileSetup()
    {
        isWalkable = true;        
    }        
    public void RunWaterTileSetup()
    {
        isWalkable = false;        
    }
    #endregion

    // Input + mouse related events
    #region
    private void OnMouseOver()
    {
        LevelManager.Instance.mousedOverTile = this;
        TileCoverHover.Instance.OnNewTileMousedOver(this);

        // TO DO: The tile colouring is currently disabled to fix highlighting moveable tiles, fix this
        if (!EventSystem.current.IsPointerOverGameObject() && DefenderPanelManager.Instance.ClickedDefender != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (LevelManager.Instance.GetSpaceShipControlZoneTile().Contains(this) == false)
                {
                    Debug.Log("Cannot place defender: Chosen tile is outside the build zone");
                }
                else if (CanBeOccupied() == false)
                {
                    Debug.Log("Cannot place defender: Chosen tile cannot be occupied");
                }
                else
                {
                    PlaceDefender();
                }                
            }
        }
    }
    public void OnMouseDown()
    {
        LevelManager.Instance.selectedTile = this;
        Defender selectedDefender = DefenderManager.Instance.selectedDefender;

        if (selectedDefender != null && selectedDefender.awaitingMoveOrder == true)
        {
            Debug.Log("Starting Movement Process...");
            selectedDefender.StartMoveAbilityProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingChargeLocationOrder == true)
        {
            selectedDefender.StartChargeProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingMeteorOrder == true)
        {
            selectedDefender.StartMeteorProcess(this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingTelekinesisLocationOrder == true)
        {
            selectedDefender.StartTelekinesisProcess(selectedDefender.myCurrentTarget, this);
        }

        else if (selectedDefender != null && selectedDefender.awaitingDashOrder == true)
        {
            selectedDefender.StartDashProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingGetDownOrder == true)
        {
            selectedDefender.StartGetDownProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingThrowHandGrenadeTarget == true)
        {
            selectedDefender.StartThrowHandGrenadeProcess(this);
        }
        else if (selectedDefender != null && selectedDefender.awaitingAreaSupressionTarget == true)
        {
            selectedDefender.StartAreaSupressionProcess(this);
        }

    }
    #endregion

    // Defender / entity creation related
    #region
    public void PlaceDefender()
    {
        GameObject newDefenderGO = Instantiate(DefenderPanelManager.Instance.ClickedDefender.defenderPrefab, DefenderManager.Instance.defendersParent.transform);

        newDefenderGO.GetComponent<Defender>().InitializeSetup(GridPosition, this);

        DefenderPanelManager.Instance.BuyDefender();

        DefenderPanelManager.Instance.ClickedDefender = null;
        
        Hover.Instance.Deactivate();
    }
    #endregion

    // Visual + Animation
    #region
    public void ColorTile(Color newColor)
    {
        //Debug.Log("Coloring tile...");
        spriteRenderer.color = newColor;
    }
    #endregion

    // Cover, LoS, Movement World Object Logic
    #region
    public bool ProvidesHalfCover()
    {
        if (myObject != null && myObject.providesHalfCover)
        {
            return true;
        }
        else
        {
            return false;
        }

    }    
    public bool ProvidesFullCover()
    {
        if (myObject != null && myObject.providesFullCover)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public bool CanBeMovedThrough()
    {
        if(isWalkable == false)
        {
            return false;
        }

        else if(myObject != null && myObject.canBeMovedThrough == false)
        {
            return false;
        }

        else if(myEntity != null)
        {
            return false;
        }
        
        else
        {
            return true;
        }
    }
    public bool CanBeOccupied()
    {
        if(isWalkable == false)
        {
            return false;
        }
        else if(myObject != null && myObject.preventsOccupation)
        {
            return false;
        }
        else if(myEntity != null)
        {
            return false;
        }
        else
        {
            return true;
        }
       
    }
    public bool CanBeSeenThrough()
    {
        if (myObject == null)
        {
            return true;
        }
        if (myObject != null && myObject.blocksLOS)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    // Occupation by Entity / World Object
    #region
    public void SetTileAsOccupiedByEntity(LivingEntity entity)
    {
        Debug.Log("Tile " + GridPosition.X + ", " + GridPosition.Y + " is now occupied");
        myEntity = entity;        
    }
    public void SetTileAsUnoccupiedByEntity()
    {
        Debug.Log("Tile " + GridPosition.X + ", " + GridPosition.Y + " is now unoccupied");
        myEntity = null;        
    }
    public void SetTileAsOccupiedByObject(WorldObject worldObject)
    {
        //Debug.Log("Tile " + GridPosition.X + ", " + GridPosition.Y + " is now occupied");
        myObject = worldObject;
    }
    public void SetTileAsUnoccupiedByObject()
    {
        //Debug.Log("Tile " + GridPosition.X + ", " + GridPosition.Y + " is now unoccupied");
        myObject = null;
    }
    #endregion





}
