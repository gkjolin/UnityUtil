using UnityEngine;
using System.Collections;

public class LevelAndManifestLoader : MonoBehaviour
{
	[SerializeField] string path;
	[SerializeField] string bundleName;
	[SerializeField] string manifestName;
	[SerializeField] string sceneName;
	AssetBundleManifest manifest;

	IEnumerator Start ()
	{
		//manifestをロード
		using (WWW www = new WWW (path + manifestName)) {
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				yield break;
			}

			manifest = (AssetBundleManifest)www.assetBundle.LoadAsset ("AssetBundleManifest");
			yield return null;
			www.assetBundle.Unload (false);
		}

		//sceneをロード
		using (WWW www = WWW.LoadFromCacheOrDownload (path + bundleName, manifest.GetAssetBundleHash (bundleName))) {
			yield return www;
			if (!string.IsNullOrEmpty (www.error)) {
				Debug.Log (www.error);
				yield break;
			}

			Application.LoadLevel (sceneName);
			yield return null;

			www.assetBundle.Unload (false);
		}
	}
}