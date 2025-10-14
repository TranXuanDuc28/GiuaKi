using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D playerRb;
    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;
    public float boost = 1f;
    private float boostPower = 5f;
    private bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject destroyEffect;

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

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        energy = maxEnergy;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        
    }

    // Update is called once per frame
    void Update()
    {
     
        float directionX = Input.GetAxisRaw("Horizontal");
        float directionY = Input.GetAxisRaw("Vertical");
        playerDirection = new Vector2(directionX, directionY).normalized;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2")) 
        {
            EnterBoost();
        }else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
        {
            ExitBoost();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetButtonDown("Fire1")) 
        {
            PhaserWeapon.Instance.Shoot();
        }
    }
    private void FixedUpdate()
    {
        playerRb.linearVelocity  = new Vector2(playerDirection.x , playerDirection.y) * moveSpeed;

        if (boosting)
        {
            if(energy >= 0.2f) energy -= 0.2f;
            else
            {
                ExitBoost();
            }
                
        }
        else
        {
            if (energy < maxEnergy) energy += energyRegen ;

        }
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }
    private void EnterBoost()
    {
        if(energy > 10){
            boost = boostPower;
            boosting = true;
        }
        
    }
    private void ExitBoost()
    {
        boost = 1f;
        boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        if (health <= 0)
        {
            boost = 0f;
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }
        
    }
}
