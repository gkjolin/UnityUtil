using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ValueSlider : MonoBehaviour
{
	//Slider
	private Slider slider;
	//SpriteSlider
	public GameObject spriteSlider;
	//Sliderがあるかないか
	//Text
	[SerializeField]
	private Text
		valueText;
	//ライフ表示のフォーマット
	public bool hasSlider;
	public bool hasText;
	public GameObject sliderCase;
	public int attackCount = 0;
	//private string format = "{0,4}/{1,4}";
	void Start ()
	{
		InitValueSlider ();
	}

	void OnEnable ()
	{
		InitValueSlider ();
	}

	void InitValueSlider ()
	{
		//sliderを取得
		slider = gameObject.GetComponent<Slider> ();
		//textを取得
		//		if (valueText == null && hasText == true) {
		////			valueText = gameObject.transform.FindChild ("ValueText").gameObject.GetComponent<Text> ();
		//		}
		//maxなのでSliderの値は1
		if (hasSlider == true) {
			slider.value = 0;
		} else {
			if (spriteSlider != null) {
				spriteSlider.transform.localScale = new Vector3 (1, spriteSlider.transform.localScale.y, spriteSlider.transform.localScale.z);
				//sliderCase = gameObject.transform.parent.transform.FindChild ("DamageSliderCase").gameObject;
			}
		}
	}

	public void changeValue (float currentValue, float newValue, float maxValue)
	{

		//ダメージを受ける前のライフ(sliderに反映させるために割合を計算)
		float originalSliderValue = (float)currentValue / (float)maxValue;
//		//print ("cvalue" + currentValue.ToString () + "mvalue" + currentValue.ToString () + "originalSliderValue" + originalSliderValue.ToString ());
		//ダメージを受けた後のライフ(sliderに反映させるために割合を計算)
		float newSliderValue = (float)newValue / (float)maxValue;

		//テキストのtweenを実行
		if (hasText == true) {
			valueText.text = currentValue.ToString ();
		}
		//sliderのtweenを実行
		OnSliderUpdate (newSliderValue);
	}
	//sliderのtween実行時に呼ばれるコールバックメソッド
	private void OnSliderUpdate (float value)
	{

		if (value < 0) {
			value = 0;
		}

		if (hasSlider == true) {
			slider.value = value;
		} else {
			if (spriteSlider != null) {

				spriteSlider.transform.localScale = new Vector3 (value, spriteSlider.transform.localScale.y, spriteSlider.transform.localScale.y);

				//				// (gameObject.name + spriteSlider.transform.localScale.ToString ());

			}
		}

	}
	//テキストのtween実行時に呼ばれるコールバックメソッド
	public void ActiveDamageSlider ()
	{
		attackCount += 1;
		StartCoroutine (ActiveAndFadeOutDamageSlider ());
	}

	private IEnumerator ActiveAndFadeOutDamageSlider ()
	{
		//t
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		sliderCase.GetComponent<SpriteRenderer> ().enabled = true;
		int thisAttackCount = attackCount;
		//2秒くらい待って
		yield return new WaitForSeconds (2.0f);
		//次の攻撃が来ていたら消さない
		if (thisAttackCount == attackCount) {
			UnActiveDamageSlider ();
		}


	}

	public void UnActiveDamageSlider ()
	{
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		sliderCase.GetComponent<SpriteRenderer> ().enabled = false;
	}
}