using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    private void Awake()
    {
        
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnInteraction()
    {
        this.gameObject.SetActive(false);
        //TODO: Add interact / fadeout animation if desired
    }
}
