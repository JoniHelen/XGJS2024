using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float velocityDecay;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 maxDistance;
    private Vector2 input;
    private Vector3 velocity;

    private void Awake()
    {
        ToggleInput(false);
    }

    public void ToggleInput(bool value)
    {
        if (value)
            GetComponent<PlayerInput>().ActivateInput();
        else
            GetComponent<PlayerInput>().DeactivateInput();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (velocity.magnitude < speed)
            velocity += acceleration * Time.deltaTime * new Vector3(0, 0, input.y);

        velocity *= velocityDecay;
        
        transform.Translate(Time.deltaTime * velocity, Space.Self);
        transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime * input.x, Space.World);

        // restrain player
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -maxDistance.x, maxDistance.x),
            0, 
            Mathf.Clamp(transform.position.z, -maxDistance.y, maxDistance.y)
        );
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        input = ctx.ReadValue<Vector2>();
    }
}
