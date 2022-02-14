using UnityEngine;
using System.Collections.Generic;

/**
 * Known issues:
 *  Character will get stuck when travelling up a ramp while simultaneously strafing into a wall, 
 *  if there is a gap between the ramp and the wall
 *  
 * Suspected Issues:
 *  I'm not convinced separating movement out into vertical and horizontal is a good idea, especially with regards
 *  to moving up ramps
 *  
 *  If you want to rotate the character a la mario galaxy, it will require some refactoring!
 */
public class KinematicBody : MonoBehaviour
{
    public float slopeLimit = 45f;
    public float stepOffset = 0.3f;
    public float skinWidth = 0.2f;

    public Vector3 overlapCenter = new Vector3();
    public float overlapRadius = 1f;

    private Vector3 m_upDirection;

    private readonly Collider[] m_overlaps = new Collider[5];
    private readonly List<RaycastHit> m_contacts = new List<RaycastHit>();

    private const int MaxSweepSteps = 5;
    private const float MinMoveDistance = 0f;

    public Vector3 velocity { get; set; }
    public bool isGrounded { get; private set; }

    private Rigidbody rb;
    private Collider c;

    private enum SweepType
    {
        LATERAL,
        VERTICAL
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        c = GetComponentInChildren<Collider>();
        InitializeRigidbody();
    }

