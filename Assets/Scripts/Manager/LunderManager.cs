using UnityEngine;
using System.Collections;

public class LunderManager : MonoBehaviour {

	private LunderStatus _Status;
	private Rigidbody2D _Rigidbody;
	private SpriteRenderer _SpriteRenderer;

	public bool isLanding = false;
	public bool isDestroy = false;

	[SerializeField]
	private float fAddRot;	// 入力されている間加算される角度
	[SerializeField]
	private float fRocketPower;
	/*---------------------------------------------------------------------*/
	void Awake () {
		_Status = this.GetComponent<LunderStatus> ();
		_Rigidbody = this.GetComponent<Rigidbody2D> ();
		_SpriteRenderer = this.GetComponent<SpriteRenderer> ();

		Init ();
	}
	
	// 更新処理
	void Update () {
		InputRotation ();
		InputRocket ();
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

		// RigidBody2Dの設定
		_Rigidbody.velocity = new Vector2();
		_Rigidbody.isKinematic = true;

		// SpriteRendererの設定
		_SpriteRenderer.enabled = true;

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

	}
	/*---------------------------------------------------------------------*/
	// 入力処理
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

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			this.transform.rotation = Quaternion.identity;
		}
	}
	private void InputRocket()
	{
		if (Input.GetKey (KeyCode.DownArrow)) {
			Debug.Log (transform.up);
			_Rigidbody.AddForce (transform.up*fRocketPower);
		}
	}
	/*---------------------------------------------------------------------*/
	// 衝突処理
	void OnCollisionEnter2D(Collision2D c)
	{
		if (c.gameObject.tag == "LandingPoint") {
			Landing ();
			Debug.Log ("LandingPoint");
		}
		if (c.gameObject.tag == "MoonLine" && isLanding==false) {
			GameOver ();
			Debug.Log ("MoonLine");
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
	}
	/*---------------------------------------------------------------------*/
}
