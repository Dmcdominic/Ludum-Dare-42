using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : ForegroundObject {

	public override bool IsSteppable(Vector2Int incomingPlayerDisplacement) {
		// TODO - Check if this can be pushed in the given direction
		return false;
	}

    public override void OnInteraction(Vector2Int incomingPlayerDisplacement)
    {
        //TODO: Move the pushable object (w/ animation??) in the desired direction
		//TODO: Update the Floor array accordingly
    }
}
