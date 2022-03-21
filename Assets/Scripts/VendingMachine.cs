using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [Header("Vending Machine")]
    public Transform weaponSpawner;
    [SerializeField] private List<WeaponObject> availableWeapons;

    public void OpenPanel()
    {
        UserInterface.instance.OpenVendingPanel(true);

        VendingMachinePanel.instance.availableWeapons = availableWeapons;
        VendingMachinePanel.instance.activeMachine = this;
    }

    public IEnumerator SpawnWeapon(WeaponObject weapon)
    {
        VendingMachinePanel.instance.gameObject.SetActive(false);
        if (weapon != null)
        {
            var newWeapon = Instantiate(weapon.weaponPrefab, weaponSpawner.position, weaponSpawner.rotation);
            newWeapon.GetComponent<Weapon>().weaponProperties = weapon;
        }
        yield return null;
    }
}