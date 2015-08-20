using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	[System.Serializable]
	public struct Prefab
	{
		public GameObject prefab;
		public int amount;
	}

	public bool isReady;
	public Prefab[] prefabs;

	public IEnumerator MakePrefabs ()
	{
		isReady = false;
		List<GameObject> objs = new List<GameObject> ();
		for (int i = 0; i <= prefabs.Length - 1; i++) {
			for (int a = 0; a <= prefabs [i].amount - 1; a++) {
				//普通に生成
				GameObject obj = GetGameObject (prefabs [i].prefab, Vector3.zero, Quaternion.identity);
				//Debug用
				obj.name += a.ToString ();
				//親に設定
				obj.transform.parent = gameObject.transform;
				//配列に格納
				objs.Add (obj);

//				print ("make" + obj.name);
			}
			foreach (GameObject obj in objs) {
				obj.SetActive (false);
			}
			yield return null;
		}
		isReady = true;
	}

	private static ObjectPool _instance;
	//シングルトン
	public static ObjectPool instance {
		get {
			//_instanceにObjectPoolが入っていなければ
			if (_instance == null) {
				//シーン上から取得する
				_instance = FindObjectOfType<ObjectPool> ();

				//シーン上にも無ければGameObjectを作成してObjectPoolをアタッチする
				if (_instance == null) {
					_instance = new GameObject ("ObjectPool").AddComponent<ObjectPool> ();
				}
			}
			return _instance;
		}
	}
	//ゲームオブジェクトのDictionary(ゲームオブジェクトごとのListを保持)
	public Dictionary<int,List<GameObject>> pooledGameObjects = new Dictionary<int,List<GameObject>> ();

	public GameObject GetGameObject (GameObject prefab, Vector3 position, Quaternion rotation)
	{
//		// ("prefab" + prefab.name + "position" + position.ToString ());
		//まず取得したいGameObjectのプレハブのインスタンスIDを取得
		int key = prefab.GetInstanceID ();

		//Dictionaryに取得したkeyが存在しなければ新たにList<GameObject>を作成してDictionaryに追加
		if (pooledGameObjects.ContainsKey (key) == false) {
			pooledGameObjects.Add (key, new List<GameObject> ());
		}

		//取得したGameObjectのprefabを取得
		List<GameObject> gameObjects = pooledGameObjects [key];

		//返すゲームオブジェクト
		GameObject go = null;

		//ある場合
		//使えるオブジェクトがないかListの中身を走査
		for (int i = 0; i < gameObjects.Count; i++) {
			//とりあえず上からgameobjectを調べていく
			go = gameObjects [i];

			//現在非アクティブ(未使用)であれば
			if (go.activeInHierarchy == false) {

				//位置を設定
				go.transform.position = position;

				//角度を設定
				go.transform.rotation = rotation;


				//これから使うのでactiveにする
				go.SetActive (true);

//				// ("recycle object");
				//ゲームオブジェクトを返す
				return go;
			}
		}

		//ない場合
		//使用できるものがないので新たに生成
		////////print ("Instantiate" + prefab.name + "!!!!");
		go = (GameObject)Instantiate (prefab, position, rotation);
//		//print ("Instantiate" + go.name + "!!!!");

		//ObjectPoolゲームオブジェクトの子要素にする
		go.transform.parent = transform;

		//作ったオブジェクトをListに追加
		gameObjects.Add (go);

//		// ("instantiate object");
		//作ったゲームオブジェクトを返す
		return go;
	}

	public GameObject GetGameObjectForBullet (GameObject prefab, Vector2 position, Quaternion rotation, GameObject target)
	{
//		// ("prefab" + prefab.name + "position" + position.ToString ());
		//まず取得したいGameObjectのプレハブのインスタンスIDを取得
		int key = prefab.GetInstanceID ();

		//Dictionaryに取得したkeyが存在しなければ新たにList<GameObject>を作成してDictionaryに追加
		if (pooledGameObjects.ContainsKey (key) == false) {
			pooledGameObjects.Add (key, new List<GameObject> ());
		}

		//取得したGameObjectのprefabを取得
		List<GameObject> gameObjects = pooledGameObjects [key];

		//返すゲームオブジェクト
		GameObject go = null;

		//ある場合
		//使えるオブジェクトがないかListの中身を走査
		for (int i = 0; i < gameObjects.Count; i++) {
			//とりあえず上からgameobjectを調べていく
			go = gameObjects [i];

			//現在非アクティブ(未使用)であれば
			if (go.activeInHierarchy == false) {

				//位置を設定
				go.transform.position = position;

				//角度を設定
				go.transform.rotation = rotation;

				//※targetに方向をあわせる
				TransformExtensions.LookAt2D (go.transform, target.transform, Vector2.up);

				//これから使うのでactiveにする
				go.SetActive (true);

				//				// ("recycle object");
				//ゲームオブジェクトを返す
				return go;
			}
		}
		//ない場合
		//使用できるものがないので新たに生成
		go = (GameObject)Instantiate (prefab, position, rotation);
		print ("make" + prefab.name + "!!!!!");
		//ObjectPoolゲームオブジェクトの子要素にする
		go.transform.parent = transform;
		//※targetに方向をあわせる
		TransformExtensions.LookAt2D (go.transform, target.transform, Vector2.up);
		//作ったオブジェクトをListに追加
		gameObjects.Add (go);

		//		// ("instantiate object");
		//作ったゲームオブジェクトを返す
		return go;
	
	}
	//Destroyの代わりにゲームオブジェクトを非アクティブにする。
	public void ReleaseGameobject (GameObject go)
	{
		//非アクティブにする
		go.SetActive (false);
	}

	public void ReleaseGameobjectDelay (GameObject go, float time)
	{
		StartCoroutine (delayRelease (go, time));
	}

	private IEnumerator delayRelease (GameObject go, float delayTime)
	{
		yield return new WaitForSeconds (delayTime);
		ReleaseGameobject (go);
	}
}
