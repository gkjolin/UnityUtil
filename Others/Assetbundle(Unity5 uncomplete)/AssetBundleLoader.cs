using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AssetBundleLoader : MonoBehaviour
{
	public Text loadingText;
	public float progresss;
	public GameObject progressBar;

	IEnumerator Start ()
	{
		//やらない
		yield return StartCoroutine (LoadCoroutine ("cocbundle"));
		// ("battlescene load done");


		yield return new WaitForSeconds (1.5f);
		//GameObject scene = AssetManager.instance.GetAssetFromBundle ("cocbundle", "StageSelectScenePrefab") as GameObject;
		Application.LoadLevel ("StageSelectScene");
	}

	IEnumerator LoadCoroutine (string bundleName)
	{
		// ("bundleName" + bundleName);
		//AssetManagerの準備が出来るまで待機
		while (!AssetManager.instance.isReady)
			yield return null;

		//シーンのバンドルをロード
		AssetManager.instance.LoadBundle (bundleName);

		//バンドルのロードが終わるまで待機
		while (!AssetManager.instance.IsBundleLoaded (bundleName)) {
			loadingText.text = "ロード中";

			progressBar.transform.localScale = new Vector3 (progressBar.transform.localScale.x + 0.0005f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
				
			yield return null;
		}
		progressBar.transform.localScale = new Vector3 (1, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
	}

	public void MoveToBattleScene ()
	{
		Application.LoadLevel ("BattleScene");
	}
}
