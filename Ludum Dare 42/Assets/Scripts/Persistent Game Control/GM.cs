using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

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
	public LevelManager _currentLevelManager;
	public LevelManager currentLevelManager {
		get {
			if (!_currentLevelManager) {
				_currentLevelManager = GameObject.FindObjectOfType<LevelManager>();
			}
			return _currentLevelManager;
		}
	}

	[HideInInspector]
	public IngameCanvas ingameCanvas;

	public MusicManager mm;

	// Singleton management
	private static GM _instance;
	public static GM Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<GM>();
				if (_instance == null) {
					Debug.LogError("No GM found in the scene");
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
		refreshGamestate();
	}

	public void refreshGamestate() {
		switch (SceneManager.GetActiveScene().name) {
			case "Init":
				currentScene = SceneType.Init;
				setGamestate(GameState.Inactive);
				loadSceneWithTransition(1);
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
		if (scene.name == "MainMenu" && PauseManager.Instance) {
			PauseManager.Instance.hideAllIngame();
		}

		if (mode != LoadSceneMode.Single || getGameState() == GameState.Transitioning) {
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
			loadSceneWithTransition(nextSceneIndex);
		} else {
			Debug.LogError("Scene of build index: " + nextSceneIndex + " not found.");
		}
	}

	public static void resetCurrentLevel() {
		Instance.setGamestate(GameState.Transitioning);
		loadSceneWithTransition(SceneManager.GetActiveScene().buildIndex);
	}

	// Called when you reach the exit and beat the level
	public static void onBeatLevel() {
		//TODO - save level progress
		int currentWorld = Instance.currentLevelManager.worldIndex;
		int currentLevel = Instance.currentLevelManager.levelIndex;

		//int absoluteLvlIndex = getLvlIndexFromWorld(currentWorld, currentLevel);
		//SaveManager.Instance.saveLevelProgress(absoluteLvlIndex);

		if (currentLevel == Instance.worldLevelTotals[currentWorld] - 1) {
			// TODO - special loading screen between worlds?
			changeToLevelScene(currentWorld + 1, 0);
		} else {
			changeToLevelScene(currentWorld, currentLevel + 1);
		}
	}

	// Util functions for determining between lvl index, world, and scene index
	public static int getLvlIndexFromWorld(int world, int level) {
		int index = levelScenesIndexOffset;
		for (int i = 0; i < world; i++) {
			index += Instance.worldLevelTotals[i];
		}
		index += level;
		return index;
	}

	// Returns the world index of a certain scene if it is a level scene.
	// Returns -1 for Init and Main Menu scene.
	// Returns -2 for the end game screen.
	// Returns -3 otherwise.
	public static int getWorldFromScene(Scene scene) {
		int index = scene.buildIndex;

		// init or main menu scenes
		int counter = levelScenesIndexOffset;
		if (index < counter) {
			return -1;
		}

		// Level scenes
		for (int i = 0; i < Instance.worldLevelTotals.Count; i++) {
			counter += Instance.worldLevelTotals[i];
			if (index < counter) {
				return i;
			}
		}

		// End game scene - "You're Hired!"
		counter += 1;
		if (index < counter) {
			return -2;
		}

		// Other
		return -3;
	}

	// Get the highest level reached according t
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
	private static void loadSceneWithTransition(int sceneIndex) {
		FadeOverlayManager.Instance.fadeToBlack(sceneIndex);
		//SceneManager.LoadScene(sceneIndex);
	}

}
