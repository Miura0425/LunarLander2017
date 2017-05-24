using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	// コンポーネント
	private Camera _Camera; // カメラ

	// ゲームオブジェクト
	[SerializeField]
	private LunderStatus _Lunder; // ランダー

	// カメラモード
	private enum MODE
	{
		NORMAL = 0,
		ZOOM,
	}
	[SerializeField]
	private MODE _Mode; // 現在のカメラモード

	// モード更新処理のデリゲート
	private delegate void ModeUpdate();
	ModeUpdate modeupdate = null;

	public Vector2 normalLeftTop;		// ノーマルモード時の画面左上座標
	public Vector2 normalRightBottom;	// ノーマルモード時の画面右下座標

	// 処理用変数
	private float fTopLine;			// カメラを上方向に動かしだす座標
	private Vector2 vLeftRightLine; // カメラを左右方向に動かしだす座標
	private Vector3 vZoom;			// ズーム時のベース座標保存用
	/*---------------------------------------------------------------------*/

	void Awake () {
		_Camera = this.GetComponent<Camera> ();
		Init ();
	}
	
	// 更新処理
	void Update () {
		modeupdate ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化処理
	/// </summary>
	public void Init()
	{
		// モードをNORMALに設定
		ChangeMode (MODE.NORMAL);

		// ノーマル時の画面端座標を取得する。
		normalLeftTop = GetScreenLeftTop ();
		normalRightBottom = GetScreenRightBottom ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// カメラモードの変更とモード初期設定
	/// </summary>
	/// <param name="mode">変更するモード</param>
	private void ChangeMode(MODE mode)
	{
		// モードの変更
		_Mode = mode;

		// ノーマルモードの初期設定
		if (_Mode == MODE.NORMAL) {
			// カメラサイズの設定
			_Camera.orthographicSize = Const.CameraData.MODE_NORMAL_SIZE;

			// 更新処理の設定
			modeupdate = new ModeUpdate (NormalUpdate);

			// カメラの座標を設定
			Vector3 vNormalPos = Vector3.zero;
			vNormalPos.z = this.transform.position.z;
			this.transform.position = vNormalPos;

			// 画面を上方向に動かすY座標を設定する。
			fTopLine = GetScreenLeftTop().y - _Lunder.transform.localScale.y ;
		}
		// ズームモードの初期設定
		if (_Mode == MODE.ZOOM) {
			// カメラサイズの設定
			_Camera.orthographicSize = Const.CameraData.MODE_ZOOM_SIZE;

			// 更新処理の設定
			modeupdate = new ModeUpdate (ZoomUpdate);

			// カメラの座標を設定
			Vector3 vZoomPos = _Lunder.transform.position;

			// 範囲外を映している場合、X座標を移動させる。
			float fZoomWidth = Mathf.Abs( this.transform.position.x - GetScreenLeftTop ().x);
			if (vZoomPos.x - fZoomWidth <= normalLeftTop.x) {
				vZoomPos.x = normalLeftTop.x + fZoomWidth;
			} else if (normalRightBottom.x <= vZoomPos.x + fZoomWidth) {
				vZoomPos.x = normalRightBottom.x - fZoomWidth;
			}
			//vZoomPos.y += _Lunder.transform.localScale.y;
			vZoomPos.z = this.transform.position.z;
			this.transform.position = vZoomPos;

			// ズーム時の座用を保持
			vZoom = vZoomPos;

			// 画面を左右方向に動かすX座標を設定する。
			vLeftRightLine.x = GetScreenLeftTop ().x + Const.CameraData.ZOOM_LEFTRIGHT_LINE;
			vLeftRightLine.y = GetScreenRightBottom ().x - Const.CameraData.ZOOM_LEFTRIGHT_LINE;
		}
	}
	/// <summary>
	/// ノーマルモードの更新処理
	/// </summary>
	private void NormalUpdate()
	{
		// ランダーのY座標が上ラインを超えた場合、カメラを動かす。
		if(_Lunder.transform.position.y >= fTopLine )
		{
			Vector3 vTopPos = this.transform.position;
			vTopPos.y = _Lunder.transform.position.y - fTopLine;
			this.transform.position = vTopPos;
		}
		// ランダーの高度が指定の高度より低くなった場合、ズームモードに変更する。
		if (_Lunder.GetStatus ().altitude <= Const.CameraData.ZOOM_LUDER_ALTITUDE) {
			ChangeMode (MODE.ZOOM);
		}
	}
	/// <summary>
	/// ズームモードの更新処理
	/// </summary>
	private void ZoomUpdate ()
	{
		// ランダーのX座標が左ラインを超えた場合 かつ 画面左端がノーマル時の左端を超えていない場合
		if (vLeftRightLine.x >= _Lunder.transform.position.x && GetScreenLeftTop ().x > normalLeftTop.x) {
			
			// カメラの座標を左に動かす
			Vector3 vLeftPos = this.transform.position;
			vLeftPos.x = vZoom.x + _Lunder.transform.position.x - vLeftRightLine.x;
			this.transform.position = vLeftPos;

			// 右ラインを更新する。
			vLeftRightLine.y = GetScreenRightBottom ().x - Const.CameraData.ZOOM_LEFTRIGHT_LINE;

		} // ランダーのX座標が右ラインを超えた場合　かつ 画面右端がノーマル時の右端を超えていない場合
		else if (vLeftRightLine.y <= _Lunder.transform.position.x && GetScreenRightBottom().x < normalRightBottom.x) {
			
			// カメラの座標を右に動かす
			Vector3 vRightPos = this.transform.position;
			vRightPos.x = vZoom.x + _Lunder.transform.position.x - vLeftRightLine.y;
			this.transform.position = vRightPos;

			// 左ラインを更新する。
			vLeftRightLine.x = GetScreenLeftTop ().x + Const.CameraData.ZOOM_LEFTRIGHT_LINE;

		} else {
			// 座標の移動がない場合は、ズーム時座標を更新する。
			vZoom = this.transform.position;
		}

		// ランダーの高度が指定の高度より高くなった場合、ノーマルモードに変更する。
		if (_Lunder.GetStatus ().altitude >= Const.CameraData.NORMAL_LUNDER_ALTITUDE) {
			ChangeMode (MODE.NORMAL);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 画面の左上の座標を返す。
	/// </summary>
	/// <returns>x成分が画面左座標　y成分が画面上座標</returns>
	public Vector2 GetScreenLeftTop()
	{
		Vector2 lefttop = _Camera.ScreenToWorldPoint (Vector3.zero);
		lefttop.Scale (new Vector2(1,-1)); // 上下反転
		return lefttop;
	}
	/// <summary>
	/// 画面の右下の座標を返す。
	/// </summary>
	/// <returns>x成分が画面右座標　y成分が画面下座標</returns>
	public Vector2 GetScreenRightBottom()
	{
		Vector2 rightbottom = _Camera.ScreenToWorldPoint (new Vector3(Screen.width,Screen.height,0));
		rightbottom.Scale (new Vector2 (1, -1)); // 上下反転
		return rightbottom;
	}
	/*---------------------------------------------------------------------*/
}
