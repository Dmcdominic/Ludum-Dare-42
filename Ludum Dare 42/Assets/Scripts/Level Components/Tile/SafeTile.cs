using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeTile : Tile {

	public override bool isSteppable()
	{
		return true;
	}
    public override bool isSteppableForNPC()
    {
        return true;
    }

    public override void OnStep()
    {
		// Do nothing
    }

    public override void OnLeave()
    {
		// Do nothing
    }

	public override bool CanBePushedOnto() {
		return true;
	}

	public void hideForStepDuration() {
		sr.enabled = false;
		StartCoroutine(revealDelayed());
	}

	IEnumerator revealDelayed() {
		yield return new WaitForSeconds(Player.moveAnimTime);
		yield return new WaitForEndOfFrame();
		sr.enabled = true;
	}

}
