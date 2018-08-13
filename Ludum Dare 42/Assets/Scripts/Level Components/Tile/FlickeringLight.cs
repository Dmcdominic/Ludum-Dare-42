using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlickeringLight : Tile {
    public int step;
    static Player player;
    UnityEvent ue;
    private SpriteRenderer spr;
    public Sprite full;
    public Sprite half;
    public Sprite off;

    void incr()
    {
        step++;
        step %= 3;
        switch (step)
        {
            case 0:
                spr.sprite = full;
                break;
            case 1:
                spr.sprite = off;
                break;
            case 2:
                spr.sprite = half;
                break;
        }
    }
    public override bool isSteppable()
    {
        switch (step%3)
        {
            case 0:
                return false;
            case 1:
                return false;
            case 2:
                return true;
        }
        return false;
    }
    public override bool isSteppableForNPC()
    {
        return true;
    }
    public override void OnStep()
    {
        
    }
    public override void OnLeave()
    {
        
    }
    public override bool CanBePushedOnto()
    {
        return false;
    }

    public override bool CanBeJumpedOver()
    {
        return true;
    }
    // Use this for initialization
    void Start () {
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        player = GM.Instance.currentLevelManager.player;
        if (player)
        {
            Debug.Log("Player found!");
        }
        ue = GM.Instance.currentLevelManager.player.OnSuccessfulStep;
        ue.AddListener(incr);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
