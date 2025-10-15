using UnityEngine;

public class PhaserWeapon : MonoBehaviour
{
    public static PhaserWeapon Instance;
    //[SerializeField] private GameObject prefab;
    [SerializeField] private ObjectPooler objectPooler;

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
        //Instantiate(prefab, transform.position, transform.rotation);
        GameObject missile = objectPooler.GetPooledObject();
        missile.transform.position = transform.position;
        missile.SetActive(true);

    }
}
