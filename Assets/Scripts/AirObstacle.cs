using UnityEngine;

public class AirObstacle : Obstacle
{
    [SerializeField] float bobSpeed = 3f, bobAmount = 0.3f;
    float startY;

    void Start() => startY = transform.position.y;

    protected override void Update()
    {
        base.Update();
        var p = transform.position;
        p.y = startY + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.position = p;
    }
}
