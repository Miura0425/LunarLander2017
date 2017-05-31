using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoonManager : MonoBehaviour {

	[SerializeField]
	private MoonLine moonline;
	[SerializeField]
	private LandingPoint[] landingPoint;
	[SerializeField]
	private int[] BonusRate;
	public enum BONUS_LEVEL
	{
		LEVEL_4 = 1,
		LEVEL_3,
		LEVEL_2,
		LEVEL_1,
	};

	public bool isInit = false; // 初期化完了フラグ

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化処理 ゲーム開始前に呼び出す
	/// </summary>
	public void Init()
	{
		// 新しい月面ステージ生成
		CreateMoonStage ();
		// 初期化完了
		isInit = true;
	}

	/// <summary>
	/// 月面ステージ自動生成
	/// </summary>
	private	void CreateMoonStage()
	{
		// 月面のライン初期化
		moonline.Init ();

		// ボーナス倍率リスト
		List<int> BonusRateList = new List<int> ();
		// 設定用配列から倍率をリストに追加
		foreach (int rate in BonusRate) {
			BonusRateList.Add (rate);
		}
		// ボーナスレベル変数
		BONUS_LEVEL level = BONUS_LEVEL.LEVEL_1;
		// 着陸地点の配置とボーナスの設定
		for (int i = 0; i < landingPoint.Length; i++) {
			// リストの中からひとつ倍率のインデックスを取得
			int pick = Random.Range (0, BonusRateList.Count);
			// 取得した倍率を設定用配列から探し、ボーナスレベルを設定
			for(int j=0;j<BonusRate.Length;j++)
			{
				if (BonusRateList [pick] == BonusRate [j]) {
					level = (BONUS_LEVEL)(j+1);
				}
			}
			// 着陸地点の始点と終点を取得　(着陸地点の数、いくつ目の着陸地点か、ボーナスレベル)
			Vector2[] pointFE = moonline.CreateFlatPoint(landingPoint.Length,i,level);
			// 着陸地点の初期化　(ボーナス倍率と始点と終点)
			landingPoint[i].Init(BonusRateList[pick],pointFE);

			// 選ばれたものをリストから削除
			BonusRateList.RemoveAt (pick);
		}
		// 月面のラインの更新
		moonline.LineAndColliderUpdate ();
		// 月面メッシュの作成
		moonline.CreateMesh ();
	}
}
