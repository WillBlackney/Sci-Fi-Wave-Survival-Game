using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{   

    [Header("Component References")]
    private SpriteRenderer spriteRenderer;
    public Animator myAnimator;

    [Header("Properties")]
    public TileType myTileType;
    public TileSetupType myTileSetupType;
    public WorldObject myObject;
    public Color32 originalColor;
    public Color32 highlightedColor = Color.white;
    public bool isEmpty;
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
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
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
    public void RunDirtTileSetup()
    {
        isWalkable = true;
        isEmpty = true;
    }
    public void RunDirtTreeTileSetup()
    {
        isWalkable = true;
        isEmpty = false;
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.treePrefab, this);
    }
    public void RunDirtRockTileSetup()
    {
        isWalkable = true;
        isEmpty = false;
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.rockWallPrefab, this);
    }
    public void RunDirtRubbleTileSetup()
    {
        isWalkable = true;
        isEmpty = false;
        WorldObjectLogic.Instance.CreateObjectAtLocation(PrefabHolder.Instance.rubblePrefab, this);
    }
    public void RunGrassTileSetup()
    {
        isWalkable = true;
        isEmpty = true;
    }
    public void RunRockTileSetup()
    {
        isWalkable = true;
        isEmpty = true;
    }
    public void RunForestTileSetup()
    {
        isWalkable = false;
        isEmpty = false;
    }
    public void RunWaterTileSetup()
    {
        isWalkable = false;
        isEmpty = true;
    }


    // Input + mouse related events
    private void OnMouseOver()
    {
        LevelManager.Instance.mousedOverTile = this;

        // TO DO: The tile colouring is currently disabled to fix highlighting moveable tiles, fix this
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedButton != null)
        {
            //Hover.Instance.SetPosition(WorldPosition);

            /*
            if (IsEmpty && !Debugging)
            {
                //ColorTile(emptyColour);
            }
            if (!IsEmpty && !Debugging)
            {
                //ColorTile(fullColour);
            }
            */
            if (Input.GetMouseButtonDown(0))
            {
                PlaceDefender();
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

    }


    // Defender / entity creation related
    public void PlaceDefender()
    {
        GameObject newDefenderGO = Instantiate(GameManager.Instance.ClickedButton.defenderPrefab, DefenderManager.Instance.defendersParent.transform);

        newDefenderGO.GetComponent<Defender>().InitializeSetup(this.GridPosition, this);

        //GameManager.Instance.ClickedButton = null;
        Hover.Instance.Deactivate();
    }

   
    // Visual + Animation
    public void ColorTile(Color newColor)
    {
        //Debug.Log("Coloring tile...");
        spriteRenderer.color = newColor;
    }


    // Cover, LoS, Movement World Object Logic
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
        if(myObject == null && isWalkable)
        {
            return true;
        }
        if (myObject != null && myObject.canBeMovedThrough && isWalkable)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CanBeOccupied()
    {
        if (myObject == null && isWalkable && isEmpty)
        {
            return true;
        }
        if (myObject != null && myObject.preventsOccupation == false && isWalkable && isEmpty)
        {
            return true;
        }
        else
        {
            return false;
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

    // World Object related



}
