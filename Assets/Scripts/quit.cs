using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class quit : MonoBehaviour {

	public void OnClick(){
		SceneManager.LoadScene ("menu");
	}
}
