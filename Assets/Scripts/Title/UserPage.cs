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
	PlayLogUI _PlayLog= null; // プレイ履歴オブジェク

	public bool isShow = false; // 表示状態フラグ

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ページを開く
	/// </summary>
	public void OpenPage()
	{
		isShow = true;

		// ユーザースコア情報の取得 通信
		StartCoroutine(this.RequestPlayData());
		// 入力アップデート開始
		StartCoroutine(this.InputUpdate());
	}
	/// <summary>
	/// ユーザー情報の取得リクエスト
	/// </summary>
	private IEnumerator RequestPlayData()
	{
		// リクエストを投げる
		yield return _PlayLog.GetPlayLog();
		// 取得した情報をUIに設定する。
		_UserName.text = cGameManager.Instance.UserData.Data.name;
		_HighScore.text = cGameManager.Instance._PlayData.HighScore.ToString();
		_HighStage.text = cGameManager.Instance._PlayData.HighStage.ToString();

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
		// プレイ履歴の表示
		_PlayLog.SetActiveUI(value);
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
		ClosePage ();
	}
	/// <summary>
	/// ユーザー削除処理
	/// </summary>
	public void OnDeleteButton()
	{
		GenericUIManager.Instance.ShowYesNoDialog ("DELETE", "REALLY DELETE?", cGameManager.Instance.UserData.DeleteYesNo);
	}
	/*---------------------------------------------------------------------*/

}
