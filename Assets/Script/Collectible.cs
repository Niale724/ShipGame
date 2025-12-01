using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Base Properties")]
    [SerializeField] protected int ptValue;
    [SerializeField] protected float floatAmplitude = 0.5f;
    [SerializeField] protected float floatFrequency = 1f;

    protected Vector3 startPos;
    protected bool collected = false;
    
    //Trigger event for when the collectible is collected
    public System.Action<Collectible> OnCollected;
    
    public int PtValue => ptValue;
    public bool Collected => collected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        startPos = transform.position;
        Debug.Log($"{GetType().Name} spawned at position {startPos}");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (collected) return;
        ApplyVisualEffects();
    }

    protected virtual void ApplyVisualEffects()
    {
        // Float the collectible up and down
        // Just for some aesthetics ^w^
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    // Method to handle collection
    protected virtual void OnCollect(Submarine sub)
    {
        if (collected) return;
        collected = true;
        OnCollected?.Invoke(this);
        Debug.Log($"{GetType().Name} collected with point value {ptValue}");
        ApplyCollectionEffects(sub);
        Destroy(gameObject);
    }

    protected virtual void ApplyCollectionEffects(Submarine sub)
    {
        // Default implementation does nothing
        // Derived classes can override to provide specific effects
        // For example, trigger shield, heal submarine, etc.
    }

    public void SetPtValue(int value)
    {
        ptValue = value;
    }

}
