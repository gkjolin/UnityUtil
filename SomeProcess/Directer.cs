using UnityEngine;
using System.Collections;

public class Directer : MonoBehaviour
{
	//移動の方向
	public enum direction
	{
		up = 1,
		rightUp = 2,
		right = 3,
		rightDown = 4,
		down = 5,
		leftDown = 6,
		left = 7,
		leftUp = 8,
		center = 0,
		none = -1
	}

	public direction testdir;

	/// <summary>
	/// ２地点間の方向、距離を取得
	/// </summary>
	public direction GetDirection (Pos originalPos, Pos targetPos)
	{
		//判定に使う値
		int xVal = targetPos.posX - originalPos.posX;
		int yVal = targetPos.posY - originalPos.posY;

		if (yVal == -1) {
			if (xVal == -1) {
				//上
				return direction.up;
			} else if (xVal == 0) {
				return direction.rightUp;
			} else if (xVal == 1) {
				return direction.right;
			}
		}

		if (yVal == 0) {
			if (xVal == -1) {
				return direction.leftUp;
			} else if (xVal == 0) {
				return direction.center;
			} else if (xVal == 1) {
				return direction.rightDown;
			}
		}

		if (yVal == 1) {
			if (xVal == -1) {
				return direction.left;
			} else if (xVal == 0) {
				return direction.leftDown;
			} else if (xVal == 1) {
				return direction.down;
			}
		}
		return direction.none;
	}
	//２地点間の角度を所得
	public float GetAim (Vector2 p1, Vector2 p2)
	{
		float dx = p2.x - p1.x;
		float dy = p2.y - p1.y;
		float rad = Mathf.Atan2 (dx, dy);
		return rad * Mathf.Rad2Deg;
	}

	public Directer.direction GetDirecterDir (float angle)
	{
		float value = 22.5f;
		int judgeValue = (int)(angle / value);
//		// ("ArcherTowerChangeSprite" + judgeValue.ToString ());
		switch (judgeValue) {
		case 0:
			return Directer.direction.up;
			break;
		case 1:
		case 2:
			return Directer.direction.rightUp;
			break;
		case -1:
		case -2:
			return Directer.direction.leftUp;
			break;
		case 3:
		case 4:
			return Directer.direction.right;
			break;
		case -3:
		case -4:
			return Directer.direction.left;
			break;
		case 5:
		case 6:
			return Directer.direction.rightDown;
			break;
		case -5:
		case -6:
			return Directer.direction.leftDown;
			break;
		case 7:
		case -7:
		case 8:
		case -8:
			return Directer.direction.down;
			break;
		default:
			return Directer.direction.none;
		}
	}
}
