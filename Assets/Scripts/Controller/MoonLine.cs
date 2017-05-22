using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoonLine : MonoBehaviour {
	private LineRenderer linerenderer;
	private EdgeCollider2D collier;

	[SerializeField]
	private Vector3 vStart;
	[SerializeField]
	private Vector3 vEnd;
	[SerializeField]
	private Vector3[] vertices; 	//　頂点

	private List<int> nExclusionIndex = new List<int>();
	/*---------------------------------------------------------------------*/
	// Use this for initialization
	void Awake () {
		linerenderer = this.GetComponent<LineRenderer> ();
		collier = this.GetComponent<EdgeCollider2D> ();

	}
	// Update is called once per frame
	void Update () {
	
	}
	/*---------------------------------------------------------------------*/
	public void Init()
	{
		nExclusionIndex.Clear ();

		int i = 0;
		float lenX = Mathf.Abs (vEnd.x - vStart.x) / (vertices.Length-1);
		vertices [i] = vStart;

		for (i=1; i < vertices.Length-1; i++) {
			vertices [i].x = vStart.x + i * lenX;
			vertices [i].y = vStart.y;
		}
		vertices [i] = vEnd;
		LineAndColliderUpdate ();
	}
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
}
