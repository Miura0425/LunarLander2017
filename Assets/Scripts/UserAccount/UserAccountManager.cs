using UnityEngine;
using System;
using System.Collections;

public class UserAccountManager   {
	/*--------------------------------------------------------------------------*/
	/// 自動サインアップ
	public static void AutoSignUp(string name)
	{
		// IDの自動生成
		string id = Guid.NewGuid ().ToString ("N");
		string _ID = id.Substring (0, 8);
		// パスワードの自動生成
		string pass = Guid.NewGuid ().ToString ("N");
		string _PASS = pass.Substring (0, 8);

		//StartCoroutine (/*サーバー通信クラス サインアップリクエスト*/);
	}
	/*--------------------------------------------------------------------------*/

	/// 自動ログイン
	public static void AutoLogin()
	{
		string _ID = StringEncrypter.DecryptString( PlayerPrefs.GetString ("ID"));
		string _PASS = StringEncrypter.DecryptString( PlayerPrefs.GetString ("PASS"));

		//StartCoroutine (/*サーバー通信クラス ログインリクエスト*/);
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
