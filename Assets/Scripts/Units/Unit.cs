using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Unit : Entity {

    //Sound used when 
    public AudioClip m_shootSound;

    //the point that the unit has come from and the point it is going too
    private Point m_source, m_destination;

    //TODO REMOVE FLAGS
    //flag used to tell if the unit has reached its destination and is on its return trip
    private bool m_reachedDestination = false;

    //Entitiy it is currently attacking
    private Entity m_attackTarget;
    
    //speed of the unit as it moves
    public float velocity = 100; // pixels per second

    //distanace that the unit can fire on something else
    public float range = 10;

    //type of munition the unit has
    private Munition m_munition;

    //TODO maybe use
    //timer used to determine the frequency it attacks
    private Timer m_attackTimer;

    //destination property
    public Point destination
    {
        get { return m_destination; }
    }

    //source propterty
    public Point source
    {
        get { return m_source; }
    }

    //Awake function
    //runs my initialization fucntions
    void Awake() {
        InitBody();
        InitRange();
    }

    //TODO try and move all things from Start() that can be in Awake()
    //Use this for initialization
    //runs after awake and also does initialization?
    new void Start()
    {
        base.Start();

        if(m_source != null && m_destination != null) {
            SetSourceAndTarget(m_source, m_destination);
        }

        m_munition = gameObject.GetComponent<Munition>();
        
        m_attackTimer = new Timer();
        m_attackTimer.time = m_munition.attack_time + Random.Range(0, m_munition.attack_time_variance);
    }

    //initializes the rigid body and collider based on the settings provided
    void InitBody(){
        Rigidbody2D rigidBody2D = gameObject.AddComponent<Rigidbody2D>();
        rigidBody2D.isKinematic = false;
        rigidBody2D.fixedAngle = true;
        rigidBody2D.gravityScale = 0f;

        BoxCollider2D colliderTemp = gameObject.AddComponent<BoxCollider2D>();
        colliderTemp.size = gameObject.GetComponent<tk2dSprite>().GetBounds().size;
        colliderTemp.isTrigger = true;
    }

    //initializes the collider used to detect attackable objects
    void InitRange(){
        CircleCollider2D colliderTemp = gameObject.AddComponent<CircleCollider2D>();
        colliderTemp.radius = range;
        colliderTemp.isTrigger = true;
    }
    
    //Update function is called once per frame
    //checks if is attacking a target.
    new void Update ()
    {
        base.Update();
        if (m_attackTarget)
        { 
            
            m_attackTimer.elapsed += Time.deltaTime;
            while (m_attackTimer.HasElapsed())
            {
                Attack(m_attackTarget);
                m_attackTimer.SetBack();
                //m_attackTimer.time = m_munition.attack_time + Random.Range(0, m_munition.attack_time_variance);
            }
        }
    }

    //TODO can this be done in a better way? getters and setters are evil
    //sets the the to and from for the unit, and sends it in the direction
    public void SetSourceAndTarget(Point source, Point target)
    {
        // Move the unit to the source
        m_source = source;
        this.transform.position = source.position;

        // at some point assign the target point and make it point in that direction
        m_destination = target;

        Vector2 direction = m_destination.position - m_source.position;
        gameObject.GetComponent<Rigidbody2D>().velocity = direction.normalized * velocity;
        if(m_owner == Faction.PLAYER){
            Debug.LogWarning(gameObject.name + " transform up before " + transform.up);
        }
        transform.LookAt(destination.gameObject.transform, transform.up);
        if(m_owner == Faction.PLAYER){
            Debug.LogWarning(gameObject.name + " transform up after " + transform.up);
        }
        transform.Rotate(0 , -90, 0);
        transform.position = transform.position + new Vector3(0 , 0, -2);
    }

    //checks if the entity detected is attackable based on this unit
    bool CanAttack(Entity target){
        if(target.m_owner == this.m_owner || target.dead){
            return false;
        }

        switch (this.type)
	    {
            case Type.bomber:
                return target.type == Type.point;
            case Type.fighter:
                return true;
            case Type.icbm:
                return target.gameObject.name == destination.gameObject.name;
            case Type.point:
                return false;
            default:
                Debug.LogError("We are attacking with an unchecked type.");
                return false;
	    }
    }

    //overidable method used to attack an object
    void Attack(Entity target)
    {
        int damage = m_munition.damage;

        Laser laser = new Laser(position, target.position, m_munition.colour);
        StartCoroutine(laser.Fade());

        AudioSource.PlayClipAtPoint(m_shootSound,transform.position, Random.Range(0.01f,0.3f));

        float hitChance = Random.Range(0, 1.0f);
        if (hitChance <= m_munition.accuracy) {
            target.TakeDamage(damage);
        }
        if(type == Type.icbm){
            ReachedDestination();
        }
    }

    //TODO shouldn't exist, why does this exist
    //Calls the take damage funciton of entity
    public new void TakeDamage(int damage){
        base.TakeDamage(damage);
    }

    //called when the unit reaches the point(location) that is its original destination
    public void ReachedDestination(){
        if(type == Type.icbm){
            Explode();      
        }
        else{
            //TODO maybe put this in update 
            gameObject.GetComponent<Rigidbody2D>().velocity *= -1;
            transform.LookAt(source.gameObject.transform, transform.up);
            transform.Rotate(0 , -90, 0);
            transform.position = transform.position + new Vector3(0 , 0, -2);
            m_reachedDestination = true;
        }
    }

    //called to give money back to player and destroy game object.
    public void FinishMission(){
        if(m_reachedDestination){
            source.UnitReturned(m_reward);
            Destroy(gameObject);
        }
    }

    //mainly used to detect if attackable item is in range
    //OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other is BoxCollider2D)
        {
            Entity possibleTarget = other.gameObject.GetComponent<Entity>();
            if(possibleTarget == null) Debug.LogError("No Entity on other.gameobject:: " + other.gameObject);
            else{
                if(CanAttack (possibleTarget)){
                m_attackTarget = possibleTarget;

                m_attackTimer.elapsed = m_attackTimer.time;
                }
            }
        }
    }

    //TODO this is a dangerous, method. un-verified behaviour.
    //typically called to end the attack when colliders exit
    //OnTriggerEnter is called when the Collider other enters the trigger.
    void OnTriggerExit2D(Collider2D other)
    {
        m_attackTarget = null;
    }   
}
