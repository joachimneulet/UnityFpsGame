using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    private PlayerWeapon currentWeapon;
    private WeaponManager weaponManager;
    private Player player;

    private bool _isDead;
    private float spreadFactor = 0.03f;

    private int maxAmmo = 100;
    private int currentMagazineAmmo;
    public static bool isAiming = false;

    public static bool hasHit = false;

    // Start is called before the first frame update
    void Start()
    {
      if(cam == null){
        Debug.LogError("No camera referenced");
        this.enabled = false;
      }

      weaponManager = GetComponent<WeaponManager>();

    }

    private void Update(){
      if(PauseMenu.isOn ==true){
        return;
      }
      currentWeapon = weaponManager.GetCurrentWeapon();
        if (currentWeapon.currentAmmo < currentWeapon.maxCapacity)
        {
          if (Input.GetButtonDown("Reload"))
          {
            weaponManager.Reload();
            return;
          }
        }
        if(!WeaponManager.isReloading){
          if(currentWeapon.fireRate <= 0){
            if(Input.GetButtonDown("Fire1")){
              Shoot();
            }
          }else{
            if(Input.GetButtonDown("Fire1")){
                InvokeRepeating("Shoot", 0f, 1f/currentWeapon.fireRate);
              }else if(Input.GetButtonUp("Fire1")){
              CancelInvoke("Shoot");
            }
          }
        }

        if(Input.GetButtonDown("Fire2") && PlayerController.isSprinting == false){
          isAiming = true;
          weaponManager.Aim();
        }
        if(Input.GetButtonUp("Fire2")){
          isAiming = false;
          weaponManager.Aim();
        }

        if(PlayerController.isSprinting == true){
          isAiming = false;
        }
    }

    [Client]
    private void Shoot(){
      if(!isLocalPlayer){
        return;
      }

      if(WeaponManager.isReloading){
        return;
      }

      if(currentWeapon.currentAmmo <= 0)
      {
          weaponManager.Reload();
          return;
      }

      currentWeapon.currentAmmo--;;

      weaponManager.Shoot();
      CmdOnShoot();

      Vector3 direction = cam.transform.forward;
      float newSpreadFactor = spreadFactor;
      if(isAiming){
        newSpreadFactor = spreadFactor/2;
      }

      if(PlayerController.isMoving){
        newSpreadFactor = spreadFactor*2;
      }
      if(PlayerController.isSprinting){
        newSpreadFactor = spreadFactor*4;
      }
      direction.x += Random.Range(-newSpreadFactor, newSpreadFactor);
      direction.y += Random.Range(-newSpreadFactor, newSpreadFactor);
      direction.z += Random.Range(-newSpreadFactor, newSpreadFactor);

      RaycastHit _hit;
      if(Physics.Raycast(cam.transform.position, direction, out _hit, currentWeapon.range, mask)){
        if(_hit.collider.tag == "Player"){
          hasHit = true;
          CmdPlayerShot(_hit.collider.name, currentWeapon.damage);
        }
        CmdOnHit(_hit.point, _hit.normal);
      }
      //hasHit = false;
      if(currentWeapon.currentAmmo <= 0)
      {
          weaponManager.Reload();
      }
    }

    [Command]
    void CmdOnHit(Vector3 _pos, Vector3 _normal){
      RpcDoHitEffect(_pos, _normal);
    }

    //Called by server when player shoots
    [Command]
    void CmdOnShoot(){
      RpcDoShootEffect();
    }

    //Instantiate shoot particles on all clients
    [ClientRpc]
    void RpcDoShootEffect(){
      weaponManager.GetCurrentGraphics().muzzleFlash.Play();
      int rand = Random.Range(-2, 2);
      if(rand == 0){
        weaponManager.GetCurrentGraphics().bulletTracer.Play();
      }

    }
    [ClientRpc]
    void RpcDoHitEffect(Vector3 _pos, Vector3 _normal){
      GameObject _hitEffect = (GameObject)Instantiate(weaponManager.GetCurrentGraphics().hitEffectPrefab, _pos, Quaternion.LookRotation(_normal));
      Destroy(_hitEffect, 2f);
    }

    [Command]
    private void CmdPlayerShot(string _playerID, float _damage){
      Debug.Log(_playerID + " has been hit");
      Player _player = GameManager.GetPlayer(_playerID);
      _player.RpcTakeDamage(_damage);
    }
}
