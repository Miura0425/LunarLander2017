using UnityEngine;
using System.Collections;

public enum UI_INFO_ITEM
{
	STAGE= 0,
	SCORE,
	FUEL,
	ALTITUDE,
	HORIZONTAL_SPEED,
	VERTICAL_SPEED,
};
public enum UI_MSG_ITEM
{
	GAME_START = 0,
	GAME_CLEAR ,
	GAME_OVER,
}

public class InfoUIManager : MonoBehaviour {

	[SerializeField]
	private GameObject _Info;
	private InfoText[] _InfoItems;
	[SerializeField]
	private GameObject _Message;
	private MsgItem[] _MsgItems;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		_InfoItems = _Info.GetComponentsInChildren<InfoText> ();
		_MsgItems = _Message.GetComponentsInChildren<MsgItem> ();
	}
	/*---------------------------------------------------------------------*/
	public void SetInfoItem(UI_INFO_ITEM item,int value)
	{
		_InfoItems [(int)item].SetNumbers (value);
	}
	public void SetMsgItem(UI_MSG_ITEM item,int value)
	{
		switch (item) {
		case UI_MSG_ITEM.GAME_START:
			_MsgItems [(int)item].SetInfo<GameStartMsg.INFO_ITEM> (GameStartMsg.INFO_ITEM.STAGE, value);
			break;
		case UI_MSG_ITEM.GAME_CLEAR:
			
			break;
		case UI_MSG_ITEM.GAME_OVER:
			break;
		default:
			break;
		}

	}
	public void SetMsgActive(UI_MSG_ITEM item, bool active)
	{
		_MsgItems [(int)item].SetItemActive (active);
	}
}
