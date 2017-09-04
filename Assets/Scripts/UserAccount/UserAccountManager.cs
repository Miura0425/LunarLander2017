using UnityEngine;
using System;
using System.Collections;

public class UserAccountData{
	// ユーザーアカウントデータ
	public struct UserData
	{
		public string id;
		public string pass;
		public string name;
		public int num;

		public UserData(string _id,string _pass, string _name,int _num)
		{
			id = _id;
			pass = _pass;
			name = _name;
			num = _num;
		}
	}
	private UserData m_Data = new UserData();

	// レスポンスデータ保持用
	private  UserAccountResponseData m_UserResData = null;
	private  InheritResponseData m_InheritResData = null;
	private  MessageResponseData m_MessageResData = null;

	// ログイン状態フラグ
	private bool m_IsLogin = false;


	// プロパティ
	public UserData Data{
		get{ return m_Data; }
		set{ m_Data = value; }
	}

	public UserAccountResponseData UserResData{
		get{ return m_UserResData; }
		set{ m_UserResData = value; }
	}
	public InheritResponseData InheritResData{
		get{ return m_InheritResData; }
		set{ m_InheritResData = value; }
	}
	public MessageResponseData MessageResData{
		get{ return m_MessageResData; }
		set{ m_MessageResData = value; }
	}

	public bool IsLogin {
		get{ return m_IsLogin; }
		set{ m_IsLogin = value; }
	}

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public UserAccountData ()
	{
	}

	/// <summary>
	/// ローカルのユーザーデータをロード
	/// </summary>
	public void LoadUserData()
	{
		string ID = PlayerPrefs.GetString (Const.UserAccount.USER_ID_KEY);
		string PASS = PlayerPrefs.GetString (Const.UserAccount.USER_PASS_KEY);
		if (ID != "")
			ID = StringEncrypter.DecryptString (ID).Substring(0,8);
		if (PASS != "")
			PASS = StringEncrypter.DecryptString (PASS).Substring(0,8);

		m_Data.id = ID;
		m_Data.pass = PASS;
		m_Data.name = PlayerPrefs.GetString (Const.UserAccount.USER_NAME_KEY);
		m_Data.num = PlayerPrefs.GetInt (Const.UserAccount.USER_NUM_KEY);



	}

	/// <summary>
	/// 新しいユーザーデータをローカルに保存する
	/// </summary>
	/// <param name="data">Data.</param>
	public void SaveUserData(UserData data)
	{
		m_Data = data;
		SaveUserData ();
	
		// デバッグUIに適用
		if (UserAccountDebugUI.Instance != null) {
			UserAccountDebugUI.Instance.SetID (m_Data.id);
			UserAccountDebugUI.Instance.SetPASS (m_Data.pass);
		}
	}
	/// <summary>
	/// ユーザデータをローカルに保存する
	/// </summary>
	public void SaveUserData()
	{
		PlayerPrefs.SetString (Const.UserAccount.USER_ID_KEY, StringEncrypter.EncryptString(m_Data.id));
		PlayerPrefs.SetString (Const.UserAccount.USER_PASS_KEY, StringEncrypter.EncryptString(m_Data.pass));
		PlayerPrefs.SetString (Const.UserAccount.USER_NAME_KEY, m_Data.name);
		PlayerPrefs.SetInt (Const.UserAccount.USER_NUM_KEY, m_Data.num);

		// ユーザーデータUIに適用
		UserAccountUIManager.Instance.SetLoginUser ();
	}

	/// <summary>
	/// ユーザーデータの引き継ぎ処理
	/// </summary>
	public void InheritUserData()
	{
		if (InheritResData != null) {
			UserData userdata = new UserData (InheritResData.id, InheritResData.pass, InheritResData.name, InheritResData.num);
			SaveUserData (userdata);
		}
	}

	/// <summary>
	/// ローカルユーザーデータの削除
	/// </summary>
	public void DeleteData()
	{
		m_Data.id = "";
		m_Data.pass = "";
		m_Data.name = "";
		m_Data.num = 0;
		SaveUserData (m_Data);

		m_IsLogin = false;
	}

	/// <summary>
	/// 削除フールプルーフ処理
	/// </summary>
	/// <param name="yes">If set to <c>true</c> yes.</param>
	public void DeleteYesNo(bool yes)
	{
		if (yes) {
			// 削除実行
			cGameManager.Instance.StartCoroutine (UserAccountManager.Delete ());
		}
	}

}

public class UserAccountManager   {
	
	public static bool IsInheritWait = false; // 引き継ぎ待ちフラグ
	/*--------------------------------------------------------------------------*/
	/// 自動サインアップ
	public static IEnumerator AutoSignUp(string name)
	{
		// 待ち処理開始
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitUserAccount ());

