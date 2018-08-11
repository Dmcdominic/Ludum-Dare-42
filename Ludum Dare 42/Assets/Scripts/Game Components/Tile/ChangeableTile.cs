using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeableTile : Tile {
    bool isSteppable = true;
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
    // Use this for initialization
    void Start () {
		
	}

    override protected void OnStep()
    {
        stepsRemaining--;
        if(stepsRemaining == 1)
        {
            sr.sprite = orangeTile;
        }
        if(stepsRemaining == 0)
        {
            sr.sprite = redTile;
        }

    }
    override protected void OnLeave()
    {
        if(stepsRemaining == 0)
        {
            sr.sprite = hole;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
