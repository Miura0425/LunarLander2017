using UnityEngine;
using System.Collections;

public class GameClearMsg : MsgItem {
	// 情報アイテムの種類
	public enum INFO_ITEM
	{
		MESSAGE = 0,
		SCORE,
		BONUSFUEL,
	};
}
