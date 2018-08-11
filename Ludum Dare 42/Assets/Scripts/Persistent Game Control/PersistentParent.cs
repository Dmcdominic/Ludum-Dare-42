using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentParent : MonoBehaviour {

	private static PersistentParent _instance;
	public static PersistentParent Instance {
		get {
			if (_instance == null) {
				_instance = new PersistentParent();
			}
			return _instance;
		}
	}

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			_instance = this;
		}
		DontDestroyOnLoad(this);
	}

}
