using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 velocity;
    private Vector3 rotation;
    private float cameraRotationX = 0f;
    private float currentCameraRotationX = 0f;
    private Rigidbody rb;

    [SerializeField]
    private float cameraRotationLimit = 90f;

    private void Start(){
      rb = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity){
      velocity = _velocity;
    }

    public void Rotate(Vector3 _rotation){
      rotation = _rotation;
    }

    public void RotateCamera(float _cameraRotationX){
      cameraRotationX = _cameraRotationX;
    }

    private void FixedUpdate(){
      PerformMovement();
      PerformRotation();
    }

    private void PerformMovement(){
      if(velocity != Vector3.zero){
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
      }
    }

    private void PerformRotation(){
      // Récuperation de la rotation + Clamp la rotation
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        // Applique les changements à la caméra après le clamp
        cam.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
}
