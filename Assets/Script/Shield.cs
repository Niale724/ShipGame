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

    protected override void ApplyCollectionEffects(Submarine sub)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnShieldCollected(this);
        }
        else
        {
            Debug.LogWarning("GameManager instance is not available.");
        }
    }

    protected override void ApplyVisualEffects()
    {
        base.ApplyVisualEffects();
    }
}
