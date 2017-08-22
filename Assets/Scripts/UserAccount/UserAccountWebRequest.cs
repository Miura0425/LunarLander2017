using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class WebRequestHeader
{
	public static string header = ""; 

	/// <summary>
	/// クッキーをヘッダー情報に設定する。
	/// </summary>
	/// <param name="request">Request.</param>
	public static void CookieHeaderSetting(UnityWebRequest request)
	{
		// クッキー取得
		string cookie_header = request.GetResponseHeader ("set-cookie");

		// ヘッダーの作成
		if (cookie_header != null) {
			string stHeader = "";
			string stJoint = "";
			string[] astCookie = cookie_header.Split (new string[]{ "; " }, System.StringSplitOptions.None);
			foreach (string  stCookie in astCookie) {
				if (!stCookie.Substring (0, 5).Equals ("path=")) {
					stHeader += stJoint + stCookie;
					stJoint = "; ";
				} 
			}
			if (stHeader.Length > 0) {
				header = stHeader;
			} else {
				header = "";
			}
		}
	}
}

public class UserAccountWebRequest  {
	private static string header = "";

	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 自動サインアップリクエスト
	/// </summary>
	/// <param name="_ID">ユーザーID</param>
	/// <param name="_PASS">ユーザーパスワード</param>
	/// <param name="_NAME">ユーザーネーム</param>
	public static IEnumerator AutoSignUpRequest(string _ID,string _PASS,string _NAME)
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL + "SignUp/";
		string url_param = "?id="+_ID+"&pass="+_PASS+"&name="+_NAME;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);
		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
			cGameManager.Instance.UserData.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// ヘッダー情報 クッキー取得
				WebRequestHeader.CookieHeaderSetting(request);

				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);
				cGameManager.Instance.UserData.UserResData = response;
				if (response.message == "Error") {
					yield return UserAccountManager.AutoSignUp(_NAME);
					yield break;
				}

				// IDとパスを暗号化
				//string encID = StringEncrypter.EncryptString (_ID);
				//string encPASS = StringEncrypter.EncryptString (_PASS);

				// ローカルへ保存
				UserAccountData.UserData userdata = new UserAccountData.UserData (_ID,_PASS,_NAME,response.num);
				cGameManager.Instance.UserData.SaveUserData(userdata);

				// ログイン状態にする。
				cGameManager.Instance.UserData.IsLogin = true;
			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 自動ログインリクエスト
	/// </summary>
	/// <returns>The login request.</returns>
	/// <param name="ID">ID</param>
	/// <param name="PASS">PASS</param>
	public static IEnumerator AutoLoginRequest(string ID,string PASS)
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL+"Login/";
		string url_param = "?id="+ID+"&pass="+PASS;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (WebRequestHeader.header.Length > 0) {
			request.SetRequestHeader ("Cookie",WebRequestHeader.header);
		}

		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
			cGameManager.Instance.UserData.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// クッキーの設定
				WebRequestHeader.CookieHeaderSetting(request);

				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);
				cGameManager.Instance.UserData.UserResData = response;

				// データの取得ができているか
				if (response.message == "Error") {
					yield break;
				}

				// ローカルに保存する。
				UserAccountData.UserData userdata = new UserAccountData.UserData (ID,PASS,response.name,response.num);
				cGameManager.Instance.UserData.SaveUserData (userdata);
				// ログイン状態にする
				cGameManager.Instance.UserData.IsLogin = true;
			}
		}
	}
	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 切断リクエスト
	/// </summary>
	/// <returns>The request.</returns>
	public static IEnumerator DisconnectRequest()
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL+"Disconnect/";
		UnityWebRequest request = UnityWebRequest.Get(url_base);

		// ヘッダー情報 クッキーがあれば設定する。
		if (WebRequestHeader.header.Length > 0) {
			request.SetRequestHeader ("Cookie",WebRequestHeader.header);
		}

		// リクエスト送信
		yield return request.Send();

		// レスポンスを処理することがないのでここで終了
	}
	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 引き継ぎ設定 確認処理
	/// </summary>
	/// <returns>The inherit setting.</returns>
	/// <param name="_ID">I.</param>
	/// <param name="_PASS">PAS.</param>
	public static IEnumerator CheckInheritSetting(string _ID, string _PASS)
	{
		// 仮待ち 1.5s
		yield return new WaitForSeconds (1.5f);

		// リクエスト作成
		string url_base = Const.WebRequest.BASE_URL + "CheckInheritSetting/";
		string url_param = "?id="+_ID+"&pass="+_PASS;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (WebRequestHeader.header.Length > 0) {
			request.SetRequestHeader ("Cookie",WebRequestHeader.header);
		}

		yield return request.Send ();

		if (request.isError) {
			Debug.Log ("Error");
			// メッセージダイアログ表示
			GenericUIManager.Instance.ShowMessageDialog ("InheritSetting", "Error");
		}else{
			if (request.responseCode == 200) {
				WebRequestHeader.CookieHeaderSetting (request);
				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				MessageResponseData response = JsonUtility.FromJson<MessageResponseData>(text);
				cGameManager.Instance.UserData.MessageResData = response;

				// メッセージダイアログ表示
				GenericUIManager.Instance.ShowMessageDialog ("InheritSetting", response.message);

			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 引き継ぎ チェック処理
	/// </summary>
	/// <returns>The inheriting.</returns>
	public static IEnumerator CheckInheriting()
	{
		// 仮待ち 1.5s
		yield return new WaitForSeconds (1.5f);

		// リクエスト作成
		string url_base = Const.WebRequest.BASE_URL + "CheckInheriting/";
		int NUM = cGameManager.Instance.UserData.Data.num;
		string url_param = "?num="+NUM.ToString();
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (WebRequestHeader.header.Length > 0) {
			request.SetRequestHeader ("Cookie",WebRequestHeader.header);
		}

		yield return request.Send ();

		if (request.isError) {
			Debug.Log ("Error");
			// メッセージダイアログ表示
			GenericUIManager.Instance.ShowMessageDialog ("Inherit", "Error");
		}else{
			if (request.responseCode == 200) {
				WebRequestHeader.CookieHeaderSetting (request);
				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				InheritResponseData result = JsonUtility.FromJson<InheritResponseData> (text);
				cGameManager.Instance.UserData.InheritResData = result;

				Debug.Log (result.message);
				string title = "Inherit";

				if (result.id != "") {
					// メッセージダイアログ表示
					GenericUIManager.Instance.ShowMessageDialog (title, result.message);
					// 引き継ぎ実行
					cGameManager.Instance.UserData.InheritUserData ();

				} else {
					// 引き継ぎキャンセル メッセージダイアログ表示
					GenericUIManager.Instance.ShowMessageDialog (title,result.message);
				}
			}
		}
	}

	/*------------------------------------------------------------------------------------------------------------*/
	/// <summary>
	/// 削除リクエスト
	/// </summary>
	/// <returns>The user account.</returns>
	/// <param name="_ID">I.</param>
	/// <param name="_PASS">PAS.</param>
	public static IEnumerator DeleteUserAccount(string _ID,string _PASS)
	{
		// リクエスト作成
		string url_base = Const.WebRequest.BASE_URL + "Delete/";
		string url_param = "?id="+_ID+"&pass="+_PASS;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (WebRequestHeader.header.Length > 0) {
			request.SetRequestHeader ("Cookie",WebRequestHeader.header);
		}

		yield return request.Send ();

		if (request.isError) {
			Debug.Log ("Error");
		}else{
			if (request.responseCode == 200) {
				WebRequestHeader.CookieHeaderSetting (request);
				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				MessageResponseData response = JsonUtility.FromJson<MessageResponseData> (text);
				Debug.Log (response.message);

				if (response.message != "") {

					// ローカルから削除
					cGameManager.Instance.UserData.DeleteData ();
					// メッセージダイアログ表示
					GenericUIManager.Instance.ShowMessageDialog ("DELETE", response.message);
				}
			}
		}
	}
	/*------------------------------------------------------------------------------------------------------------*/

}
