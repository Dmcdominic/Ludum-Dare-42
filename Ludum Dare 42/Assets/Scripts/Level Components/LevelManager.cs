using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Inspector options
	public Vector2Int startingPosition;

	// References
	[HideInInspector]
	public Floor floor;
	public Player player;
	public GameObject tilesParent;
	public GameObject foregroundParent;

	private void Awake() {
		if (GM.Instance.currentLevelManager != null) {
			Debug.LogError("Multiple LevelManagers active. Please destroy the first one before instantiating another.");
		}
		GM.Instance.currentLevelManager = this;

		if (tilesParent == null) {
			Debug.LogError("Please drag the Tiles parent object into the Tiles Parent field of the LevelManager.");
		} else if (foregroundParent == null) {
			Debug.LogError("Please drag the ForegroundObjects parent object into the Foreground Parent field of the LevelManager.");
		} else {
			floor = new Floor(tilesParent, foregroundParent);
		}
	}

	// Use this for initialization
	void Start () {
		// TODO - player animation when he arrives at the starting point?
		player.placeAtPosition(startingPosition);

		// TODO - position camera to view the whole floor?
	}

	// Floor, tile, and foreground utility functions
	public static Tile getTile(Vector2Int tilePosition) {
		return GM.Instance.currentLevelManager.floor.tileGrid[tilePosition.x, tilePosition.y];
	}

	public static ForegroundObject getForegroundObject(Vector2Int objPosition) {
		return GM.Instance.currentLevelManager.floor.foregroundGrid[objPosition.x, objPosition.y];
	}

}
