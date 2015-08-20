using UnityEngine;
using System.Collections;

public class SpriteFade : MonoBehaviour
{
	public bool m_startWithFadeIn = false;
	public bool m_startWithFadeOut = false;
	public float m_fadeTime;
	private SpriteRenderer m_Sprite;
	private float alpha;

	void Awake ()
	{
		m_Sprite = GetComponent<SpriteRenderer> ();

	}

	void Start ()
	{
		if (m_startWithFadeIn) {
			StartCoroutine ("FadeIn", m_fadeTime);
		}
		if (m_startWithFadeOut) {
			StartCoroutine ("FadeOut", m_fadeTime);
		}
	}

	public IEnumerator FadeIn (float time)
	{
		while (m_Sprite.color.a < 1) {
			alpha += Time.deltaTime / time;
			m_Sprite.color = new Color (1, 1, 1, alpha);
			yield return null;
		}
	}

	public IEnumerator FadeOut (float time)
	{
		print ("call fade out");
		float currentAlpha = m_Sprite.color.a;
		alpha = currentAlpha;
		while (m_Sprite.color.a > 0) {

			alpha -= (currentAlpha * Time.deltaTime) / time;
			print (alpha.ToString ());
			m_Sprite.color = new Color (1, 1, 1, alpha);
			yield return null;
		}
	}
}
