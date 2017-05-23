using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Camera _Camera;

	[SerializeField]
	private LunderStatus _Lunder;

	private enum MODE
	{
		NORMAL = 0,
		ZOOM,
	}
	[SerializeField]
	private MODE _Mode;
	/*---------------------------------------------------------------------*/
	void Awake () {
		_Camera = this.GetComponent<Camera> ();
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		//CheckLunderALTITUDE ();
	}
	/*---------------------------------------------------------------------*/
	public void Init()
	{
		_Mode = MODE.NORMAL;

		_Camera.orthographicSize = Const.CameraData.MODE_NORMAL_SIZE;
	}
	/*---------------------------------------------------------------------*/
	private void CheckLunderALTITUDE()
	{
		if (_Mode!=MODE.ZOOM && _Lunder.GetStatus ().altitude <= Const.CameraData.ZOOM_LUDER_ALTITUDE) {
			_Mode = MODE.ZOOM;
			_Camera.orthographicSize = Const.CameraData.MODE_ZOOM_SIZE;

		}
		else if (_Mode!=MODE.NORMAL && _Lunder.GetStatus ().altitude > Const.CameraData.ZOOM_LUDER_ALTITUDE) {
			_Mode = MODE.NORMAL;
			_Camera.orthographicSize = Const.CameraData.MODE_NORMAL_SIZE;
		}
	}
}
