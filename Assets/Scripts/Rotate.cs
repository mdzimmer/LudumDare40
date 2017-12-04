using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    float rate;

    void Start()
    {
        rate = Random.Range(250, 400);
    }

	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.right * Time.deltaTime * rate);
	}
}
