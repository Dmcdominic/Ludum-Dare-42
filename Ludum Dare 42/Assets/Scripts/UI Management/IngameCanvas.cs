using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameCanvas : MonoBehaviour {

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

	public void resetAll() {
		hideAllKeycards();
		resetPowerupDisplays();
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
}
