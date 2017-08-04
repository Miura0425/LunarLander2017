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
			cGameManager.Instance.UserData.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// ヘッダー情報 クッキー取得
				CookieHeaderSetting(request);

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

				UserAccountData.UserData userdata = new UserAccountData.UserData (_ID,_PASS,_NAME,response.num);
				// ローカルへ保存
				cGameManager.Instance.UserData.SaveUserData(userdata);

				cGameManager.Instance.UserData.IsLogin = true;
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
			cGameManager.Instance.UserData.IsLogin = false;
		} else {

			if (request.responseCode == 200) {

				// クッキーの設定
				CookieHeaderSetting(request);

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

				cGameManager.Instance.UserData.IsLogin = true;
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
				MessageResponseData response = JsonUtility.FromJson<MessageResponseData>(text);
				cGameManager.Instance.UserData.MessageResData = response;

				UserAccountUIManager.Instance.ShowMessageDialog ("InheritSetting", response.message);

			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator CheckInheriting()
	{
		yield return new WaitForSeconds (1.5f);

		string url_base = Const.WebRequest.BASE_URL + "CheckInheriting/";
		int NUM = cGameManager.Instance.UserData.Data.num;
		string url_param = "?num="+NUM.ToString();
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

				Debug.Log (result.message);
				string title = "Inherit";

				if (result.id != "") {
					UserAccountUIManager.Instance.ShowMessageDialog (title, result.message);

					UserAccountData.UserData userdata = new UserAccountData.UserData (result.id, result.pass, result.name, NUM);
					cGameManager.Instance.UserData.SaveUserData (userdata);

				} else {
					UserAccountUIManager.Instance.ShowMessageDialog (title,result.message);
				}
			}
		}
	}

	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator DeleteUserAccount(string _ID,string _PASS)
	{
		string url_base = Const.WebRequest.BASE_URL + "Delete/";
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
				MessageResponseData response = JsonUtility.FromJson<MessageResponseData> (text);
				Debug.Log (response.message);

				if (response.message != "") {
					cGameManager.Instance.UserData.DeleteData ();
					UserAccountUIManager.Instance.ShowMessageDialog ("DELETE", response.message);
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
