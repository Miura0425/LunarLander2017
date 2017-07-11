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
		} else {

			if (request.responseCode == 200) {

				// ヘッダー情報 クッキー取得
				CookieHeaderSetting(request);


				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);
				if (response.Message == "Error") {
					UserAccountManager.AutoSignUp(_NAME);
					yield break;
				}

				// IDとパスを暗号化
				string encID = StringEncrypter.EncryptString (_ID);
				string encPASS = StringEncrypter.EncryptString (_PASS);

				// ローカルへ保存
				PlayerPrefs.SetString("ID",encID);
				PlayerPrefs.SetString ("PASS", encPASS);
				PlayerPrefs.SetInt ("NUM", response.num);
			}
		}

	}
	/*------------------------------------------------------------------------------------------------------------*/
	public static IEnumerator AutoLoginRequest()
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL+"Login/";
		string url_param = "?id="+PlayerPrefs.GetString("ID")+"&pass="+PlayerPrefs.GetString("PASS");
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
		} else {

			if (request.responseCode == 200) {

				// クッキーの設定
				CookieHeaderSetting(request);

				// レスポンスからJson形式のテキストデータを取得する。
				string text = request.downloadHandler.text;
				UserAccountResponseData response = JsonUtility.FromJson<UserAccountResponseData>(text);

				if (response.Message == "Error") {

					yield break;
				}

				PlayerPrefs.SetInt ("NUM", response.num);
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
			Debug.Log (header);
		}
	}
}
