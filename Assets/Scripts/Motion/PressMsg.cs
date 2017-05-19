using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PressMsg : MonoBehaviour {
	
	private Graphic MsgImg;
	private float fNextTime=0;
	[SerializeField]
	private float fIntervalTime=0;
	/*---------------------------------------------------------------------*/
	void Awake () {
		MsgImg = GetComponent<Graphic> ();
		fNextTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		FlashingMsg ();
	}

	private void FlashingMsg()
	{
		if (Time.time > fNextTime) {
			MsgImg.enabled = !MsgImg.enabled;
			fNextTime += fIntervalTime;
		}
	}
	/*---------------------------------------------------------------------*/
}
