using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Tile {

	public override bool isSteppable()
	{
		return false;
	}
    public override bool isSteppableForNPC()
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

	public override bool CanBePushedOnto() {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return false;
	}
}
