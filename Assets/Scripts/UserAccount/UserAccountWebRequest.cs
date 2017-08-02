using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

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
			UserAccountManager.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// ヘッダー情報 クッキー取得
				CookieHeaderSetting(request);

				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);
				if (response.Message == "Error") {
					yield return UserAccountManager.AutoSignUp(_NAME);
					yield break;
				}

				// IDとパスを暗号化
				string encID = StringEncrypter.EncryptString (_ID);
				string encPASS = StringEncrypter.EncryptString (_PASS);
				UserAccountDebugUI.Instance.SetID (_ID);
				UserAccountDebugUI.Instance.SetPASS (_PASS);
				// ローカルへ保存
				PlayerPrefs.SetString(Const.UserAccount.USER_ID_KEY,encID);
				PlayerPrefs.SetString (Const.UserAccount.USER_PASS_KEY, encPASS);
				PlayerPrefs.SetInt (Const.UserAccount.USER_NUM_KEY, response.num);
				PlayerPrefs.SetString (Const.UserAccount.USER_NAME_KEY, response.name);

				UserAccountManager.IsLogin = true;
			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator AutoLoginRequest(string ID,string PASS)
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL+"Login/";
		string url_param = "?id="+ID+"&pass="+PASS;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (header.Length > 0) {
			request.SetRequestHeader ("Cookie",header);
		}

		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
			UserAccountManager.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// クッキーの設定
				CookieHeaderSetting(request);

				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);

				// データの取得ができているか
				if (response.Message == "Error") {
					yield break;
				}

				// ローカルに保存する。
				PlayerPrefs.SetString (Const.UserAccount.USER_NAME_KEY, response.name);
				PlayerPrefs.SetInt (Const.UserAccount.USER_NUM_KEY, response.num);

				UserAccountManager.IsLogin = true;
			}
		}
	}
	/*------------------------------------------------------------------------------------------------------------*/

	public static IEnumerator DisconnectRequest()
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL+"Disconnect/";
		UnityWebRequest request = UnityWebRequest.Get(url_base);

		// ヘッダー情報 クッキーがあれば設定する。
		if (header.Length > 0) {
			request.SetRequestHeader ("Cookie",header);
		}

		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
		} else {
			if (request.responseCode == 200) {
				Debug.Log ("Disconnect");
			}
		}
	}
	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator CheckInheritSetting(string _ID, string _PASS)
	{
		yield return new WaitForSeconds (1.5f);

		string url_base = Const.WebRequest.BASE_URL + "CheckInheritSetting/";
		string url_param = "?id="+_ID+"&pass="+_PASS;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (header.Length > 0) {
			request.SetRequestHeader ("Cookie",header);
		}

		yield return request.Send ();

		if (request.isError) {
			Debug.Log ("Error");
		}else{
			if (request.responseCode == 200) {
				CookieHeaderSetting (request);
				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;

				UserAccountManager.IsInherit = true;
			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator CheckInheriting()
	{
		yield return new WaitForSeconds (1.5f);

		string url_base = Const.WebRequest.BASE_URL + "CheckInheriting/";
		string url_param = "?num="+PlayerPrefs.GetInt(Const.UserAccount.USER_NUM_KEY);
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);

		// ヘッダー情報 クッキーがあれば設定する。
		if (header.Length > 0) {
			request.SetRequestHeader ("Cookie",header);
		}

		yield return request.Send ();

		if (request.isError) {
			Debug.Log ("Error");
		}else{
			if (request.responseCode == 200) {
				CookieHeaderSetting (request);
				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				InheritResponseData result = JsonUtility.FromJson<InheritResponseData> (text);

				Debug.Log (result.Message);

				if (result.id != "") {

					// IDとパスを暗号化
					string encID = StringEncrypter.EncryptString (result.id);
					string encPASS = StringEncrypter.EncryptString (result.pass);

					// ローカルへ保存
					PlayerPrefs.SetString (Const.UserAccount.USER_ID_KEY, encID);
					PlayerPrefs.SetString (Const.UserAccount.USER_PASS_KEY, encPASS);
					PlayerPrefs.SetString (Const.UserAccount.USER_NAME_KEY, result.name);


					UserAccountManager.IsInherit = true;
				}
			}
		}
	}
	/*------------------------------------------------------------------------------------------------------------*/
	private static void CookieHeaderSetting(UnityWebRequest request)
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
