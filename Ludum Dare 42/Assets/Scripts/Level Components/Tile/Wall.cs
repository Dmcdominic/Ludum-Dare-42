using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Tile {

	public override bool isSteppable()
	{
		return false;
	}

	public override void OnStep()
    {
		// Does not apply
    }

    public override void OnLeave()
    {
		// Does not apply
    }
}
