using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoginUI : UserAccountUI{

	public virtual void Init()
	{
		this.SetActive (true);

		StartCoroutine( UserAccountManager.AutoLogin());
	}

	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
			transform.GetChild (i).gameObject.SetActive (value);
		}
	}
}
