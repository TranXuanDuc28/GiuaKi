using UnityEngine;

public class StarController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int experienceToGive = 1;
    [SerializeField] private Sprite[] starSprites;

    private Vector3 targetPosition;
    private float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = starSprites[Random.Range(0, starSprites.Length)];
        moveSpeed = Random.Range(0.5f, 3f);
        generateRandomPosition();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        float moveX = GameManager.Instance.worldSpeed  * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0f, 0f);

        if(transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
    private void generateRandomPosition()
    {
        float randomX = Random.Range(-9f, 9f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector3(randomX, randomY, 0f);
        moveSpeed = Random.Range(0.1f, 1f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.GetExperience(experienceToGive);
            // also add score toward win condition
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
