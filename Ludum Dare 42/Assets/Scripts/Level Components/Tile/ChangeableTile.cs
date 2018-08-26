using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeableTile : Tile {

    public int stepsRemaining;

    public List<Sprite> whiteTile;
    public List<Sprite> orangeTile;
    public List<Sprite> redTile;
	
	private int preEventStepsRemaining;

	[SerializeField]
	private Animator animator;
	//[SerializeField]
	//private AnimationClip fallingAnim;

	
	protected override void Awake() {
		sr = GetComponent<SpriteRenderer>();
	}

	protected override void Start() {
		spriteUpdate();
		Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
		LevelManager.getFloor().updateTile(truePos, this);

		// Subscribe to the player step start event
		Player player = GM.Instance.currentLevelManager.player;
		UnityEvent PlayerSSS = player.StartSuccessfulStep;
		PlayerSSS.AddListener(beforePlayerStep);
	}

	private void beforePlayerStep() {
		preEventStepsRemaining = stepsRemaining;
	}

	public override bool isSteppable() {
		if (stepsRemaining == 0) {
			return false;
		}
		return true;
	}

    public override bool isSteppableForNPC() {
        return true;
    }

    public override void OnStep() {
        stepsRemaining--;
		spriteUpdate();
    }

    public override void OnLeave() {
		if (preEventStepsRemaining == 0) {
			Vector2Int truePos = Floor.pos3dToVect2Int(this.transform.position);
			createHoleAt(truePos);
			playTileFallAnim();
		}
    }

	private void createHoleAt(Vector2Int truePos) {
		Hole hole = Instantiate(PrefabManager.Instance.hole);
		hole.transform.position = Floor.Vect2IntToPos3d(truePos);

		LevelManager.getFloor().updateTile(truePos, hole);
	}

	public override bool CanBePushedOnto() {
		return true;
	}

	private void spriteUpdate() {
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

	private void playTileFallAnim() {
		sr.enabled = false;
		animator.Play("Crumbling World" + GM.Instance.currentLevelManager.worldIndex);
	}

}
