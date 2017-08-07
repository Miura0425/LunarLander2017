using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountMessageUI : DialogUI {

	[SerializeField]
	private Text _Message;

	public delegate void OKFunc ();
	OKFunc _fcOK = null;

	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (value);
		}
	}

	public void SetTitleAndMessage(string title,string msg,OKFunc func = null)
	{
		SetTitle (title);
		SetMessage (msg);
		_fcOK = func;
	}
	public void SetMessage(string msg)
	{
		_Message.text = msg;
	}

	public void OnOKButton()
	{
		SetActive (false);
		if (_fcOK != null) {
			_fcOK ();
		}

	}
}
