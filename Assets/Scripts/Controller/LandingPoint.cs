using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LandingPoint : MonoBehaviour {

	private int nBonusRate = 0;		// ボーナス倍率
	private Vector2 vStart;		// 着陸地点の始点
	private Vector2 vEnd;		// 着陸地点の終点

	[SerializeField]
	private SpriteRenderer _BonusImg;

	[SerializeField]
	private Sprite[] _BonusRateSprites;

	/*---------------------------------------------------------------------*/
	// ボーナス倍率のゲッター
	public int GetBonusRate()
	{
		return nBonusRate;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 着陸地点の初期化
	/// </summary>
	/// <param name="bonus">ボーナス倍率</param>
	/// <param name="start">始点</param>
	/// <param name="end">終点</param>
	public void Init(int bonus,Vector2 start,Vector2 end)
	{
		// ボーナス倍率の設定
		nBonusRate = bonus;
		// 始点終点の設定
		vStart = start;
		vEnd = end;
		// 長さを取得
		float lenx = vEnd.x-vStart.x;
		// サイズの設定
		Vector2 scale = new Vector2 (lenx, transform.localScale.y);
		transform.localScale = scale;
		// 座標の設定
		Vector2 pos = new Vector2(vStart.x+lenx/2,vStart.y+scale.y/2);
		transform.position = pos;
		// ボーナス画像の座標設定
		_BonusImg.transform.position = new Vector3 (transform.position.x,transform.position.y+transform.localScale.y/4);
		_BonusImg.sprite = _BonusRateSprites [nBonusRate-1];
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
