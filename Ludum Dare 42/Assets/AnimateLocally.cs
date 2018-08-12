using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLocally : MonoBehaviour {
    Vector2 pos;
    // Use this for initialization
    private void Awake()
    {
        pos = this.gameObject.transform.position;
    }
    void Start () {
       
        this.gameObject.transform.position = pos;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        
    }
}
