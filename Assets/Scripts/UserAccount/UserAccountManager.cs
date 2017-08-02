using UnityEngine;
using System;
using System.Collections;

public class UserAccountManager   {
	public static bool IsLogin = false;
	public static bool IsInherit = false;
	public static bool IsInheritWait = false;
	/*--------------------------------------------------------------------------*/
	/// 自動サインアップ
	public static IEnumerator AutoSignUp(string name)
	{
		// IDの自動生成
		string id = Guid.NewGuid ().ToString ("N");
		string _ID = id.Substring (0, 8);
		// パスワードの自動生成
		string pass = Guid.NewGuid ().ToString ("N");
		string _PASS = pass.Substring (0, 8);

		yield return UserAccountWebRequest.AutoSignUpRequest(_ID,_PASS,name);

		TitleManger.Instance.isWait = false;
		
	}
	/*--------------------------------------------------------------------------*/

	/// 自動ログイン
	public static IEnumerator AutoLogin()
	{
		string _ID = StringEncrypter.DecryptString( PlayerPrefs.GetString (Const.UserAccount.USER_ID_KEY));
		string _PASS = StringEncrypter.DecryptString( PlayerPrefs.GetString (Const.UserAccount.USER_PASS_KEY));

		if (UserAccountDebugUI.Instance != null) {
			UserAccountDebugUI.Instance.SetID (_ID);
			UserAccountDebugUI.Instance.SetPASS (_PASS);
		}

		yield return UserAccountWebRequest.AutoLoginRequest(_ID,_PASS);

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
	public static void Delete()
	{
		//StartCoroutine(/*サーバー通信クラス データ削除リクエスト*/);
		PlayerPrefs.DeleteAll();
	}
	/*--------------------------------------------------------------------------*/
	/// 引き継ぎ設定
	public static IEnumerator InheritSetting(string _ID,string _PASS)
	{
		if (IsLogin) {
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
		if (IsLogin) {
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
		TitleManger.Instance.isWait = false;
	}
	/*--------------------------------------------------------------------------*/
}
