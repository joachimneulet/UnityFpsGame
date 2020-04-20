using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[System.Serializable]
public class PlayerWeapon
{
    public string name = "AK74";
    public int damage = 10;
    public float range = 100f;
    public int maxCapacity = 30;
    [HideInInspector]
    public int currentAmmo;

    public AudioSource shootingSound;

    public float fireRate = 0f;

    public GameObject graphics;

    public float reloadTime = 1f;

    public PlayerWeapon(){
      currentAmmo = maxCapacity;
    }
}
