using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private int maximumHealth = 100;

    [SerializeField] private IntVariable health_so;
    private int health_prim;

    private OnDiedHandler onDiedHandler;
    private OnHitHandler[] onHitHandlers;

    public int Value {
        get
        {
            return health_so != null ? health_so.Value : health_prim;
        }
        set
        {
            if (value < 0) value = 0;

            if (value == 0 && onDiedHandler != null)
                onDiedHandler.OnDied();

            if (value < Value)
                foreach (OnHitHandler onHitHandler in onHitHandlers)
                    onHitHandler.OnHit();

            if (health_so != null)
                health_so.Value = value;
            else
                health_prim = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        onHitHandlers = GetComponents<OnHitHandler>();
        onDiedHandler = GetComponent<OnDiedHandler>();
        Value = maximumHealth;
    }
}
