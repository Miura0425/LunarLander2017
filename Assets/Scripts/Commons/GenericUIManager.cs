using UnityEngine;
using System.Collections;

/// <summary>
/// 汎用UIを管理する
/// </summary>
public class GenericUIManager : MonoBehaviour {
	private static GenericUIManager instance = null;
	public static GenericUIManager Instance{
		get{
			return instance;
		}
	}

	// UIの種類
	public enum eType{
		MESSAGE=0,
		YESNO,
	}
	private eType mode;

	[SerializeField]
	private DialogUI[] _DialogUIs; // ダイアログ

	void Awake()
	{
		if (instance == null) {
			instance = this;
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// UIのアクティブを設定する
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	public void SetActiveUI(bool value)
	{
		switch(mode)
		{
		case eType.MESSAGE:
			(_DialogUIs [(int)mode] as MessageUI).SetActive (value);
			break;
		case eType.YESNO:
			(_DialogUIs [(int)mode] as YesNoUI).SetActive (value);
			break;
		default:
			break;
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// メッセージダイアログを表示する。
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="message">Message.</param>
	/// <param name="func">Func.</param>
	public void ShowMessageDialog(string title ,string message,MessageUI.OKFunc func=null)
	{
		SetActiveUI (false);
		mode = eType.MESSAGE;
		(_DialogUIs [(int)mode] as MessageUI).SetTitleAndMessage (title, message,func);
		SetActiveUI(true);
	}
	/// <summary>
	/// YesNoダイアログを表示する。
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="message">Message.</param>
	/// <param name="func">Func.</param>
	public void ShowYesNoDialog(string title,string message,YesNoUI.YesNoFunc func=null)
	{
		SetActiveUI (false);
		mode = eType.YESNO;
		(_DialogUIs [(int)mode] as YesNoUI).SetTitleAndMessage (title, message, func);
		SetActiveUI (true);
	}
}
