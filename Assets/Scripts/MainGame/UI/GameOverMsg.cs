using UnityEngine;
using System.Collections;

public class GameOverMsg : MsgItem {
	// 情報アイテムの種類
	public enum INFO_ITEM
	{
		MESSAGE = 0,
		CLEARSTAGE,
		SCORE,
	};
}