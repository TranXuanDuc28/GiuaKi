using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private int lives;
    private PolygonCollider2D polygonCollider;
    [SerializeField] private Sprite[] asteroidSprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();

        spriteRenderer.sprite = asteroidSprites[Random.Range(0, asteroidSprites.Length)];

        // Tạo PolygonCollider2D mới và khớp theo sprite
        polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        UpdateColliderShape();

        // Random hướng di chuyển
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(pushX, pushY).normalized * Random.Range(1f, 3f);
    }
    private void UpdateColliderShape()
    {
        if (polygonCollider != null && spriteRenderer.sprite != null)
        {
            polygonCollider.isTrigger = false;

            // Xóa path cũ (nếu có)
            polygonCollider.pathCount = 0;

            // Tạo List chứa dữ liệu hình dạng vật lý
            List<Vector2> physicsShape = new List<Vector2>();

            // Lấy physics shape của sprite
            spriteRenderer.sprite.GetPhysicsShape(0, physicsShape);

            // Gán path mới cho collider
            polygonCollider.SetPath(0, physicsShape);
        }
    }
    // Update is called once per frame
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
    }
    //Va chạm với player(tàu vũ trụ) hoặc missile(đạn)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Missile"))

        {
            spriteRenderer.material = whiteMaterial;
            StartCoroutine("ResetMaterial");
            lives--;
            if (lives <= 0)
            {
                Instantiate(destroyEffect, transform.position, transform.rotation);
                Destroy(gameObject);    
            }
        }

    }
    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
