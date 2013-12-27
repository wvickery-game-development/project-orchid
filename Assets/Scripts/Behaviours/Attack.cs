using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
    /////////////////////////////////////////////////////////////////////////////
    //Data Members
    /////////////////////////////////////////////////////////////////////////////
    public float attackRange = 50.0f;

    /////////////////////////////////////////////////////////////////////////////
    //Public Properties
    /////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////
    //Methods
    /////////////////////////////////////////////////////////////////////////////
	void Awake () {
        //add the rigid body component if it hasn't been added already
        if(gameObject.GetComponent<Rigidbody2D>() == null){
            Rigidbody2D rbComponent = gameObject.AddComponent<Rigidbody2D>();
            rbComponent.isKinematic = false;
            rbComponent.fixedAngle = true;
            rbComponent.gravityScale = 0f;
        }	

        //use a circle 2d collider as a trigger for attack detection
        CircleCollider2D colliderTemp = gameObject.AddComponent<CircleCollider2D>();
        colliderTemp.radius = attackRange;
        colliderTemp.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
