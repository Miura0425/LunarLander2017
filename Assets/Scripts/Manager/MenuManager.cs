using UnityEngine;
using System.Collections;

// メニューアイテムID
public enum MENU_ITEM_ID
{
	START =0,
	HOWTOPLAY,
	USERPAGE,
	OPTION,
	EXIT,
}

public class MenuManager : MonoBehaviour {

	private int nSelectIdx = 0;	// 選択中インデックス
	private MenuItem[] _Items;	// メニューアイテムリスト

	[SerializeField]
	private HowToMsg _howto;	// ゲーム説明UIオブジェクト
	private bool isHowto;	// ゲーム説明中フラグ
	[SerializeField]
	private OptionMenu _option;	// オプションUIオブジェクト


	private bool isInputEnable ;	// 入力可能フラグ

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// アイテム取得
		_Items = this.GetComponentsInChildren<MenuItem> ();
	}
	void Start()
	{
		// フラグ初期化
		isInputEnable = false;
		isHowto = false;
	}
	
	// 更新処理
	void Update () {
		
		InputSelect ();
		InputHowto ();

		// 入力制限中 かつ オプションUI非表示なら 入力可能にする。
		if (isInputEnable == false && _option.isEnable == false) {
			isInputEnable = true;
		}
	}

	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 初期化
	/// </summary>
	public void Init()
	{
		// アイテムを表示する。
		foreach (MenuItem item in _Items) {
			item.SetEnable (true);
		}
		// インデックスを初期化
		nSelectIdx = 0;
		// 初期位置のアイテムを選択中にする。
		_Items [nSelectIdx].Select ();
		// 入力可能にする。
		isInputEnable = true;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 入力処理
	/// </summary>
	private void InputSelect()
	{
		// 入力制限中は処理しない
		if (isInputEnable == false) {
			return;
		}
		// ↓キーが押されたら次のアイテムを選択中にする。
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			_Items [nSelectIdx].NotSelect ();
			nSelectIdx = (nSelectIdx + 1) % _Items.Length;
			_Items [nSelectIdx].Select ();
		}
		// ↑キーが押されてあら前のアイテムを選択中にする。
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			_Items [nSelectIdx].NotSelect ();
			nSelectIdx = (nSelectIdx + _Items.Length-1)%_Items.Length;
			_Items [nSelectIdx].Select ();
		}
		// エンターキーが押されたら選択中のアイテムの処理を行う。
		if (Input.GetKeyDown (KeyCode.Return)) {
			// アイテムのIDを取得
			MENU_ITEM_ID _id = _Items [nSelectIdx].Dicide ();

			// IDによって処理を実行。
			switch (_id) {
			case MENU_ITEM_ID.START: // メインゲームへ遷移
				cGameManager.Instance.ChangeScene(GAME_SCENE.MAINGAME);
				break;
			case MENU_ITEM_ID.HOWTOPLAY: // ゲーム説明を開く
				_howto.HowToStart ();
				isHowto = true;
				isInputEnable = false;
				break;
			case MENU_ITEM_ID.OPTION: // オプションを開く
				_option.SetEnable (true);
				isInputEnable = false;
				break;
			case MENU_ITEM_ID.EXIT: // ゲームを終了する。
				Application.Quit ();
				break;
			default:
				break;
			}
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// ゲーム説明中の 入力処理
	/// </summary>
	private void InputHowto()
	{
		if (isHowto == false) {
			return;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (!_howto.NextMsg ()) {
				isHowto = false;
				isInputEnable = true;
			}
		}
	}
	/*---------------------------------------------------------------------*/
}
