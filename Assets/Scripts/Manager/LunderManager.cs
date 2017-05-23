using UnityEngine;
using System.Collections;

public class LunderManager : MonoBehaviour {

	private LunderStatus _Status;
	private Rigidbody2D _Rigidbody;
	private SpriteRenderer _SpriteRenderer;
	private CircleCollider2D _Collider;

	[SerializeField]
	private GameObject _RocketFire;

	public bool isLanding = false;
	public bool isDestroy = false;
	public bool isInputEnable = true;

	[SerializeField]
	private float fAddRot;	// 入力されている間加算される角度
	[SerializeField]
	private float fRocketPower;
	[SerializeField]
	private float fRocketCost;
	private float fFuel;
	/*---------------------------------------------------------------------*/
	void Awake () {
		_Status = this.GetComponent<LunderStatus> ();
		_Rigidbody = this.GetComponent<Rigidbody2D> ();
		_SpriteRenderer = this.GetComponent<SpriteRenderer> ();
		_Collider = this.GetComponent<CircleCollider2D> ();

		Init ();
	}
	
	// 更新処理
	void Update () {
		StatusUpdate ();
		if (isInputEnable == true) {
			InputRotation ();
			InputRocket ();
		}
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
		LunderStatus.STATUS newStatus = new LunderStatus.STATUS ();
		newStatus.fuel = fuel; 
		_Status.SetStatus (newStatus);
		fFuel = fuel;

		// RigidBody2Dの設定
		_Rigidbody.velocity = new Vector2();
		_Rigidbody.isKinematic = true;

		// SpriteRendererの設定
		_SpriteRenderer.enabled = true;

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
		isInputEnable = true;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ステータスの更新処理
	/// </summary>
	private void StatusUpdate()
	{
		if (isLanding == false && isDestroy == false) {
			// 速度の更新
			_Status.SetHorizontalSpeed (_Rigidbody.velocity.x * 200);
			_Status.SetVerticalSpeed (_Rigidbody.velocity.y * 200);
			// 高度の取得と更新
			int layer = LayerMask.GetMask (new string[]{ "Moon" });
			Ray2D ray = new Ray2D (transform.position-new Vector3(0,_Collider.radius-_Collider.offset.y,0), Vector2.down);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin,ray.direction,10.0f,layer);
			if (hit.collider != null) {
				_Status.SetAltitude (hit.distance*100);
			}
			// 燃料の更新
			_Status.SetFuel(fFuel);
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
			// ロケットの向いている方向にロケットの力を加える。
			_Rigidbody.AddForce (transform.up*fRocketPower);
		}
	}
	/*---------------------------------------------------------------------*/
	// 衝突処理
	void OnCollisionEnter2D(Collision2D c)
	{
		// 着陸地点に衝突した場合、着陸処理。
		if (c.gameObject.tag == "LandingPoint") {
			Landing ();
		}
		// 月面ラインに衝突した場合、ゲームオーバー処理。
		if (c.gameObject.tag == "MoonLine" && isLanding==false) {
			GameOver ();
		}
	}
	/*---------------------------------------------------------------------*/
	private bool CheckLanding()
	{
		// 速度が低速かチェックする。
		// 自身の回転Z値が０かどうか

		return true;
	}
	private void Landing()
	{
		isLanding = CheckLanding();
		isInputEnable = false;
		_Rigidbody.isKinematic = true;
		if (isLanding == false) {
			GameOver ();
		}
	}
	private void GameOver()
	{
		_Rigidbody.isKinematic = true;
		_SpriteRenderer.enabled = false;
		isDestroy = true;
		isInputEnable = false;
	}
	/*---------------------------------------------------------------------*/
}
