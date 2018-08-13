using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType { Init, MainMenu, Other };
public enum GameState { Playing, Paused, Inactive, Transitioning };

public class GM : MonoBehaviour {

	// Settings
	private static readonly int levelScenesIndexOffset = 2;

	// Properties
	public List<int> worldLevelTotals;

	// References
	[HideInInspector]
	public SceneType currentScene;

	[HideInInspector]
	private GameState gameState;

	[HideInInspector]
	public LevelManager currentLevelManager;

	[HideInInspector]
	public IngameCanvas ingameCanvas;


	// Singleton management
	private static GM _instance;
    public static GM Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<GM>();
				if (_instance == null) {
					Debug.Log("No GM found in the scene");
				}
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

		SceneManager.sceneLoaded += OnSceneLoaded;

		if (ingameCanvas == null) {
			ingameCanvas = GameObject.FindObjectOfType<IngameCanvas>();
		}
	}

	private void Start() {
		switch (SceneManager.GetActiveScene().name) {
			case "Init":
				currentScene = SceneType.Init;
				setGamestate(GameState.Inactive);
				break;
			case "MainMenu":
				currentScene = SceneType.MainMenu;
				setGamestate(GameState.Inactive);
				break;
			default:
				currentScene = SceneType.Other;
				setGamestate(GameState.Playing);
				break;
		}
	}

	// Project management utility
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if (mode != LoadSceneMode.Single) {
			return;
		}

		switch (scene.name) {
			case "Init":
				currentScene = SceneType.Init;
				setGamestate(GameState.Inactive);
				break;
			case "MainMenu":
				currentScene = SceneType.MainMenu;
				setGamestate(GameState.Inactive);
				break;
			default:
				currentScene = SceneType.Other;
				setGamestate(GameState.Playing);
				break;
		}
	}

	public static void changeScene(SceneType scene) {
		Instance.currentScene = scene;
		switch (scene) {
			case (SceneType.Init):
				Instance.currentScene = SceneType.Init;
				Instance.setGamestate(GameState.Transitioning);
				SceneManager.LoadScene(0);
				break;
			case (SceneType.MainMenu):
				Instance.currentScene = SceneType.MainMenu;
				Instance.setGamestate(GameState.Transitioning);
				SceneManager.LoadScene(1);
				break;
		}
	}

	public static void changeToLevelScene(int world, int level) {
		int nextSceneIndex = getLvlIndexFromWorld(world, level);
		
		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
			Instance.currentScene = SceneType.Other;
			Instance.setGamestate(GameState.Transitioning);
			SceneManager.LoadScene(nextSceneIndex);
		} else {
			Debug.LogError("Scene of build index: " + nextSceneIndex + " not found.");
		}
	}

	public static void resetCurrentLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// Called when you reach the exit and beat the level
	public static void onBeatLevel() {
		Debug.Log("YOU BEAT IT!");
		int currentWorld = Instance.currentLevelManager.worldIndex;
		int currentLevel = Instance.currentLevelManager.levelIndex;

		int absoluteLvlIndex = getLvlIndexFromWorld(currentWorld, currentLevel);
		SaveManager.Instance.saveLevelProgress(absoluteLvlIndex);

		if (currentLevel == Instance.worldLevelTotals[currentWorld] - 1) {
			// TODO - Conclude this world. Cutscene?
			// Temporary:
			changeToLevelScene(currentWorld + 1, 0);
		} else {
			// TODO - Loading screen?
			changeToLevelScene(currentWorld, currentLevel + 1);
		}
	}

	public static int getLvlIndexFromWorld(int world, int level) {
		int index = levelScenesIndexOffset;
		for (int i = 0; i < world; i++) {
			index += Instance.worldLevelTotals[i];
		}
		index += level;
		return index;
	}

	public static int getHighestLevel() {
		return SaveManager.saveData.levelProgress;
	}

	// Ingame canvas and UI management
	public void setGamestate(GameState newGameState) {
		if (ingameCanvas) {
			if (newGameState == GameState.Inactive) {
				ingameCanvas.gameObject.SetActive(false);
			} else {
				ingameCanvas.gameObject.SetActive(true);
			}
		}
		
		gameState = newGameState;
	}

	public GameState getGameState() {
		return gameState;
	}

	// Load scene with transition
	private void loadSceneWithTransition(int sceneIndex) {

		SceneManager.LoadScene(sceneIndex);
	}
}
