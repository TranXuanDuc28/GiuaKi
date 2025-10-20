using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D playerRb;
    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;
    public bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject destroyEffect;

    [SerializeField] private int experiencePoints;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private List<int> playerLevelUps;


    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;


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
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        for(int i = playerLevelUps.Count; i < maxLevel; i++)
        {
            playerLevelUps.Add(Mathf.CeilToInt(playerLevelUps[playerLevelUps.Count - 1] * 1.1f + 15));
        }

        energy = maxEnergy;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        experiencePoints = 0;
        UIController.Instance.UpdateExperienceSlider(experiencePoints, playerLevelUps[currentLevel]);
        
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

        if (Input.GetKeyDown(KeyCode.F) || Input.GetKey(KeyCode.G)) 
        {
            PhaserWeapon.Instance.Shoot();
        }
    }
    private void FixedUpdate()
    {
        playerRb.linearVelocity  = new Vector2(playerDirection.x , playerDirection.y) * moveSpeed;

        if (boosting)
        {
            if(energy >= 0.5f) energy -= 0.5f;
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
           GameManager.Instance.SetWorldSpeed(7f);
            boosting = true;
        }
        
    }
    private void ExitBoost()
    {
        GameManager.Instance.SetWorldSpeed(1f);
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
        spriteRenderer.material = whiteMaterial;
        StartCoroutine("ResetMaterial");
        if (health <= 0)
        {
            ExitBoost();
            GameManager.Instance.SetWorldSpeed(0f);
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameResult(false);
        }

    }
    public void GetExperience(int exp)
    {
        experiencePoints += exp;
        UIController.Instance.UpdateExperienceSlider(experiencePoints, playerLevelUps[currentLevel]);
        if(experiencePoints > playerLevelUps[currentLevel])
        {
            LevelUp();
        }
    }
    public void LevelUp()
    {
        experiencePoints -= playerLevelUps[currentLevel];
        if (currentLevel < maxLevel - 1)
        {
            currentLevel++;
        }
        UIController.Instance.UpdateExperienceSlider(experiencePoints, playerLevelUps[currentLevel]);
        PhaserWeapon.Instance.LevelUpWeapon();
        //maxHealth++;
        //health = maxHealth;
        health++;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);   
    }
    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
