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
	private void OpenMenu()
	{
		UserAccountUIManager.Instance.SetActiveUI (false);
		_Menu.Init ();
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
			if (cGameManager.Instance.IsOffline) {
				OpenMenu ();
			} else {
				UserAccountUIManager.Instance.CheckUserAccount ();
				StartCoroutine (WaitUserAccount ());
			}
		}
	}
	/*---------------------------------------------------------------------*/
	public IEnumerator WaitUserAccount()
	{
		isWait = true;
		while (isWait) {
			yield return null;
		}
		if (!UserAccountManager.IsLogin) {
			UserAccountUIManager.Instance.ChangeMode (UserAccountUIManager.eMode.ERROR);
		} else {
			UserAccountUIManager.Instance.SetLoginUser ();
			OpenMenu ();
		}
	}

	public IEnumerator WaitIntherit()
	{
		isWait = true;
		while (isWait) {
			yield return null;
		}
		if (!UserAccountManager.IsInherit) {
			// 汎用ダイアログ表示 引継処理失敗
			Debug.Log("失敗");
		} else {
			// 汎用ダイアログ表示 引き継ぎ処理成功
			Debug.Log("成功");
		}

	}
	/*---------------------------------------------------------------------*/
	public void UserDataDelete()
	{
		UserAccountManager.Delete ();
	}
	public void UserDataInheritSetting()
	{
	}
	public void UserDataInheriting()
	{
	}
	/*---------------------------------------------------------------------*/

}
