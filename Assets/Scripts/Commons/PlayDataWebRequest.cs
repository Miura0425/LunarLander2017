using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public static class PlayDataWebRequest {

	/// <summary>
	/// プレイデータの送信リクエスト
	/// </summary>
	/// <returns>The play data request.</returns>
	/// <param name="playdata">Playdata.</param>
	/// <param name="userdata">Userdata.</param>
	public static IEnumerator SendPlayDataRequest(PlayData playdata,UserAccountData.UserData userdata)
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL + "SendPlayData/";
		string url_param = "?id="+userdata.id+"&score="+playdata.Score+"&stage="+playdata.ClearStage;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);
		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
			GenericUIManager.Instance.ShowMessageDialog ("Error", request.error);
		} 
	}

	/// <summary>
	/// プレイログの取得リクエスト
	/// </summary>
	/// <returns>The log request.</returns>
	/// <param name="userdata">Userdata.</param>
	public static IEnumerator PlayLogRequest(UserAccountData.UserData userdata)
	{
		// リクエストURLを生成
		string url_base = Const.WebRequest.BASE_URL + "PlayLog/";
		string url_param = "?id="+userdata.id;
		UnityWebRequest request = UnityWebRequest.Get(url_base+url_param);
		// リクエスト送信
		yield return request.Send();

		// 通信エラーチェック
		if (request.isError) {
			Debug.Log (request.error);
			GenericUIManager.Instance.ShowMessageDialog ("Error", request.error);
		} else {
			if (request.responseCode == 200) {
				string text = request.downloadHandler.text;
				// レスポンスデータの変換
				PlayLogResponseData res = JsonUtility.FromJson<PlayLogResponseData> (text);
				cGameManager.Instance._PlayData.PlayLogData = res.LogData;
				cGameManager.Instance._PlayData.HighScore = res.High_Score;
				cGameManager.Instance._PlayData.ClearStage = res.High_ClearStage;
			}
		}
	}
}
