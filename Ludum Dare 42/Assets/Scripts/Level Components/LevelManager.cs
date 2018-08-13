using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeycardColor { Red, Yellow, Green, Blue };

public class LevelManager : MonoBehaviour {

	// Properties
	public int worldIndex;
	public int levelIndex;

	// References
	[HideInInspector]
	public Floor floor;
	public Player player;
	public GameObject tilesParent;
	public GameObject foregroundParent;

	[HideInInspector]
	public Dictionary<KeycardColor, int> obtainedKeycards;

	public static LevelManager Instance {
		get {
			return GM.Instance.currentLevelManager;
		}
	}

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

		IngameCanvas.Instance.resetAll();

		obtainedKeycards = new Dictionary<KeycardColor, int>();
		foreach (KeycardColor color in System.Enum.GetValues(typeof(KeycardColor))) {
			obtainedKeycards.Add(color, 0);
			if (floor.keycardTotals[color] > 0) {
				IngameCanvas.Instance.initKeycard(color);
			}
		}
	}

	// Use this for initialization
	void Start () {
		// TODO - position camera to view the whole floor? Or is that just being done level by level
	}

	// Keycard management
	public static void obtainKeycard(KeycardColor color) {
		GM.Instance.currentLevelManager.obtainedKeycards[color]++;
		int newTotal = GM.Instance.currentLevelManager.obtainedKeycards[color];
		if (newTotal == LevelManager.getFloor().keycardTotals[color]) {
			foreach (LockedDoor lockedDoor in getFloor().lockedDoors) {
				lockedDoor.tryToUnlock(color);
			}
		}

		IngameCanvas.Instance.updateKeycard(color, newTotal);
	}

	// Powerup management
	public static void applyPowerup(PowerupType type) {
		switch (type) {
			case PowerupType.Coffee:
				GM.Instance.currentLevelManager.player.jumpTwoActivated = true;
				break;
		}
		IngameCanvas.Instance.displayPowerup(type);
	}

	// Floor, tile, and foreground utility functions
	public static Floor getFloor() {
		return GM.Instance.currentLevelManager.floor;
	}

    public static Player getPlayer()
    {
        return GM.Instance.currentLevelManager.player;
    }

	public static Tile getTile(Vector2Int tilePosition) {
		return GM.Instance.currentLevelManager.floor.getTile(tilePosition);
	}

	public static ForegroundObject getForegroundObject(Vector2Int objPosition) {
		return GM.Instance.currentLevelManager.floor.getForegroundObj(objPosition);
	}

}
