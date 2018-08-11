using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {

    void OnInteraction()
    {
        this.gameObject.SetActive(false);
        //TODO: Add interact / fadeout animation if desired
    }
}