    private void InitializeRigidbody()
    {
        rb.isKinematic = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void Move(Vector3 motion)
    {
        Vector3 startPosition;

        //Initialise variables
        {
            startPosition = rb.position;
            m_upDirection = transform.up;
            velocity = motion;

            m_contacts.Clear();
            isGrounded = false;

            //If we are not rising, we can set grounded at the start which is useful later
            if (motion.y <= 0)
            {
                RaycastHit hit;
                if (rb.SweepTest(Vector3.down, out hit, skinWidth + 0.01f))
                    isGrounded = true;
            }
        }

        //Collide and Slide
        if (velocity.sqrMagnitude > MinMoveDistance)
        {
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            Vector3 lateralVelocity = new Vector3(localVelocity.x, 0, localVelocity.z);
            Vector3 verticalVelocity = new Vector3(0, localVelocity.y, 0);

            lateralVelocity = transform.TransformDirection(lateralVelocity);
            verticalVelocity = transform.TransformDirection(verticalVelocity);

            CapsuleSweep(
                SweepType.LATERAL,
                lateralVelocity.normalized,
                lateralVelocity.magnitude,
                stepOffset
            );
            CapsuleSweep(
                SweepType.VERTICAL,
                verticalVelocity.normalized,
                verticalVelocity.magnitude,
                0
            );
        }

        //Handle contacts
        if (m_contacts.Count > 0)
        {
            float angle;

            foreach (RaycastHit contact in m_contacts)
            {
                angle = Vector3.Angle(m_upDirection, contact.normal);

                if (angle <= slopeLimit)
                {
                    isGrounded = true;
                }

                velocity -= Vector3.Project(velocity, contact.normal);
            }
        }

        //Depenetrate
        {
            int overlapsNum = Physics.OverlapSphereNonAlloc(
                transform.position + overlapCenter,
                overlapRadius,
                m_overlaps
            );

            if (overlapsNum > 0)
            {
                for (int i = 0; i < overlapsNum; i++)
                {
                    var otherCollider = m_overlaps[i];

                    if (otherCollider == c)
                        continue; // skip ourself

                    Vector3 otherPosition = otherCollider.gameObject.transform.position;
                    Quaternion otherRotation = otherCollider.gameObject.transform.rotation;

                    Vector3 direction;
                    float distance;

                    if (
                        Physics.ComputePenetration(
                            c,
                            c.transform.position,
                            c.transform.rotation,
                            otherCollider,
                            otherPosition,
                            otherRotation,
                            out direction,
                            out distance
                        )
                    )
                    {
                        rb.position += direction * (distance + skinWidth);
                        velocity -= Vector3.Project(velocity, -direction);
                    }
                }
            }
        }

        //Move
        {
            Vector3 m_position = rb.position;
            rb.position = startPosition;
            rb.MovePosition(m_position);
        }
    }

    private void CapsuleSweep(
        SweepType sweepType,
        Vector3 direction,
        float distance,
        float stepOffset
    )
    {
        Vector3 initialDirection = direction;
        RaycastHit hitInfo;
        RaycastHit stepHitInfo;
        Vector3 preSweepPosition;
        bool collision;
        bool stepCollision;

        bool stepEnabled = stepOffset > 0;

        //For Vertical, blocking angle is between 0 - slopeLimit
        //For Lateral, blocking angle is slopeLimit - 90 (grounded) or 360 (not grounded)
        float minBlockAngle = sweepType == SweepType.LATERAL ? slopeLimit : 0;
        float maxBlockAngle = slopeLimit;
        if (sweepType == SweepType.LATERAL)
            maxBlockAngle = isGrounded ? 360 : 90;

        for (int i = 0; i < MaxSweepSteps; i++)
        {
            preSweepPosition = rb.position;
            collision = rb.SweepTest(direction.normalized, out hitInfo, distance);

            if (stepEnabled)
            {
                float nudgeUpDistance = stepOffset;

                RaycastHit nudgeUpHitInfo;
                rb.SweepTest(m_upDirection, out nudgeUpHitInfo, stepOffset);
                if (nudgeUpHitInfo.collider != null)
                    nudgeUpDistance = nudgeUpHitInfo.distance - skinWidth;

                rb.position += Vector3.up * nudgeUpDistance;

                stepCollision = rb.SweepTest(direction.normalized, out stepHitInfo, distance * 2);
                bool stepAvoidedCollision = !stepCollision && collision;
                bool stepCollidedFurther =
                    stepCollision && collision && stepHitInfo.distance > hitInfo.distance + 0.001;
                bool stepHitSteepSlope =
                    stepCollision && Vector3.Angle(m_upDirection, stepHitInfo.normal) >= slopeLimit;
                bool walkHitShallowSlope =
                    collision && Vector3.Angle(m_upDirection, hitInfo.normal) < slopeLimit;

                if (
                    stepEnabled
                    && !walkHitShallowSlope
                    && !stepHitSteepSlope
                    && (stepAvoidedCollision || stepCollidedFurther)
                )
                {
                    IterateMove(
                        sweepType,
                        stepHitInfo,
                        initialDirection,
                        ref direction,
                        ref distance,
                        minBlockAngle,
                        maxBlockAngle
                    );

                    //Clamp rigibody back down
                    {
                        float downDistance = stepOffset; //Maximum we might clamp down
                        RaycastHit clampDownHitInfo;

                        if (rb.SweepTest(Vector3.down, out clampDownHitInfo, stepOffset))
                        {
                            downDistance = clampDownHitInfo.distance - skinWidth;
                        }
                        else
                        {
                            //TODO this is being thrown - figure out why (doesn't seem to cause any issues)
                            //Debug.LogWarning("KinematicBody detected a step that wasn't there :(", gameObject);
                        }

                        rb.position += Vector3.down * downDistance;
                    }

                    return; //Stepped!
                }
            }

            //Did not step
            {
                //Move rigibody back to pre step check position before continuing
                rb.position = preSweepPosition;
                IterateMove(
                    sweepType,
                    hitInfo,
                    initialDirection,
                    ref direction,
                    ref distance,
                    minBlockAngle,
                    maxBlockAngle
                );
            }
        }
    }

    private void IterateMove(
        SweepType sweepType,
        RaycastHit hitInfo,
        Vector3 initialDirection,
        ref Vector3 direction,
        ref float distance,
        float minBlockAngle,
        float maxBlockAngle
    )
    {
        float safeDistance = distance;
        if (hitInfo.collider != null)
            safeDistance = Mathf.Clamp(hitInfo.distance - skinWidth, 0, distance);

        if (safeDistance > 0)
        {
            rb.position += direction * safeDistance;
            distance -= safeDistance;
        }

        Vector3 projectionNormal = hitInfo.normal;

        float surfaceAngle = Vector3.Angle(m_upDirection, hitInfo.normal) - 0.001f;
        if ((surfaceAngle >= minBlockAngle) && (surfaceAngle <= maxBlockAngle))
        {
            if (sweepType == SweepType.LATERAL)
            {
                projectionNormal = new Vector3(hitInfo.normal.x, 0, hitInfo.normal.z);
            }
            if (sweepType == SweepType.VERTICAL)
            {
                projectionNormal = Vector3.up;
            }
        }

        /**
         * A bit confusing, I think!
         * 
         * For some situations such as climbing a ramp while also sort of strafing into a wall, it is important
         * that the 'collide and slide' off one follows the 'collide and slide' off the other. 
         * 
         * In some situations, such as running into a corner between 90-180 degrees, the collide and slides should converge
         * into each other.
         */
        var continueDirection = Vector3.ProjectOnPlane(direction, projectionNormal);
        var initialInfluenceDirection = Vector3.ProjectOnPlane(initialDirection, projectionNormal);
        direction =
            Vector3.Dot(continueDirection, initialDirection) < 0
                ? initialInfluenceDirection
                : continueDirection;
    }
}
