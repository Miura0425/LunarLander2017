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

	// UIオブジェクト
	[SerializeField]
	private MenuManager _Menu; // メニュー
	[SerializeField]
	private PressMsg _PressMsg; // プレスメッセージ

	private bool isPress = false; // プレスキーフラグ
	public bool isWait = false; // 処理待ち用フラグ

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}
	void Start()
	{
		// ローカルのデータをUIに適用する。
		UserAccountUIManager.Instance.SetLoginUser ();
		// デバッグUIに適用
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
	/// <summary>
	/// メニューを表示する。
	/// </summary>
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

			// オフラインプレイ または 既にログイン状態である場合は 即メニュー表示
			if (cGameManager.Instance.IsOffline || cGameManager.Instance.UserData.IsLogin) {
				OpenMenu ();
			} else { // ログイン 又は サインアップ の開始
				UserAccountUIManager.Instance.ShowUserAccountUI ();
			}
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ユーザーアカウント処理待ち
	/// </summary>
	/// <returns>The user account.</returns>
	public IEnumerator WaitUserAccount()
	{
		isWait = true;
		while (isWait) {
			yield return null;
		}

		// ログイン失敗の場合 エラーダイアログ表示
		if (!cGameManager.Instance.UserData.IsLogin) {
			UserAccountUIManager.Instance.ChangeMode (UserAccountUIManager.eMode.ERROR);
		} else { // ログイン成功の場合 
			// ユーザー情報更新
			UserAccountUIManager.Instance.SetLoginUser ();
			// メニュー表示
			OpenMenu ();
		}
	}
	/*---------------------------------------------------------------------*/
}
