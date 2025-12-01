using UnityEngine;

public class HpSystem : MonoBehaviour
{
    //I dont think we need a max hp field for now
    [SerializeField] private int currentHp=10;
    //This event will notify when hp changes
    //the two int parameters are current hp and the change in hp
    //ex: if current hp is 8 and we took 2 damage, the event will be invoked as OnHpChanged(8,-2)
    public System.Action<int,int> OnHpChanged;
    public System.Action OnDeath;
    public int CurrentHp => currentHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //This is to notify the current hp at the start of the game
        OnHpChanged?.Invoke(currentHp,0);
        //Just in case, log
        Debug.Log("Hp System started with hp: "+currentHp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetCurrentHp()
    {
        return currentHp;
    }

    public bool IsAlive()
    {
        return currentHp>0;
    }
    public void IncreaseHP(int amount)
    {
        if (amount <= 0) return; 
        currentHp += amount;
        OnHpChanged?.Invoke(currentHp, amount);
        Debug.Log("Increased HP by "+amount+". Current HP: "+currentHp);
    }

    public void DecreaseHP(int amount)
    {
        if (amount <= 0) return; 
        currentHp -= amount;
        //Set currentHp to 0 if it goes below 0
        if (currentHp < 0) currentHp = 0;
        //This notifies the current hp and the negative change
        OnHpChanged?.Invoke(currentHp, -amount);
        Debug.Log("Decreased HP by " + amount + ". Current HP: " + currentHp);
        if (currentHp <= 0)
        {
            Debug.Log("Submarine has sunk.");
            OnDeath?.Invoke();
        }
    }
}
