using UnityEngine;
using System.Collections.Generic;
public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; 
    private Rigidbody2D rb;
    private PolygonCollider2D polygonCollider;
    [SerializeField] private Sprite[] asteroidSprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost ) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }   
    }
}
