using UnityEngine;
using System.Collections;

/// <summary>
/// 3D空間上で2Dオブジェクトに対してRayCasterを飛ばして判定させたいときに使う
/// オブジェクトがアニメーションしていてCanvasに入れられないときかつカメラがPerspectiveのときに用いる
/// </summary>
public class RayCasterFor2DObjectIn3DGame : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;
	public Camera camera;
	public static bool throwRayCast = true;
	//押したい2Dオブジェクトがあるz座標
	private GameObject targetZ;

	void Awake ()
	{
		targetZ = GameObject.Find ("ZodiacImageCanvas").gameObject;
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0) && throwRayCast) {
			//押したい2dオブジェクトのz座標に合わせてrayをずらす
			Vector2 rayOrigin = (Vector2)camera.ScreenToWorldPoint (Input.mousePosition + new Vector3 (0f, 0f, -Input.mousePosition.z + targetZ.transform.position.z));
		
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, new Vector3 (0, 0, 1), Mathf.Infinity, layerMask);
			//なにかと衝突した時だけそのオブジェクトの名前をログに出す
			if (hit.collider) {
				//Dosomething
			}
		}


	}
}
