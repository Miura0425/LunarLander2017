using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressMsg : MonoBehaviour {
	
	private Graphic MsgImg;
	[SerializeField]
	private float fIntervalTime=0;
	private float fStartTime = 0;
	/*---------------------------------------------------------------------*/
	void Awake () {
		MsgImg = GetComponent<Graphic> ();
		MsgImg.enabled = false;
	}

	void Start()
	{
		fStartTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update () {
		FlashingMsg ();
	}

	private void FlashingMsg()
	{
		if (Time.timeSinceLevelLoad -fStartTime > fIntervalTime) {
			MsgImg.enabled = !MsgImg.enabled;
			fStartTime = Time.timeSinceLevelLoad;
		}
	}
	/*---------------------------------------------------------------------*/
}
