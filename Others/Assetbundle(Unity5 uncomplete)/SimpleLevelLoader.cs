using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SimpleLevelLoader : MonoBehaviour
{
	[SerializeField] string path;
	[SerializeField] string sceneName;
	[SerializeField] Text text;

	IEnumerator Start ()
	{
		//download something from internet
		using (WWW www = WWW.LoadFromCacheOrDownload (path, 0)) {
			//このyield文でダウンロードが終わるまで待つという意味になる
			yield return www;

			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				text.text = www.error;
				yield break;
			}

			Application.LoadLevel (sceneName);

			//シーンのロードが終わるまで待つ
			yield return null;

			//assetbundleをアンロード
			www.assetBundle.Unload (false);
		}
	
	}
}
