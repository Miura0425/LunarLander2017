using UnityEngine;
using System.Collections;

public class LandingPoint : MonoBehaviour {

	
	private int nBonusRate = 0;		// ボーナス倍率
	private Vector2 vStart;		// 着陸地点の始点
	private Vector2 vEnd;		// 着陸地点の終点
	/*---------------------------------------------------------------------*/
	// ボーナス倍率のゲッター
	public int GetBonusRate()
	{
		return nBonusRate;
	}

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		
	}
	/// <summary>
	/// 着陸地点の初期化
	/// </summary>
	/// <param name="bonus">ボーナス倍率</param>
	/// <param name="start">始点</param>
	/// <param name="end">終点</param>
	public void Init(int bonus,Vector2 start,Vector2 end)
	{
		nBonusRate = bonus;
		vStart = start;
		vEnd = end;

		float lenx = end.x-start.x;
		// サイズの設定
		Vector2 scale = new Vector2 (lenx, transform.localScale.y);
		transform.localScale = scale;
		// 座標の設定
		Vector2 pos = new Vector2(vStart.x+lenx/2,vStart.y);
		transform.position = pos;
	}
	/// <summary>
	/// 着陸地点の初期化
	/// </summary>
	/// <param name="bonus">ボーナス倍率</param>
	/// <param name="pos">始点と終点の配列</param>
	public void Init(int bonus,Vector2[] pos)
	{
		this.Init (bonus, pos [0], pos [1]);
	}
}
