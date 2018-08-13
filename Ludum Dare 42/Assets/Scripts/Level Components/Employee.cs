using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Employee : MonoBehaviour {
    [HideInInspector]
    public UnityEvent ue;
    public Direction dir;
    private SpriteRenderer spr;
    private Player player;

    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    [HideInInspector]
    public enum Direction {Up, Down, Left, Right};

	// Use this for initialization
	void Start () {
        player = GM.Instance.currentLevelManager.player;
        // Round the player's position to whole numbers
        Vector3 pos = transform.position;
        placeAtPosition(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
        ue = player.OnSuccessfulStep;
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        ue.AddListener(MoveDir);
    }

    // References
    private Animator animator;

    // Properties
    [HideInInspector]
    public Vector2Int posRounded;

    //Event fires when a successful step (one or two) is taken
    [HideInInspector]
    public UnityEvent OnSuccessfulStep = new UnityEvent();

    // Initialization
    private void Awake()
    {
        
        
    }

    void SwitchDir(Direction d)
    {
        switch (d)
        {
            case Direction.Down:
                dir = Direction.Up;
                spr.sprite = up;
                break;
            case Direction.Up:
                dir = Direction.Down;
                spr.sprite = down;
                break;
            case Direction.Left:
                dir = Direction.Right;
                spr.sprite = right;
                break;
            case Direction.Right:
                dir = Direction.Left;
                spr.sprite = left;
                break;
        }
        
    }

    void MoveDir()
    {
        int distance = 1;
        switch (dir)
        {
            case Direction.Up:
                tryMove(new Vector2Int(0, distance));
                break;
            case Direction.Down:
                tryMove(new Vector2Int(0, -distance));
                break;
            case Direction.Left:
                tryMove(new Vector2Int(-distance, 0));
                break;
            case Direction.Right:
                tryMove(new Vector2Int(distance, 0));
                break;
        }
    }
    // Player input management
    void Update()
    {
        
    }


    // Player movement
    private bool tryMove(Vector2Int displacement)
    {
        Vector2Int targetPos = posRounded + displacement;
        
        if (canMoveNormal(displacement, targetPos))
        {
            move(displacement, targetPos);
            OnSuccessfulStep.Invoke();
            return true;
        }

        // TODO - play "invalid move" sound effect?
        SwitchDir(dir);
        return false;
    }

    public bool canMoveNormal(Vector2Int displacement, Vector2Int targetPos)
    {
        Tile tile = LevelManager.getTile(targetPos);
        ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
        if (foregroundObj)
        {
            return false;
        }
        return (tile != null && (tile.isSteppableForNPC() || tile.isHole()));
    }

    public void move(Vector2Int displacement, Vector2Int targetPosition)
    {
        Tile prevTile = LevelManager.getTile(posRounded);
        prevTile.OnLeave();
        Tile nextTile = LevelManager.getTile(targetPosition);
        nextTile.OnStep();
        if (nextTile.isHole())
        {
            Destroy(this.gameObject);
        }
        normalMoveAnim(displacement, targetPosition);      
    }

    public void placeAtPosition(Vector2Int position)
    {
        transform.position = new Vector3(position.x, position.y, transform.position.z);
        posRounded = position;
    }


    // Animation management
    private void normalMoveAnim(Vector2Int displacement, Vector2Int targetPosition)
    {
        // TODO - add animation
        // For now:
        this.transform.position += new Vector3(displacement.x, displacement.y, 0);
        posRounded = targetPosition;
    }

    //public void onAnimationEnd() {
    //	Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("TODO"));

    //	//isAnimating = false;
    //}

}
