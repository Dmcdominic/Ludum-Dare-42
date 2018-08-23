using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IngameCanvas : MonoBehaviour {

	public GameObject HUD;

	// Keycards
	public GameObject redKeycard;
	public Text redCounter;

	public GameObject greenKeycard;
	public Text greenCounter;

	public GameObject blueKeycard;
	public Text blueCounter;

	public GameObject yellowKeycard;
	public Text yellowCounter;

	// Powerups
	public GameObject coffee;

	// Level text
	public Text lvlText;

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
		this.hideAllKeycards();
		this.gameObject.SetActive(false);
	}

	// TEMPORARY - DO NOT ENABLE INGAME CANVAS FOR FINAL SCENE: "YOU'RE HIRED!"
	private void OnEnable() {
		// TODO - Remove this
		if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
			this.gameObject.SetActive(false);
		}
	}

	public void resetAll() {
		hideAllKeycards();
		resetPowerupDisplays();
		HUD.SetActive(true);
	}

	// Keycard displays
	public void initKeycard(KeycardColor color) {
		updateKeycard(color, 0);
	}

	public void updateKeycard(KeycardColor color, int newCount) {
		int total = LevelManager.getFloor().keycardTotals[color];

		switch (color) {
			case KeycardColor.Red:
				redKeycard.SetActive(true);
				redCounter.text = newCount + "/" + total;
				break;
			case KeycardColor.Green:
				greenKeycard.SetActive(true);
				greenCounter.text = newCount + "/" + total;
				break;
			case KeycardColor.Blue:
				blueKeycard.SetActive(true);
				blueCounter.text = newCount + "/" + total;
				break;
			case KeycardColor.Yellow:
				yellowKeycard.SetActive(true);
				yellowCounter.text = newCount + "/" + total;
				break;
		}
	}

	public void hideAllKeycards() {
		redKeycard.SetActive(false);
		greenKeycard.SetActive(false);
		blueKeycard.SetActive(false);
		yellowKeycard.SetActive(false);
	}

	// Powerup displays
	public void displayPowerup(PowerupType type) {
		switch (type) {
			case PowerupType.Coffee:
				coffee.SetActive(true);
				break;
		}
	}

	public void resetPowerupDisplays() {
		coffee.SetActive(false);
	}

	// Level text display
	public void updateLvlText(int world, int level) {
		int wOneIndexed = world + 1;
		int lOneIndexed = level + 1;
		lvlText.text = "Level " + wOneIndexed + "-" + lOneIndexed;
	}

}
