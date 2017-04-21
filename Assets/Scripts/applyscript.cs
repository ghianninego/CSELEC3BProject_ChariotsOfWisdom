using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
public class applyscript : MonoBehaviour {
	public Dropdown cam;
	public Dropdown game;
	public void OnClick(){
		Settings.camSetting = cam.value;
		Settings.numOfGames = Int32.Parse (game.options [game.value].text);
		this.gameObject.transform.parent.gameObject.SetActive (false);
	}
}
