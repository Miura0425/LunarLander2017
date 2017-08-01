using UnityEngine;
using System.Collections;

public class TitleManger : MonoBehaviour {
	static private TitleManger instance=null;
	static public TitleManger Instance
	{
		get{
			return instance;
		}
	}


	[SerializeField,TooltipAttribute("スタートするためのキー")]
	private KeyCode StartKey;

	// UIオブジェクト
	[SerializeField]
	private MenuManager _Menu; // メニュー
	[SerializeField]
	private UserAccountUIManager _UserAccountMgr;
	[SerializeField]
	private PressMsg _PressMsg; // プレスメッセージ

	// はじめのキーが押されているか
	private bool isPress = false;
	public bool isWait = false;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
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
			_UserAccountMgr.CheckUserAccount ();
			StartCoroutine (WaitUserAccount ());
		}
	}
	/*---------------------------------------------------------------------*/
	private IEnumerator WaitUserAccount()
	{
		isWait = true;
		while (isWait) {
			yield return null;
		}
		_UserAccountMgr.SetActiveUI (false);
		_UserAccountMgr.SetLoginUser ();
		_Menu.Init ();
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
