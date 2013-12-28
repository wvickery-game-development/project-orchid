using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
    /////////////////////////////////////////////////////////////////////////////
    //Data Members
    /////////////////////////////////////////////////////////////////////////////
    public float range = 50.0f;
    public float frequency = 1.0f;
    public float variance = 0.5f;
    public float damage = 10;

    private Timer m_attackTimer;
    private GameObject m_attackTarget;
    private LaserFactory m_laserFactory;
    private Color m_attackColour;

    /////////////////////////////////////////////////////////////////////////////
    //Public Properties
    /////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////
    //Methods
    /////////////////////////////////////////////////////////////////////////////
	void Awake () {
        
        //**************************
        if(variance > frequency/2) Debug.LogError("Attack Variance for " + gameObject.name + " is too high.\nIt must be <= half of Attack Frequency.");
        //**************************

        //add the rigid body component if it hasn't been added already
        if(gameObject.GetComponent<Rigidbody2D>() == null){
            Rigidbody2D rbComponent = gameObject.AddComponent<Rigidbody2D>();
            rbComponent.isKinematic = false;
            rbComponent.fixedAngle = true;
            rbComponent.gravityScale = 0f;
        }	

        //use a circle 2d collider as a trigger for attack detection
        CircleCollider2D colliderTemp = gameObject.AddComponent<CircleCollider2D>();
        colliderTemp.radius = range;
        colliderTemp.isTrigger = true;

        m_attackTimer = new Timer();        
        m_attackTimer.time = frequency + Random.Range(-variance, variance);
	}

    void Start() {
        m_laserFactory = GameObject.Find("LaserFactory").GetComponent<LaserFactory>();

        if(gameObject.GetComponent<Info>().faction == Faction.PLAYER){ m_attackColour = Color.blue;}
        else{ m_attackColour = Color.red;}
    }
	
	void Update () {
        m_attackTimer.elapsed += Time.deltaTime;
	    while (m_attackTimer.HasElapsed()){
            if(m_attackTarget != null){
                AttackTarget();
            }
            m_attackTimer.SetBack();
            m_attackTimer.time = frequency + Random.Range(-variance, variance);
        }
	}

    void AttackTarget(){
        m_laserFactory.ShootLaser(transform.position, m_attackTarget.transform.position, m_attackColour);
        m_attackTarget.GetComponent<Health>().TakeDamage(damage);
    }

    bool CanAttack(Type otherType, Faction otherFaction){
        Type thisType = gameObject.GetComponent<Info>().type;
        Faction thisFaction = gameObject.GetComponent<Info>().faction;

        if(thisFaction == otherFaction){
            return false;
        }

        switch (thisType) {
            case Type.BOMBER:
                return otherType == Type.LOCATION;
            case Type.FIGHTER:
                return true;
            case Type.ICBM:
                //return target.gameObject.name == destination.gameObject.name;
                return true;
            case Type.LOCATION:
                Debug.LogError("An attack component was placed on a location object.");
                return false;
            default:
                return false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (m_attackTarget == null) {
            if (other is BoxCollider2D){ // want to check for body, box colliders rather than range, circle colliders
                GameObject possibleTarget = other.gameObject;
                if(possibleTarget.GetComponent<Health>() != null) {
                    Info objectInfo = possibleTarget.GetComponent<Info>();
                    if(CanAttack (objectInfo.type, objectInfo.faction)){
                        m_attackTarget = possibleTarget;
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        m_attackTarget = null;
    } 
}
