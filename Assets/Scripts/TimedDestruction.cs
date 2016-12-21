using UnityEngine;
using System.Collections;

public class TimedDestruction : MonoBehaviour {

    public float Lifespan;
    public bool FlipDirection;

    private SpriteRenderer[] childSprites;    

    void Awake()
    {
        Destroy(gameObject, Lifespan);
    }

    void Start()
    {
        childSprites = GetComponentsInChildren<SpriteRenderer>();

        transform.GetComponentInChildren<SpriteRenderer>().flipX = FlipDirection;
    }
}
