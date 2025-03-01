using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : PortalTraveller {

    public float walkSpeed = 3;
    public float runSpeed = 6;
    public float smoothMoveTime = 0.1f;
    public float jumpForce = 8;
    public float gravity = 18;

    public bool lockCursor;
    public float mouseSensitivity = 5;
    public Vector2 pitchMinMax = new Vector2(-40, 85); // Clamp values for pitch
    public float rotationSmoothTime = 0.1f;

    public Transform cameraPivot; // Empty GameObject to act as a camera pivot
    public Transform cameraTransform; // The actual camera
    public Vector3 cameraOffset = new Vector3(0, 2, -5); // Camera offset from the player

    CharacterController controller;
    float yaw;
    float pitch;
    float smoothYaw;
    float smoothPitch;

    float yawSmoothV;
    float pitchSmoothV;
    float verticalVelocity;
    Vector3 velocity;
    Vector3 smoothV;

    bool jumping;
    float lastGroundedTime;
    bool disabled;

    void Start() {
        if (lockCursor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        controller = GetComponent<CharacterController>();

        yaw = transform.eulerAngles.y;
        pitch = cameraPivot.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;

        // Initialize camera position
        if (cameraTransform != null && cameraPivot != null) {
            cameraTransform.position = cameraPivot.position + cameraPivot.TransformDirection(cameraOffset);
            cameraTransform.LookAt(cameraPivot);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Break();
        }
        if (Input.GetKeyDown(KeyCode.O)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            disabled = !disabled;
        }

        if (disabled) {
            return;
        }

        // Movement
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 inputDir = new Vector3(input.x, 0, input.y).normalized;
        Vector3 worldInputDir = transform.TransformDirection(inputDir);

        float currentSpeed = (Input.GetKey(KeyCode.LeftShift)) ? runSpeed : walkSpeed;
        Vector3 targetVelocity = worldInputDir * currentSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref smoothV, smoothMoveTime);

        verticalVelocity -= gravity * Time.deltaTime;
        velocity = new Vector3(velocity.x, verticalVelocity, velocity.z);

        var flags = controller.Move(velocity * Time.deltaTime);
        if (flags == CollisionFlags.Below) {
            jumping = false;
            lastGroundedTime = Time.time;
            verticalVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            float timeSinceLastTouchedGround = Time.time - lastGroundedTime;
            if (controller.isGrounded || (!jumping && timeSinceLastTouchedGround < 0.15f)) {
                jumping = true;
                verticalVelocity = jumpForce;
            }
        }

        // Camera rotation
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        RotateCamera(mouseX, mouseY);
        clampCameraY();

        // Update camera position
        if (cameraTransform != null && cameraPivot != null) {
            cameraTransform.position = cameraPivot.position + cameraPivot.TransformDirection(cameraOffset);
            cameraTransform.LookAt(cameraPivot);
        }
    }

    private void RotateCamera(float mouseX, float mouseY) {
        // Rotate the player horizontally (yaw)
        yaw += mouseX * mouseSensitivity;
        smoothYaw = yaw;
        transform.eulerAngles = Vector3.up * smoothYaw;

        // Rotate the camera pivot vertically (pitch)
        pitch -= mouseY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y); // Clamp pitch
        smoothPitch = pitch;
        cameraPivot.localEulerAngles = Vector3.right * smoothPitch;
    }

    private void clampCameraY() {
        Vector3 angles = cameraPivot.localEulerAngles;

        // Normalize the angle to be within -180 to 180
        if (angles.x > 180) {
            angles.x -= 360;
        }

        // Clamp the Up/Down rotation
        angles.x = Mathf.Clamp(angles.x, pitchMinMax.x, pitchMinMax.y);

        // Apply the clamped angles
        cameraPivot.localEulerAngles = angles;
    }
    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle (smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        transform.eulerAngles = Vector3.up * smoothYaw;
        velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (velocity));
        Physics.SyncTransforms ();
    }
}