		// IDの自動生成
		string id = Guid.NewGuid ().ToString ("N");
		string _ID = id.Substring (0, 8);
		// パスワードの自動生成
		string pass = Guid.NewGuid ().ToString ("N");
		string _PASS = pass.Substring (0, 8);

		// 自動サインアップリクエスト処理
		yield return UserAccountWebRequest.AutoSignUpRequest(_ID,_PASS,name);

		// 仮待ち処理 2s
		//yield return new WaitForSeconds (2.0f);

		// 待ち状態終了
		TitleManger.Instance.isWait = false;
		
	}
	/*--------------------------------------------------------------------------*/

	/// 自動ログイン
	public static IEnumerator AutoLogin()
	{
		// 待ち処理開始
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitUserAccount ());

		// ローカルユーザーデータの取得
		string _ID = cGameManager.Instance.UserData.Data.id;
		string _PASS = cGameManager.Instance.UserData.Data.pass;

		// 自動ログインリクエスト処理
		yield return UserAccountWebRequest.AutoLoginRequest(_ID,_PASS);

		// 仮待ち処理 2s
		//yield return new WaitForSeconds (2.0f);

		// 待ち状態終了
		TitleManger.Instance.isWait = false;
		
	}
	/*--------------------------------------------------------------------------*/
	/// <summary>
	/// 切断処理
	/// </summary>
	/// <returns>The request.</returns>
	public static IEnumerator DisconnectRequest()
	{
		// 切断リクエスト
		yield return UserAccountWebRequest.DisconnectRequest ();
	}
	/*--------------------------------------------------------------------------*/
	/// データ削除
	public static IEnumerator Delete()
	{
		// ログイン状態でのみ実行
		if (cGameManager.Instance.UserData.IsLogin) {

			// ローカルユーザーデータ削除
			cGameManager.Instance.UserData.DeleteData ();

			// ユーザーデータUIに適用
			UserAccountUIManager.Instance.SetLoginUser ();
			// 一度タイトルへ戻る
			cGameManager.Instance.ChangeScene (GAME_SCENE.TITLE);

			yield return null;
		}
	}
	/*--------------------------------------------------------------------------*/
	/// 引き継ぎ設定
	public static IEnumerator InheritSetting(string _ID,string _PASS)
	{
		// ログイン状態でのみ実行
		if (cGameManager.Instance.UserData.IsLogin) {

			// 引き継ぎ設定ページを開く
			string url_base = Const.WebRequest.BASE_URL + "top/";
			string url_param = "?id=" + _ID + "&pass=" + _PASS + "&mode=" + "SET";
			Application.OpenURL (url_base + url_param);

			// 引き継ぎ設定待ち処理 開始
			yield return InheritSettingWait (_ID, _PASS);
		}
	}
	/// <summary>
	/// 引き継ぎ設定待ち処理
	/// </summary>
	/// <returns>The setting wait.</returns>
	/// <param name="_ID">I.</param>
	/// <param name="_PASS">PAS.</param>
	public static IEnumerator InheritSettingWait(string _ID,string _PASS)
	{
		IsInheritWait = true;
		while (IsInheritWait) {
			yield return null;
		}

		// 引き継ぎ設定チェックリクエスト
		yield return UserAccountWebRequest.CheckInheritSetting(_ID,_PASS);

	}

	/// 引き継ぎ処理
	public static IEnumerator Inheriting(string _ID,string _PASS)
	{
		if (cGameManager.Instance.UserData.IsLogin) {

			// 引き継ぎページを開く
			string url_base = Const.WebRequest.BASE_URL + "top/";
			string url_param = "?id=" + _ID + "&pass=" + _PASS + "&mode=" + "GET";
			Application.OpenURL (url_base + url_param);

			// 引き継ぎ待ち処理開始
			yield return InheritingWait ();
		}
	}
	public static void InheritOKFunc()
	{
		// タイトルに戻る 非ログイン状態
		cGameManager.Instance.UserData.IsLogin = false;
		cGameManager.Instance.ChangeScene (GAME_SCENE.TITLE);
	}

	/// <summary>
	/// 引き継ぎ待ち処理
	/// </summary>
	/// <returns>The wait.</returns>
	public static IEnumerator InheritingWait()
	{
		IsInheritWait = true;
		while (IsInheritWait) {
			yield return null;
		}

		// 引き継ぎチェックリクエスト
		yield return UserAccountWebRequest.CheckInheriting ();
	}
	/*--------------------------------------------------------------------------*/
}
