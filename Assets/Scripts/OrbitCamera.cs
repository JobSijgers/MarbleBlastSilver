using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
    [SerializeField] private Transform _Player;
    [SerializeField] private Transform _Camera;
    [SerializeField] private float _CameraDistance = 5.0f;
    [SerializeField] private float _HorizontalSensitivity = 5.0f;
    [SerializeField] private float _VerticalSensitivity = 5.0f;
    [SerializeField] private Vector3 _CameraOffset;
    private bool _freeCameraMode;
    private EscapeMenuManager _EscapeMenuManager;
    void Start()
    {
        if (FindObjectOfType<EscapeMenuManager>() != null)
        {
            _EscapeMenuManager = FindObjectOfType<EscapeMenuManager>();
            _EscapeMenuManager.SensitivityChanceEvent += SetNewSensitivity;
        }
        

        //locks the cursor and sets the camera offset
        Cursor.lockState = CursorLockMode.Locked;
        _CameraOffset = transform.position - _Player.position;

        //sets the camera in the correct position
        transform.position = _Player.position + _CameraOffset - transform.forward * _CameraDistance;
        _Camera.transform.LookAt(_Player.transform.position);
    }

    void LateUpdate()
    {
        //rotates the camera around the player based on 'horizontalInput'
        float horizontalInput = Input.GetAxis("Mouse X");
        transform.RotateAround(_Player.position, Vector3.up, horizontalInput * _HorizontalSensitivity * Time.deltaTime);

        //if rightmb is pressed also rotate the camera verticaly based on the 'verticalInput'
        if (Input.GetMouseButton(1))
        {
            float verticalInput = Input.GetAxis("Mouse Y") * -1;
            _CameraOffset.y += verticalInput * _VerticalSensitivity * Time.deltaTime;
            //Here I clamp the offset so that it doest go in the ground nor the sky
            if (!_freeCameraMode)
            {
                _CameraOffset.y = Mathf.Clamp(_CameraOffset.y, 1f, 7f);
            }
        }

        //makes the cameraRig follow the player position
        transform.position = _Player.position + _CameraOffset - transform.forward * _CameraDistance;

        //look at player
        _Camera.transform.LookAt(_Player.transform.position);
    }
    private void OnDestroy()
    {
        _EscapeMenuManager.SensitivityChanceEvent -= SetNewSensitivity;
    }

    private void SetNewSensitivity(float horizontal, float verical)
    {
        _VerticalSensitivity = verical;
        _HorizontalSensitivity = horizontal;
    }
    public void SetCameraFreeMode(bool enabled)
    {
        _freeCameraMode = enabled;
    }
}
