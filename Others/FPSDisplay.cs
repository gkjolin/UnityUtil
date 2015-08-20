using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
	float deltaTime = 0.0f;
	private Text displayText;

	void Start ()
	{

		QualitySettings.vSyncCount = 0; // VSyncをOFFにする
		Application.targetFrameRate = 60;
		displayText = gameObject.GetComponent<Text> ();
	}

	void Update ()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format ("{0:0.0} ms ({1:0.} fps)", msec, fps);
		displayText.text = text;
	}
}