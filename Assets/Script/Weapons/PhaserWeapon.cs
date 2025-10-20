using UnityEngine;

public class PhaserWeapon : Weapon
{
    public static PhaserWeapon Instance;
    [SerializeField] private ObjectPooler objectPooler;
    [Tooltip("Shots per second when holding fire")]
    public float fireRate = 8f;

    // internal cooldown timer
    private float nextFireTime = 0f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Shoot()
    {
        // rate limit shooting so holding the fire button doesn't spawn every frame
        if (Time.time < nextFireTime)
            return;

        // schedule next allowed shot
        float cooldown = fireRate > 0f ? 1f / fireRate : 0f;
        nextFireTime = Time.time + cooldown;

        for (int i = 0; i < stats[weaponLevel].amount; i++)
        {
            GameObject missile = objectPooler.GetPooledObject();
            float yPos = transform.position.y;

            if (stats[weaponLevel].amount > 1)
            {
                float spacing = stats[weaponLevel].range / (stats[weaponLevel].amount - 1);
                yPos = transform.position.y - (stats[weaponLevel].range / 2) + (i * spacing);
            }

            missile.transform.position = new Vector2(transform.position.x, yPos);
            missile.transform.localScale = new Vector2(stats[weaponLevel].size, stats[weaponLevel].size);
            missile.SetActive(true);
        }
    }
    public void LevelUpWeapon()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;
        }
    }

}
