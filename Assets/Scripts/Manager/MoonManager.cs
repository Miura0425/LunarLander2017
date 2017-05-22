using UnityEngine;
using System.Collections;

public class MoonManager : MonoBehaviour {

	[SerializeField]
	private MoonLine moonline;
	[SerializeField]
	private LandingPoint[] landingPoint = new LandingPoint[4];

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		Init ();
	}

	// Update is called once per frame
	void Update () {
	
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化処理 ゲーム開始前に呼び出す
	/// </summary>
	private void Init()
	{
		// 新しい月面ステージ生成
		CreateMoonStage ();
	}

	/// <summary>
	/// 月面ステージ自動生成
	/// </summary>
	void CreateMoonStage()
	{
		// 月面のライン初期化
		moonline.Init ();
		// 着陸地点の配置とボーナスの設定
		landingPoint[0].Init(2,moonline.CreateFlatPoint());
		landingPoint[1].Init(3,moonline.CreateFlatPoint());
		landingPoint[2].Init(4,moonline.CreateFlatPoint());
		landingPoint[3].Init(5,moonline.CreateFlatPoint());
		moonline.LineAndColliderUpdate ();

		Debug.Log ("Create Moon");
	}
}
