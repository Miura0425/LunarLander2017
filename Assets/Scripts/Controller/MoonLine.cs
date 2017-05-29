using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoonLine : MonoBehaviour {
	// コンポーネント
	private LineRenderer linerenderer;	// LineRenderer
	private EdgeCollider2D collier;		// EdgeCollider2D

	[SerializeField]
	private CameraManager _Camera; // カメラ

	[SerializeField]
	private int nVertexNum; // 月面ステージの頂点数
	[SerializeField][Range(2,15)]
	private int CutNum;	// Y座標範囲分割数
	[SerializeField]
	private float Max_Y; // Y座標範囲最大値
	[SerializeField]
	private float Min_Y; // Y座標範囲最小値
	[SerializeField]
	private Vector3[] vertices; 	//　頂点

	private List<int> nExclusionIndex = new List<int>(); // 着陸地点選択から除外する頂点リスト
	/*---------------------------------------------------------------------*/
	// Use this for initialization
	void Awake () {
		linerenderer = this.GetComponent<LineRenderer> ();
		collier = this.GetComponent<EdgeCollider2D> ();
		vertices = new Vector3[nVertexNum];
	}
	// Update is called once per frame
	void Update () {
		
	}
	/*---------------------------------------------------------------------*/
	public void Init()
	{
		// 月面ラインの頂点設定
		CreateLine ();
		// LineRendererとCollierの頂点を更新
		LineAndColliderUpdate ();
	}
	private void CreateLine()
	{
		// 着陸地点選択から除外する頂点リストをクリア
		nExclusionIndex.Clear ();

		// Y座標範囲を分割した配列を作成。
		float[] groupY = new float[CutNum+1];
		float groupLenY = Mathf.Abs(Max_Y - Min_Y)/CutNum;
		for (int i = 0; i < groupY.Length; i++) {
			groupY [i] = Min_Y + groupLenY *i;
		}
		// 分割されたY座標範囲のどの区分を使うか選択する。
		int pick = PickGroup();

		// 画面の左端と右端を取得する。
		float left = _Camera.normalLeftTop.x;
		float right = _Camera.normalRightBottom.x;
		// 画面幅/頂点数で1辺の長さを取得する。
		float lenX = Mathf.Abs (right - left) / (vertices.Length-1);
		// 頂点の座標を設定する。
		for (int i=0; i < vertices.Length; i++) {
			vertices [i].x = left + i * lenX;
			vertices [i].y = Random.Range(groupY[pick],groupY[pick+1]);	// 選択した範囲内でY座標を乱数で設定

			// 次の頂点のY座標範囲を決定する。
			pick = PickGroup (pick);
		}
	}
	/// <summary>
	/// 使用する範囲番号を返す。
	/// </summary>
	/// <returns>使用する範囲番号</returns>
	/// <param name="oldpick">1つ前に使用した範囲番号　引数がなければ-1を設定する。</param>
	private int PickGroup(int oldpick=-1)
	{
		int pick;
		// ⊶1は初回と判断し、すべての範囲からランダムに設定する。
		if (oldpick == -1) {
			pick = Random.Range (0, CutNum );
		}
		// 1つ前が最初の範囲の場合、次の範囲を設定する。
		else if (oldpick == 0) {
			pick = oldpick+1;
		}
		// 1つ前が最後の範囲の場合、前の範囲を設定する。
		else if (oldpick == CutNum - 1) {
			pick = oldpick-1;
		} 
		// 前と次に範囲がある場合、どちらかランダムに設定する。
		else {
			pick = oldpick + (Random.Range (0,2)==0?-1:1);
		}
		return pick;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 着陸地点用の平坦なラインを作成する。
	/// </summary>
	/// <returns>The flat point.</returns>
	public Vector2[] CreateFlatPoint(int pointNum,int partNum,MoonManager.BONUS_LEVEL level)
	{
		// 1範囲の頂点数
		int partVertNum = vertices.Length / pointNum;

		// 範囲の最初の頂点と最後の頂点
		int PartMin = partNum * partVertNum;
		int partMax = pointNum==partNum+1 ? vertices.Length -1 : PartMin + partVertNum;

		// ボーナスレベルを範囲減少値に使用
		int plusVert = (int)level;

		// 始点を範囲内からランダムに選択する。
		int firstIdx = Random.Range (PartMin + 1, partMax-plusVert);
		for (int i = 1; i <= plusVert; i++) {
			vertices [firstIdx + i].y = vertices [firstIdx].y;
		}

		// 返却用の配列作成
		Vector2[] flatPoints = new Vector2[2];
		flatPoints [0] = vertices [firstIdx];			// 始点
		flatPoints [1] = vertices [firstIdx+plusVert];	// 終点
		return flatPoints;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// verticesを線とあたり判定の頂点に設定する。
	/// </summary>
	public void LineAndColliderUpdate()
	{
		// 線の頂点の設定
		linerenderer.SetVertexCount (vertices.Length);
		linerenderer.SetPositions (vertices);

		// あたり判定の設定
		Vector2[] vertices2D = new Vector2[vertices.Length];
		for (int i = 0; i < vertices.Length; i++) {
			vertices2D [i] = vertices [i];
		}
		collier.points= vertices2D;
	}
	/*---------------------------------------------------------------------*/
	// 作成途中
	[SerializeField]
	Vector3[] AddVertices ;
	public void CreateMesh()
	{
		// メッシュの頂点を作成
		List<Vector3> meshVertices = new List<Vector3>();
		for (int i = 0; i < vertices.Length; i++) {
			meshVertices.Add(vertices [i]);
		}
		for (int i = 0; i < AddVertices.Length; i++) {
			meshVertices.Add(AddVertices [i]);
		}
		// メッシュのインデックスを作成
		List<Vector3> meshIndices = new List<Vector3>();
	
		string st = "";
		foreach (Vector3 v in meshVertices) {
			st += v.ToString ()+"\n";
		}

	}
}
