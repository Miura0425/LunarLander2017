using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanderStatus : MonoBehaviour {
	// ステータス構造体
	[System.Serializable]
	public struct STATUS
	{
		public float fuel;				// 燃料
		public float altitude;			// 高度
		public float horizontal_speed;	// 水平速度
		public float vertical_speed;	// 垂直速度
	};

	// ステータス情報
	[SerializeField]
	private STATUS status;

	/*---------------------------------------------------------------------*/
	// ステータスのセッター・ゲッター
	public STATUS GetStatus()
	{
		return status;
	}
	public void SetStatus(STATUS status)
	{
		this.status = status;
	}
	/*---------------------------------------------------------------------*/
	// ステータスのパラメータごとのセッター
	// 燃料
	public void SetFuel(float fuel)
	{
		status.fuel = fuel;
	}
	// 高度
	public void SetAltitude(float altitude){
		status.altitude = altitude;	
	}
	// 水平速度
	public void SetHorizontalSpeed(float speed)
	{
		status.horizontal_speed = speed;
	}
	// 垂直速度
	public void SetVerticalSpeed(float speed)
	{
		status.vertical_speed = speed;
	}
	// 水平速度と垂直速度
	public void SetSpeed (Vector2 speed)
	{
		SetHorizontalSpeed (speed.x);
		SetVerticalSpeed (speed.y);
	}
	/*---------------------------------------------------------------------*/
}
