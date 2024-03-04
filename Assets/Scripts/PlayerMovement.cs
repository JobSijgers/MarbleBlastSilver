using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float rollForce;
    [SerializeField] private float jumpForce;

    [Header("Camera")]
    [SerializeField] private Transform cameraRig;
    [SerializeField] private Transform _Camera;

    private OrbitCamera _orbitCamera;
    private bool _noclipMode;
    private float xInput;
    private float zInput;
    private Rigidbody rb;
    private bool grounded;
    private Vector3 jumpDirection;
    private int contacts;
    private float yInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        PlayerJump();
        PlayerMove();
    }
    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        jumpDirection = collision.GetContact(0).normal;
        contacts++;

    }
    private void OnCollisionExit(Collision collision)
    {
        contacts--;
        if (contacts <= 0)
        {
            grounded = false;
        }
    }

    /// <summary>
    /// Movement for player
    /// </summary>
    private void PlayerMove()
    {
        xInput = Input.GetAxisRaw("Vertical") * Time.deltaTime * rollForce;
        zInput = Input.GetAxisRaw("Horizontal") * Time.deltaTime * rollForce;
        if (!_noclipMode)
        {
            rb.AddForce(cameraRig.forward * xInput, ForceMode.Acceleration);
            rb.AddForce(cameraRig.right * zInput, ForceMode.Acceleration);
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                yInput = 1 * Time.deltaTime * rollForce * 0.5f;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                yInput = -1 * Time.deltaTime * rollForce * 0.5f;
            }
            else
            {
                yInput = 0;
            }
            transform.position += xInput * 0.5f * _Camera.transform.forward + _Camera.right * zInput * 0.5f + yInput * _Camera.up;

        }
        
    }

    /// <summary>
    /// Jump fof player
    /// </summary>
    private void PlayerJump()
    {
        if (Input.GetKey(KeyCode.Space) && grounded && !_noclipMode)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            grounded = false;
        }
    }

    /// <summary>
    /// Sets the player velocity to 0 in every direction
    /// </summary>
    public void ResetVelocity()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    /// <summary>
    /// adds impulse force in the direction of the player
    /// </summary>
    /// <param name="Pamount">Amount of force</param>
    public void AddForceInPlayerDirection(float Pamount)
    {
        rb.AddForce(cameraRig.forward * Pamount, ForceMode.Impulse);
    }

    public void SetPlayerNoclipMode(bool Penabled)
    {
        _noclipMode = Penabled;
        if (_orbitCamera == null)
        {
            _orbitCamera = FindObjectOfType<OrbitCamera>();
        }
        _orbitCamera.SetCameraFreeMode(Penabled);
        rb.useGravity = !Penabled;
    }

}
