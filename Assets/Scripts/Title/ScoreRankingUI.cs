using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreRankingUI : MonoBehaviour {
	[SerializeField]
	Text _TitleText = null;
	[SerializeField]
	GameObject _ItemTitle = null;
	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Image _RankingPanel = null;
	[SerializeField]
	RectTransform _RankingFrame = null;
	[SerializeField]
	Scrollbar _Scrollbar = null;

	[SerializeField]
	ScoreRankingUIItem _ScoreRankingItemPrefab=null;

	private List<ScoreRankingUIItem> RankingItemList = new List<ScoreRankingUIItem>();


	public void SetActiveUI(bool value)
	{
		if (value)
			Open ();
		else
			Close ();
	}
	private void Open()
	{
		// UIオブジェクトと生成したランキングリストをアクティブにする。
		_TitleText.enabled = true;
		_ItemTitle.SetActive (true);
		_BackGround.enabled = true;
		_RankingPanel.enabled = true;
		_Scrollbar.gameObject.SetActive (true);

		foreach (var rankingitem in RankingItemList) {
			rankingitem.SetActiveUI (true);
		}
	}
	private void Close()
	{
		// UIオブジェクトを非アクティブにする
		_TitleText.enabled = false;
		_ItemTitle.SetActive (false);
		_BackGround.enabled = false;
		_RankingPanel.enabled = false;
		_Scrollbar.gameObject.SetActive (false);

		foreach (var rankingitem in RankingItemList) {
			rankingitem.SetActiveUI (false);
		}
	}

	public IEnumerator GetScoreRanking()
	{
		// ランキング取得リクエスト
		yield return PlayDataWebRequest.ScoreRankingRequest(cGameManager.Instance.UserData.Data);

		// ランキングUIオブジェクトの初期化
		_RankingFrame.transform.DetachChildren();
		RankingItemList.Clear ();

		// ランキングデータをUIアイテムとして追加していく。
		if (cGameManager.Instance._RankingData.Data.Count != 0) {
			ScoreRankingResponseData _RankingData = cGameManager.Instance._RankingData;

			// データフレームのサイズ設定
			Vector2 frameSize = _RankingPanel.rectTransform.sizeDelta; // 初期サイズ
			float ItemHeight = _ScoreRankingItemPrefab.GetComponent<RectTransform> ().sizeDelta.y; // データUIの高さ
			int addDataLen = (_RankingData.Data.Count + 1) - (int)(frameSize.y / ItemHeight);
			if (addDataLen > 0) {
				frameSize.y += addDataLen * ItemHeight;
			}
			_RankingFrame.sizeDelta = frameSize;
			Vector3 newpos = _RankingFrame.localPosition;
			newpos -= new Vector3 (0,(addDataLen*ItemHeight)/2,0);
			_RankingFrame.localPosition = newpos;
			Vector3 firstItempos = new Vector3 (0, _RankingFrame.sizeDelta.y / 2 - ItemHeight/2, 0);

			// ユーザーランク生成
			RankingData userRank = cGameManager.Instance._RankingData.UserRank;
			ScoreRankingUIItem userRankItem = Instantiate (_ScoreRankingItemPrefab);
			userRankItem.transform.SetParent (_RankingFrame.transform, false);
			userRankItem.transform.localPosition = firstItempos;
			userRankItem.SetRanking (userRank);
			userRankItem.SetColor (Color.yellow);
			RankingItemList.Add (userRankItem);
			// ランキングリスト生成
			List<RankingData> rankingData = cGameManager.Instance._RankingData.Data;
			for (int i = 0; i < rankingData.Count; i++) {
				ScoreRankingUIItem item = Instantiate (_ScoreRankingItemPrefab);
				item.transform.SetParent (_RankingFrame.transform, false);
				item.transform.localPosition = firstItempos - new Vector3 (0, item.GetComponent<RectTransform> ().sizeDelta.y * (i + 1), 0);
				item.SetRanking (rankingData [i]);
				RankingItemList.Add (item);
			}
		}
	}

}
