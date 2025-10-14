using UnityEngine;

public class PhaserWeapon : MonoBehaviour
{
    public static PhaserWeapon Instance;
    [SerializeField] private GameObject prefab;

    public float speed;
    public int damage;

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
        Instantiate(prefab, transform.position, transform.rotation);
    }
}
