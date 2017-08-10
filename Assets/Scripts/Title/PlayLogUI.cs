using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayLogUI : MonoBehaviour {

	[SerializeField]
	Text _TitleText =null;
	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Image _LogFrame = null;

	[SerializeField]
	PlayLogUIItem _PlayLogItemPrefab=null;

	private List<PlayLogUIItem> PlayLogList = new List<PlayLogUIItem>();


	public void SetActiveUI(bool value)
	{
		if (value)
			Open ();
		else
			Close ();
	}
	private void Open()
	{
		// UIオブジェクトと生成したログリストをアクティブにする。
		_TitleText.enabled = true;
		_BackGround.enabled = true;
		_LogFrame.enabled = true;

		foreach (var logitem in PlayLogList) {
			logitem.SetActiveUI (true);
		}
	}
	private void Close()
	{
		// UIオブジェクトを非アクティブにする
		_TitleText.enabled = false;
		_BackGround.enabled = false;
		_LogFrame.enabled = false;
		// 生成したログリストを削除する。
		_LogFrame.transform.DetachChildren();
		PlayLogList.Clear ();
	}

	public IEnumerator GetPlayLog()
	{
		// プレイログ取得リクエスト
		yield return PlayDataWebRequest.PlayLogRequest (cGameManager.Instance.UserData.Data);
		// ログリスト生成
		List<PlayLogData> logData = cGameManager.Instance._PlayData.PlayLogData;
		for(int i=0;i<logData.Count;i++) {
			PlayLogUIItem item = Instantiate (_PlayLogItemPrefab);
			item.transform.SetParent (_LogFrame.transform,false);
			item.transform.localPosition -= new Vector3(0,item.GetComponent<RectTransform>().sizeDelta.y*i,0);
			item.SetPlayLog (logData[i]);
			PlayLogList.Add (item);
		}
	}
}
