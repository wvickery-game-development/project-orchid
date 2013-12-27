using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	/////////////////////////////////////////////////////////////////////////////
    //Data Members
    /////////////////////////////////////////////////////////////////////////////
    public float maxHealth = 100.0f;
    private float m_damage = 0.0f;

    private LineRenderer m_healthBar;
    public bool showHealthBar = true;
    private float m_maxLength;

    /////////////////////////////////////////////////////////////////////////////
    //Public Properties
    /////////////////////////////////////////////////////////////////////////////
    public float CurrentHealth{
        get { return maxHealth - m_damage; }
    }

    /////////////////////////////////////////////////////////////////////////////
    //Methods
    /////////////////////////////////////////////////////////////////////////////
	void Awake () {
        m_healthBar = gameObject.AddComponent<LineRenderer>();
        m_healthBar.material = Resources.Load("Line") as Material;
   
        BoxCollider2D colliderTemp = gameObject.AddComponent<BoxCollider2D>();
        colliderTemp.size = gameObject.GetComponent<tk2dSprite>().GetBounds().size;
        colliderTemp.isTrigger = true;
    }

    void Start () {
        tk2dSprite sprite = gameObject.GetComponent<tk2dSprite>();
        
        m_maxLength = sprite.GetBounds().extents.x * 2;

        m_healthBar.SetPosition(0, gameObject.transform.position + new Vector3(-m_maxLength/2,-10, 0));
        m_healthBar.SetPosition(1, gameObject.transform.position + new Vector3(m_maxLength/2,-10, 0));
                
        m_healthBar.SetWidth(2f, 2f);
	}
	
	void Update () {
        UpdateHealthBar();

        CheckHealth();
    }

    void CheckHealth() {
        if(CurrentHealth <= 0){
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar() {
        if(!showHealthBar){
            m_healthBar.enabled = false;
        }
        else{
            m_healthBar.enabled = true;
            //sets length of the health bar 
            m_healthBar.SetPosition(0, gameObject.transform.position + new Vector3(-m_maxLength/2,-10, 0)); //left point of bar
            m_healthBar.SetPosition(1, gameObject.transform.position + new Vector3((-m_maxLength/2) + m_maxLength * (CurrentHealth/maxHealth),-10, 0)); //right point of bar

            //fades colour from green (highest health) to red (lowest health)
            Color colour = new Color(1 - (CurrentHealth/maxHealth), (CurrentHealth/maxHealth),0 );
            m_healthBar.SetColors(colour, colour);
        }
    }

    void TakeDamage(float damage) {
        m_damage += damage;
    }
}
