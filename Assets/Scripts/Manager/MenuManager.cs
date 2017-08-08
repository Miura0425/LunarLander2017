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

	public int nSelectIdx = 0;	// 選択中インデックス
	private MenuItem[] _Items;	// メニューアイテムリスト

	[SerializeField]
	private HowToMsg _howto=null;	// ゲーム説明UIオブジェクト
	[SerializeField]
	private OptionMenu _option = null;	// オプションUIオブジェクト
	[SerializeField]
	private UserPage _userpage = null ; // ユーザーページUIオブジェクト

	private bool isInputEnable = false;	// 入力可能フラグ
	private bool isShow =false; // 表示状態フラグ

	/*---------------------------------------------------------------------*/
	void Awake()
	{
		// アイテム取得
		_Items = this.GetComponentsInChildren<MenuItem> ();
		for (int i=0;i<_Items.Length;i++) {
			_Items[i].transform.localPosition = new Vector2(0,i*-30);
			_Items[i]._MenuManger = this;
		}
	}
	void Start()
	{
		// フラグ初期化
		isInputEnable = false;
		isShow = false;
	}
	
	// 更新処理
	void Update () {
		// 非表示状態 又は 操作説明 ユーザーページが表示されているなら処理しない。
		if (!isShow || _howto.isShow || _userpage.isShow)
			return;
		
		// キー入力処理
		InputKeySelect ();

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
		isShow = true;
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// キー入力処理
	/// </summary>
	private void InputKeySelect()
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
			SelectMenu (_Items [nSelectIdx].ID);
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// 選択中以外のメニュー項目を非選択状態にする。
	/// </summary>
	/// <param name="_id">Identifier.</param>
	public void AnotherMenuNotSelect(MENU_ITEM_ID _id)
	{
		foreach(var item in _Items)
		{
			if(_id != item.ID)
			{
				item.NotSelect ();
			}
		}
	}
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// メニューの選択実行
	/// </summary>
	/// <param name="_id">Identifier.</param>
	private void SelectMenu(MENU_ITEM_ID _id)
	{
		// IDによって処理を実行。
		switch (_id) {
		case MENU_ITEM_ID.START: // メインゲームへ遷移
			cGameManager.Instance.ChangeScene(GAME_SCENE.MAINGAME);
			break;
		case MENU_ITEM_ID.HOWTOPLAY: // ゲーム説明を開く
			_howto.HowToStart ();
			isInputEnable = false;
			break;
		case MENU_ITEM_ID.USERPAGE:// ユーザーページを開く
			if (cGameManager.Instance.UserData.IsLogin) {
				_userpage.OpenPage ();
				isInputEnable = false;
			}
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
	/*---------------------------------------------------------------------*/
	/// <summary>
	/// メニューをクリックしたときの処理
	/// </summary>
	/// <param name="item">Item.</param>
	public void OnClickMenu(MenuItem item)
	{
		if (!isInputEnable) return;
		SelectMenu (item.ID);
	}
	/*---------------------------------------------------------------------*/
}
