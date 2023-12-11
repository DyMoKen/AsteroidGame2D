using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] [Range(100.0f, 500.0f)] private float maxSpeed = 5f;
    [SerializeField] [Range(5.0f, 50.0f)] private float acceleration = 5.0f;
    [SerializeField] [Range(5.0f, 90.0f)] private float rotationSpeed = 5.0f;
    [SerializeField] private GameObject deathVFX;
    
    private float _currentSpeed = 0.0f;
    private float _rotationInput = 0.0f;
    private bool _isAccelerating = false;
    private bool _isBraking = false;
    
    private Rigidbody2D _rb;
    private PlayerControlls _input = null;

    private void Awake()
    {
        _input = new PlayerControlls();
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.PC.Accelereation.started += AccelerationStarted;
        _input.PC.Accelereation.canceled += AccelerationCanceled;
        
        _input.PC.Brake.started += BrakeStarted;
        _input.PC.Brake.canceled += BrakeCanceled;
        
        _input.PC.Rotate.performed += OnRotationPerformed;
        _input.PC.Rotate.canceled += OnRotationCanceled;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.PC.Accelereation.started -= AccelerationStarted;
        _input.PC.Accelereation.canceled -= AccelerationCanceled;
        
        _input.PC.Brake.started -= BrakeStarted;
        _input.PC.Brake.canceled -= BrakeCanceled;
        
        _input.PC.Rotate.performed -= OnRotationPerformed;
        _input.PC.Rotate.canceled -= OnRotationCanceled;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Accelerate();
        Brake();
        Rotate();
        
    }

    private void Accelerate()
    {
        if (!_isAccelerating) return;
        
        if (_currentSpeed < maxSpeed)
            _currentSpeed += acceleration;
        else
        {
            _currentSpeed = maxSpeed;
        }
    
        _rb.velocity = GetFacingDirection() * (_currentSpeed * Time.fixedDeltaTime);
    }
    
    private void Brake()
    {
        if (!_isBraking) return;
        
        if (_currentSpeed - acceleration > 0)
            _currentSpeed -= acceleration;
        else
        {
            _currentSpeed = 0;
        }
            
        _rb.velocity = _rb.velocity.normalized * (_currentSpeed * Time.fixedDeltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
        Instantiate(deathVFX, transform.position, Quaternion.identity);
    }

    private void Rotate()
    {
        if (_rotationInput == 0) return;
        
        float rotation = _rotationInput * rotationSpeed * Time.fixedDeltaTime;
        _rb.MoveRotation(_rb.rotation - rotation);
    }
    
    private Vector2 GetFacingDirection()
    {
        float angle = Mathf.Deg2Rad * (transform.eulerAngles.z + 90.0f);

        Vector2 facingDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        return facingDir;
    }

    private void OnRotationPerformed(InputAction.CallbackContext ctx)
    {
        _rotationInput = ctx.ReadValue<float>();
    }
    
    private void OnRotationCanceled(InputAction.CallbackContext ctx)
    {
        _rotationInput = 0;
    }
    
    private void AccelerationStarted(InputAction.CallbackContext ctx)
    {
        _isAccelerating = true;
    }

    private void AccelerationCanceled(InputAction.CallbackContext ctx)
    {
        _isAccelerating = false;
    }
    
    private void BrakeStarted(InputAction.CallbackContext ctx)
    {
        _isBraking = true;
    }

    private void BrakeCanceled(InputAction.CallbackContext ctx)
    {
        _isBraking = false;
    }
}
