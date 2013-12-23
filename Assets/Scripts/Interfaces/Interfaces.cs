using UnityEngine;
using System.Collections;

public interface IDamageable {
    int Damage{get; set;}

    void TakeDamage(int damageTaken);
}

public interface IOwnable {
    Faction Owner{get;}
}

public interface IHealthy {
    int Health {get;}
}

public interface IRewarding {
    int Reward {get;}
}


