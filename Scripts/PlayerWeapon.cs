using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[System.Serializable]
public class PlayerWeapon
{
    public string name;
    public int damage;
    public float range;
    public int maxCapacity;
    [HideInInspector]
    public int currentAmmo;

    public AudioSource shootingSound;

    public float fireRate;

    public GameObject graphics;

    public float reloadTime;

    public float spreadFactor;

    public Vector3 hipSight;
    public Vector3 aimSight;

    public PlayerWeapon(){

    }
}
