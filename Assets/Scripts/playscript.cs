using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class playscript : MonoBehaviour {

	public void OnClick(){
		Settings.blackPoints = 0;
		Settings.whitePoints = 0;
		SceneManager.LoadScene ("gamescene");
	}
}
