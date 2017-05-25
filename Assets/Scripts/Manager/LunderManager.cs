using UnityEngine;
using System.Collections;

public class LunderManager : MonoBehaviour {

	// コンポーネント
	public LunderStatus _Status;			// ステータススクリプト
	private Rigidbody2D _Rigidbody;			// Rigidbody2D
	private SpriteRenderer _SpriteRenderer; // SpriteRenderer
	private CircleCollider2D _Collider;		// CircleCollider2D

	// ゲームオブジェクト
	[SerializeField]
	private GameObject _RocketFire; // 推進ロケットの炎
	[SerializeField]
	private CameraManager _MainCamera; // カメラ
	[SerializeField]
	private GameObject _DestroyParticle; // 破壊時パーティクル

	// フラグ
	public bool isLanding = false;	// 着陸フラグ
	public bool isDestroy = false;	// 破壊フラグ
	public bool isInit = false; // 初期化完了フラグ
	[SerializeField]
	private bool isInputEnable = true; // 入力可能フラグ

	// 処理用変数
	[SerializeField]
	private float fAddRot;	// 入力されている間加算される角度
	[SerializeField]
	private float fRocketPower; // 推進ロケットが加える力
	[SerializeField]
	private float fRocketCost; // 推進ロケットで使用する燃料コスト
	private float fFuel;		// 書き込み用燃料
	public LandingPoint _LandingPoint; // 着陸した着陸地点
	/*---------------------------------------------------------------------*/
	void Awake () {
		_Status = this.GetComponent<LunderStatus> ();
		_Rigidbody = this.GetComponent<Rigidbody2D> ();
		_SpriteRenderer = this.GetComponent<SpriteRenderer> ();
		_Collider = this.GetComponent<CircleCollider2D> ();

	}
	
