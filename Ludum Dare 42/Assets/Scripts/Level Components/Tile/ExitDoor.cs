using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Tile {

	public override bool isSteppable()
	{
		return true;
	}

	public override void OnStep()
    {
        //Change scene to next level
    }

    public override void OnLeave()
	{
		// Does not apply
	}
}
