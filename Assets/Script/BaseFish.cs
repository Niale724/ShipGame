using UnityEngine;

public abstract class BaseFish : Collectible
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        SetPtValue(GetHpValue());
        base.Start();
        Debug.Log($"{GetType().Name} spawned, recovers HP value: {ptValue}");
    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    protected abstract int GetHpValue();
    protected override void ApplyCollectionEffects(Submarine sub)
    {
        HpSystem hpSystem = sub.GetComponent<HpSystem>();
        if (hpSystem != null)
        {
            hpSystem.IncreaseHP(ptValue);
            Debug.Log("Fish collected, HP+ " + ptValue);
        }

        TriggerSpecialEffects(sub);
    }
    protected virtual void TriggerSpecialEffects(Submarine sub)
    {
        // Default implementation does nothing
        // Derived classes can override to provide specific effects
    }
}


