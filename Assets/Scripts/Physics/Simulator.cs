using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : Singleton<Simulator>
{
    [SerializeField] List<Force> forces;
    [SerializeField] IntData fixedFPS;
    [SerializeField] StringData fps;

    public List<Body> bodies { get; set; } = new List<Body>();
    public float fixedDeltaTime { get => 1.0f / fixedFPS.value; }

    Camera activeCamera;
    float timeAccumulator = 0;

    private void Start()
    {
        activeCamera = Camera.main;
    }

    private void Update()
    {
        fps.value = (1.0f / Time.deltaTime).ToString("F2");

        timeAccumulator += Time.deltaTime;

        forces.ForEach(force => force.ApplyForce(bodies));

        while (timeAccumulator > fixedDeltaTime)
        {
            bodies.ForEach(body => body.shape.color = Color.white);
            Collision.CreateContacts(bodies, out var contacts);
            contacts.ForEach(contact => { 
                contact.bodyA.shape.color = Color.red; 
                contact.bodyB.shape.color = Color.red; 
            });

            Collision.SeparateContacts(contacts);
            Collision.ApplyImpulses(contacts);

            bodies.ForEach(body =>
            {
                //body.Step(Time.deltaTime);
                Integrator.SemiImplicitEuler(body, fixedDeltaTime);
            });
            timeAccumulator -= fixedDeltaTime;
        }

        bodies.ForEach(body => body.acceleration = Vector2.zero);
    }

    public Body GetScreenToBody(Vector3 screen)
    {
        Body body = null;

        Ray ray = activeCamera.ScreenPointToRay(screen);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider)
        {
            hit.collider.gameObject.TryGetComponent<Body>(out body);
        }

        return body;
    }

    public Vector3 GetScreenToWorldPosition(Vector2 screen)
    {
        Vector2 world = activeCamera.ScreenToWorldPoint(screen);
        return world;
    }



}
