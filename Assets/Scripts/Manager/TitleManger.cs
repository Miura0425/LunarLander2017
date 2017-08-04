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
	void Start()
	{
		UserAccountUIManager.Instance.SetLoginUser ();
		if (UserAccountDebugUI.Instance != null) {
			UserAccountDebugUI.Instance.SetID (cGameManager.Instance.UserData.Data.id);
			UserAccountDebugUI.Instance.SetPASS (cGameManager.Instance.UserData.Data.pass);
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
			if (cGameManager.Instance.IsOffline || cGameManager.Instance.UserData.IsLogin) {
				OpenMenu ();
			} else {
				UserAccountUIManager.Instance.ShowUserAccountUI ();
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
		if (!cGameManager.Instance.UserData.IsLogin) {
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

	}
	/*---------------------------------------------------------------------*/
	public void UserDataDelete()
	{
		//UserAccountManager.Delete ();
	}
	public void UserDataInheritSetting()
	{
	}
	public void UserDataInheriting()
	{
	}
	/*---------------------------------------------------------------------*/

}
