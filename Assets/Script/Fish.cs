using UnityEngine;

public enum FishType
{
    NORMAL,
    BURNING,
    EXPLODING
}
public class Fish : Collectible
{
    private FishType fishType = FishType.NORMAL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        int hpValue = fishType switch
        {
            FishType.NORMAL => 1,
            FishType.BURNING => 2,
            FishType.EXPLODING => 3,
            _ => 1
        };

        SetPtValue(hpValue);
        base.Start();
        Debug.Log("Fish " + fishType + " spawned, recovers HP value: " + hpValue);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

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

    private void TriggerSpecialEffects(Submarine sub)
    {
        switch (fishType)
        {
            case FishType.NORMAL:
                // No special effect
                break;
            case FishType.BURNING:
                // Implement burning effect from sprite here
                Debug.Log("Burning fish collected! Ouch!");
                break;
            case FishType.EXPLODING:
                // Implement explosion effect from sprite here
                Debug.Log("Exploding fish collected! Boom!");
                break;
            
        }
    }

    public void SetFishType(FishType type)
    {
        fishType = type;
    }

    public FishType GetFishType()
    {
        return fishType;
    }
}
