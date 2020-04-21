using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivityX = 5f;
    [SerializeField]
    private float lookSensitivityY = 5f;

    private PlayerMotor motor;

    public static bool isSprinting = false;
    public static bool isMoving = false;

    private void Start(){
      motor = GetComponent<PlayerMotor>();
    }

    private void Update(){
      //Check if character is moving
      if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0){
        isMoving = true;
      }else{
        isMoving = false;
      }

      if(Input.GetButton("Sprint")){
        isSprinting = true;
        speed = 10f;
      }else{
        isSprinting = false;
        speed = 5f;
      }

      if(PauseMenu.isOn ==true){
        //Unlock Mouse
        if(Cursor.lockState != CursorLockMode.None){
          Cursor.visible = true;
          Cursor.lockState = CursorLockMode.None;
        }
        //Lock movement and mouse rotation on pause
        motor.Move(Vector3.zero);
        motor.Rotate(Vector3.zero);
        motor.RotateCamera(0f);
        return;
      }
      //Lock Mouse
      if(Cursor.lockState != CursorLockMode.Locked){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
      }
    }

    private void FixedUpdate(){
      float _xMov = Input.GetAxisRaw("Horizontal");
      float _zMov = Input.GetAxisRaw("Vertical");

      Vector3 _movHorizontal = transform.right * _xMov;
      Vector3 _movVertical = transform.forward * _zMov;

      Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

      motor.Move(_velocity);

      //Calcul rotation joueur
      float _yRot = Input.GetAxisRaw("Mouse X");
      Vector3 _rotation = new Vector3(0, _yRot, 0) * lookSensitivityX;

      motor.Rotate(_rotation);

      float _xRot = Input.GetAxisRaw("Mouse Y");
      float _cameraRotationX = _xRot * lookSensitivityY;

      motor.RotateCamera(_cameraRotationX);
    }
}
