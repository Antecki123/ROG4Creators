using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Component References")]
    [HideInInspector] public WeaponObject weaponProperties;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    public GameObject markerFX;
    [Space]

    [SerializeField, Tooltip("Only for shotguns")]
    private float maxBulletDispersion = 10f;

    private int actualAmmo;
    private bool isReloading = false;

    private Renderer render;
    private MaterialPropertyBlock materialProperty;

    public float FireRate { get => weaponProperties.weaponFireRate; private set { } }
    public int ActualAmmo { get => actualAmmo; private set { } }
    public int MaxAmmoAmount { get => weaponProperties.weaponMagazineSize; private set { } }
    public bool IsReloading { get => isReloading; private set { } }

    public Sprite WeaponImage { get => weaponProperties.weaponUI; private set { } }

    private void Awake()
    {
        materialProperty = new MaterialPropertyBlock();
        render = GetComponent<Renderer>();

        StartCoroutine(SpawnWeapon());
    }

    private void Start() => actualAmmo = weaponProperties.weaponMagazineSize;

    public void Shoot()
    {
        if (!isReloading && actualAmmo > 0)
        {
            switch (weaponProperties.weaponType)
            {
                case WeaponObject.WeaponType.Pistol:
                    StartCoroutine(ShotPistol());
                    break;
                case WeaponObject.WeaponType.Machinegun:
                    StartCoroutine(ShotRifle());
                    break;
                case WeaponObject.WeaponType.Shotgun:
                    StartCoroutine(ShotShotgun());
                    break;
                case WeaponObject.WeaponType.Rifle:
                    StartCoroutine(ShotRifle());
                    break;
                default:
                    break;
            }
        }
    }

    private IEnumerator SpawnWeapon()
    {
        var progress = 0f;
        var spawnSpeed = 200f;

        while (progress < 5f)
        {
            materialProperty.SetFloat("_Progress", progress);
            render.SetPropertyBlock(materialProperty);

            progress += .01f;
            yield return new WaitForSeconds(1 / spawnSpeed);
        }
    }

    public IEnumerator ReloadWeapon()
    {
        if (actualAmmo < weaponProperties.weaponMagazineSize && !isReloading)
        {
            isReloading = true;

            StartCoroutine(UserInterface.instance.ReloadingPopupAnimation(weaponProperties.weaponReloadingTime));
            SoundManager.instance.Play("Reload");

            yield return new WaitForSeconds(weaponProperties.weaponReloadingTime);
            actualAmmo = weaponProperties.weaponMagazineSize;

            isReloading = false;
        }
    }

    private IEnumerator ShotRifle()
    {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletProperties = bullet.GetComponent<Bullet>();
        bulletProperties.damage = weaponProperties.weaponDamage;
        bulletProperties.speed = weaponProperties.weaponBulletVelocity;

        actualAmmo--;
        SoundManager.instance.Play("Shot_Rifle");

        Destroy(bullet, 3f);

        yield return null;
    }

    private IEnumerator ShotShotgun()
    {
        var bulletNumber = Random.Range(5f, 10f);

        for (int i = 0; i < bulletNumber; i++)
        {
            var randomAngle = Random.Range(-maxBulletDispersion, maxBulletDispersion);
            var bulletDirection = Quaternion.Euler(0, transform.rotation.eulerAngles.y, randomAngle);

            var bullet = Instantiate(bulletPrefab, firePoint.position, bulletDirection);
            var bulletProperties = bullet.GetComponent<Bullet>();
            bulletProperties.damage = weaponProperties.weaponDamage;
            bulletProperties.speed = weaponProperties.weaponBulletVelocity;

            Destroy(bullet, 3f);
        }
        actualAmmo--;
        SoundManager.instance.Play("Shot_Pump");

        yield return null;
    }

    private IEnumerator ShotPistol()
    {
        for (int i = 0; i < 2; i++)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            var bulletProperties = bullet.GetComponent<Bullet>();
            bulletProperties.damage = weaponProperties.weaponDamage;
            bulletProperties.speed = weaponProperties.weaponBulletVelocity;

            actualAmmo--;
            SoundManager.instance.Play("Shot_Pistol");

            Destroy(bullet, 3f);
            yield return new WaitForSeconds(.1f);
        }
    }
}