using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour {

	// Prefabs go here
	public SafeTile safeTile;

	public Color redHex;
	public Color greenHex;
	public Color blueHex;
	public Color yellowHex;

	// Singleton instance setup
	private static PrefabManager _instance;
	public static PrefabManager Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad(this);
		}
	}

	public Color getColorHex(KeycardColor color) {
		switch (color) {
			case KeycardColor.Red:
				return redHex;
			case KeycardColor.Green:
				return greenHex;
			case KeycardColor.Blue:
				return blueHex;
			case KeycardColor.Yellow:
				return yellowHex;
		}
		return Color.black;
	}
}
