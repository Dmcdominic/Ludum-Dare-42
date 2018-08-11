using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene { Init, MainMenu, Game };
public enum GameState { Playing, Paused, Inactive };

public class GM : MonoBehaviour {

	[HideInInspector]
	public Scene currentScene;
	[HideInInspector]
	public GameState gameState;
	[HideInInspector]
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
			return;
		} else {
			_instance = this;
		}

		if (transform.parent == null) {
			DontDestroyOnLoad(this);
		}

		int sceneIndex = SceneManager.GetActiveScene().buildIndex;
		switch(sceneIndex) {
			case 0:
				currentScene = Scene.Init;
				break;
			case 1:
				currentScene = Scene.MainMenu;
				break;
			case 2:
				currentScene = Scene.Game;
				break;
		}
	}

	private void Start() {
		if (currentScene == Scene.Init) {
			changeScene(Scene.MainMenu);
		}
	}

	// Project management utility
	public static void changeScene(Scene scene) {
		Instance.currentScene = scene;
		switch (scene) {
			case (Scene.Init):
				SceneManager.LoadScene(0);
				break;
			case (Scene.MainMenu):
				SceneManager.LoadScene(1);
				break;
		}
	}
}
