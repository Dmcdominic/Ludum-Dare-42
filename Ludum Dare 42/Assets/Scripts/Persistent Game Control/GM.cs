using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState { Playing, Paused, Inactive, Transitioning };

public class GM : MonoBehaviour {

	// Settings
	private static readonly int levelScenesIndexOffset = 3;

	// Properties
	public List<int> worldLevelTotals;

	// References
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

	// Project management utility
	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		if ((scene.name == "MainMenu" || scene.name == "Elevator") && PauseManager.Instance) {
			PauseManager.Instance.hideAllIngame();
		}

		if (mode != LoadSceneMode.Single || getGameState() == GameState.Transitioning) {
			return;
		}

		refreshGamestate();
	}

	public static void changeToMainMenu(bool withTransition = true) {
		Instance.setGamestate(GameState.Transitioning);
		if (withTransition) {
			loadSceneWithTransition(1);
		} else {
			SceneManager.LoadScene(1);
		}
	}

	public void refreshGamestate() {
		switch (SceneManager.GetActiveScene().name) {
			case "Init":
				setGamestate(GameState.Inactive);
				loadSceneWithTransition(1);
				break;
			case "MainMenu":
				setGamestate(GameState.Inactive);
				break;
			case "Elevator":
				setGamestate(GameState.Transitioning);
				break;
			default:
				setGamestate(GameState.Playing);
				break;
		}
	}

	public static void changeToLevelSceneDirect(int world, int level) {
		int nextSceneIndex = getLvlIndexFromWorld(world, level);

		if (nextSceneIndex < SceneManager.sceneCountInBuildSettings) {
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

		if (isFinalLvlInWorld(currentWorld, currentLevel)) {
			ElevatorTransition.nextWorldIndex = currentWorld + 1;
			ElevatorTransition.nextLevelIndex = 0;
			loadSceneWithTransition(2);
		} else {
			changeToLevelSceneDirect(currentWorld, currentLevel + 1);
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
	// Returns -1 for Init scene.
	// Returns -2 for Main Menu scene.
	// Returns -3 for elevator transition.
	// Returns -4 for the end game screen.
	// Returns -5 otherwise.
	public static int getWorldFromSceneIndex(int index) {
		// init or main menu scenes
		if (index == 0) {
			return -1;
		} else if (index == 1) {
			return -2;
		} else if (index == 2) {
			return -3;
		}

		int counter = levelScenesIndexOffset;

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
			return -4;
		}

		// Other
		return -5;
	}

	public static bool isFinalLvlInWorld(int worldIndex, int levelIndex) {
		return levelIndex == Instance.worldLevelTotals[worldIndex] - 1;
	}

	// Get the highest level reached according t
	public static int getHighestLevel() {
		return SaveManager.saveData.levelProgress;
	}

	// Ingame canvas and UI management
	public void setGamestate(GameState newGameState) {
		gameState = newGameState;

		if (newGameState == GameState.Transitioning) {
			return;
		}

		if (ingameCanvas) {
			if (newGameState == GameState.Inactive) {
				ingameCanvas.gameObject.SetActive(false);
			} else {
				ingameCanvas.gameObject.SetActive(true);
			}
		}
	}

	public GameState getGameState() {
		return gameState;
	}

	// Load scene with transition
	private static void loadSceneWithTransition(int sceneIndex) {
		FadeOverlayManager.Instance.fadeToBlack(sceneIndex);
	}

}
