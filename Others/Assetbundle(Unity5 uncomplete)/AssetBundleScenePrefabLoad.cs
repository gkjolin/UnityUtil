using UnityEngine;
using System.Collections;

public class AssetBundleScenePrefabLoad: MonoBehaviour
{
	public string assetbundle;
	public string assetName;
	public GameObject scene;

	IEnumerator Start ()
	{
		Debug.Log ("スタート");
		//AssetManagerの準備が出来るまで待機
		while (!AssetManager.instance.isReady)
			yield return null;

		//シーンのバンドルをロード
		AssetManager.instance.LoadBundle (assetbundle);

		//バンドルのロードが終わるまで待機
		while (!AssetManager.instance.IsBundleLoaded (assetbundle))
			yield return null;


		//prefabをロード
		scene = AssetManager.instance.GetAssetFromBundle (assetbundle, assetName) as GameObject;
		Instantiate (scene);
	}
}
