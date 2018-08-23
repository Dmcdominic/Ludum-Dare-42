using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	// Settings
	public static Color selectableHighlight;

	// Editor fields
	public bool overrideDefaultHighlights = false;

    public GameObject defaultSelect;
    public GameObject defaultUpSelect;

    public Button backButton;

	// Properties
    public static float backTimer;

    private bool buttonSelected;
    private GameObject prevSelected;

    private bool delayed;
    private int lastDirection;
    private float delayTimer;
    private float holdTimer;

    private float slowDelay = 0.65f;
    private float fastDelay = 0.14f;
    private float currentDelay;

    private EventSystem eventSystem;


	private void Start() {
		// Set all UI selectables to have the desired highlight color
		if (!overrideDefaultHighlights) {
			// Set the color here, if provided by some static instance object
			selectableHighlight = PrefabManager.Instance.getColorHex(KeycardColor.Blue);
			Selectable[] selectables = GetComponentsInChildren<Selectable>();
			foreach (Selectable selectable in selectables) {
				ColorBlock colorBlock = selectable.colors;
				colorBlock.highlightedColor = selectableHighlight;
				selectable.colors = colorBlock;
			}
		}
	}

	void OnEnable() {
        resetDelay();
    }

    void Update(){
        if (eventSystem == null) {
            eventSystem = EventSystem.current;
        }

        // Press 'b' on joystick or backspace on keyboard to go back
        backTimer += Time.deltaTime;
        if (Input.GetAxisRaw("Cancel") > 0 && backButton != null && (backTimer >= 0.5f || PauseManager.paused)) {
            backButton.onClick.Invoke();
            backTimer = 0;
        }

        // Default select the top or bottom of the menu
        if (eventSystem.currentSelectedGameObject == null) {
            buttonSelected = false;
        }

        float vertical = Input.GetAxisRaw("Vertical");
		if ((vertical == -1 || vertical == 1) && !buttonSelected) {
			if (vertical == -1) {
                eventSystem.SetSelectedGameObject(defaultSelect);
            }
            else if (vertical == 1) {
                eventSystem.SetSelectedGameObject(defaultUpSelect);
            }

            buttonSelected = true;
        }

        // Wrapping top-to-bottom and bottom-to-top
        if (prevSelected != eventSystem.currentSelectedGameObject) {
            delayed = true;
            delayTimer = 0;
        }
        if (vertical != 0) {
            lastDirection = (int)vertical;
        }

        if ((vertical == -1 || vertical == 1) && buttonSelected) {
            if (prevSelected != eventSystem.currentSelectedGameObject) {
                delayed = true;
                delayTimer = 0;
            }

            if ((int)vertical == lastDirection) {
                delayTimer += Time.deltaTime;
                holdTimer += Time.deltaTime;
            } else {
                resetDelay();
            }

            if (holdTimer >= slowDelay) {
                currentDelay = fastDelay;
            } else {
                currentDelay = slowDelay;
            }

            if (eventSystem.currentSelectedGameObject == defaultSelect && vertical == 1 && (!delayed || (delayTimer >= currentDelay))) {
                eventSystem.SetSelectedGameObject(defaultUpSelect);
                delayTimer = 0;
            }
            else if (eventSystem.currentSelectedGameObject == defaultUpSelect && vertical == -1 && (!delayed || (delayTimer >= currentDelay))) {
                eventSystem.SetSelectedGameObject(defaultSelect);
                delayTimer = 0;
            }

        } else {
            resetDelay();
        }

        prevSelected = eventSystem.currentSelectedGameObject;
    }

    private void resetDelay() {
        delayTimer = 0;
        holdTimer = 0;
        lastDirection = 2;
        delayed = false;
    }

    private void OnDisable() {
        buttonSelected = false;
    }

    // Functions for buttons to call
	public void NewGame() {
		SaveManager.Instance.resetProgress();
		LoadLevelWorld0(0);
	}

    public void LoadMainMenu() {
		PauseManager.Instance.resume();
		LevelManager.getPlayer().disableControl();
		GM.changeToMainMenu();
    }

	public void LoadLevelWorld0(int level) {
		GM.changeToLevelSceneDirect(0, level);
	}
	public void LoadLevelWorld1(int level) {
		GM.changeToLevelSceneDirect(1, level);
	}
	public void LoadLevelWorld2(int level) {
		GM.changeToLevelSceneDirect(2, level);
	}
	public void LoadLevelWorld3(int level) {
		GM.changeToLevelSceneDirect(3, level);
	}

	public void Quit() {
		#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
    }
}
