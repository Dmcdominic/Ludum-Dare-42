using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuHandler : MonoBehaviour {

	public GameObject MainMenuScreen;
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown)
        {
            this.gameObject.SetActive(false);
			MainMenuScreen.SetActive(true);
        }
	}
}
