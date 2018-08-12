using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {

    public abstract bool isSteppable();
    public abstract void OnStep();
    public abstract void OnLeave();

	public virtual bool CanBePushedOnto() {
		return false;
	}

	public virtual bool CanBeJumpedOver() {
		return true;
	}

	public bool isHole() {
		return (this is ChangeableTile && ((ChangeableTile)this).stepsRemaining == 0);
	}

}
