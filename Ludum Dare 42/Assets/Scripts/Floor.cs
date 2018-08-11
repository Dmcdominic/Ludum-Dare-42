using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor {

	// TODO - sub those other classes for GameObject here
	public GameObject[,] tileGrid;
	public GameObject[,] foregroundGrid;


	public void populateTileGrid(GameObject parent) {
		// TODO - sub here
		GameObject[] tiles = parent.GetComponentsInChildren<GameObject>();

		int maxX = 0;
		int maxY = 0;

		// TODO - sub here
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

		tileGrid = new GameObject[maxX + 1, maxY + 1];

		for (int i = 0; i < tiles.Length; i++) {
			int x = Mathf.RoundToInt(tiles[i].transform.position.x);
			int y = Mathf.RoundToInt(tiles[i].transform.position.y);

			tileGrid[x,y] = tiles[i];
		}
	}


	public void populateForegroundGrid(GameObject parent) {
		// TODO - sub here
		GameObject[] fgObjects = parent.GetComponentsInChildren<GameObject>();

		int maxX = 0;
		int maxY = 0;

		// TODO - sub here
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

		foregroundGrid = new GameObject[maxX + 1, maxY + 1];

		for (int i = 0; i < fgObjects.Length; i++) {
			int x = Mathf.RoundToInt(fgObjects[i].transform.position.x);
			int y = Mathf.RoundToInt(fgObjects[i].transform.position.y);

			foregroundGrid[x, y] = fgObjects[i];
		}
	}

}
