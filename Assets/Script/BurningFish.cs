using UnityEngine;

public class BurningFish : BaseFish
{
    protected override int GetHpValue()
    {
        return 2; 
    }

    protected override void TriggerSpecialEffects(Submarine sub)
    {
        //waiting for special effects
    }
}
