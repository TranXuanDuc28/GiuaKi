using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    float backgoundImageWidth;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgoundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = moveSpeed * PlayerController.Instance.boost * Time.deltaTime;
        transform.position += new Vector3(moveX, 0);
        if (Mathf.Abs(transform.position.x) - 5 > 0)
        {
            transform.position = new Vector3(0f, transform.position.y);
        }
        
    }
}
