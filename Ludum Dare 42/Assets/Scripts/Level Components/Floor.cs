using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

	public Tile[,] tileGrid;
	public ForegroundObject[,] foregroundGrid;

	public Floor(GameObject tilesParent, GameObject foregroundParent) {
		populateTileGrid(tilesParent);
		populateForegroundGrid(foregroundParent);
	}

	private void populateTileGrid(GameObject parent) {
		Tile[] tiles = parent.GetComponentsInChildren<Tile>();

		int maxX = 0;
		int maxY = 0;
		
		for (int i = 0; i < tiles.Length; i++) {
			int x = Mathf.RoundToInt(tiles[i].transform.position.x);
			int y = Mathf.RoundToInt(tiles[i].transform.position.y);

			if (x > maxX) {
				maxX = x;
			}
			if (y > maxY) {
				maxY = y;
			}
		}

		tileGrid = new Tile[maxX + 1, maxY + 1];

		for (int i = 0; i < tiles.Length; i++) {
			int x = Mathf.RoundToInt(tiles[i].transform.position.x);
			int y = Mathf.RoundToInt(tiles[i].transform.position.y);

			if (tileGrid[x, y] != null) {
				Debug.LogError("Two Tile objects were found at (" + x + ", " + y + "). Please delete one.");
			} else {
				tileGrid[x, y] = tiles[i];
			}
		}
	}

	// Sort all of the ForegroundObject children of "parent" into the grid
	private void populateForegroundGrid(GameObject parent) {
		ForegroundObject[] fgObjects = parent.GetComponentsInChildren<ForegroundObject>();

		int maxX = 0;
		int maxY = 0;
		
		for (int i = 0; i < fgObjects.Length; i++) {
			int x = Mathf.RoundToInt(fgObjects[i].transform.position.x);
			int y = Mathf.RoundToInt(fgObjects[i].transform.position.y);

			if (x > maxX) {
				maxX = x;
			}
			if (y > maxY) {
				maxY = y;
			}
		}

		foregroundGrid = new ForegroundObject[maxX + 1, maxY + 1];

		for (int i = 0; i < fgObjects.Length; i++) {
			int x = Mathf.RoundToInt(fgObjects[i].transform.position.x);
			int y = Mathf.RoundToInt(fgObjects[i].transform.position.y);

			if (foregroundGrid[x, y] != null) {
				Debug.LogError("Two foregroundObject objects were found at (" + x + ", " + y + "). Please delete one.");
			} else {
				foregroundGrid[x, y] = fgObjects[i];
			}
		}
	}

}
