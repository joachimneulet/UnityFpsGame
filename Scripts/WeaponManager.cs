using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;
    [SerializeField]
    private string weaponLayerName = "Weapon";
    [SerializeField]
    private Transform weaponHolder;

    private PlayerWeapon currentWeapon;
    private PlayerShoot ps;
    private WeaponGraphics currentGraphics;

    private AudioSource _shootingSound;

    public static bool isReloading = false;
    private bool isShooting = false;

    void Start()
    {
        EquipWeapon(primaryWeapon);
        ps = GetComponent<PlayerShoot>();
        _shootingSound = currentWeapon.shootingSound.GetComponent<AudioSource>();
    }

    public PlayerWeapon GetCurrentWeapon(){
      return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics(){
      return currentGraphics;
    }


    private void EquipWeapon(PlayerWeapon _weapon){
      currentWeapon = _weapon;

      GameObject _weaponIns = (GameObject)Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation);
      _weaponIns.transform.SetParent(weaponHolder);

      currentGraphics = _weaponIns.GetComponent<WeaponGraphics>();
      if(currentGraphics == null){
        Debug.LogError("Missing WeaponGraphics script on weapon: " + _weaponIns.name);
      }

      if(isLocalPlayer){
        Util.SetLayerRecursively(_weaponIns, LayerMask.NameToLayer(weaponLayerName));
      }
    }

    public void Reload()
    {
        if(isReloading == true)
        {
            return;
        }

        StartCoroutine(Reload_Coroutine());
    }

    public void Aim(){
      Animator anim = currentGraphics.GetComponent<Animator>();
      if(PlayerShoot.isAiming){
        anim.SetBool("Aiming", true);
      }else{
        anim.SetBool("Aiming", false);
      }
    }

    public void Shoot(){
      if(isShooting == true)
      {
          return;
      }

      isShooting = true;
      CmdOnShoot();
      isShooting = false;
    }

    public IEnumerator Reload_Coroutine()
    {
        isReloading = true;
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadTime);
        currentWeapon.currentAmmo = currentWeapon.maxCapacity;
        isReloading = false;
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [Command]
    void CmdOnShoot(){
      RpcOnShoot();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if(anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

    [ClientRpc]
    void RpcOnShoot()
    {
        _shootingSound.Play(0);
        Animator anim = currentGraphics.GetComponent<Animator>();
        if(anim != null)
        {
            anim.SetTrigger("Fire");
        }
    }

}
