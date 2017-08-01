using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UserAccountUI : MonoBehaviour {

	[SerializeField]
	protected Image _BackBord;
	[SerializeField]
	protected Text _TitleName;
	[SerializeField]
	protected Image _WaitImg;

	public virtual void Init (){
		
	}

	public virtual void SetActive (bool value){}


}
