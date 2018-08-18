using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimUtil {

	public static string getDirectionPostfix(Vector2Int direction) {
		if (direction.y > 0) {
			return " Up";
		} else if (direction.x > 0) {
			return " Right";
		} else if (direction.y < 0) {
			return " Down";
		} else if (direction.x < 0) {
			return " Left";
		}

		Debug.LogError("No postfix string found for direction: " + direction);
		return null;
	}

}
