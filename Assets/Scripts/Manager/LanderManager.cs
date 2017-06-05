using UnityEngine;
using System.Collections;

public class LanderManager : MonoBehaviour {

	// コンポーネント
	public LanderStatus _Status;			// ステータススクリプト
	private Rigidbody2D _Rigidbody;			// Rigidbody2D
	[SerializeField]
	private SpriteRenderer _SpriteRenderer; // SpriteRenderer
	[SerializeField]
	private BoxCollider2D _Foot; // Foot BoxCollider

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
	private bool isRocket = false;	// 推進ロケットフラグ
	private bool isFuelAlert = false; // 燃料警報フラグ
	private bool isSuccess = false; // 着陸可能フラグ

	// 処理用変数
	[SerializeField]
	private float fAddRot;	// 入力されている間加算される角度
	[SerializeField]
	private float fRocketPower; // 推進ロケットが加える力
	[SerializeField]
	private float fRocketCost; // 推進ロケットで使用する燃料コスト
	private float fFuel;		// 書き込み用燃料
	[SerializeField]
	private float fAlertTime; // 燃料警報の点滅時間
	private float fAlertStartTIme; // 燃料警報開始時間

	[SerializeField]
	private Color cSuccess; // 着陸できる時の色

	public LandingPoint _LandingPoint; // 着陸した着陸地点

	private int layer;	// ray用のレイヤーマスク
	private Ray2D ray = new Ray2D();
	/*---------------------------------------------------------------------*/
	void Awake () {
		_Status = this.GetComponent<LanderStatus> ();
		_Rigidbody = this.GetComponent<Rigidbody2D> ();

	}
	void Start()
	{
		// ray の初期化
		layer = LayerMask.GetMask (new string[]{ "Moon" });
		ray.direction = Vector2.down;
	}
	
