using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	// Inspector options
	public Vector2Int startingPosition;

	// References
	[HideInInspector]
	public Floor currentFloor;
	public Player player;

	private void Awake() {
		if (GM.Instance.currentLevelManager != null) {
			Debug.LogError("Multiple LevelManagers active. Please destroy the first one before instantiating another.");
		}
		GM.Instance.currentLevelManager = this;
	}

	// Use this for initialization
	void Start () {
		// TODO - position camera to view the whole floor?
	}

}
