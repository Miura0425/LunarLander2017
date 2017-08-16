using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreRanking : MonoBehaviour {

	[SerializeField]
	Image _BackGround = null;
	[SerializeField]
	Button _CloseButton = null;
	[SerializeField]
	Text _TitleText = null;

	// ランキングオブジェクト
	[SerializeField]
	ScoreRankingUI _ScoreRanking = null;

	public bool isShow = false;
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページを開く
	/// </summary>
	public void OpenPage()
	{
		isShow = true;

		// スコアランキングの取得 通信
		StartCoroutine(this.RequestRankingData());
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// スコアランキングを取得してページを表示する。
	/// </summary>
	/// <returns>The ranking data.</returns>
	private IEnumerator RequestRankingData()
	{
		// スコアランキングデータの取得
		yield return _ScoreRanking.GetScoreRanking();

		// UIを表示する
		SetActiveUI(true);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// UIの表示を切り替える
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void SetActiveUI(bool value)
	{
		// オブジェクトをアクティブにする。
		_BackGround.enabled = value;
		_CloseButton.gameObject.SetActive (value);
		_TitleText.enabled = value;
		// ランキングの表示
		_ScoreRanking.SetActiveUI(value);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページを閉じる処理
	/// </summary>
	public void ClosePage()
	{
		isShow = false;
		SetActiveUI (isShow);
	}
	/*---------------------------------------------------------------------*/
}
