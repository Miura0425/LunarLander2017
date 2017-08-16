using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreRankingUI : MonoBehaviour {

	[SerializeField]
	Text _TitleText =null;
	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Image _RankingFrame = null;

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
		_BackGround.enabled = true;
		_RankingFrame.enabled = true;

		foreach (var rankingitem in RankingItemList) {
			rankingitem.SetActiveUI (true);
		}
	}
	private void Close()
	{
		// UIオブジェクトを非アクティブにする
		_TitleText.enabled = false;
		_BackGround.enabled = false;
		_RankingFrame.enabled = false;
		// 生成したランキングリストを削除する。
		_RankingFrame.transform.DetachChildren();
		RankingItemList.Clear ();
	}

	public IEnumerator GetScoreRanking()
	{
		// ランキング取得リクエスト
		yield return PlayDataWebRequest.ScoreRankingRequest();
		// ランキングリスト生成
		List<RankingData> rankingData = cGameManager.Instance._RankingData;
		for(int i=0;i<rankingData.Count;i++) {
			ScoreRankingUIItem item = Instantiate (_ScoreRankingItemPrefab);
			item.transform.SetParent (_RankingFrame.transform,false);
			item.transform.localPosition -= new Vector3(0,item.GetComponent<RectTransform>().sizeDelta.y*i,0);
			item.SetRanking (rankingData[i]);
			RankingItemList.Add (item);
		}
	}

}
