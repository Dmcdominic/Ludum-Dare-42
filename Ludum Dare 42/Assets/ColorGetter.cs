using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGetter : MonoBehaviour {
    private KeycardColor kc;
    private LockedDoor ld;
    private SpriteRenderer spr;
    
    private void Awake()
    {
        ld = this.gameObject.GetComponent<LockedDoor>();
        kc = ld.keycardsRequired[0];
        spr = this.gameObject.GetComponent<SpriteRenderer>();
        
        switch (kc)
        {
            case KeycardColor.Red:
                spr.color = new Color(1f, 0f, 0f, 1f);
                break;
            case KeycardColor.Blue:
                spr.color = new Color(0f, 0f, 1f, 1f);
                break;
            case KeycardColor.Green:
                spr.color = new Color(0f, 1f, 0f, 1f);
                break;
            case KeycardColor.Yellow:
                spr.color = new Color(1f, 1f, 0f, 1f);
                break;
        }
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
