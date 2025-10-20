using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private Slider energySlider;
    [SerializeField] private TMP_Text energyText;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthText;

    [SerializeField] private Slider experienceSlider;
    [SerializeField] private TMP_Text experienceText;

    public GameObject pauseMenu;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // ensure the pause menu is hidden when the game starts
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
    }

    public void UpdateEnergySlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        energySlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        energySlider.value = cur;
        energyText.text = cur + " / " + Mathf.RoundToInt(max);
    }
    public void UpdateHealthSlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        healthSlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        healthSlider.value = cur;
        healthText.text = cur + " / " + Mathf.RoundToInt(max);
    }
     public void UpdateExperienceSlider(float current, float max)
    {
        // ensure slider max is set first, then assign a clamped integer value
        experienceSlider.maxValue = max;
        int cur = Mathf.Clamp(Mathf.RoundToInt(current), 0, Mathf.RoundToInt(max));
        experienceSlider.value = cur;
        experienceText.text = cur + " / " + Mathf.RoundToInt(max);
    }
}
