using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountYesNoUI : UserAccountUI {

	[SerializeField]
	private Text _Message;

	private bool IsYes = false;

	public delegate void YesNoFunc (bool yes);
	YesNoFunc _fcYesNo = null;

	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (value);
		}
	}

	public void SetTitleAndMessage(string title,string msg,YesNoFunc func = null)
	{
		SetTitle (title);
		SetMessage (msg);
		_fcYesNo = func;
	}
	public void SetMessage(string msg)
	{
		_Message.text = msg;
	}

	public void OnYesNoButton(bool yes)
	{
		IsYes = yes;
		SetActive (false);
		if (_fcYesNo != null) {
			_fcYesNo (IsYes);
		}

	}
}
