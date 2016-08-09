using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour {

	public int[] snareScore;

	// Use this for initialization
	void Start () {
	
		snareScore = new int[16];
		for (int i = 0; i < 16; i++)
			snareScore [i] = Random.Range (0, 2);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
