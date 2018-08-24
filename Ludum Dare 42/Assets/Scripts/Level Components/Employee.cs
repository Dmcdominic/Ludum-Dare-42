using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[HideInInspector]
public enum Direction { Up, Down, Left, Right };

public class Employee : MonoBehaviour {
	// Editor settings
	public Direction dir;
	
	public Sprite up;
	public Sprite down;
	public Sprite right;

	// References
	[SerializeField]
	private Animator animator;
	private SpriteRenderer sr;

	// Properties
	[HideInInspector]
	public Vector2Int truePos;


	// Initialization
	private void Awake() {
		sr = GetComponentInChildren<SpriteRenderer>();

		// Round the position to whole numbers
		Vector3 pos = transform.position;
		placeAtPosition(new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
	}

	// Use this for initialization
	void Start() {
		Player player = GM.Instance.currentLevelManager.player;
		UnityEvent ue = player.StartSuccessfulStep;
		ue.AddListener(MoveDir);

		LevelManager.getFloor().employees.Add(this);

		updateSprite();
	}

	// Reverse the currently facing direction
	private void SwitchDir(Direction d) {
		switch (d) {
			case Direction.Down:
				dir = Direction.Up;
				break;
			case Direction.Up:
				dir = Direction.Down;
				break;
			case Direction.Left:
				dir = Direction.Right;
				break;
			case Direction.Right:
				dir = Direction.Left;
				break;
		}
		updateSprite();
	}

	private void updateSprite() {
		switch(dir) {
			case Direction.Down:
				sr.sprite = up;
				break;
			case Direction.Up:
				sr.sprite = down;
				break;
			case Direction.Left:
				sr.sprite = right;
				sr.flipX = true;
				break;
			case Direction.Right:
				sr.sprite = right;
				sr.flipX = false;
				break;
		}
	}

	// Try to move one unit in the current direction.
	// Will turn to face the opposite direction if the move fails.
	private void MoveDir() {
		tryMove(getNextMoveVector());
	}

	private bool tryMove(Vector2Int displacement) {
		Vector2Int targetPos = truePos + displacement;

		if (canMoveNormal(displacement, targetPos)) {
			move(displacement, targetPos);
			return true;
		}

		SwitchDir(dir);
		return false;
	}

	public bool canMoveNormal(Vector2Int displacement, Vector2Int targetPos) {
		Tile tile = LevelManager.getTile(targetPos);
		ForegroundObject foregroundObj = LevelManager.getForegroundObject(targetPos);
		if (foregroundObj) {
			return false;
		}
		return (tile != null && (tile.isSteppableForNPC() || tile.isHole()));
	}

	public void move(Vector2Int displacement, Vector2Int targetPosition) {
		Tile prevTile = LevelManager.getTile(truePos);
		prevTile.OnLeave();
		Tile nextTile = LevelManager.getTile(targetPosition);
		nextTile.OnStep();

		placeAtPosition(targetPosition);
		normalMoveAnim(displacement, targetPosition);

		// What happens when an employee falls into a hole
		if (nextTile.isHole()) {
			Destroy(this.gameObject);
		}
	}

	public void placeAtPosition(Vector2Int position) {
		transform.position = new Vector3(position.x, position.y, transform.position.z);
		truePos = position;
	}

	// Animation management
	private void normalMoveAnim(Vector2Int displacement, Vector2Int targetPosition) {
		string animName = "Walking" + AnimUtil.getDirectionPostfix(displacement);
		animator.Play(animName);
	}

	// Get the displacement vector of the next move that will be attempted
	public Vector2Int getNextMoveVector() {
		int distance = 1;
		switch (dir) {
			case Direction.Up:
				return new Vector2Int(0, distance);
			case Direction.Down:
				return new Vector2Int(0, -distance);
			case Direction.Left:
				return new Vector2Int(-distance, 0);
			case Direction.Right:
				return new Vector2Int(distance, 0);
			default:
				Debug.LogError("Invalid direction: " + dir);
				return Vector2Int.zero;
		}
	}

	// Returns the position that this employee will move to on the next step event
	public Vector2Int getNextTruePos() {
		Vector2Int displacement = getNextMoveVector();
		Vector2Int targetPos = truePos + displacement;

		if (canMoveNormal(displacement, targetPos)) {
			return targetPos;
		} else {
			return truePos;
		}
	}

	// Predicate to check if the given position will be occupied by any employees on the next step event
	public static bool willEmployeeMoveInto(Vector2Int targetPos) {
		foreach (Employee employee in LevelManager.getFloor().employees) {
			if (employee != null && employee.getNextTruePos() == targetPos) {
				return true;
			}
		}

		return false;
	}

	// Predicate to check if any employees will be making the opposite move, i.e. stepping across the player
	public static bool willEmployeeSwapWith(Vector2Int playerCurrentPos, Vector2Int playerTargetPos) {
		foreach (Employee employee in LevelManager.getFloor().employees) {
			if (employee != null && employee.getNextTruePos() == playerCurrentPos && employee.truePos == playerTargetPos) {
				return true;
			}
		}

		return false;
	}

}
