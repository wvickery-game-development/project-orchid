using UnityEngine;
using System.Collections;

public class Motion : MonoBehaviour {

    /////////////////////////////////////////////////////////////////////////////
    //Motion Specific Constants
    /////////////////////////////////////////////////////////////////////////////
    public float accuracyDistance = 0.5f;
    public enum MotionType { LINEAR };

    /////////////////////////////////////////////////////////////////////////////
    //Data Members
    /////////////////////////////////////////////////////////////////////////////
    public MotionType type = MotionType.LINEAR;
    public float velocity = 10.0f;
    private Vector2 m_source = new Vector2(0, 0);
    private Vector2 m_destination = new Vector2(110, 100);

    /////////////////////////////////////////////////////////////////////////////
    //Public Properties
    /////////////////////////////////////////////////////////////////////////////
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

    public Vector2 Position {
        get {
            return  new Vector2(transform.position.x, transform.position.y);
        }
    }


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

        m_source = Position;
    }
	
	void Update () {
        if(Vector2.Distance(Position, m_destination) < accuracyDistance){
            ReverseCourse();
        }
	}

    void FixedUpdate () {
	    LookAtDestination();
     
        rigidbody2D.velocity = velocity * transform.right;
    }

    void LookAtDestination () {
        float degreesChange = Vector2.Angle(transform.right, (m_destination - Position).normalized);
        transform.Rotate(Vector3.forward * degreesChange);
    }
    void ReverseCourse() {
        Vector2 oldDestination = m_destination;
        m_destination = m_source;
        m_source = oldDestination;
    }
}
