using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        if(gameObject.GetComponent<Rigidbody2D>() == null){
            gameObject.AddComponent<Rigidbody2D>();
        }	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
