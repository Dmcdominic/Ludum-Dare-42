using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetKeycardColor : MonoBehaviour {
    private Keycard kc;
    private KeycardColor kcc;
    private ParticleSystem ps;
    ParticleSystem.MainModule ma;
    private Color red = new Color(1f, 0f, 0f, 1f);
    private Color blue = new Color(0f, 0f, 1f, 1f);
    private Color green = new Color(0f, 1f, 0f, 1f);
    private Color yellow = new Color(1f, 1f, 0f, 1f);

    private void Awake()
    {
        ps = this.gameObject.GetComponent<ParticleSystem>();
        ma = ps.main;
        kc = this.gameObject.GetComponentInParent<Keycard>();
        kcc = kc.color;
        switch (kcc)
        {
            case (KeycardColor.Red):
                ma.startColor = red;
                break;
            case (KeycardColor.Blue):
                ma.startColor = blue;
                break;
            case (KeycardColor.Green):
                ma.startColor = green;
                break;
            case (KeycardColor.Yellow):
                ma.startColor = yellow;
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
