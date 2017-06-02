using UnityEngine;
using System.Collections;

public class GameInit  {
	[RuntimeInitializeOnLoadMethod]
	static void OnRuntimeMethodLoad()
	{
		Screen.SetResolution (960, 540, false, 60);
	}

}
