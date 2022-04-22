using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitationalForce : Force
{
    [SerializeField] FloatData gravitation;

    public override void ApplyForce(List<Body> bodies)
    {
        for (int i = 0; i < bodies.Count - 1; i++)
        {
            for (int j = i + 1; j < bodies.Count; j++)
            {
                if (i == j) continue;

                Body bodyA = bodies[i];
                Body bodyB = bodies[j];
                // apply gravitational force
                Vector2 direction = bodyA.position - bodyB.position;

                //float distanceSqr = Mathf.Max(direction.sqrMagnitude, 1);

                float distance = Mathf.Max(1, direction.magnitude);
                float force = gravitation.value * (bodyA.mass * bodyB.mass) / distance;

                //float force = gravatation.value * ((bodyA.mass * bodyB.mass) / distanceSqr);

                bodyA.ApplyForce(-direction.normalized * force, Body.eForceMode.Force);

                bodyB.ApplyForce(direction.normalized * force, Body.eForceMode.Force);
            }
        }
    }

    private void Update()
    {
        
    }

    
}
