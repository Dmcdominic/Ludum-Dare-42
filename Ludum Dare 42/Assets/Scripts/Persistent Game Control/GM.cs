using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene { Init, MainMenu };
public enum GameState { Playing, Paused, Inactive };

public class GM : MonoBehaviour {

	public GameState gameState;
	public LevelManager currentLevelManager;

	// Singleton management
	private static GM _instance;
    public static GM Instance {
        get {
            if (_instance == null) {
                _instance = new GM();
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

		if (transform.parent == null) {
			DontDestroyOnLoad(this);
		}
	}
	
	public static void changeScene(Scene scene) {
		switch(scene) {
			case (Scene.Init):

				SceneManager.LoadScene(0);
				break;
			case (Scene.MainMenu):
				SceneManager.LoadScene(1);
				break;
		}
	}
}
