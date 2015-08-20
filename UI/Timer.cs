using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using UnityEngine.UI;
using Koganeburogu;

public class Timer : MonoBehaviour
{
	public bool timerActiveFLG = false;
	public float limitTime;
	private float currentTimef;
	private double currentTimed;
	public Text timerText;
	private GameSequenceManager gameSequencerManagerClass;
	int minute;
	int second;
	// Use this for initialization
	public void TimerStart ()
	{
//		Debug.Log ("Timer at" + gameObject.name);
		timerActiveFLG = true;
		gameSequencerManagerClass = GameObject.FindGameObjectWithTag (TagName.GameSequencerManager).GetComponent<GameSequenceManager> ();
	}
	// Update is called once per frame
	void Update ()
	{
		if (timerActiveFLG == true) {
			limitTime = limitTime - Time.deltaTime;
			currentTimed = System.Convert.ToDouble (limitTime);
			currentTimed = System.Math.Round (currentTimed, 0, MidpointRounding.AwayFromZero);
			minute = (int)(currentTimed + 1) / 60;
			second = (int)(currentTimed + 1) % 60;
			timerText.text = minute.ToString () + "分" + second.ToString () + "秒";
			if (currentTimed < 0) {
				//0を突っ込む
				timerText.text = 0.ToString () + "秒";

				//ゲーム終了
				gameSequencerManagerClass.GameEnd ();
			}
		}
	}

	public void StopTimer ()
	{
		timerActiveFLG = false;
	}

	public void RestartTimer ()
	{
		timerActiveFLG = true;
	}
}
