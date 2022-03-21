using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/New Weapon")]
public class WeaponObject : ScriptableObject
{
    public enum WeaponType { Pistol, Machinegun, Shotgun , Rifle}

    public string weaponName;
    public WeaponType weaponType;

    [Header("Weapon Stats")]
    public float weaponDamage;
    public float weaponFireRate;
    public int weaponMagazineSize;
    public float weaponPrice;
    [Space]
    public float weaponReloadingTime;
    public float weaponBulletVelocity;
    [Space]
    public Sprite weaponImage;
    public Sprite weaponUI;
    public GameObject weaponPrefab;
}