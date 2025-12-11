using UnityEngine;

public class HpSystem : MonoBehaviour
{
    [SerializeField] private int currentHp = 10;

    public event System.Action<int> OnHpChanged;
    public System.Action OnDeath;

    public int CurrentHp => currentHp;

    void Start()
    {
        OnHpChanged?.Invoke(currentHp);
        Debug.Log("Hp System started with hp: " + currentHp);
    }

    public int GetCurrentHp() => currentHp;

    public bool IsAlive() => currentHp > 0;

    public void IncreaseHP(int amount)
    {
        if (amount <= 0) return;

        currentHp += amount;
        OnHpChanged?.Invoke(currentHp);

        Debug.Log($"Increased HP by {amount}. Current HP: {currentHp}");
    }

    public void DecreaseHP(int amount)
    {
        if (amount <= 0) return;

        currentHp -= amount;
        if (currentHp < 0) currentHp = 0;

        OnHpChanged?.Invoke(currentHp);

        Debug.Log($"Decreased HP by {amount}. Current HP: {currentHp}");

        if (currentHp <= 0)
        {
            Debug.Log("Submarine has sunk.");
            OnDeath?.Invoke();
        }
    }
}
