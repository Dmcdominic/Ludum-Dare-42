using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableTile : Tile {

    public int stepsRemaining;
    public Sprite whiteTile;
    public Sprite orangeTile;
    public Sprite redTile;
    public Sprite hole;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
    }

	public override bool isSteppable()
	{
		if (stepsRemaining == 0)
		{
			return false;
		}
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
            sr.sprite = hole;
        }
    }

	public override bool CanBePushedOnto() {
		return true;
	}

}
