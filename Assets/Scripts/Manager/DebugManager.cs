using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DebugManager : MonoBehaviour {

	private List<GameObject> _DebugUIs = new List<GameObject> ();	// デバッグUIリスト

	private LanderManager _Lander;	// ランダーオブジェクト
	private MoonManager _Moon;		// 月面オブジェクト

	// UIアイテム
	[SerializeField]
	private Slider _StageCut;		// ステージ Y軸分割数 スライダー
	[SerializeField]
	private Slider _StageYMAX;
	[SerializeField]
	private LineRenderer _YMAXLine;
	[SerializeField]
	private Slider _StageYMIN;
	[SerializeField]
	private LineRenderer _YMINLine;

	[SerializeField]
	private Toggle _DrawLine;
	[SerializeField]
	private Toggle _DrawMesh;
	[SerializeField]
	private Toggle _DrawLanding;


	// 処理用変数
	private bool isOpen = false;	// デバッグUIを開いているかどうか

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// デバッグUIを取得し、非表示にする。
		for (int i = 0; i < this.transform.childCount; i++) {
			_DebugUIs.Add(this.transform.GetChild(i).gameObject);
			_DebugUIs [i].SetActive (false);
		}
		// ランダーと月面オブジェクトを取得する。
		_Lander = FindObjectOfType<LanderManager> ();
		_Moon = FindObjectOfType<MoonManager> ();
	}
	void Start () {
		// UIアイテムに現在の値を適用する。
		_StageCut.value = _Moon.BFmoonline.BFCutNum;
		_DrawLine.isOn = _Moon.BFmoonline.GetComponent<LineRenderer> ().enabled;
		_DrawMesh.isOn = _Moon.BFmoonmesh.enabled;
		_StageYMAX.value = _Moon.BFmoonline.BFMax_Y;
		_StageYMIN.value = _Moon.BFmoonline.BFMin_Y;
	}
	
	// 更新処理
	void Update () {
		InputOpen ();
		DrawYLines ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// デバッグUIを開く入力チェック
	/// </summary>
	private void InputOpen()
	{
		if (Input.GetKeyDown (KeyCode.D) && Input.GetKey (KeyCode.LeftControl)) {
			isOpen = !isOpen;

			foreach (GameObject ui in _DebugUIs) {
				ui.SetActive (isOpen);
			}
		}
	}

	private void DrawYLines()
	{
		if (isOpen == true) {

			if (_StageYMIN.value > _StageYMAX.value + 0.5f) {
				_StageYMIN.value = _StageYMAX.value - 0.5f;
			}

			float maxX = 18;
			float minX = -18;
			_YMAXLine.SetPosition (0, new Vector3 (minX, _StageYMAX.value, -1));
			_YMAXLine.SetPosition (1, new Vector3 (maxX, _StageYMAX.value, -1));
			_YMINLine.SetPosition (0, new Vector3 (minX, _StageYMIN.value, -1));
			_YMINLine.SetPosition (1, new Vector3 (maxX, _StageYMIN.value, -1));
		}
	}
	/*---------------------------------------------------------------------*/
	// ボタン処理
	public void OnGameStop()
	{
		Time.timeScale = 0;
	}
	public void OnGameStart ()
	{
		Time.timeScale = 1;
	}
	public void OnStageCreate()
	{
		_Moon.BFmoonline.BFCutNum = (int)_StageCut.value;
		_Moon.BFmoonline.BFMax_Y = _StageYMAX.value;
		_Moon.BFmoonline.BFMin_Y = _StageYMIN.value;

		_Moon.BFmoonline.Init ();
		_Moon.BFSetLandingPoint ();
		_Moon.BFmoonline.LineAndColliderUpdate ();
		_Moon.BFmoonline.CreateMesh ();

	}
	public void OnCreateLine()
	{
		_Moon.BFmoonline.BFCutNum = (int)_StageCut.value;
		_Moon.BFmoonline.BFMax_Y = _StageYMAX.value;
		_Moon.BFmoonline.BFMin_Y = _StageYMIN.value;


		_Moon.BFmoonline.Init ();
		_Moon.BFmoonline.LineAndColliderUpdate ();

		_DrawMesh.isOn = false;
		ToggleMesh ();
		_DrawLine.isOn = true;
		ToggleLine ();
		_DrawLanding.isOn = false;
		ToggleLanding ();
	}
	public void OnCreateLnading ()
	{
		_Moon.BFSetLandingPoint ();
		_Moon.BFmoonline.LineAndColliderUpdate ();

		_DrawLine.isOn = true;
		ToggleLine ();
		_DrawMesh.isOn = false;
		ToggleMesh ();
		_DrawLanding.isOn = true;
		ToggleLanding ();
	}
	public void OnCreateMesh ()
	{
		_Moon.BFmoonline.CreateMesh ();
		_DrawMesh.isOn = true;
		ToggleMesh ();
	}

	/*---------------------------------------------------------------------*/
	public void ToggleLine()
	{
		_Moon.BFmoonline.GetComponent<LineRenderer> ().enabled = _DrawLine.isOn;
	}
	public void ToggleMesh()
	{
		_Moon.BFmoonmesh.enabled = _DrawMesh.isOn;
	}
	public void ToggleLanding()
	{
		foreach (SpriteRenderer landing in _Moon.GetComponentsInChildren<SpriteRenderer> ()) {
			landing.enabled = _DrawLanding.isOn;
		}
	}
	/*---------------------------------------------------------------------*/
}
