using UnityEngine;
using System.Collections;

public class HowToMsg : MonoBehaviour {

	[SerializeField]
	private GameObject[] _Msgs;
	private int nIdx=0;
	public bool isFinish = false;

	void Awake()
	{
		foreach (GameObject msg in _Msgs) {
			msg.SetActive (false);
		}
	}

	public void HowToStart(){
		nIdx = 0;
		_Msgs [nIdx].SetActive (true);
		isFinish = false;
	}

	public bool NextMsg()
	{
		_Msgs [nIdx].SetActive (false);
		nIdx++;
		if (nIdx >= _Msgs.Length) {
			isFinish = true;
			return false;
		}
		_Msgs [nIdx].SetActive (true);
		return true;
	}
}
