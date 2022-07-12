using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyCamController : MonoBehaviour {

    public HAL hal;

    private Camera cam;
    
    private bool follow_robot;

    public float move_speed;
    public float mouse_sensitivity;
    public DefaultInputActions FlyCamControls;

    private GameObject robot;

    private InputAction move;
    private InputAction look;
    private InputAction elev;
    private InputAction view;

    private Vector2 moveDir;
    private Vector2 lookDir;

    private float elevDir;

    private float zoomDir;
    
    private float zoom;
    
    private void Awake() {
        // Get the Camera component from this GameObject.
        cam = gameObject.GetComponent<Camera>();
        
        // Create Input Actions.
        FlyCamControls = new DefaultInputActions();
        
        // Lock the cursor so it isn't visible.
        Cursor.lockState = CursorLockMode.Locked;

        // I don't want to update the robot's position if there is no robot!
        // Assigning this to null will let me make sure there is a robot before
        // I try to move it.
        robot = null;
        
        // This is the default zoom value used when follow_robot is true.
        // Decreasing this value zooms in. Increasing this value zooms out.
        // Please don't make this a negative number.
        zoom = 1.0f;
    }
    
    // I assign the input actions and enable them here.
    private void OnEnable() {
        move = FlyCamControls.Player.Move;
        move.Enable();
        
        look = FlyCamControls.Player.Look;
        look.Enable();
    
        elev = FlyCamControls.Player.Elevate;
        elev.Enable();
        
        view = FlyCamControls.Player.ViewMode;
        view.Enable();
        
        // ToggleView is a function. This weird "+=" adds the function as a
        // callback(?) to this input action.
        view.performed += ToggleView;
    }

    private void ToggleView(InputAction.CallbackContext context) {
        // Flip the follow_robot bool.
        follow_robot = !follow_robot;
        
        // I want the view to be orthographic when following the robot.
        if (follow_robot) {
            cam.orthographic = true;
        } else {
            cam.orthographic = false;
        }
    }

    // Update() is called every frame.
    void Update() {
        if (!follow_robot) {
            // I am not following the robot, so I want to read input from the user.
            moveDir = move.ReadValue<Vector2>();
            lookDir += look.ReadValue<Vector2>() * mouse_sensitivity;
            elevDir = elev.ReadValue<float>();
            
            // This keeps you from looking backward!
            lookDir.y = Mathf.Clamp(lookDir.y, -90.0f, 90.0f);
        } else {
            // I am following the robot, so I will ignore input from the user (besides
            // the scroll wheel).
            moveDir = Vector2.zero;
            
            // Different OS's return different values when reading from the scroll wheel
            // for some reason. This ensures that zoomDir is either -1.0f or 1.0f (there
            // are no in between values).
            zoomDir = Mathf.Clamp(Mouse.current.scroll.ReadValue().y, -1.0f, 1.0f);
            
            // Update the current zoom. Notice that the farther you zoom out, the faster
            // the zoom value increases or decreases.
            zoom += 0.5f * (zoomDir * (0.33f * zoom));
            
            // I don't want you to zoom in closer than 0.5f or farther than 20.0f.
            zoom = Mathf.Clamp(zoom, 0.5f, 20.0f);
            
            // I don't like the zoom being "clicky." This quick lerp smoothes thingsa bit.
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoom, 15.0f * Time.deltaTime);
            
            if (!robot) {
                // robot was set to null in Awake() because hal.Robot would not exist yet.
                // However, by the time this Update() method is called, the Awake() method
                // of HAL has already been called. This gaurantees that hal.Robot will not
                // be null here (robot cannot be set to null again).
                robot = hal.Robot;
            }
            
            // Lerp the current camera's position to an overhead position.
            transform.position = Vector3.Lerp(transform.position, new Vector3(robot.transform.position.x, 3.0f, robot.transform.position.z), 10.0f * Time.deltaTime);
            
            // Snap the camera's rotation to a direct overhead rotation (maybe this could be a Quaternion.Slerp?).
            transform.rotation = Quaternion.Euler(90.0f, 0.0f, -robot.transform.rotation.eulerAngles.y);
        }
    }

    private void OnDisable() {
        // Disable all Input Actions.
        move.Disable();
        look.Disable();
        elev.Disable();
        view.Disable();
    }

    private void FixedUpdate() {
        // Move the camera in the direction of moveDir.
        transform.position += (transform.forward * moveDir.y + transform.right * moveDir.x + transform.up * elevDir).normalized * move_speed;
        
        // Rotate the camera by lookDir.
        transform.rotation = Quaternion.Euler(-lookDir.y, lookDir.x, 0.0f);
    }
}
