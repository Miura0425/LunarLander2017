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

}