	// 更新処理
	void Update () {
		StatusUpdate ();
		if (isInputEnable == true) {
			InputRotation ();
			InputRocket ();
		}
		MoveLimit ();
		CheckLanding ();
		FuelAlert ();
	}
	void FixedUpdate()
	{
		Rocket ();
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化処理　
	/// 初回の場合は引数の指定無し、ステージ移動の場合は引数に値を設定すること。
	/// </summary>
	/// <param name="fuel">燃料の値</param>
	public void Init(float fuel=Const.LanderData.INIT_FUEL)
	{
		// ステータスの初期化
		_Status.SetFuel(fuel);
		fFuel = fuel;

		// RigidBody2Dの設定
		_Rigidbody.velocity = new Vector2();
		_Rigidbody.isKinematic = true;

		// SpriteRendererの設定
		_SpriteRenderer.gameObject.SetActive(true);
		// 推進ロケットの炎を非表示にする。
		_RocketFire.SetActive (false);

		// 座標の初期化
		Vector2 vInitPos = new Vector2();
		vInitPos.x = Random.Range (Const.LanderData.INIT_POS_X_MIN, Const.LanderData.INIT_POS_X_MAX);
		vInitPos.y = Const.LanderData.INIT_POS_Y;
		this.transform.position = vInitPos;
		// 回転値の初期化
		this.transform.rotation = Quaternion.identity;

		// フラグの初期化
		isLanding = false;
		isDestroy = false;
		isInputEnable =	false;
		isRocket = false;
		isFuelAlert = false;
		_SpriteRenderer.color = Color.white;

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
			_Status.SetHorizontalSpeed (_Rigidbody.velocity.x * Const.LanderData.SPEED_VALUE_RATE);
			_Status.SetVerticalSpeed (_Rigidbody.velocity.y * Const.LanderData.SPEED_VALUE_RATE);
			// 高度の取得と更新
			ray.origin = (Vector2)_Foot.transform.position + new Vector2(0,-_Foot.size.y);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin,ray.direction,float.MaxValue,layer);
			if (hit.collider != null) {
				_Status.SetAltitude (hit.distance*Const.LanderData.ALTITUDE_VALUE_RATE);
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
			isRocket = true;
			_RocketFire.SetActive (true);
		}
		// ↓キーが離されたら 炎を非表示
		if (Input.GetKeyUp (KeyCode.DownArrow)) {
			isRocket = false;
			_RocketFire.SetActive (false);
		}
		// ↓キーが押されている間　推進ロケット処理を行う。
		if (isRocket == true) {
			// 燃料を消費
			fFuel = Mathf.Max (0, fFuel - fRocketCost);
			// 燃料が０になったら入力フラグをfalseにし、炎を消す。
			if (fFuel == 0) {
				isInputEnable = false;
				isRocket = false;
				_RocketFire.SetActive (false);
			}
		}
	}
	/// <summary>
	/// 残り燃料が少なくなったら赤く点滅する
	/// </summary>
	private void FuelAlert()
	{
		float fuel = _Status.GetStatus ().fuel;
		Color baseColor = _SpriteRenderer.color;
		if (isFuelAlert == false && fuel <= Const.LanderData.ALERT_FUEL) {
			isFuelAlert = true;
			fAlertStartTIme = Time.timeSinceLevelLoad;

		}

		if (isFuelAlert == true) {
			
			float diff = Time.timeSinceLevelLoad - fAlertStartTIme;
			if (diff >= fAlertTime*2) {
				fAlertStartTIme = Time.timeSinceLevelLoad;
			}
			float rate = diff / (fAlertTime*2) ;
			Color _color = _SpriteRenderer.color;
			if (diff < fAlertTime){
				_color.r = Mathf.Lerp (baseColor.r, Color.red.r, rate);
				_color.g = Mathf.Lerp(baseColor.g,Color.red.g,rate);
				_color.b = Mathf.Lerp(baseColor.b,Color.red.b,rate);
			} else {
				_color.r = Mathf.Lerp (Color.red.r, baseColor.r, rate);
				_color.g = Mathf.Lerp (Color.red.g, baseColor.g, rate);
				_color.b = Mathf.Lerp (Color.red.b, baseColor.b, rate);
			}
			_SpriteRenderer.color = _color;
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ロケットの力を加える処理
	/// </summary>
	private void Rocket()
	{
		if (isRocket == true) {
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
	/// 着陸できる状態かチェックする。
	/// </summary>
	/// <returns><c>true</c>着陸成功 <c>false</c> 着陸失敗</returns>
	private void CheckLanding()
	{
		isSuccess = true;
		// 速度が低速かチェックする。
		if (Mathf.Abs (_Status.GetStatus ().horizontal_speed) > Const.LanderData.LANDIGN_SUCCESS_SPEED ||
			Mathf.Abs (_Status.GetStatus ().vertical_speed) > Const.LanderData.LANDIGN_SUCCESS_SPEED) {
			isSuccess = false;
		}
		// 自身の回転Z値が制限範囲内かどうか
		if (this.transform.rotation.eulerAngles.z > Const.LanderData.LANDING_SUCCESS_ROTATION_Z &&
		    this.transform.rotation.eulerAngles.z < 360.0f - Const.LanderData.LANDING_SUCCESS_ROTATION_Z) {
			isSuccess = false;
		}
		// Footが着陸地点の範囲に入っているかRayで判定する。
		// Footの左からRayを飛ばす
		ray.origin = new Vector2 (_Foot.transform.position.x - _Foot.size.x / 2, _Foot.transform.position.y - _Foot.size.y/2);
		RaycastHit2D hitleft = Physics2D.Raycast (ray.origin,ray.direction,float.MaxValue,layer);
		// Fottの右からRayを飛ばす
		ray.origin = new Vector2 (_Foot.transform.position.x + _Foot.size.x / 2, _Foot.transform.position.y - _Foot.size.y/2);
		RaycastHit2D hitright = Physics2D.Raycast (ray.origin, ray.direction, float.MaxValue, layer);
		// 得られたオブジェクトの左右どちらかが着陸地点ではない場合、フラグをFalseにする。
		if (hitleft.collider != null && hitright.collider != null) {
			if (hitleft.collider != null && hitleft.collider.tag != "LandingPoint") {
				isSuccess = false;
			}
			if (hitright.collider != null && hitright.collider.tag != "LandingPoint") {
				isSuccess = false;
			}
		}
		// なにもオブジェクトを得られていない場合もフラグをFalseにする。
		else {
			isSuccess = false;
		}

		// 着陸可能な状態ならランダーの色を設定色にする。
		if (isSuccess == true) {
			_SpriteRenderer.color = cSuccess;
		} else {
			_SpriteRenderer.color = Color.white;
		}
	}
	/// <summary>
	/// 着陸処理
	/// </summary>
	private void Landing(GameObject point)
	{
		// 推進ロケットの炎を非表示にする。
		_RocketFire.SetActive (false);
		// 着陸に成功しているかチェックする。
		isLanding = isSuccess;
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
		_SpriteRenderer.gameObject.SetActive(false);
		// 破壊エフェクトを表示する。
		GameObject.Instantiate(_DestroyParticle,this.transform.position,Quaternion.identity);
		// 破壊フラグをtrueにする。
		isDestroy = true;
		// 入力フラグをfalseにする。
		isInputEnable = false;
	}
	/*---------------------------------------------------------------------*/
}
