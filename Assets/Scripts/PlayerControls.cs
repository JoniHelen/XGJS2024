using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float acceleration;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Vector2 maxDistance;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource engineSound;
    private Vector2 input;
    private Vector3 velocity;
    private bool paused = true;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPause()
    {
        Time.timeScale = paused ? 1.0f : 0.0f;
        
        paused = !paused;
        pauseMenu.SetActive(paused);
    }
    
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        
        Time.timeScale = paused ? 1.0f : 0.0f;
        
        paused = !paused;
        pauseMenu.SetActive(paused);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 2.0f) * 0.1f, transform.position.z);
        
        if (paused) return;
        
        if (input.y != 0)
        {
            velocity += acceleration * Time.deltaTime * new Vector3(0, 0, input.y);
            velocity = Vector3.ClampMagnitude(velocity, speed);
        }
        else if (velocity.magnitude > 0)
        {
            float sgn = Mathf.Sign(velocity.z);
            velocity -= acceleration * Time.deltaTime * new Vector3(0.0f, 0.0f, sgn);
            if (sgn != Mathf.Sign(velocity.z))
                velocity = Vector3.zero;
        }

        engineSound.pitch = 0.5f + (1.5f - 0.5f) * (velocity.magnitude / 7.0f);
        
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
