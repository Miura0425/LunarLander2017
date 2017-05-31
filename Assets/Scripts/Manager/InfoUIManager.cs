using UnityEngine;
using System.Collections;

// UIの情報アイテムの種類
public enum UI_INFO_ITEM
{
	STAGE= 0,
	SCORE,
	FUEL,
	ALTITUDE,
	HORIZONTAL_SPEED,
	VERTICAL_SPEED,
};
// UIのメッセージアイテムの種類
public enum UI_MSG_ITEM
{
	GAME_START = 0,
	GAME_CLEAR ,
	GAME_OVER,
}

public class InfoUIManager : MonoBehaviour {

	[SerializeField]
	private GameObject _Info;		// UIの情報アイテムの親オブジェクト
	private InfoText[] _InfoItems;	// 情報アイテム
	[SerializeField]
	private GameObject _Message;	// メッセージアイテムの親オブジェクト
	private MsgItem[] _MsgItems;	// メッセージアイテム

	[SerializeField]
	private GameObject _InputImage; // 操作方法画像
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// 親オブジェクトからアイテムを取得する。
		_InfoItems = _Info.GetComponentsInChildren<InfoText> ();
		_MsgItems = _Message.GetComponentsInChildren<MsgItem> ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 情報アイテムの値を設定する。
	/// </summary>
	/// <param name="item">情報アイテムの種類</param>
	/// <param name="value">設定値</param>
	public void SetInfoItem(UI_INFO_ITEM item,int value)
	{
		_InfoItems [(int)item].SetNumbers (value);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// メッセージアイテムの値を設定する。
	/// </summary>
	/// <param name="item">メッセージアイテムの種類</param>
	/// <param name="info">メッセージアイテム内の情報アイテムの種類</param></param>
	/// <param name="value">設定値</param>
	/// <typeparam name="T">MsgItemを継承したクラス内のEnumを指定</typeparam>
	public void SetMsgItem<T> (UI_MSG_ITEM item, T info, int value)where T:struct
	{
		_MsgItems [(int)item].SetInfo<T> (info, value);
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// メッセージアイテムのアクティブを切り替える。
	/// </summary>
	/// <param name="item">メッセージアイテムの種類</param>
	/// <param name="active">設定値 true or false</param>
	public void SetMsgActive(UI_MSG_ITEM item, bool active)
	{
		_MsgItems [(int)item].SetItemActive (active);
	}
	/*---------------------------------------------------------------------*/
	public void SetInputImageActive(bool active)
	{
		_InputImage.SetActive(active);
	}
}
