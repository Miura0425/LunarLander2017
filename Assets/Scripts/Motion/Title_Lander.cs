using UnityEngine;
using System.Collections;

public class Title_Lander : MonoBehaviour {

	[SerializeField]
	private Camera _Camera;	// カメラ

	private Vector3 vStartPoint; // 移動開始位置
	private Vector3 vGoalPoint;	// 移動先位置

	private bool isMove = false; // 移動フラグ
	private bool isWait = false; // 待ちフラグ

	[SerializeField]
	private float fMoveTime;		// 移動にかける時間
	private float fMoveStartTime;	// 移動開始時間
	[SerializeField]
	private float fWaitTime;		// 待ち時間
	private float fWaitStartTime;	// 待ち開始時間
	[SerializeField]
	private int CutNum;	// 範囲分割数
	/*---------------------------------------------------------------------*/
	void Start()
	{
		SelectPoint ();
	}

	// Update is called once per frame
	void Update () {
		WaitUpdate ();
		MoveUpdate ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 移動開始位置と移動先位置を設定する。
	/// </summary>
	private void SelectPoint()
	{
		// 左上座標の取得
		Vector2 lefttop = _Camera.ScreenToWorldPoint (Vector3.zero) ;
		lefttop.Scale (new Vector2(1,-1)); // 上下反転
		lefttop.x -= this.transform.localScale.x;
		lefttop.y += this.transform.localScale.y;
		// 右下座標の取得
		Vector2 rightbottom = _Camera.ScreenToWorldPoint (new Vector3(Screen.width,Screen.height,0));
		rightbottom.Scale (new Vector2 (1, -1)); // 上下反転
		rightbottom.x += this.transform.localScale.x;
		rightbottom.y -= this.transform.localScale.y;

		// X軸Y軸の長さを取得
		float lenX = rightbottom.x - lefttop.x;
		float lenY = lefttop.y - rightbottom.y;

		// 左端の位置のY座標をランダムに設定
		int leftIdx = Random.Range (0, CutNum+1);
		vStartPoint = new Vector3 ();
		vStartPoint.x = lefttop.x;
		vStartPoint.y = lefttop.y - ((lenY / CutNum)*leftIdx);

		// 右端の位置のY座標をランダムに設定
		int rightIdx = Random.Range (0, CutNum);
		vGoalPoint = new Vector3 ();
		vGoalPoint.x = rightbottom.x;
		vGoalPoint.y = rightbottom.y + ((lenY / CutNum)*rightIdx);

		// 移動開始位置を左右どちらにするかランダムに設定
		if (Random.Range (0, 2) == 0) {
			Vector3 tmp = vStartPoint;
			vStartPoint = vGoalPoint;
			vGoalPoint = tmp;
		}

		// 移動の準備
		Ready ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 移動開始前の準備処理
	/// </summary>
	private void Ready()
	{
		this.transform.position = vStartPoint;
		// 移動先方向を向く
		Vector3 diff = vGoalPoint - this.transform.position;
		this.transform.rotation = Quaternion.FromToRotation (Vector3.up, diff);
		// 移動開始
		fMoveStartTime = Time.timeSinceLevelLoad;
		isMove = true;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 移動更新処理
	/// </summary>
	private void MoveUpdate()
	{
		if (isMove == true) {
			float diff = Time.timeSinceLevelLoad - fMoveStartTime;
			if (diff > fMoveTime) {
				isMove = false;
				isWait = true;
				fWaitStartTime = Time.timeSinceLevelLoad;
			}
			float rate = diff / fMoveTime;
			this.transform.position = Vector3.Lerp (vStartPoint, vGoalPoint, rate);
		}
	}
	/// <summary>
	/// 待ち更新処理
	/// </summary>
	private void WaitUpdate()
	{
		if (isWait == true) {
			float diff = Time.timeSinceLevelLoad - fWaitStartTime;
			if (diff > fWaitTime) {
				isWait = false;
				SelectPoint ();
			}
		}
	}
	/*---------------------------------------------------------------------*/

}
