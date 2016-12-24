using UnityEngine;
using System.Collections;

public class TimedDestruction : MonoBehaviour {

    public float Lifespan;
    public bool FlipDirection;

    void Awake()
    {
        Destroy(gameObject, Lifespan);
    }

    void Start()
    {
        transform.GetComponentInChildren<SpriteRenderer>().flipX = FlipDirection;
    }
}
