using UnityEngine;

public class Coin : Interactable
{
    [SerializeField] int value = 10;
    [SerializeField] GameObject pickupFx;

    protected override void Interact(Player player)
    {
        GameManager.Instance.AddScore(value);
        if (pickupFx) Instantiate(pickupFx, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
