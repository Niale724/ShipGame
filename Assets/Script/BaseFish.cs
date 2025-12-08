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
    public override void ApplyCollectionEffects(Submarine sub)
    {
        TriggerSpecialEffects(sub);
    }
    protected virtual void TriggerSpecialEffects(Submarine sub)
    {
        // Default implementation does nothing
        // Derived classes can override to provide specific effects
    }
}


