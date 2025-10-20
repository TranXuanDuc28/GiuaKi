using UnityEngine;

public class Boom : MonoBehaviour
{
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        float destroyDelay = 1f; // default fallback
        if (animator != null && animator.runtimeAnimatorController != null)
        {
            destroyDelay = animator.GetCurrentAnimatorStateInfo(0).length;
        }
        Destroy(gameObject, destroyDelay);
    }
    void Update()
    {
        float moveX = GameManager.Instance.worldSpeed  * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
    }
    
}
