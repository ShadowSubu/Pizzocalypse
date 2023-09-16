using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float ViewRadius;
    [Range(0,360)]
    public float ViewAngle;

    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public Animator animator;

    void Start()
    {
        StartCoroutine("FindTargetsWithDelay", .2f);
    }


    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, TargetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < ViewAngle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, ObstacleMask))
                {
                    visibleTargets.Add(target);
                    animator.SetBool("IsChasing", true);
                    animator.SetBool("IsPatrolling", false);
                }
            }
        }

    }

    public Vector3 DirectionFromAngle(float angleInDegrees,bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;  
        }
        return new Vector3(Mathf.Sin(angleInDegrees* Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
