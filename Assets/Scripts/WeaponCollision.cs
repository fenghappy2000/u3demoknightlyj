using UnityEngine;
using System.Collections;


public class WeaponCollision : MonoBehaviour
{
    Collider m_collider = null;
    void Awake()
    {
        m_collider = GetComponent<Collider>();
    }

    public bool colliderEanbled
    {
        set
        {
            m_collider.enabled = value;
        }
        get
        {
            return m_collider.enabled;
        }
    }

    public delegate void OnHitCallback(Collision collision);
    public OnHitCallback onHit;
    void OnCollisionEnter(Collision collision)
    {
        if (onHit != null)
            onHit(collision);
    }
}
