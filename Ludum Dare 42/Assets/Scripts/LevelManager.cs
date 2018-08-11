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
		player.levelManager = this;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
