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

	public Image _BackGround =null;
	public InputField _ID = null;
	public InputField _PASS =null;
	public Button _InheritSettingButton=null;
	public Button _InheritingButton=null;
	public Button _DeleteButton=null;

	bool isOpen = false;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}
	void Update()
	{
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D)) {
			isOpen = !isOpen;
			SetActiveUI (isOpen);
		}
	}
	void SetActiveUI(bool value)
	{
		_BackGround.enabled = value;
		_ID.gameObject.SetActive (value);
		_PASS.gameObject.SetActive(value);
		_InheritSettingButton.gameObject.SetActive (value);
		_InheritingButton.gameObject.SetActive (value);
		_DeleteButton.gameObject.SetActive (value);
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
