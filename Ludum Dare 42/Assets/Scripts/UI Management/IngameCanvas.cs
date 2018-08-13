using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameCanvas : MonoBehaviour {

	// Singleton management
	private static IngameCanvas _instance;
	public static IngameCanvas Instance { get { return _instance; } }

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
		GM.Instance.ingameCanvas = this;
	}

}
