using UnityEngine;
using System.Collections;

public class TitleManger : MonoBehaviour {
	[SerializeField,TooltipAttribute("スタートするためのキー")]
	private KeyCode StartKey;

	// UIオブジェクト
	[SerializeField]
	private MenuManager _Menu; // メニュー
	[SerializeField]
	private SignUpUI _SignUpUI;
	[SerializeField]
	private PressMsg _PressMsg; // プレスメッセージ

	// はじめのキーが押されているか
	private bool isPress = false;

	/*---------------------------------------------------------------------*/
	void Start()
	{
		CheckStartUp ();
	}
	// 更新処理
	void Update () {
		CheckStartKey ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// スタートキーが押されたかチェックする。
	/// </summary>
	private void CheckStartKey()
	{
		if(isPress == false && Input.anyKeyDown)
		{
			isPress = true;
			_PressMsg.gameObject.SetActive (false);
			_Menu.Init ();
		}
	}
	/*---------------------------------------------------------------------*/
	private void CheckStartUp()
	{
		if (PlayerPrefs.GetString ("ID") == "") {
			// サインアップウィンドウ表示
			_SignUpUI.Init();
		} else {
			// ログイン処理
			UserAccountManager.AutoLogin();
		}
	}
	/*---------------------------------------------------------------------*/
	public void UserDataDelete()
	{
		UserAccountManager.Delete ();
	}
	public void UserDataInheritSetting()
	{
		UserAccountManager.InheritSetting ();
	}
	public void UserDataInheriting()
	{
		UserAccountManager.Inheriting ();
	}
	/*---------------------------------------------------------------------*/

}
