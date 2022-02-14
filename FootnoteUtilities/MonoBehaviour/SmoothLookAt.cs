using UnityEngine;

public class SmoothLookAt : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float lookSpeed = 2f;
    [SerializeField]
    public bool lockVertical = true;
    [SerializeField]
    private bool defaultToCamera = true;

    private Vector3 staticTarget;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetStaticTarget(Vector3 newStaticTarget)
    {
        staticTarget = newStaticTarget;
    }

    private void Awake()
    {
        if (target == null && defaultToCamera)
            target = Camera.main.transform;
    }

    void Update()
    {
        Vector3 currentTarget = target != null ? target.position : staticTarget;
        if (currentTarget != null)
        {
            var lookPos = currentTarget - transform.position;
            if (lockVertical)
                lookPos.y = 0; //at somepoint lock max vertical angles probably

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(lookPos),
                Time.deltaTime * lookSpeed
            );
        }
    }
}
