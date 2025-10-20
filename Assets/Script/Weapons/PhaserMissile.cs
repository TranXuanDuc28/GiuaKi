using UnityEngine;

public class PhaserMissile : MonoBehaviour
{
    PhaserWeapon weapon;
    void Start()
    {
        weapon = PhaserWeapon.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(weapon.stats[weapon.weaponLevel].speed * Time.deltaTime, 0f);
        if (transform.position.x > 9)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameObject.SetActive(false);
        }
    }
}
