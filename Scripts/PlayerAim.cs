using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [SerializeField]
    private Vector3 aimDownSights;
    [SerializeField]
    private Vector3 hipFire;
    [SerializeField]
    private float aimSpeed;


    void Update(){
      if(PlayerShoot.isAiming){
          AimDownSight(aimDownSights, aimSpeed);
      }
      else{
        AimFromHip(hipFire, aimSpeed);
      }
    }

    private void AimDownSight(Vector3 _aimSight, float _aimSpeed){
        transform.localPosition = Vector3.Slerp(transform.localPosition, _aimSight, _aimSpeed * Time.deltaTime);
    }

    private void AimFromHip(Vector3 _aimHip, float _aimSpeed){
      if(transform.localPosition != _aimHip){
        transform.localPosition = Vector3.Slerp(transform.localPosition, _aimHip, _aimSpeed * Time.deltaTime);
      }else{
        return;
      }

    }
}
