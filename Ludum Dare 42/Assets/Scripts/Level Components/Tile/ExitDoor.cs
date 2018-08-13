﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : Tile {

	public override bool isSteppable() {
		return true;
	}

    public override bool isSteppableForNPC()
    {
        return false;
    }

    public override void OnStep() {
		GM.onBeatLevel();
	}

	public override void OnLeave() {
		// Does not apply
	}

	public override bool CanBePushedOnto() {
		return false;
	}

	public override bool CanBeJumpedOver() {
		return false;
	}
}