	// 更新処理
	void Update () {
		StatusUpdate ();
		if (isInputEnable == true) {
			InputRotation ();
			InputRocket ();
		}
		MoveLimit ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化処理　
	/// 初回の場合は引数の指定無し、ステージ移動の場合は引数に値を設定すること。
	/// </summary>
	/// <param name="fuel">燃料の値</param>
	public void Init(float fuel=Const.LunderData.INIT_FUEL)
	{
		// ステータスの初期化
		_Status.SetFuel(fuel);
		fFuel = fuel;

		// RigidBody2Dの設定
		_Rigidbody.velocity = new Vector2();
		_Rigidbody.isKinematic = true;

		// SpriteRendererの設定
		_SpriteRenderer.enabled = true;
		// 推進ロケットの炎を非表示にする。
		_RocketFire.SetActive (false);

		// 座標の初期化
		Vector2 vInitPos = new Vector2();
		vInitPos.x = Random.Range (Const.LunderData.INIT_POS_X_MIN, Const.LunderData.INIT_POS_X_MAX);
		vInitPos.y = Const.LunderData.INIT_POS_Y;
		this.transform.position = vInitPos;
		// 回転値の初期化
		this.transform.rotation = Quaternion.identity;

		// フラグの初期化
		isLanding = false;
		isDestroy = false;
		isInputEnable =	false;

		// 着陸した着陸地点をnullにする。
		_LandingPoint = null;

		// 初期化完了
		isInit = true;
	}
	/// <summary>
	/// プレイ開始処理
	/// </summary>
	public void PlayStart()
	{
		isInputEnable = true;
		_Rigidbody.isKinematic = false;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ステータスの更新処理
	/// </summary>
	private void StatusUpdate()
	{
		if (isLanding == false && isDestroy == false) {
			// 速度の更新
			_Status.SetHorizontalSpeed (_Rigidbody.velocity.x * Const.LunderData.SPEED_VALUE_RATE);
			_Status.SetVerticalSpeed (_Rigidbody.velocity.y * Const.LunderData.SPEED_VALUE_RATE);
			// 高度の取得と更新
			int layer = LayerMask.GetMask (new string[]{ "Moon" });
			Ray2D ray = new Ray2D (transform.position-new Vector3(0,_Collider.radius-_Collider.offset.y,0), Vector2.down);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin,ray.direction,float.MaxValue,layer);
			if (hit.collider != null) {
				_Status.SetAltitude (hit.distance*Const.LunderData.ALTITUDE_VALUE_RATE);
			}
			// 燃料の更新
			_Status.SetFuel(fFuel);
		}
	}
	/// <summary>
	/// 移動限界処理
	/// </summary>
	private void MoveLimit()
	{
		// 画面左端を超えた場合、画面内左端にランダーの座標を設定する。
		if (this.transform.position.x - this.transform.localScale.x <= _MainCamera.normalLeftTop.x) {
			Vector2 vec = this.transform.position;
			vec.x = _MainCamera.normalLeftTop.x + this.transform.localScale.x;
			this.transform.position = vec;
			// 水平速度のリセット
			Vector2 velocity = _Rigidbody.velocity;
			velocity.x = 0;
			_Rigidbody.velocity = velocity;
		}
		// 画面右端を超えた場合、画面内右端にランダーの座標を設定する。
		if (this.transform.position.x + this.transform.localScale.x >= _MainCamera.normalRightBottom.x) {
			Vector2 vec = this.transform.position;
			vec.x = _MainCamera.normalRightBottom.x - this.transform.localScale.x;
			this.transform.position = vec;
			// 水平速度のリセット
			Vector2 velocity = _Rigidbody.velocity;
			velocity.x = 0;
			_Rigidbody.velocity = velocity;
		}
	}
	/*---------------------------------------------------------------------*/
	// 入力処理
	/// <summary>
	/// 回転処理
	/// </summary>
	private void InputRotation()
	{
		// ←キーが押されている間　左回転
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Quaternion rot = this.transform.rotation;
			Vector3 angles = rot.eulerAngles;
			rot = Quaternion.Euler (angles.x, angles.y, angles.z + fAddRot);

			this.transform.rotation = rot;
		}
		// →キーが押されている間　右回転
		if (Input.GetKey (KeyCode.RightArrow)) {
			Quaternion rot = this.transform.rotation;
			Vector3 angles = rot.eulerAngles;
			rot = Quaternion.Euler (angles.x, angles.y, angles.z - fAddRot);

			this.transform.rotation = rot;
		}
		// ↑キーが押されたら 回転リセット
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			this.transform.rotation = Quaternion.identity;
		}
	}
	/// <summary>
	/// 推進ロケット処理
	/// </summary>
	private void InputRocket()
	{
		// ↓キーが押されたら 炎を表示
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			_RocketFire.SetActive (true);
		}
		// ↓キーが離されたら 炎を非表示
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			_RocketFire.SetActive (false);
		}
		// ↓キーが押されている間　推進ロケット処理を行う。
		if (Input.GetKey (KeyCode.DownArrow)) {
			// 燃料を消費
			fFuel = Mathf.Max (0, fFuel - fRocketCost);
			// 燃料が０になったら入力フラグをfalseにし、炎を消す。
			if (fFuel == 0) {
				isInputEnable = false;
				_RocketFire.SetActive (false);
			}
			// ランダーの向いている方向に推進ロケットの力を加える。
			_Rigidbody.AddForce (transform.up*fRocketPower);
		}
	}
	/*---------------------------------------------------------------------*/
	// 衝突処理
	void OnCollisionEnter2D(Collision2D c)
	{
		// 着陸地点に衝突した場合、着陸処理。
		if (c.gameObject.tag == "LandingPoint" ) {
			Landing (c.gameObject);
		}
		// 月面ラインに衝突した場合、ゲームオーバー処理。
		if (c.gameObject.tag == "MoonLine" && isLanding==false) {
			GameOver ();
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 着陸に成功しているかチェックする。
	/// </summary>
	/// <returns><c>true</c>着陸成功 <c>false</c> 着陸失敗</returns>
	private bool CheckLanding()
	{
		// 速度が低速かチェックする。
		if (Mathf.Abs (_Status.GetStatus ().horizontal_speed) > Const.LunderData.LANDIGN_SUCCESS_SPEED ||
			Mathf.Abs (_Status.GetStatus ().vertical_speed) > Const.LunderData.LANDIGN_SUCCESS_SPEED) {
			return false;
		}
		// 自身の回転Z値が制限範囲内かどうか
		if (this.transform.rotation.eulerAngles.z > Const.LunderData.LANDING_SUCCESS_ROTATION_Z &&
		    this.transform.rotation.eulerAngles.z < 360.0f - Const.LunderData.LANDING_SUCCESS_ROTATION_Z) {
			return false;
		}

		return true;
	}
	/// <summary>
	/// 着陸処理
	/// </summary>
	private void Landing(GameObject point)
	{
		// 推進ロケットの炎を非表示にする。
		_RocketFire.SetActive (false);
		// 着陸に成功しているかチェックする。
		isLanding = CheckLanding();
		// 入力フラグをfalseにする。
		isInputEnable = false;
		// RigidbodyのisKinematicをtrueにする。
		_Rigidbody.isKinematic = true;
		// 着陸に失敗しているならゲーム－バー処理
		if (isLanding == false) {
			GameOver ();
		}
		// 着陸に成功しているなら着陸地点を保持
		else {
			_LandingPoint = point.GetComponent<LandingPoint> ();
		}
	}
	/// <summary>
	/// 着陸失敗 ゲームオーバー処理
	/// </summary>
	private void GameOver()
	{
		// 推進ロケットの炎を非表示にする。
		_RocketFire.SetActive (false);
		// RigidbodyのisKinematicをtrueにする。
		_Rigidbody.isKinematic = true;
		// ランダーのスプライトを非表示にする。
		_SpriteRenderer.enabled = false;
		// 破壊エフェクトを表示する。
		GameObject.Instantiate(_DestroyParticle,this.transform.position,Quaternion.identity);
		// 破壊フラグをtrueにする。
		isDestroy = true;
		// 入力フラグをfalseにする。
		isInputEnable = false;
	}
	/*---------------------------------------------------------------------*/
}
