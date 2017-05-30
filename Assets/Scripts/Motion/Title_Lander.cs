using UnityEngine;
using System.Collections;

public class Title_Lander : MonoBehaviour {
	[SerializeField]
	private GameObject[] points;

	private GameObject targetObj = null;

	private bool isMove = false;
	private bool isRot = false;

	private Vector3 startPos;
	private Vector3 targetPos;
	private Quaternion startRot;
	private Quaternion targetRot;

	private float fStartTime;
	[SerializeField]
	private float fMoveTime;
	[SerializeField]
	private float fRotTime;
	/*---------------------------------------------------------------------*/
	void Awake()
	{
		Goal ();
	}

	// Update is called once per frame
	void Update () {
		Rotation ();
		Move ();
	}
	void FixedUpdate()
	{
		
	}
	/*---------------------------------------------------------------------*/
	private void CheckUseRocket()
	{
		
	}
	private void CheckMove()
	{
		
	}
	private void CheckRotation()
	{
		
	}
	/*---------------------------------------------------------------------*/
	private void Move()
	{
		if (isMove) {
			float diff = Time.timeSinceLevelLoad - fStartTime;
			if (diff >= fMoveTime) {
				isMove = false;
				Goal ();
			}
			float rate = diff / fMoveTime;
			this.transform.position = Vector3.Lerp (startPos, targetPos, rate);
		}
	}
	private void Rotation()
	{
		if (isRot == true) {
			float diff = Time.timeSinceLevelLoad - fStartTime;
			if (diff >= fRotTime) {
				isRot = false;
				MoveStart ();
			}
			float rate = diff / fRotTime;
			this.transform.rotation = Quaternion.Lerp (startRot, targetRot, rate);
		}
	}
	private void Goal()
	{
		isMove = false;
		int nextIdx = Random.Range (0, points.Length);
		targetObj = points [nextIdx];

		startRot = this.transform.rotation;
		Vector3 vec = targetObj.transform.position - this.transform.position;
		vec.z = 0;
		targetRot = Quaternion.LookRotation(vec);


		fStartTime = Time.timeSinceLevelLoad;

		isRot = true;
	}
	private void MoveStart (){
		isMove = true;
		startPos = this.transform.position;
		targetPos = targetObj.transform.position;

		fStartTime = Time.timeSinceLevelLoad;
	}
	/*---------------------------------------------------------------------*/

}
