using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

    ///////////////////////////
    //Motion Specific Constants
    ///////////////////////////
    public enum MotionType { LINEAR };

    //////////////
    //Data Members
    //////////////
    public MotionType type;
    public float velocity;
    public Vector2 m_source, m_destination;

    ///////////////////
    //Public Properties
    ///////////////////
    public Vector2 Source {
        get {
            return m_source;
        }
        set {
            m_source = value;
        }
    }

    public Vector2 Destination {
        get {
            return m_destination;
        }
        set {
            m_destination = value;
        }
    }


    /////////
    //Methods
    /////////
	void Awake () {
        if(gameObject.GetComponent<Rigidbody2D>() == null){
            Rigidbody2D rbComponent = gameObject.AddComponent<Rigidbody2D>();
            rbComponent.isKinematic = false;
            rbComponent.fixedAngle = true;
            rbComponent.gravityScale = 0f;
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate () {
	    transform.LookAt(m_destination, transform.up);
        transform.Rotate(0 , -90, 0);   
     
        rigidbody2D.velocity = velocity * transform.right;
    }
}
