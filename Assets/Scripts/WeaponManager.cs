using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon Controller")]
    [HideInInspector] public bool isFire;
    [SerializeField] private Transform armTransform;

    [SerializeField] private List<Weapon> weapons;
    private Weapon activeWeapon;

    private int weaponIndex = 0;
    private float coutdownTimer;

    private void Start() => activeWeapon = null;

    private void Update()
    {
        coutdownTimer -= Time.deltaTime;
        OnFire();

        // Reloading condition
        if (activeWeapon != null && activeWeapon.ActualAmmo <= 0 && !activeWeapon.IsReloading)
            StartCoroutine(activeWeapon.ReloadWeapon());

        // Ammunition amount bar
        if (activeWeapon != null)
            UserInterface.instance.AmmoIndicator(activeWeapon.ActualAmmo, activeWeapon.weaponProperties.weaponMagazineSize, weaponIndex);
    }

    #region ON INPUT CONTROLS

    public void OnFire()
    {
        if (activeWeapon != null && coutdownTimer <= 0 && isFire)
        {
            activeWeapon.Shoot();
            coutdownTimer = activeWeapon.FireRate;
        }
    }
    public void OnInteraction(GameObject item) => PickupItem(item);
    public void OnSwapWeapon() => SwapWeapon();
    public void OnReload() => StartCoroutine(activeWeapon.ReloadWeapon());

    #endregion

    private void SwapWeapon()
    {
        weaponIndex++;
        weaponIndex %= 2;

        if (weapons[weaponIndex] == null)
        {
            weaponIndex++;
            weaponIndex %= 2;
            return;
        }
        activeWeapon = weapons[weaponIndex];

        if (activeWeapon.IsReloading)
        {
            StopCoroutine(activeWeapon.ReloadWeapon());         //TODO: stop reloading when weapon swap
            StopCoroutine(UserInterface.instance.ReloadingPopupAnimation(.1f));
        }

        weapons.Find(weapon => weapon == weapons[weaponIndex]).gameObject.SetActive(true);
        weapons.Find(weapon => weapon != weapons[weaponIndex]).gameObject.SetActive(false);

        UserInterface.instance.SetActiveWeapon(weaponIndex);
    }

    private void PickupItem(GameObject item)
    {
        var index = 0;
        var newWeapon = item.GetComponent<Weapon>();

        // Drop active weapon when inventory is fulle
        if (weapons[0] != null && weapons[1] != null)
        {
            var weaponToThrow = weapons.Find(weapon => weapon == activeWeapon);
            index = weapons.IndexOf(weaponToThrow);

            weapons[index] = null;
            weaponToThrow.transform.parent = null;
            weaponToThrow.markerFX.SetActive(true);
        }

        // Find first available slot
        foreach (var weapon in weapons)
        {
            if (weapon == null)
            {
                weapons[index] = newWeapon;
                activeWeapon = newWeapon;

                newWeapon.markerFX.SetActive(false);
                newWeapon.transform.parent = armTransform;
                newWeapon.transform.position = armTransform.position;
                newWeapon.transform.rotation = armTransform.rotation;

                if (weapons[0] != null && weapons[1] != null)
                {
                    weapons.Find(weapon => weapon != newWeapon).gameObject.SetActive(false);
                    weapons.Find(weapon => weapon == newWeapon).gameObject.SetActive(true);
                }

                weaponIndex = index;
                UserInterface.instance.SetActiveWeapon(weaponIndex);
                UserInterface.instance.SetWeaponIcons(activeWeapon, weaponIndex);
                return;
            }
            index++;
        }
    }
}