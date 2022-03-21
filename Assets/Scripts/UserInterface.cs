using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UserInterface : MonoBehaviour
{
    public static UserInterface instance;

    #region CLASS HEADING

    [Header("Active Weapon Indicator")]
    [SerializeField] private Image weaponUpperBorder;
    [SerializeField] private Image weaponUpperAmmoBorder;
    [SerializeField] private Image weaponUpperAmmoFill;
    [SerializeField] private TextMeshProUGUI weaponUpperName;
    [Space]
    [SerializeField] private Image weaponLowerBorder;
    [SerializeField] private Image weaponLowerAmmoBorder;
    [SerializeField] private Image weaponLowerAmmoFill;
    [SerializeField] private TextMeshProUGUI weaponLowerName;
    [Space]
    [SerializeField] private Image weaponUpperImage;
    [SerializeField] private Image weaponLowerImage;

    [Header("Player Stats")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image avatarHead;
    [SerializeField] private Image damageMarker;
    [SerializeField] private TextMeshProUGUI pointsNumber;

    [Header("Reload Popup")]
    [SerializeField] private GameObject reloadingPopup;
    [SerializeField] private Image reloadFill;

    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject vendingPanel;
    [SerializeField] private GameObject fadeScene;
    private bool isPauseMenuOpen = false;

    [Header("GameOver Screen")]
    [SerializeField] private GameObject overScreen;

    [Header("Boss Panel")]
    [SerializeField] private GameObject bossHealthPanel;
    [SerializeField] private Image bossHealthBar;
    [SerializeField] private TextMeshProUGUI bossName;

    #endregion

    private void Awake()
    {
        if (!instance)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // HUD - weapon panel ======================================
    //
    public void SetActiveWeapon(int index)
    {
        var colorVisible = new Color32(255, 255, 255, 255);
        var colorInvisible = new Color32(255, 255, 255, 0);

        if (index == 0)
        {
            weaponUpperBorder.color = colorVisible;
            weaponUpperAmmoBorder.color = colorVisible;
            weaponUpperAmmoFill.color = colorVisible;
            weaponUpperName.color = colorVisible;
            weaponUpperImage.color = colorVisible;

            weaponLowerBorder.color = colorInvisible;
            weaponLowerAmmoBorder.color = colorInvisible;
            weaponLowerAmmoFill.color = colorInvisible;
            weaponLowerName.color = colorInvisible;
            weaponLowerImage.color = colorInvisible;
        }

        else if (index == 1)
        {
            weaponUpperBorder.color = colorInvisible;
            weaponUpperAmmoBorder.color = colorInvisible;
            weaponUpperAmmoFill.color = colorInvisible;
            weaponUpperName.color = colorInvisible;
            weaponUpperImage.color = colorInvisible;

            weaponLowerBorder.color = colorVisible;
            weaponLowerAmmoBorder.color = colorVisible;
            weaponLowerAmmoFill.color = colorVisible;
            weaponLowerName.color = colorVisible;
            weaponLowerImage.color = colorVisible;
        }
    }

    public void SetWeaponIcons(Weapon weapon, int index)
    {
        if (index == 0)
        {
            weaponUpperImage.sprite = weapon.weaponProperties.weaponUI;
            weaponUpperName.text = weapon.weaponProperties.weaponName;
        }
        else if (index == 1)
        {
            weaponLowerImage.sprite = weapon.weaponProperties.weaponUI;
            weaponLowerName.text = weapon.weaponProperties.weaponName;
        }
    }

    public void AmmoIndicator(int actualAmmo, int maxAmmo, int index)
    {
        float actual = (float)actualAmmo;
        float max = (float)maxAmmo;
        if (index == 0)
            weaponUpperAmmoFill.fillAmount = actual / max;

        else if (index == 1)
            weaponLowerAmmoFill.fillAmount = actual / max;
    }
    //
    // =========================================================

    // HUD - player statistics =================================
    //
    public void UpdateHealthBar(float health, float maxHealth) =>
        healthBar.fillAmount = Mathf.Lerp(0, .7f, health / maxHealth);

    public void UpdatePoints(int reward)
    {
        PlayerManager.money += reward;
        pointsNumber.text = $"${PlayerManager.money}";
    }
    //
    // =========================================================

    // POPUP PANELS ============================================
    //
    public void OpenPausePanel()
    {
        isPauseMenuOpen = !isPauseMenuOpen;
        pausePanel.SetActive(isPauseMenuOpen);
    }

    public void OpenVendingPanel(bool state) => vendingPanel.SetActive(state);
    //
    // =========================================================

    public void GameOver() => overScreen.SetActive(true);

    public void BossFight(string bossName, bool isActive)
    {
        bossHealthPanel.SetActive(isActive);
        this.bossName.text = bossName;
    }

    public void UpdateBossHealth(float health, float maxHealth) =>
        bossHealthBar.fillAmount = health / maxHealth;

    // COROUTINES ==============================================
    //
    public IEnumerator AvatarHit(Color color, byte alpha)
    {
        avatarHead.color = color;
        damageMarker.color = new Color32(255, 255, 255, alpha);

        yield return new WaitForSeconds(.5f);
        avatarHead.color = Color.white;
        damageMarker.color = new Color32(255, 255, 255, 0);
    }

    public IEnumerator ReloadingPopupAnimation(float reloadTime)
    {
        reloadingPopup.SetActive(true);

        var timeOffset = .1f;
        reloadFill.fillAmount = 0;

        while (reloadTime >= 0)
        {
            reloadFill.fillAmount += timeOffset / reloadTime;
            reloadTime -= timeOffset;

            yield return new WaitForSeconds(timeOffset);
        }
        reloadingPopup.SetActive(false);
    }

    public IEnumerator FadeScene(float duration)
    {
        var faderAnim = fadeScene.GetComponent<Animator>();
        faderAnim.SetFloat("speedMultiplier", 1 / duration);
        faderAnim.SetTrigger("Fade");

        yield return null;
    }
}