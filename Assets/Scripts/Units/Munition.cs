using UnityEngine;
using System.Collections;

//TODO Delete this class.
//TODO put this into inherited units (for fighter ect)
public class Munition : MonoBehaviour {

    public Color colour = Color.red;
    
    public int damage = 1;

    public float attack_time = 0.3f;
    public float attack_time_variance = .1f;
    public float accuracy = 0.8f;

}
