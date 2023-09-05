using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class ShootBehavior : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab = null;
    private GameObject closestTarget;
        
    protected float ShootDelay = 2f;

    private bool targetDeadVar;
    private bool targetDead
    {
        get => targetDeadVar;
        set
        {
            Debug.Log("targetDead changed to " + value);
            targetDeadVar = value;
        }
    }
    private bool shotReady;
    private float changedTargetShootDelay = .5f;
    private float shootCdTime;
    private readonly float rotSpeed = 200f;
    protected float Range = 30f;
    public float CurrentRange
    {
        get => Range;
        set
        {
            if (value <= 0)
            {
                Debug.LogError($"Range has been changed to an invalid range: {value}");
            }
            else
            {
                Range = value;
            }
        }
    }
    
    private WaitForSeconds searchDelayInterval = new(.05f);

    public event Action<GameObject, float> OnShipTargetFound;
    public event EventHandler<EventArgs> OnShipTargetLost;

    private void OnEnable()
    {
        StartCoroutine(SearchForTargets());
    }

    private void Update()
    {
        shootCdTime -= Time.deltaTime;

        shotReady = shootCdTime <= 0;
    }

    protected abstract ObservableCollection<GameObject> GetTargetableObjects();

    protected virtual bool DoSearch()
    {
        if (gameObject != null) return true;
        return false;
    }
    
    private IEnumerator SearchForTargets()
    {
        while (DoSearch()) // perhaps have a bool that is true when the correct ship list is nonempty.
        {
            float sqrRange = Range * Range;

            yield return searchDelayInterval;
            
            Vector3 currentPos = transform.position;

            var objects = GetTargetableObjects();

            float shortestSqrDist = sqrRange + 50;
            GameObject closestTarget = null;

            foreach (var obj in objects)
            {
                float sqrDist = Vector3.SqrMagnitude(obj.transform.position - currentPos);

                if (sqrDist > sqrRange) continue;
                
                if (sqrDist < shortestSqrDist)
                {
                    shortestSqrDist = sqrDist;
                    closestTarget = obj;
                }
            }

            if (this.closestTarget != null)
            {
                this.closestTarget.GetComponent<Targetable>().OnObjectTargetable -= TargetDead;
                Debug.Log("Unsubscribed to death.");
            }

            if (closestTarget == null)
            {
                OnShipTargetLost?.Invoke(this, EventArgs.Empty);
                Debug.Log(gameObject + ": continued.");
                continue;
            }

            Debug.Log(gameObject + ": closest object is: " + closestTarget);

            this.closestTarget = closestTarget;
            OnShipTargetFound?.Invoke(closestTarget, Mathf.Sqrt(shortestSqrDist));

            targetDead = false;
            closestTarget.GetComponent<Targetable>().OnObjectTargetable += TargetDead;
            Debug.Log("Resubscribed to death.");

            shootCdTime += changedTargetShootDelay;
                    
            while (!shotReady && !targetDead)
            {
                RotateTowardsTarget(closestTarget.transform); // maybe make this func more general for any necessary animations/movements

                yield return null;
            }

            while (!targetDead)
            {
                Debug.Log(gameObject + ": targetDead = " + targetDead);
                Shoot(closestTarget.transform, 0, transform.rotation.eulerAngles.z);
                shootCdTime = ShootDelay;
                shotReady = false;
                
                while (!shotReady && !targetDead)
                {
                    RotateTowardsTarget(closestTarget.transform);
                            
                    yield return null;
                }

                yield return null;
            }
            
            Debug.Log(gameObject + ": terminated search.");
        }
    }

    protected virtual void RotateTowardsTarget(Transform targetTransform)
    {
        Vector3 dir = (targetTransform.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(dir.y, dir.x);
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle * Mathf.Rad2Deg - 90);
        Quaternion currentRot = transform.rotation;

        transform.rotation = Quaternion.RotateTowards(currentRot, targetRot, Time.deltaTime * rotSpeed);
    }
    
    protected virtual void Shoot(Transform target, int damage, float shotAngle)
    {
        Vector3 pos = transform.position;
        Vector3 spawnPos = new Vector3(pos.x, pos.y, -2);
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.InitiateBulletValues(target.transform, shotAngle, damage);
    }
    
    private void TargetDead(GameObject hexObject, bool isTargetable)
    {
        Debug.Log(": Target is dead? = " + !isTargetable);
        targetDead = !isTargetable;
    }
}
