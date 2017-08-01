using UnityEngine;
using System;
using System.Collections;

public class UserAccountManager   {
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

		UserAccountDebugUI.Instance.SetID (_ID);
		UserAccountDebugUI.Instance.SetPASS (_PASS);

		yield return UserAccountWebRequest.AutoLoginRequest(_ID,_PASS);
		yield return new WaitForSeconds (2);
		TitleManger.Instance.isWait = false;
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
	public static void InheritSetting()
	{

	}

	/// 引き継ぎ処理
	public static void Inheriting()
	{

	}
	/*--------------------------------------------------------------------------*/
}
