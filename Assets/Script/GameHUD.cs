using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HpSystem hpSystem;
    [SerializeField] private Submarine submarine;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI shieldText;
    [SerializeField] private TextMeshProUGUI fishText;

    private int fishCount = 0;

    private void Start()
    {
        if (hpSystem != null)
        {
            hpSystem.OnHpChanged += UpdateHP;
            UpdateHP(hpSystem.CurrentHp);
        }

        if (submarine != null)
        {
            submarine.OnShieldChanged += UpdateShield;
            UpdateShield(submarine.ShieldStacks);
        }

        SetFish(0);
    }

    private void OnDestroy()
    {
        if (hpSystem != null)
            hpSystem.OnHpChanged -= UpdateHP;

        if (submarine != null)
            submarine.OnShieldChanged -= UpdateShield;
    }

    private void UpdateHP(int newHP)
    {
        if (hpText != null)
            hpText.text = "HP: " + newHP;
    }

    private void UpdateShield(int newShield)
    {
        if (shieldText != null)
            shieldText.text = "Shield: " + newShield;
    }

    public void SetFish(int count)
    {
        fishCount = count;
        if (fishText != null)
            fishText.text = $"Fish: {fishCount}/100";
    }
}

