using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour {
    abstract public bool isSteppable();
    abstract public void OnStep();
    abstract public void OnLeave();
}
