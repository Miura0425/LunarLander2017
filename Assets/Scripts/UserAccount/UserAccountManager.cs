using UnityEngine;
using System;
using System.Collections;

public class UserAccountData{
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

	private  UserAccountResponseData m_UserResData = null;
	private  InheritResponseData m_InheritResData = null;
	private  MessageResponseData m_MessageResData = null;

	private bool m_IsLogin = false;

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

	public UserAccountData ()
	{
	}

	public void LoadUserData()
	{
		m_Data.id = PlayerPrefs.GetString (Const.UserAccount.USER_ID_KEY);
		m_Data.pass = PlayerPrefs.GetString (Const.UserAccount.USER_PASS_KEY);
		m_Data.name = PlayerPrefs.GetString (Const.UserAccount.USER_NAME_KEY);
		m_Data.num = PlayerPrefs.GetInt (Const.UserAccount.USER_NUM_KEY);

	}
	public void SaveUserData(UserData data)
	{
		m_Data = data;
		PlayerPrefs.SetString (Const.UserAccount.USER_ID_KEY, m_Data.id);
		PlayerPrefs.SetString (Const.UserAccount.USER_PASS_KEY, m_Data.pass);
		PlayerPrefs.SetString (Const.UserAccount.USER_NAME_KEY, m_Data.name);
		PlayerPrefs.SetInt (Const.UserAccount.USER_NUM_KEY, m_Data.num);

		if (UserAccountDebugUI.Instance != null) {
			UserAccountDebugUI.Instance.SetID (m_Data.id);
			UserAccountDebugUI.Instance.SetPASS (m_Data.pass);
		}
	}

	public void InheritUserData()
	{
		if (InheritResData != null) {
			UserData userdata = new UserData (InheritResData.id, InheritResData.pass, InheritResData.name, m_Data.num);
			SaveUserData (userdata);
		}
	}
	public void DeleteData()
	{
		m_Data.id = "";
		m_Data.pass = "";
		m_Data.name = "";
		m_Data.num = 0;
		SaveUserData (m_Data);

		m_IsLogin = false;
	}
	public void DeleteYesNo(bool yes)
	{
		if (yes) {
			cGameManager.Instance.StartCoroutine (UserAccountManager.Delete ());
		} else {
		}
	}

}

public class UserAccountManager   {
	
	public static bool IsInherit = false;
	public static bool IsInheritWait = false;
	/*--------------------------------------------------------------------------*/
	/// 自動サインアップ
	public static IEnumerator AutoSignUp(string name)
	{
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitUserAccount ());
		// IDの自動生成
		string id = Guid.NewGuid ().ToString ("N");
		string _ID = id.Substring (0, 8);
		// パスワードの自動生成
		string pass = Guid.NewGuid ().ToString ("N");
		string _PASS = pass.Substring (0, 8);

		yield return UserAccountWebRequest.AutoSignUpRequest(_ID,_PASS,name);


		yield return new WaitForSeconds (2.0f);
		TitleManger.Instance.isWait = false;
		
	}
	/*--------------------------------------------------------------------------*/

	/// 自動ログイン
	public static IEnumerator AutoLogin()
	{
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitUserAccount ());

		string _ID = PlayerPrefs.GetString (Const.UserAccount.USER_ID_KEY);
		string _PASS = PlayerPrefs.GetString (Const.UserAccount.USER_PASS_KEY);


		yield return UserAccountWebRequest.AutoLoginRequest(_ID,_PASS);

		yield return new WaitForSeconds (2.0f);
		TitleManger.Instance.isWait = false;
		
	}
	/*--------------------------------------------------------------------------*/
	/// <summary>
	/// 切断処理
	/// </summary>
	/// <returns>The request.</returns>
	public static IEnumerator DisconnectRequest()
	{
		yield return UserAccountWebRequest.DisconnectRequest ();
	}
	/*--------------------------------------------------------------------------*/
	/// データ削除
	public static IEnumerator Delete()
	{
		if (!cGameManager.Instance.UserData.IsLogin)
			yield return null;
		string _ID = cGameManager.Instance.UserData.Data.id;
		string _PASS = cGameManager.Instance.UserData.Data.pass;
		//yield return UserAccountWebRequest.DeleteUserAccount(_ID,_PASS);
		cGameManager.Instance.UserData.DeleteData ();
		GenericUIManager.Instance.ShowMessageDialog ("DELETE", "Complete");
		UserAccountUIManager.Instance.SetLoginUser ();
		cGameManager.Instance.ChangeScene (GAME_SCENE.TITLE);
	}
	/*--------------------------------------------------------------------------*/
	/// 引き継ぎ設定
	public static IEnumerator InheritSetting(string _ID,string _PASS)
	{
		if (!cGameManager.Instance.UserData.IsLogin)
			yield return null;
		if (cGameManager.Instance.UserData.IsLogin) {
			IsInheritWait = true;
			string url_base = Const.WebRequest.BASE_URL + "top/";
			string url_param = "?id="+_ID+"&pass="+_PASS+"&mode="+"SET";
			Application.OpenURL (url_base + url_param);
		}
		yield return InheritSettingWait(_ID,_PASS);
	}
	public static IEnumerator InheritSettingWait(string _ID,string _PASS)
	{
		while (IsInheritWait) {
			yield return null;
		}
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitIntherit ());
		yield return UserAccountWebRequest.CheckInheritSetting(_ID,_PASS);
		TitleManger.Instance.isWait = false;
	}

	/// 引き継ぎ処理
	public static IEnumerator Inheriting(string _ID,string _PASS)
	{
		if (cGameManager.Instance.UserData.IsLogin) {
			IsInheritWait = true;
			string url_base = Const.WebRequest.BASE_URL + "top/";
			string url_param = "?id=" + _ID + "&pass=" + _PASS + "&mode=" + "GET";
			Application.OpenURL (url_base + url_param);

		}
		yield return InheritingWait();
	}
	public static IEnumerator InheritingWait()
	{
		while (IsInheritWait) {
			yield return null;
		}
		cGameManager.Instance.StartCoroutine (TitleManger.Instance.WaitIntherit ());
		yield return UserAccountWebRequest.CheckInheriting ();
		UserAccountUIManager.Instance.SetLoginUser ();
		TitleManger.Instance.isWait = false;
	}
	/*--------------------------------------------------------------------------*/
}
