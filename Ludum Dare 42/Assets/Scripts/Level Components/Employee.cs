using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Employee : MonoBehaviour {
    [HideInInspector]
    public UnityEvent ue;
    public Direction dir;

    [HideInInspector]
    public enum Direction {Up, Down, Left, Right};

	// Use this for initialization
	void Start () {
        ue = GM.Instance.currentLevelManager.player.OnSuccessfulStep;
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
        // Round the player's position to whole numbers
        Vector3 pos = transform.position;
        placeAtPosition(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));

        ue.AddListener(MoveDir);
    }

    void SwitchDir(Direction d)
    {
        switch (d)
        {
            case Direction.Down:
                d = Direction.Up;
                //TODO: change sprite
                break;
            case Direction.Up:
                d = Direction.Down;
                //TODO: change sprite
                break;
            case Direction.Left:
                d = Direction.Right;
                //TODO: change sprite
                break;
            case Direction.Right:
                d = Direction.Left;
                //TODO: change sprite
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
        Debug.Log(posRounded);
        
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
        return ((tile != null && (tile.isSteppable() || false)) && (foregroundObj == null || foregroundObj.IsSteppable(MoveType.normal, displacement)));
    }

    public void move(Vector2Int displacement, Vector2Int targetPosition)
    {
        Tile prevTile = LevelManager.getTile(posRounded);
        prevTile.OnLeave();
        Tile nextTile = LevelManager.getTile(targetPosition);
        nextTile.OnStep();

        ForegroundObject foregroundObject = LevelManager.getForegroundObject(targetPosition);
        if (foregroundObject != null)
        {
            foregroundObject.OnInteraction(MoveType.normal, displacement);
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
