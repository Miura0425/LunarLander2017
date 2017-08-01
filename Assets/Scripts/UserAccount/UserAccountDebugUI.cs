using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountDebugUI : MonoBehaviour {
	static UserAccountDebugUI instance = null;
	static public UserAccountDebugUI Instance
	{
		get{ return instance; }
	}

	public InputField _ID;
	public InputField _PASS;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}
	/*---------------------------------------------------------------------*/
	public void SetID(string id){
		_ID.text = id;
	}

	public void SetPASS(string pass)
	{
		_PASS.text = pass;
	}
	/*---------------------------------------------------------------------*/
}
