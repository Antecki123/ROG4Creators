using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VendingMachinePanel : MonoBehaviour
{
    public static VendingMachinePanel instance;

    [Header("Vending Machine UI Panel")]
    [SerializeField] private GameObject weaponsGridPanel;
    [SerializeField] private GameObject slotPrefab;

    [Header("Weapon List")]
    [HideInInspector] public List<WeaponObject> availableWeapons;
    [HideInInspector] public List<Image> weaponSlot;

    private WeaponObject selectedWeapon;
    private int positionIndex = 0;

    [Header("Weapon Stats Panel")]
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponType;
    [SerializeField] private TextMeshProUGUI weaponDamage;
    [SerializeField] private TextMeshProUGUI weaponFireRate;
    [SerializeField] private TextMeshProUGUI weaponMagazine;
    [SerializeField] private TextMeshProUGUI weaponPrice;
    [Space]
    [SerializeField] private GameObject returnButton;

    [SerializeField] private Sprite activeButton;
    [SerializeField] private Sprite inactiveButton;
    private Color32 activeSlot = new Color32(0, 255, 80, 255);
    private Color32 inactiveSlot = new Color32(255, 255, 255, 140);

    [HideInInspector] public VendingMachine activeMachine;
    private InputControls inputActions;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        inputActions = new InputControls();
        inputActions.UI.Navigation.performed += ctx => ButtonNavigations((int)ctx.ReadValue<float>());
        inputActions.UI.Accept.performed += ctx => BuyWeapon();
        inputActions.UI.CloseMenu.performed += ctx => this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Enable();

        positionIndex = 0;
        SetWeaponOnEnable();
    }

    private void OnDisable()
    {
        inputActions.Disable();


    }

    private void Start()
    {
        // Create weapons slot
        foreach (var weapon in availableWeapons)
        {
            var slot = Instantiate(slotPrefab, weaponsGridPanel.transform.localPosition, weaponsGridPanel.transform.localRotation);
            slot.transform.SetParent(weaponsGridPanel.transform, false);
            weaponSlot.Add(slot.transform.GetChild(0).GetComponentInChildren<Image>());
        }

        // Set weapon images
        for (int i = 0; i < weaponSlot.Count; i++)
            weaponSlot[i].sprite = availableWeapons[i].weaponUI;

        SetWeaponOnEnable();
    }

    private void ButtonNavigations(int direction)
    {
        positionIndex += direction;
        positionIndex = Mathf.Clamp(positionIndex, 0, weaponSlot.Count);

        // Set active weapon slot
        if (positionIndex < weaponSlot.Count)
        {
            foreach (var slot in weaponSlot)
            {
                if (weaponSlot.IndexOf(slot) == positionIndex)
                    slot.color = activeSlot;
                else
                    slot.color = inactiveSlot;
            }

            selectedWeapon = availableWeapons[positionIndex];
            UpdateWeaponStatsPanel();
            returnButton.GetComponent<Image>().sprite = inactiveButton;
        }

        // Active RETURN button
        else if (positionIndex == weaponSlot.Count)
        {
            foreach (var slot in weaponSlot)
                slot.color = inactiveSlot;

            selectedWeapon = null;
            SetStatsNull();
            returnButton.GetComponent<Image>().sprite = activeButton;
        }
    }

    private void SetWeaponOnEnable()
    {
        // Set 1st weapon label as active
        if (availableWeapons.Count > 0)
        {
            foreach (var slot in weaponSlot)
                slot.color = inactiveSlot;

            selectedWeapon = availableWeapons[0];
            weaponSlot[0].color = activeSlot;
            returnButton.GetComponent<Image>().sprite = inactiveButton;
            UpdateWeaponStatsPanel();
        }
        else
        {
            selectedWeapon = null;
            returnButton.GetComponent<Image>().sprite = activeButton;
        }
    }

    private void UpdateWeaponStatsPanel()
    {
        weaponName.text = selectedWeapon.weaponName;
        weaponType.text = selectedWeapon.weaponType.ToString();
        weaponDamage.text = selectedWeapon.weaponDamage.ToString();
        weaponFireRate.text = $"{ 60 / selectedWeapon.weaponFireRate} RPM";
        weaponMagazine.text = selectedWeapon.weaponMagazineSize.ToString();
        weaponPrice.text = $"${selectedWeapon.weaponPrice}";

        if (PlayerManager.money < selectedWeapon.weaponPrice)
            weaponPrice.color = Color.red;
        else
            weaponPrice.color = Color.white;
    }

    private void SetStatsNull()
    {
        weaponName.text = null;
        weaponType.text = null;
        weaponDamage.text = null;
        weaponFireRate.text = null;
        weaponMagazine.text = null;
        weaponPrice.text = null;
    }

    private void BuyWeapon()
    {
        if (positionIndex == availableWeapons.Count)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (PlayerManager.money >= selectedWeapon.weaponPrice)
        {
            StartCoroutine(activeMachine.SpawnWeapon(selectedWeapon));
            UserInterface.instance.UpdatePoints((int)(-selectedWeapon.weaponPrice));
        }
    }
}