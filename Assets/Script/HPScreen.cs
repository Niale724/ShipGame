using UnityEngine;
using TMPro;

public class HPScreen : MonoBehaviour
{
    [SerializeField] private HpSystem hpSystem;
    [SerializeField] private TextMeshProUGUI finalHPText;

    private void Start()
    {
        if (hpSystem != null)
        {
            hpSystem.OnHpChanged += UpdateHPScreen;
            UpdateHPScreen(hpSystem.CurrentHp);
        }
    }

    private void OnDestroy()
    {
        if (hpSystem != null)
            hpSystem.OnHpChanged -= UpdateHPScreen;
    }

    private void UpdateHPScreen(int hp)
    {
        if (finalHPText != null)
            finalHPText.text = "Final HP: " + hp;
    }
}
