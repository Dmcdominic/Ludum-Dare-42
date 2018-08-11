﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

	// Tiles and foreground object grids
	private Tile[,] tileGrid;
	private ForegroundObject[,] foregroundGrid;

	// Grid dimensions
	public int gridWidth;
	public int gridHeight;

	private int minX = int.MaxValue;
	private int minY = int.MaxValue;
	private int maxX = int.MinValue;
	private int maxY = int.MinValue;

	// Obtainable keycards
	public Dictionary<KeycardColor, int> keycardTotals;


	// Constructor
	public Floor(GameObject tilesParent, GameObject foregroundParent) {
		initGrids(tilesParent, foregroundParent);
		populateTileGrid(tilesParent);
		populateForegroundGrid(foregroundParent);
		getKeycardTotals(tilesParent);
	}

	private void initGrids(GameObject tilesParent, GameObject foregroundParent) {
		Tile[] tiles = tilesParent.GetComponentsInChildren<Tile>();
		ForegroundObject[] fgObjects = foregroundParent.GetComponentsInChildren<ForegroundObject>();

		if (tiles.Length == 0) {
			Debug.LogError("Please add some tiles to the editor.");
			return;
		}

		for (int i = 0; i < tiles.Length; i++) {
			int x = Mathf.RoundToInt(tiles[i].transform.position.x);
			int y = Mathf.RoundToInt(tiles[i].transform.position.y);

			if (x < minX) {
				minX = x;
			}
			if (y < minY) {
				minY = y;
			}
			if (x > maxX) {
				maxX = x;
			}
			if (y > maxY) {
				maxY = y;
			}
		}

		for (int i = 0; i < fgObjects.Length; i++) {
			int x = Mathf.RoundToInt(fgObjects[i].transform.position.x);
			int y = Mathf.RoundToInt(fgObjects[i].transform.position.y);

			if (x < minX) {
				minX = x;
			}
			if (y < minY) {
				minY = y;
			}
			if (x > maxX) {
				maxX = x;
			}
			if (y > maxY) {
				maxY = y;
			}
		}

		gridWidth = (maxX - minX) + 1;
		gridHeight = (maxY - minY) + 1;

		tileGrid = new Tile[gridWidth, gridHeight];
		foregroundGrid = new ForegroundObject[gridWidth, gridHeight];
	}

	private void populateTileGrid(GameObject parent) {
		Tile[] tiles = parent.GetComponentsInChildren<Tile>();

		for (int i = 0; i < tiles.Length; i++) {
			int x = Mathf.RoundToInt(tiles[i].transform.position.x) - minX;
			int y = Mathf.RoundToInt(tiles[i].transform.position.y) - minY;
			
			if (tileGrid[x, y] != null) {
				int trueX = x + minX;
				int trueY = y + minY;
				Debug.LogError("Two Tile objects were found at (" + trueX + ", " + trueY + "). Please delete one.");
			} else {
				tileGrid[x, y] = tiles[i];
			}
		}
	}

	// Sort all of the ForegroundObject children of "parent" into the grid
	private void populateForegroundGrid(GameObject parent) {
		ForegroundObject[] fgObjects = parent.GetComponentsInChildren<ForegroundObject>();

		for (int i = 0; i < fgObjects.Length; i++) {
			int x = Mathf.RoundToInt(fgObjects[i].transform.position.x) - minX;
			int y = Mathf.RoundToInt(fgObjects[i].transform.position.y) - minY;

			if (foregroundGrid[x, y] != null) {
				int trueX = x + minX;
				int trueY = y + minY;
				Debug.LogError("Two foregroundObject objects were found at (" + trueX + ", " + trueY + "). Please delete one.");
			} else {
				foregroundGrid[x, y] = fgObjects[i];
			}
		}
	}

	// Find the total number of each keycard color
	private void getKeycardTotals(GameObject tilesParent) {
		keycardTotals = new Dictionary<KeycardColor, int>();
		foreach (KeycardColor color in System.Enum.GetValues(typeof(KeycardColor))) {
			keycardTotals.Add(color, 0);
		}

		Keycard[] keycards = tilesParent.GetComponentsInChildren<Keycard>();
		foreach (Keycard keycard in keycards) {
			keycardTotals[keycard.color]++;
		}
	}

	// Object access methods to account for grid offset
	public Tile getTile(Vector2Int pos) {
		int gridX = pos.x - minX;
		int gridY = pos.y - minY;
		if (gridX < 0 || gridX >= gridWidth ||
			gridY < 0 || gridY >= gridHeight) {
			return null;
		}
		return tileGrid[gridX, gridY];
	}

	public ForegroundObject getForegroundObj(Vector2Int pos) {
		int gridX = pos.x - minX;
		int gridY = pos.y - minY;
		if (gridX < 0 || gridX >= gridWidth ||
			gridY < 0 || gridY >= gridHeight) {
			return null;
		}
		return foregroundGrid[gridX, gridY];
	}

}
