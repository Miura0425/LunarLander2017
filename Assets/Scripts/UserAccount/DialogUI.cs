using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour {

	[SerializeField]
	protected Image _BackBord;
	[SerializeField]
	protected Text _TitleName;
	[SerializeField]
	protected Image _WaitImg;

	public virtual void Init (){
		
	}

	public virtual void SetActive (bool value){}

	public virtual void SetTitle(string title)
	{
		_TitleName.text = title;
	}

}
