using UnityEngine;
using System.Collections;

public enum MENU_ITEM_ID
{
	START =0,
	HOWTOPLAY,
	OPTION,
	EXIT,
}

public class MenuManager : MonoBehaviour {
	
	private int nSelectIdx = 0;
	private MenuItem[] _Items;

	[SerializeField]
	private HowToMsg _howto;
	private bool isHowto;
	[SerializeField]
	private OptionMenu _option;

	private bool isInputEnable ;

	void Awake()
	{
		_Items = this.GetComponentsInChildren<MenuItem> ();
	}
	void Start()
	{
		isInputEnable = false;
		isHowto = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		InputSelect ();
		InputHowto ();

		if (isInputEnable == false && _option.isEnable == false) {
			isInputEnable = true;
		}
	}

	public void Init()
	{
		foreach (MenuItem item in _Items) {
			item.SetEnable (true);
		}
		nSelectIdx = 0;
		_Items [nSelectIdx].Select ();
		isInputEnable = true;
	}

	private void InputSelect()
	{
		if (isInputEnable == false) {
			return;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			_Items [nSelectIdx].NotSelect ();
			nSelectIdx = (nSelectIdx + 1) % _Items.Length;
			_Items [nSelectIdx].Select ();
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			_Items [nSelectIdx].NotSelect ();
			nSelectIdx = (nSelectIdx + _Items.Length-1)%_Items.Length;
			_Items [nSelectIdx].Select ();
		}
		if (Input.GetKeyDown (KeyCode.Return)) {
			MENU_ITEM_ID _id = _Items [nSelectIdx].Dicide ();

			switch (_id) {
			case MENU_ITEM_ID.START:
				TransitionManager.Instance.ChangeScene(GAME_SCENE.MAINGAME);
				break;
			case MENU_ITEM_ID.HOWTOPLAY:
				_howto.HowToStart ();
				isHowto = true;
				isInputEnable = false;
				break;
			case MENU_ITEM_ID.OPTION:
				_option.SetEnable (true);
				isInputEnable = false;
				break;
			case MENU_ITEM_ID.EXIT:
				Application.Quit ();
				break;
			default:
				break;
			}
		}
	}
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
}
