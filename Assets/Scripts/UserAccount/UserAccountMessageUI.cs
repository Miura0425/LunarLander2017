using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountMessageUI : UserAccountUI {

	[SerializeField]
	private Text _Message;

	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
			if(transform.GetChild(i).name != _WaitImg.name || !value)
				transform.GetChild (i).gameObject.SetActive (value);
		}
	}

	public void SetTitleAndMessage(string title,string msg)
	{
		SetTitle (title);
		SetMessage (msg);
	}
	public void SetMessage(string msg)
	{
		_Message.text = msg;
	}
}
