using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ユーザーアカウントに関するデバッグ用UI
/// </summary>
public class UserAccountDebugUI : MonoBehaviour {
	static UserAccountDebugUI instance = null;
	static public UserAccountDebugUI Instance
	{
		get{ return instance; }
	}

	public InputField _ID;
	public InputField _PASS;
	public Button _InheritSettingButton;
	public Button _InheritingButton;

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
	public void OnInheritSettingButton()
	{
		StartCoroutine(UserAccountManager.InheritSetting(_ID.text,_PASS.text));
	}
	public void OnInheritting()
	{
		StartCoroutine (UserAccountManager.Inheriting (_ID.text, _PASS.text));
	}
	public void OnDeleteButton()
	{
		GenericUIManager.Instance.ShowYesNoDialog ("DELETE", "REALLY DELETE?", cGameManager.Instance.UserData.DeleteYesNo);
	}
}
