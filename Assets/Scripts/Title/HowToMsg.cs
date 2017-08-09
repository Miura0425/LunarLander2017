using UnityEngine;
using System.Collections;

public class HowToMsg : MonoBehaviour {

	[SerializeField]
	private GameObject[] _Msgs=null;
	private int nIdx=0;
	public bool isFinish = false;
	public bool isShow = false;

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		isShow = false;
		foreach (GameObject msg in _Msgs) {
			msg.SetActive (isShow);
		}
	}
	/*---------------------------------------------------------------------*/
	/*---------------------------------------------------------------------*/
	public void HowToStart(){
		nIdx = 0;
		isShow = true;
		isFinish = false;
		_Msgs [nIdx].SetActive (isShow);

		StartCoroutine (this.ShowInputLoop ());
	}
	/*---------------------------------------------------------------------*/
	private IEnumerator ShowInputLoop()
	{
		while (isShow) {

			if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetMouseButtonDown(0)) {
				NextMsg ();
			}

			yield return null;
		}
	}
	/*---------------------------------------------------------------------*/
	public bool NextMsg()
	{
		_Msgs [nIdx].SetActive (false);
		nIdx++;
		if (nIdx >= _Msgs.Length) {
			isFinish = true;
			isShow = false;
			return false;
		}
		_Msgs [nIdx].SetActive (true);
		return true;
	}
	/*---------------------------------------------------------------------*/
}
