using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// ユーザーページオブジェクト
/// </summary>
public class UserPage : MonoBehaviour {

	// UIオブジェクト
	[SerializeField]
	Image _BackGround=null; // 背景
	[SerializeField]
	Button _CloseButton=null; // 閉じるボタン
	[SerializeField]
	Text _TitleText=null; // ページタイトル
	[SerializeField]
	Text _UserName=null; // ユーザーネーム
	[SerializeField]
	Text _HighScore=null; // ハイスコア
	[SerializeField]
	Text _HighStage=null; // 最高クリアステージ
	[SerializeField]
	Button _InheritSettingButton=null; // 引き継ぎ設定ボタン
	[SerializeField]
	Button _InheritingButton=null; // 引き継ぎボタン
	[SerializeField]
	Button _DeleteButton=null; // 削除ボタン
	[SerializeField]
	Button _ChangeButton =null; // 表示情報変更ボタン

	[SerializeField]
	PlayLogUI _PlayLog= null; // プレイ履歴オブジェクト
	[SerializeField]
	ScoreRankingUI _RankingUI = null; // スコアランキングオブジェクト


	public bool isShow = false; // 表示状態フラグ

	private enum eShowInfoType
	{
		PlayLog= 0,
		Ranking,

		TYPE_NUM,
	};
	private eShowInfoType _ShowInfoType = eShowInfoType.PlayLog;

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページを開く
	/// </summary>
	public void OpenPage()
	{
		isShow = true;
		_ShowInfoType = eShowInfoType.PlayLog;

		// ユーザースコア情報の取得 通信
		StartCoroutine(this.RequestInfo());
		// 入力アップデート開始
		StartCoroutine(this.InputUpdate());
	}
	/// <summary>
	/// ユーザー情報の取得リクエスト
	/// </summary>
	private IEnumerator RequestInfo()
	{
		// プレイログリクエストを投げる
		yield return _PlayLog.GetPlayLog();
		// 取得した情報をUIに設定する。
		_UserName.text = cGameManager.Instance.UserData.Data.name;
		_HighScore.text = cGameManager.Instance._PlayData.HighScore.ToString();
		_HighStage.text = cGameManager.Instance._PlayData.HighStage.ToString();

		// スコアランキングリクエストを投げる
		yield return _RankingUI.GetScoreRanking();

		// ページオブジェクトを表示する
		SetActivePage(isShow);
	}

	/// <summary>
	/// 入力アップデート
	/// </summary>
	private IEnumerator InputUpdate()
	{
		while (isShow) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				ClosePage ();
			}
			yield return null;
		}
	}

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページの表示を切り替える
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	private void SetActivePage(bool value)
	{
		// オブジェクトをアクティブにする。
		_BackGround.enabled = value;
		_CloseButton.gameObject.SetActive (value);
		_TitleText.enabled = value;
		_UserName.enabled = value;
		_HighScore.gameObject.SetActive (value);
		_HighStage.gameObject.SetActive (value);
		_InheritSettingButton.gameObject.SetActive (value);
		_InheritingButton.gameObject.SetActive (value);
		_DeleteButton.gameObject.SetActive (value);
		_ChangeButton.gameObject.SetActive (value);

		if (_ShowInfoType == eShowInfoType.PlayLog) {
			// プレイ履歴の表示
			_PlayLog.SetActiveUI (value);
		} else {
			// ランキングを表示
			_RankingUI.SetActiveUI(value);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページを閉じる処理
	/// </summary>
	public void ClosePage()
	{
		isShow = false;
		SetActivePage (isShow);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ユーザーネームの変更
	/// </summary>
	public void OnChangeName()
	{
	}
	/// <summary>
	/// 引き継ぎ設定処理
	/// </summary>
	public void OnInheritSettingButton()
	{
		string _ID = cGameManager.Instance.UserData.Data.id;
		string _PASS = cGameManager.Instance.UserData.Data.pass;
		StartCoroutine(UserAccountManager.InheritSetting(_ID,_PASS));
	}
	/// <summary>
	/// 引き継ぎ処理
	/// </summary>
	public void OnInheritting()
	{
		string _ID = cGameManager.Instance.UserData.Data.id;
		string _PASS = cGameManager.Instance.UserData.Data.pass;
		StartCoroutine (UserAccountManager.Inheriting (_ID, _PASS));

	}
	/// <summary>
	/// ユーザー削除処理
	/// </summary>
	public void OnDeleteButton()
	{
		GenericUIManager.Instance.ShowYesNoDialog ("DELETE", "REALLY DELETE?", cGameManager.Instance.UserData.DeleteYesNo);
	}

	/// <summary>
	/// 表示情報を切り替える
	/// </summary>
	public void OnChangeButton()
	{
		_ShowInfoType =(eShowInfoType)((int)(_ShowInfoType+1)%(int)eShowInfoType.TYPE_NUM );
		switch (_ShowInfoType) {
		case eShowInfoType.PlayLog:
			_RankingUI.SetActiveUI (false);
			_PlayLog.SetActiveUI (true);
			_ChangeButton.GetComponentInChildren<Text> ().text = eShowInfoType.Ranking.ToString();
			break;
		case eShowInfoType.Ranking:
			_PlayLog.SetActiveUI (false);
			_RankingUI.SetActiveUI (true);
			_ChangeButton.GetComponentInChildren<Text> ().text = eShowInfoType.PlayLog.ToString ();
			break;
		default:
			break;
		}
	}
	/*---------------------------------------------------------------------*/

}
