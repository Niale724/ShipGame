using UnityEngine;

public class NormalFish : BaseFish
{
    protected override int GetHpValue()
    {
        return 1; // Normal fish restores 1 HP
    }

    protected override void TriggerSpecialEffects(Submarine sub)
    {
        // Normal fish has no special effects
    }
}
