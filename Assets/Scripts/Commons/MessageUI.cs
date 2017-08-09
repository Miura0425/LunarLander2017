using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// メッセージダイアログ
/// </summary>
public class MessageUI : DialogUI {

	[SerializeField]
	private Text _Message; // メッセージテキスト

	// OKボタン押した際に実行する処理
	public delegate void OKFunc ();
	OKFunc _fcOK = null;

	/*---------------------------------------------------------------------*/

	/// <summary>
	/// アクティブを設定する。
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public virtual void SetActive(bool value)
	{
		_BackBord.enabled = value;
		for (int i = 0; i < transform.childCount; i++) {
				transform.GetChild (i).gameObject.SetActive (value);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// タイトルテキスト・メッセージテキスト・OK処理を設定する。
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="msg">Message.</param>
	/// <param name="func">Func.</param>
	public void SetTitleAndMessage(string title,string msg,OKFunc func = null)
	{
		SetTitle (title);
		SetMessage (msg);
		_fcOK = func;
	}
	/*---------------------------------------------------------------------*/
	// メッセージを設定する
	public void SetMessage(string msg)
	{
		_Message.text = msg;
	}
	/*---------------------------------------------------------------------*/
	// OKボタンクリック処理
	public void OnOKButton()
	{
		SetActive (false);

		// OK処理があるなら実行
		if (_fcOK != null) {
			_fcOK ();
		}

	}
	/*---------------------------------------------------------------------*/
}
