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
    [SerializeField] private AudioSource hornSound;
    [SerializeField] private ParticleSystem trail1;
    [SerializeField] private ParticleSystem trail2;
    private Vector2 input;
    private Vector3 velocity;
    private bool paused = true;
    private ParticleSystem.EmissionModule emissionModuleT1;
    private ParticleSystem.EmissionModule emissionModuleT2;

    private void Awake()
    {
        emissionModuleT1 = trail1.emission;
        emissionModuleT2 = trail2.emission;
    }

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

    public void Toot(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        
        hornSound.PlayOneShot(hornSound.clip);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 2.0f) * 0.1f, transform.position.z);
        
        emissionModuleT1.rateOverTimeMultiplier = velocity.magnitude / speed * 200.0f;
        emissionModuleT2.rateOverTimeMultiplier = velocity.magnitude / speed * 200.0f;
        
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

        engineSound.pitch = 0.5f + (1.5f - 0.5f) * (velocity.magnitude / speed);
        
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
