using UnityEngine;

public class Shield : Collectible
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Debug.Log("Shield collectible initialized.");
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }


    protected override void ApplyVisualEffects()
    {
        base.ApplyVisualEffects();
    }

    public override void MarkAsCollected()
    {
        if (!collected)
        {
            collected = true;
            OnCollected?.Invoke(this);

            Destroy(gameObject, 0.1f); 
            Debug.Log("Shield marked as collected and scheduled for destruction");
        }
    }
}
