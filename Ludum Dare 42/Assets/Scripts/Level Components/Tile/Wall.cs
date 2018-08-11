using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Tile {

	override protected bool isSteppable()
	{
		return false;
	}

	override protected void OnStep()
    {

    }

    override protected void OnLeave()
    {

    }
}
