using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Endless
{
	public class  BlinkSprite: MonoBehaviour
	{
		public int ZODIAC_NUMBER = 0;
		private ZodiacBuyControl m_zodiacBuyControl;
		private SpriteRenderer m_image;
		private bool fadeflg = false;
		private float time = 2.4f;
		public float interval = 0.2f;
		private int count;
		private bool fadeIn = true;
		private bool fadeOut = false;
		private float blinkSpeed = 0.3f;
		private float val;
		// 点滅周期
		void Awake ()
		{
			//zodiacBuyControl
			m_zodiacBuyControl = GameObject.Find ("HomeFrontCanvas").GetComponent<ZodiacBuyControl> ();
			m_image = transform.GetComponent<SpriteRenderer> ();

		}
		// Update is called once per frame
		void Update ()
		{
		
			if (m_zodiacBuyControl.CheckBoughtZodiac (ZODIAC_NUMBER)) {
			
				//買った
				Color tmp = new Color (1, 1, 1, 1);
				m_image.color = tmp;
			} else {
				//買える
				if (m_zodiacBuyControl.CheckCanBuyZodiac (ZODIAC_NUMBER)) {
					if (fadeIn) {
						val += Time.deltaTime * blinkSpeed;
						if (val > 0.4f) {
							m_image.color = new Color (1, 1, 1, val);
							fadeIn = false;
							fadeOut = true;
						}
					} else if (fadeOut) {
						val -= Time.deltaTime * blinkSpeed;
						if (val <= 0) {
							m_image.color = new Color (1, 1, 1, val);
							fadeIn = true;
							fadeOut = false;
						}
					}



				} else {

					//買えない
					Color tmp = m_image.color;
					tmp.a = 0.1f;
					m_image.color = tmp;
				}
			}
			
		}
	}
}