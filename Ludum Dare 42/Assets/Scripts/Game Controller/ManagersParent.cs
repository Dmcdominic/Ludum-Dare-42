using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersParent : MonoBehaviour {

	private static ManagersParent _instance;
	public static ManagersParent Instance {
		get {
			if (_instance == null) {
				_instance = new ManagersParent();
			}
			return _instance;
		}
	}

	private void Awake() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
		DontDestroyOnLoad(this);
	}

}
