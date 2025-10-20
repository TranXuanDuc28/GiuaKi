using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponLevel;
    public List<WeaponStats> stats;
    public float speed;
    public int damage;
    public float size;
    public int amount;
    public float range;

    [System.Serializable]
    public class WeaponStats
    {
        public float speed;
        public int damage;
        public float size;
        public int amount;
        public float range;
    }
}
