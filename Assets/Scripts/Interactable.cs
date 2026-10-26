using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] float destroyX = -15f;

    protected virtual void Update()
    {
        transform.Translate(Vector3.left * GameManager.Instance.Speed * Time.deltaTime, Space.World);
        if (transform.position.x < destroyX) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Player p)) Interact(p);
    }

    protected abstract void Interact(Player player);
}
