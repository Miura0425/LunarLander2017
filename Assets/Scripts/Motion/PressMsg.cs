using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressMsg : MonoBehaviour {
	
	private Graphic MsgImg;	// グラフィックコンポーネント
	[SerializeField]
	private float fIntervalTime=0;	// インターバル時間
	private float fStartTime = 0;	// 開始時間
	/*---------------------------------------------------------------------*/
	void Awake () {
		MsgImg = GetComponent<Graphic> ();
		MsgImg.enabled = false;
	}

	void Start()
	{
		fStartTime = Time.timeSinceLevelLoad;
	}
	
	// 更新処理
	void Update () {
		FlashingMsg ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 点滅処理
	/// </summary>
	private void FlashingMsg()
	{
		if (Time.timeSinceLevelLoad -fStartTime > fIntervalTime) {
			MsgImg.enabled = !MsgImg.enabled;
			fStartTime = Time.timeSinceLevelLoad;
		}
	}
	/*---------------------------------------------------------------------*/
}
