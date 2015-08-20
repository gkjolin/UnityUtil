using UnityEngine;
using System.Collections;

//public class LoadAsset : MonoBehaviour
//{
//	public string assetbundle;
//	public string assetName;
//	public GameObject player;
//
//	IEnumerator Start ()
//	{
//		Debug.Log ("スタート");
//		//AssetManagerの準備が出来るまで待機
//		while (!AssetManager.instance.isReady)
//			yield return null;
//
//		//シーンのバンドルをロード
//		AssetManager.instance.LoadBundle (assetbundle);
//
//		//バンドルのロードが終わるまで待機
//		while (!AssetManager.instance.IsBundleLoaded (assetbundle))
//			yield return null;
//
//
//		//prefabをロード
//		player = AssetManager.instance.GetAssetFromBundle (assetbundle, assetName) as GameObject;
//	}
//
//	public void makeObj ()
//	{
//		Instantiate (player, new Vector3 (0, 0, 0), Quaternion.identity);
//	}
//}
