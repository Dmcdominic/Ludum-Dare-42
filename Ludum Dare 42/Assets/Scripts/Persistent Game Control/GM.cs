using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene { Init, MainMenu, Other };
public enum GameState { Playing, Paused, Inactive };

public class GM : MonoBehaviour {

	// Settings
	private static readonly int levelScenesIndexOffset = 2;

	// Properties
	public List<int> worldLevelTotals;

	// References
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

		//int sceneIndex = SceneManager.GetActiveScene().buildIndex;
		string sceneName = SceneManager.GetActiveScene().name;
		switch(sceneName) {
			case "Init":
				currentScene = Scene.Init;
				gameState = GameState.Inactive;
				break;
			case "MainMenu":
				currentScene = Scene.MainMenu;
				gameState = GameState.Inactive;
				break;
			default:
				currentScene = Scene.Other;
				gameState = GameState.Playing;
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
				Instance.currentScene = Scene.Init;
				Instance.gameState = GameState.Inactive;
				SceneManager.LoadScene(0);
				break;
			case (Scene.MainMenu):
				Instance.currentScene = Scene.MainMenu;
				Instance.gameState = GameState.Inactive;
				SceneManager.LoadScene(1);
				break;
		}
	}

	public static void changeToLevelScene(int world, int level) {
		int nextSceneIndex = levelScenesIndexOffset;
		for (int i = 0; i < world; i++) {
			nextSceneIndex += Instance.worldLevelTotals[i];
		}
		nextSceneIndex += level;
		
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
			Instance.currentScene = Scene.Other;
			Instance.gameState = GameState.Playing;
			SceneManager.LoadScene(level + levelScenesIndexOffset);
		} else {
			Debug.LogError("Scene of build index: " + nextSceneIndex + " not found.");
		}
	}

	// Called when you reach the exit and beat the level
	public static void onBeatLevel() {
		Debug.Log("YOU BEAT IT!");
		// TODO - figure out the next level in the world, or conclude the world if it was the last one.
		int currentWorld = Instance.currentLevelManager.worldIndex;
		int currentLevel = Instance.currentLevelManager.levelIndex;
		if (currentLevel == Instance.worldLevelTotals[currentWorld] - 1) {
			// TODO - Conclude this world. Cutscene?
			// Temporary:
			changeToLevelScene(currentWorld + 1, 0);
		} else {
			// TODO - Loading screen?
			changeToLevelScene(currentWorld, currentLevel + 1);
		}
	}
}
