using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTile : Tile {
    int stepsRemaining = 100;
    bool isSteppable = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    override protected void OnStep()
    {

    }
    override protected void OnLeave()
    {

    }
}
