using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] float factor = 0.5f, menuSpeed = 3f;
    float startX, width;

    void Start()
    {
        startX = transform.position.x;
        width = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float speed = GameManager.Instance ? GameManager.Instance.Speed : menuSpeed;
        transform.Translate(Vector3.left * speed * factor * Time.deltaTime, Space.World);
        if (transform.position.x <= startX - width)
            transform.Translate(Vector3.right * width, Space.World);
    }
}
