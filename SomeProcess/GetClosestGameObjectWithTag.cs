using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Koganeburogu;

public class GetClosestGameObjectWithTag : MonoBehaviour
{
	public Vector2 testvalue;
	public List<GameObject> toCheck;
	//最も近いゲームオブジェクトをタグで返す
	public GameObject GetClosestPoint (Vector2 selfPos, string tag)
	{

		GameObject[] tagObjs = GameObject.FindGameObjectsWithTag (tag);
		GameObject closestPointObj = null;
		float distance = 9999;
		float closestDistance = 9999;

		foreach (GameObject obj in tagObjs) {
			distance = Vector2.Distance (selfPos, obj.transform.position);
			if (distance < closestDistance) {
				closestPointObj = obj;
				closestDistance = distance;
			}
		}
		return closestPointObj;
	}
	//キャラクターのポジションを取得
	public void get ()
	{
		GetCharacterPos (testvalue);
	}

	public Pos GetCharacterPos (Vector2 characterPos)
	{
		Vector2 characterPosWorld = rectangleToWorld (characterPos);
		Pos returnPos = new Pos ();
		returnPos.posX = (int)characterPosWorld.x;
		returnPos.posY = (int)characterPosWorld.y;
		return returnPos;
	}

	public Vector2 rectangleToWorld (Vector2 rectanglePos)
	{
//		////////print ("characterpos" + rectanglePos.ToString ());
		//直行方向のX
		float xVal = StageCreator.tileXWidth / 2;
//		////////print ("xVal" + xVal.ToString ());
		//直行方向のY
		float yVal = StageCreator.tileYWidth / 2;
//		////////print ("yVal" + yVal.ToString ());

		float xPos = rectanglePos.x / (2 * xVal) - rectanglePos.y / (2 * yVal);
		int x = (int)(xPos + 0.5f);
//		////////print ((rectanglePos.x / 2 * xVal).ToString () + ":" + (rectanglePos.y / 2 * yVal).ToString ());
		float yPos = rectanglePos.y / (-2 * yVal) + rectanglePos.x / (-2 * xVal);
		int y = (int)(yPos + 0.5f);
//		////////print ((rectanglePos.y / (-2 * yVal) + ":" + (rectanglePos.x / (-2 * xVal)).ToString ()));
		return new Vector2 (x, y);
	}

	public Vector2 worldToRectangle (Vector2 worldPos)
	{
		int rectangleX = (int)(worldPos.x / StageCreator.tileXWidth + worldPos.y / StageCreator.tileYWidth);
		int rectangleY = (int)(-worldPos.x / StageCreator.tileXWidth + worldPos.y / StageCreator.tileYWidth);

		return new Vector2 ((float)rectangleX, (float)rectangleY);
	}

	public List<GameObject> GetGameObjectsArrayWithTagByRange (Vector2 selfPos, string tag, float damageDistance)
	{
		//一旦全てとる
		GameObject[] tagObjs = GameObject.FindGameObjectsWithTag (tag);
		//返すリスト
		List<GameObject> targetObjs = new List<GameObject> ();
		float distance = 0;
	
		foreach (GameObject obj in tagObjs) {
			distance = Vector2.Distance (selfPos, obj.transform.position);
			//オブジェクトが範囲内にいたら
			if (distance < damageDistance) {
				targetObjs.Add (obj);
			}
		}

		return targetObjs;
	}
	//Morter用一定距離より外にいるキャラクターを取得
	public List<GameObject> GetGameObjectsArrayWithTagByRangeUpper (Vector2 selfPos, string tag, float damageDistance)
	{
		//一旦全てとる
		GameObject[] tagObjs = GameObject.FindGameObjectsWithTag (tag);
//		// (tagObjs.ToString ());
		//返すリスト
		List<GameObject> targetObjs = new List<GameObject> ();
		float distance = 0;

		foreach (GameObject obj in tagObjs) {
			distance = Vector2.Distance (selfPos, obj.transform.position);
			//// ("distance" + distance.ToString () + "damageDistance" + damageDistance.ToString ());
			//オブジェクトが範囲内にいたら
			if (distance > damageDistance) {
				targetObjs.Add (obj);
				//	// ("add target objes" + obj.name);
			}
		}

		return targetObjs;
	}
	//Morter用最低距離より外にいる中で一番近いキャラクターを取得
	public GameObject getMorterTarget (Pos selfPos, Vector2 selfPosTransform, int range, float minimumDistance)
	{
		//周りにいるキャラクターを取得
		List<GameObject> characters = GetAroundCharacter (selfPos, range);
		GameObject morterTargetDistCharacter = null;
		float distance = 9999;
		float closestDistance = 9999;

		foreach (GameObject character in characters) {
			distance = Vector2.Distance (selfPosTransform, character.transform.position);
			if (distance < closestDistance && distance > minimumDistance) {
				morterTargetDistCharacter = character;
				closestDistance = distance;
			}
		}
		return morterTargetDistCharacter;
	}

	public GameObject GetClosestCharacterForEnemyCharacter (Pos selfPos, Vector2 selfPosTransform, int range)
	{
		//範囲内にいるキャラクターを取得
		List<GameObject> characters = GetAroundCharacter (selfPos, range);
		//取得したキャラクターの中から最も近くにいるキャラクターを取得
		GameObject closestCharacter = null;
		float distance = 9999;
		float closestDistance = 9999;

		foreach (GameObject character in characters) {
			distance = Vector2.Distance (selfPosTransform, character.transform.position);
			if (distance < closestDistance) {
				closestCharacter = character;
				closestDistance = distance;
			}
		}
		return closestCharacter;
	}

	public List<GameObject> GetAroundCharacter (Pos selfPos, int range)
	{
		GameObject[] characters = GameObject.FindGameObjectsWithTag (TagName.Giant);
		List<GameObject> targetCharacters = new List<GameObject> ();
		if (characters.Length != 0) {
		
			foreach (GameObject chr in characters) {
				if (chr.GetComponent<Character> ().currentPos != null) {
					//キャラクターの座標を取得
					Pos chrPos = chr.GetComponent<Character> ().currentPos;
					//範囲内にいればリストに追加
//				print (gameObject.name + "self pos" + selfPos.posX.ToString () + selfPos.posY.ToString ());
//				print (gameObject.name + "target pos" + chrPos.posX.ToString () + chrPos.posY.ToString ());
//				print ("range is" + "X" + (selfPos.posX - range).ToString () + "to" + (selfPos.posX + range).ToString () + "Y" + (chrPos.posY - range).ToString () + "to" + (chrPos.posY + range).ToString ());
					if (selfPos.posX - range <= chrPos.posX && chrPos.posX <= selfPos.posX + range && selfPos.posY - range <= chrPos.posY && chrPos.posY <= selfPos.posY + range) {
						targetCharacters.Add (chr);
						//chr.GetComponent<Character> ().childSprite.color = Color.red;
					}
				}
			}
		}
		return targetCharacters;
	}

	public bool CheckTargetInRange (Pos selfPos, Pos targetPos, int rangeGrid)
	{
		if (selfPos.posX - rangeGrid <= targetPos.posX && targetPos.posX <= selfPos.posX + rangeGrid && selfPos.posY - rangeGrid <= targetPos.posY && targetPos.posY <= selfPos.posY + rangeGrid) {
			return true;
		} else {
			return false;
		}
	}
}
