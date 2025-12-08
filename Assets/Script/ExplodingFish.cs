using UnityEngine;

public class ExplodingFish : BaseFish
{
   protected override int GetHpValue()
    {
        return 3; 
    }
    protected override void TriggerSpecialEffects(Submarine sub)
    {
        //waiting for special effects
    }
}
