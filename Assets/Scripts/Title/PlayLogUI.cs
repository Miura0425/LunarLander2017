using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayLogUI : MonoBehaviour {

	[SerializeField]
	Text _TitleText =null;
	[SerializeField]
	GameObject _ItemTitlte = null;
	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Transform _LogFrame = null;

	[SerializeField]
	PlayLogUIItem _PlayLogItemPrefab=null;

	private List<PlayLogUIItem> PlayLogList = new List<PlayLogUIItem>();

	public bool isShow = false;


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
		_ItemTitlte.SetActive (true);
		_BackGround.enabled = true;

		foreach (var logitem in PlayLogList) {
			logitem.SetActiveUI (true);
		}
		isShow = true;
	}
	private void Close()
	{
		// UIオブジェクトを非アクティブにする
		_TitleText.enabled = false;
		_ItemTitlte.SetActive (false);
		_BackGround.enabled = false;

		foreach (var logitem in PlayLogList) {
			logitem.SetActiveUI (false);
		}

		isShow = false;
	}

	public IEnumerator GetPlayLog()
	{
		// プレイログ取得リクエスト
		yield return PlayDataWebRequest.PlayLogRequest (cGameManager.Instance.UserData.Data);

		_LogFrame.transform.DetachChildren ();
		PlayLogList.Clear ();
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
