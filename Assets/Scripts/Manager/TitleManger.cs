using UnityEngine;
using System.Collections;

public class TitleManger : MonoBehaviour {
	[SerializeField,TooltipAttribute("スタートするためのキー")]
	private KeyCode StartKey;

	/*---------------------------------------------------------------------*/
	// 更新処理
	void Update () {
		CheckStartKey ();
	}
	/// <summary>
	/// スタートキーが押されたかチェックする。
	/// </summary>
	private void CheckStartKey()
	{
		if(Input.GetKeyDown(StartKey))
		{
			// メインゲームへ遷移する。
			TransitionManager.Instance.ChangeScene(GAME_SCENE.MAINGAME);
		}
	}
	/*---------------------------------------------------------------------*/
}
