using UnityEngine;
using System.Collections;

public class Loadermake : MonoBehaviour
{
	public GameObject loader;
	// Use this for initialization
	void Start ()
	{
		Instantiate (loader);
	}
}
