using UnityEngine;
using System.Collections;

public class settingsbutton : MonoBehaviour {

	public GameObject panel;
	public void OnClick(){
		panel.GetComponent<CanvasRenderer> ().SetAlpha (200);
		panel.SetActive (true);
	}
}
