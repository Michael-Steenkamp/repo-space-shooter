using UnityEngine;

public class AsteroidDynamicEnemyLogic : DynamicEnemyLogic
{
    private const float MIN_FORCE = 0.1f;

    private Vector2 forceTowardsPlayer;
    private float gravityScale;

    private GameObject target;

    private void Start()
    {
        target = GetTarget();

        if (target.GetComponent<PlayerController>())
        {
            gravityScale = target.GetComponent<PlayerController>().GetGravitationalPull();
        }
    }

    private void Update()
    {
        targetDirection = (target.transform.position - transform.position);
        targetDistance = targetDirection.magnitude;
        forceTowardsPlayer = targetDirection.normalized * ((gravityScale / (targetDistance + 1f)) + MIN_FORCE);
        Debug.Log($"Force towards player: {forceTowardsPlayer}, Distance: {targetDistance}");
    }

    private void FixedUpdate()
    {
        rb.AddForce(forceTowardsPlayer);
        rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
}
