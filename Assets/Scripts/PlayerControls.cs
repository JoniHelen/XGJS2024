using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float velocityDecay;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    private Vector2 input;
    private Vector3 velocity;

    private void Update()
    {
        if (velocity.magnitude < speed)
            velocity += acceleration * Time.deltaTime * new Vector3(0, 0, input.y);

        velocity *= velocityDecay;
        
        transform.Translate(Time.deltaTime * velocity, Space.Self);
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * input.x, Space.World);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }
}
