using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableTile : Tile {

    public int stepsRemaining;
    public Sprite whiteTile;
    public Sprite orangeTile;
    public Sprite redTile;
    public Sprite hole;
	public Sprite holeShaded;
    private SpriteRenderer sr;
    private bool trueHoleStatus = false;
   
    private void Awake()
    {
        if (stepsRemaining == 0)
        {
            sr = this.gameObject.GetComponent<SpriteRenderer>();
            sr.sprite = hole;
            checkForSpriteUpdate();
            Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
            trueHoleStatus = true;
            Debug.Log(trueHoleStatus);
            LevelManager.getFloor().updateTile(truePos, this);
        }
        sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

	private void Start() {
		checkForSpriteUpdate();
    }

	public override bool isSteppable()
	{
		if (stepsRemaining == 0)
		{
			return false;
		}
		return true;
	}

    public override bool isSteppableForNPC()
    {
        return true;
    }

    public override void OnStep()
    {
        stepsRemaining--;
        if(stepsRemaining == 1)
        {
            sr.sprite = orangeTile;
        }
        else if(stepsRemaining == 0)
        {
            sr.sprite = redTile;
        }
    }

    public override void OnLeave()
    {
        if(stepsRemaining == 0)
        {
			trueHoleStatus = true;
			sr.sprite = hole;
			Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
			checkForSpriteUpdate();
			LevelManager.getFloor().updateTile(truePos, this);
		}
    }

	public override bool CanBePushedOnto() {
		return true;
	}

	// Methods to keep the hole sprites correct between shaded/unshaded
	public override void onAboveTileUpdated(Tile aboveTile) {
		checkForSpriteUpdate(aboveTile);
	}

	private void checkForSpriteUpdate() {
		if (this.isHole()) {
			Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
			Vector2Int aboveTruePos = truePos + new Vector2Int(0, 1);
			this.checkForSpriteUpdate(LevelManager.getFloor().getTile(aboveTruePos));
		}
	}

	private void checkForSpriteUpdate(Tile aboveTile) {
		if (this.isHole()) {
			if (aboveTile != null && aboveTile.isHole()) {
				sr.sprite = hole;
			} else {
				sr.sprite = holeShaded;
			}
		}
	}
    public override bool isHole()
    {
        return trueHoleStatus;
    }

}
