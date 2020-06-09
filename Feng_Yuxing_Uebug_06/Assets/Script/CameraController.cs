using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cameraControl;
    float fov;
    public float minFov, maxFov;
    public Transform playerTransform;
    protected float speed;
    protected float hSpeed = 0.50f;
    protected float vSpeed = 1.0f;
    protected Vector3 camOffset;

    // Start is called before the first frame update
    void Start()
    {
        this.speed = 10f;
        this.camOffset = cameraControl.transform.position - playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.ZoomCamer();
        this.MoveCamer();
    }
    public void ZoomCamer()
    {
        fov += Input.GetAxis("Mouse ScrollWheel") * speed;
        Mathf.Clamp(fov, minFov, maxFov);
        cameraControl.fieldOfView = fov;
    }
    public void MoveCamer()
    {
        if (Input.GetMouseButton(1))
        {
            Vector3 currentCameraPos = cameraControl.transform.position;
            float deltaX = Input.GetAxis("Mouse X") * hSpeed;
            float deltaY = Input.GetAxis("Mouse Y") * vSpeed;
            //move X
            currentCameraPos.x += deltaX;
            float angle = Vector3.Angle(cameraControl.transform.position - playerTransform.position, currentCameraPos - playerTransform.position);
            this.cameraControl.transform.RotateAround(playerTransform.position, new Vector3(0f, 1f, 0f), angle);
            currentCameraPos = cameraControl.transform.position;
            //move 
            if (currentCameraPos.y <= 16 && deltaY <= 0)
                return;  
            Debug.Log(cameraControl.fieldOfView);
            currentCameraPos.y += deltaY;
            angle = Vector3.Angle(cameraControl.transform.position - playerTransform.position, currentCameraPos - playerTransform.position);
            Vector3 axisRotate = Vector3.Cross(cameraControl.transform.position - playerTransform.position, currentCameraPos - playerTransform.position).normalized;
            currentCameraPos = cameraControl.transform.position;
            this.cameraControl.transform.RotateAround(playerTransform.position, axisRotate, angle);
            if (Input.GetKeyDown(KeyCode.F1))
            {
               
                this.FollowPlayer();
            }
        }
    }
    public void FollowPlayer()
    {
        cameraControl.transform.position = playerTransform.position + this.camOffset;// Vector3.Slerp(transform.position, playerTransform.position + this.CamOffset, 0.5f);
        transform.LookAt(playerTransform.position);
    }
}
