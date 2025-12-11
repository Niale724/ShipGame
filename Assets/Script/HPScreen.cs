using TMPro;
using UnityEngine;

public class HPScreen : MonoBehaviour
{
    private TextMeshProUGUI HPText;
    private HpSystem hpSystem;

    void Start()
    {
        Debug.Log("HPScreen Start() called");
        
        HPText = GetComponent<TextMeshProUGUI>();
        
        if (HPText == null)
        {
            return;
        }

        Submarine submarine = FindAnyObjectByType<Submarine>();

        if (submarine != null)
        {
            hpSystem = submarine.GetComponent<HpSystem>();

            if (hpSystem != null)
            {
                hpSystem.OnHpChanged += UpdateHPScreen;
                UpdateHPScreen(hpSystem.GetCurrentHp(), 0);
            }
    
        }
       
    }

    void UpdateHPScreen(int currentHP, int change)
    {
        
        if(HPText != null)
        {
            HPText.text = $"HP: {currentHP}";
        }
    }

    void OnDestroy()
    {
        if (hpSystem != null)
        {
            hpSystem.OnHpChanged -= UpdateHPScreen;
        }
    }
}