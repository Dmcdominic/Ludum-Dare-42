using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlickeringLight : Tile {

    public int step;
    public Sprite full;
    public Sprite half;
    public Sprite off;

	// Use this for initialization
	private new void Start() {
		UnityEvent ue = GM.Instance.currentLevelManager.player.StartSuccessfulStep;
		ue.AddListener(incr);
	}

	private void incr() {
        step++;
        step %= 3;
        switch (step) {
            case 0:
                sr.sprite = full;
                break;
            case 1:
                sr.sprite = off;
                break;
            case 2:
                sr.sprite = half;
                break;
        }
    }

    public override bool isSteppable() {
        switch (step % 3) {
            case 0:
                return false;
            case 1:
                return false;
            case 2:
                return true;
        }
        return false;
    }

    public override bool isSteppableForNPC() {
        return true;
    }

    public override void OnStep() {
    }

    public override void OnLeave() {
    }

    public override bool CanBePushedOnto() {
        return true;
    }

    public override bool CanBeJumpedOver() {
        return true;
    }
    
}
