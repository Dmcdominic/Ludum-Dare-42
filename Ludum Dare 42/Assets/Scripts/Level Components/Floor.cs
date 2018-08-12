using System.Collections;
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

	// Locked doors list and obtainable keycard total
	public List<LockedDoor> lockedDoors;
	public Dictionary<KeycardColor, int> keycardTotals;


	// Constructor
	public Floor(GameObject tilesParent, GameObject foregroundParent) {
		initGrids(tilesParent, foregroundParent);
		populateTileGrid(tilesParent);
		populateForegroundGrid(foregroundParent);
	}

	private void initGrids(GameObject tilesParent, GameObject foregroundParent) {
		Tile[] tiles = tilesParent.GetComponentsInChildren<Tile>();
		ForegroundObject[] fgObjects = foregroundParent.GetComponentsInChildren<ForegroundObject>();

		lockedDoors = new List<LockedDoor>();
		keycardTotals = new Dictionary<KeycardColor, int>();
		foreach (KeycardColor color in System.Enum.GetValues(typeof(KeycardColor))) {
			keycardTotals.Add(color, 0);
		}

		if (tiles.Length == 0) {
			Debug.LogError("Please add some tiles to the level, or remove the level management framework.");
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
			Vector2Int truePos = pos3dToVect2Int(fgObjects[i].transform.position);

			updateFgGridForAllPos(fgObjects[i], truePos, fgObjects[i].additionalCoords, true);

			if (fgObjects[i] is LockedDoor) {
				lockedDoors.Add((LockedDoor)fgObjects[i]);
			} else if (fgObjects[i] is Keycard) {
				Keycard keycard = (Keycard)fgObjects[i];
				keycardTotals[keycard.color]++;
			}
		}
	}

	// Grid utility
	public void updateTile(Vector2Int truePos, Tile newTile) {
		Vector2Int gridVect = posToGridVect(truePos);
		if (gridVect.x < 0 || gridVect.x >= gridWidth ||
			gridVect.y < 0 || gridVect.y >= gridHeight) {
			return;
		}
		tileGrid[gridVect.x, gridVect.y] = newTile;

		Vector2Int belowTruePos = truePos + new Vector2Int(0, -1);
		if (gridCoordsValid(belowTruePos)) {
			getTile(belowTruePos).onAboveTileUpdated(newTile);
		}
	}

	// Make private
	public void updateForegroundObj(Vector2Int truePos, ForegroundObject newObj) {
		Vector2Int gridVect = posToGridVect(truePos);
		if (gridVect.x < 0 || gridVect.x >= gridWidth ||
			gridVect.y < 0 || gridVect.y >= gridHeight) {
			return;
		}
		//Debug.Log("Updating position: " + truePos + " to object: " + newObj);
		foregroundGrid[gridVect.x, gridVect.y] = newObj;
	}

	// Update the foregroundGrid for this objects position, and all its additionalCoords
	public void updateFgGridForAllPos(ForegroundObject obj, Vector2Int truePos, List<Vector2Int> additionalCoords, bool initializing) {
		updateForegroundObj(truePos, obj);
		
		foreach (Vector2Int relativePos in additionalCoords) {
			Vector2Int pos = truePos + relativePos;
			if (initializing && getForegroundObj(pos) != null) {
				Debug.LogError("Two foregroundObject objects were found at (" + pos.x + ", " + pos.y + "). Please delete one.");
			} else {
				updateForegroundObj(pos, obj);
			}
		}
	}

	// Object access methods to account for grid offset
	public Tile getTile(Vector2Int pos) {
		Vector2Int gridVect = posToGridVect(pos);
		if (!gridCoordsValid(gridVect)) {
			return null;
		}
		return tileGrid[gridVect.x, gridVect.y];
	}

	public ForegroundObject getForegroundObj(Vector2Int truePos) {
		Vector2Int gridVect = posToGridVect(truePos);
		if (!gridCoordsValid(gridVect)) {
			return null;
		}
		return foregroundGrid[gridVect.x, gridVect.y];
	}

	private bool gridCoordsValid(Vector2Int gridVect) {
		return (gridVect.x >= 0 && gridVect.x < gridWidth && gridVect.y >= 0 && gridVect.y < gridHeight);
	}

	// Vector utility
	private Vector2Int pos3dToGridVect(Vector3 pos) {
		return posToGridVect(pos3dToVect2Int(pos));
	}

	private Vector2Int posToGridVect(Vector2Int pos) {
		int gridX = pos.x - minX;
		int gridY = pos.y - minY;
		return new Vector2Int(gridX, gridY);
	}

	public static Vector2Int pos3dToVect2Int(Vector3 pos) {
		return new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
	}

	public static Vector3 Vect2IntToPos3d(Vector2Int coords) {
		return new Vector3(coords.x, coords.y, 0);
	}

}
