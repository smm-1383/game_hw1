public abstract class Obstacle : Interactable
{
    protected override void Interact(Player player) => player.TakeHit();
}
