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
	private MeshFilter _MoonMesh;

	[SerializeField]
	private int nVertexNum; // 月面ステージの頂点数
	[SerializeField][Range(2,15)]
	private int CutNum;	// Y座標範囲分割数
	[SerializeField]
	private float Max_Y; // Y座標範囲最大値
	[SerializeField]
	private float Min_Y; // Y座標範囲最小値
	[SerializeField]
	private List<Vector3> vertices = new List<Vector3>(); 	//　頂点

	private List<int> nExclusionIndex = new List<int>(); // 着陸地点選択から除外する頂点リスト
	/*---------------------------------------------------------------------*/
	public int BFnVertexNum{get{return nVertexNum;}set{nVertexNum =value;}}
	public int BFCutNum{get{return CutNum;}set{CutNum =value;}}
	public float BFMax_Y{get{return Max_Y;}set{Max_Y =value;}}
	public float BFMin_Y{get{return Min_Y;}set{Min_Y =value;}}
	/*---------------------------------------------------------------------*/
	// Use this for initialization
	void Awake () {
		linerenderer = this.GetComponent<LineRenderer> ();
		collier = this.GetComponent<EdgeCollider2D> ();
	}
	/*---------------------------------------------------------------------*/
	public void Init()
	{
		// 月面ラインの頂点設定
		CreateLine ();
		// LineRendererとCollierの頂点を更新
		LineAndColliderUpdate ();
	}
	/*---------------------------------------------------------------------*/
	private void CreateLine()
	{
		vertices.Clear ();
		for (int i = 0; i < nVertexNum; i++) {
			vertices.Add (Vector3.zero);
		}
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
		float lenX = Mathf.Abs (right - left) / (vertices.Count-1);
		// 頂点の座標を設定する。
		for (int i=0; i < vertices.Count; i++) {
			Vector3 tmp = Vector3.zero;
			tmp.x = left + i * lenX;
			tmp.y = Random.Range(groupY[pick],groupY[pick+1]);	// 選択した範囲内でY座標を乱数で設定
			vertices[i] = tmp;

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
		int partVertNum = vertices.Count / pointNum;

		// 範囲の最初の頂点と最後の頂点
		int PartMin = partNum * partVertNum;
		int partMax = pointNum==partNum+1 ? vertices.Count -1 : PartMin + partVertNum;

		// ボーナスレベルを範囲減少値に使用
		int plusVert = (int)level;

		// 始点を範囲内からランダムに選択する。
		int firstIdx = Random.Range (PartMin + 1, partMax-plusVert);
		for (int i = 1; i <= plusVert; i++) {
			Vector3 tmp = vertices [firstIdx + i];
			tmp.y = vertices[firstIdx].y;
			vertices [firstIdx + i] = tmp;
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
		linerenderer.SetVertexCount (vertices.Count);
		linerenderer.SetPositions (vertices.ToArray());

		// あたり判定の設定
		Vector2[] vertices2D = new Vector2[vertices.Count];
		for (int i = 0; i < vertices.Count; i++) {
			vertices2D [i] = vertices [i];
		}
		collier.points= vertices2D;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 月面メッシュの作成
	/// </summary>
	public void CreateMesh()
	{
		// メッシュの頂点を作成
		List<Vector3> meshVertices = new List<Vector3>();
		for (int i = 0; i < vertices.Count; i++) {
			meshVertices.Add(vertices [i]);
		}
		float bottom = _Camera.GetScreenRightBottom ().y-5.0f;
		for (int i = vertices.Count-1; i >= 0; i--) {
			meshVertices.Add (new Vector2 (vertices [i].x, bottom));
		}
		// メッシュのインデックスを作成
		List<int> meshIndices = new List<int>();
		for (int i = 0; i < vertices.Count-1; i++) {
			meshIndices.Add (i);
			meshIndices.Add (i + 1);
			meshIndices.Add (meshVertices.Count - 1 - i);
		}
		for (int i = vertices.Count,j=0; i < meshVertices.Count-1; i++,j++) {
			meshIndices.Add (i);
			meshIndices.Add (i + 1);
			meshIndices.Add (vertices.Count - 1 - j);
		}

		float left = _Camera.GetScreenLeftTop ().x;	// 画面左端座標
		float lenX = _Camera.GetScreenRightBottom().x - _Camera.GetScreenLeftTop().x; // X軸の長さ
		float lenY = Mathf.Abs(Max_Y - Min_Y);	// Y軸の長さ
		// UV作成
		List<Vector2> meshUVs = new List<Vector2> ();
		for (int i = 0; i < meshVertices.Count; i++) {
			float uvX = Mathf.Abs ((meshVertices[i].x - left)/lenX);
			float uvY = Mathf.Abs ((Max_Y- meshVertices [i].y) / lenY);
			Vector2 uv = new Vector2 (uvX,uvY);
			meshUVs.Add (uv);
		}

		// メッシュの作成し、頂点とインデックス、UVを設定する。
		Mesh mesh = new Mesh ();
		mesh.vertices = meshVertices.ToArray ();
		mesh.triangles = meshIndices.ToArray ();
		mesh.uv = meshUVs.ToArray ();

		// メッシュフィルターにメッシュを設定する。
		_MoonMesh.sharedMesh = mesh;

	}
}
