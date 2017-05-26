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
		groupY [0] = Min_Y;	// 先頭は最小値
		for (int j = 1; j < groupY.Length-1; j++) {
			groupY [j] = Min_Y + Mathf.Abs (Max_Y * j / CutNum); 
		}
		groupY[groupY.Length-1] = Max_Y; // 末尾は最大値

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
	public Vector2[] CreateFlatPoint()
	{
		// 平坦なラインの始点を乱数で選択。両端の頂点と末尾から1つ前の頂点を除く
		int vertIndex = Random.Range (1, vertices.Length - 2);
		if (nExclusionIndex.Count != 0) {
			bool isEqual = true;
			while (isEqual == true) {
				isEqual = false;
				for (int i = 0; i < nExclusionIndex.Count; i++) {
					if (vertIndex == nExclusionIndex [i]) {
						isEqual = true;
						break;
					}
				}
				if (isEqual == true) {
					vertIndex = Random.Range (1, vertices.Length - 2);
				}
				if (Input.GetKeyDown (KeyCode.C))
					break;
			}
		}

		// 設定頂点除外リストに登録
		nExclusionIndex.Add (vertIndex-1);
		nExclusionIndex.Add (vertIndex);
		nExclusionIndex.Add (vertIndex + 1);

		// 乱数で取得した頂点の高さに右側の頂点の高さを合わせる。
		vertices [vertIndex + 1].y = vertices [vertIndex].y;

		Vector2[] flatPoints = new Vector2[2];
		flatPoints [0] = vertices [vertIndex];
		flatPoints [1] = vertices [vertIndex + 1];
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
}
