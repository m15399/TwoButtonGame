using UnityEngine;
using System.Collections;

public static class Utils {

	public static T TryAddComponent<T>(Component o) where T : Component {
		T comp = o.GetComponent<T>();
		if(comp == null){
			comp = o.gameObject.AddComponent<T>();
		}
		return comp;
	}

}
