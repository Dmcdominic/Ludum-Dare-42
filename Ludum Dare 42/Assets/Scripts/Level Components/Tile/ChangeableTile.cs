using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableTile : Tile {

    public int stepsRemaining;
    public List<Sprite> whiteTile;
    public List<Sprite> orangeTile;
    public List<Sprite> redTile;
    public Sprite hole;
	public List<Sprite> holeShaded;
    private bool trueHoleStatus = false;

	//[SerializeField]
	//private Animator animator;
	//[SerializeField]
	//private AnimationClip fallingAnim;


    private new void Awake() {
		base.Awake();
		if (stepsRemaining == 0) {
            Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
            trueHoleStatus = true;
        }
    }

	private new void Start() {
		checkForSpriteUpdate();
		Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
		LevelManager.getFloor().updateTile(truePos, this);
	}

	public override bool isSteppable()
	{
		if (stepsRemaining == 0 || trueHoleStatus)
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
            sr.sprite = orangeTile[GM.Instance.currentLevelManager.worldIndex];
        }
        else if(stepsRemaining == 0)
        {
            sr.sprite = redTile[GM.Instance.currentLevelManager.worldIndex];
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
			//playTileFallAnim();
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
		normalSpriteCheck();

		if (this.isHole()) {
			Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
			Vector2Int aboveTruePos = truePos + new Vector2Int(0, 1);
			this.checkForSpriteUpdate(LevelManager.getFloor().getTile(aboveTruePos));
		}
	}

	private void checkForSpriteUpdate(Tile aboveTile) {
		normalSpriteCheck();

		if (this.isHole()) {
			if (aboveTile != null && aboveTile.isHole()) {
				sr.sprite = hole;
			} else {
				sr.sprite = holeShaded[GM.Instance.currentLevelManager.worldIndex];
			}
		}
	}

	private void normalSpriteCheck() {
		switch (stepsRemaining) {
			case 2:
				sr.sprite = whiteTile[GM.Instance.currentLevelManager.worldIndex];
				break;
			case 1:
				sr.sprite = orangeTile[GM.Instance.currentLevelManager.worldIndex];
				break;
			case 0:
				sr.sprite = redTile[GM.Instance.currentLevelManager.worldIndex];
				break;
		}
	}

    public override bool isHole()
    {
        return trueHoleStatus;
    }

	private void playTileFallAnim() {
		//animator.Play("HoleFall");
		// Try to make the hole below dissapear
	}

}
