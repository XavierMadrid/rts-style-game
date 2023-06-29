using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public abstract class ShootBehavior : MonoBehaviour
{
    [SerializeField] protected GameObject bulletPrefab = null;
    private GameObject closestShip;
    
    private bool targetDead;
    private bool shotReady;
    private float shootDelay = 2f;
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
        StartCoroutine(SearchForShipTargets());
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
    
    private IEnumerator SearchForShipTargets()
    {
        while (DoSearch()) // perhaps have a bool that is true when the correct ship list is nonempty.
        {
            float sqrRange = Range * Range;

            yield return searchDelayInterval;
            
            Vector3 currentPos = transform.position;

            var objects = GetTargetableObjects();
            
            // objects = isEnemy
            //     ? ManagerReferences.Instance.ShipController.ShipUnits
            //     : ManagerReferences.Instance.EnemyHandler.EnemyShips;

            float shortestSqrDist = sqrRange + 50;
            GameObject closestShip = null;

            foreach (var obj in objects)
            {
                float sqrDist = Vector3.SqrMagnitude(obj.transform.position - currentPos);

                if (!(sqrDist < sqrRange)) continue;
                
                if (sqrDist < shortestSqrDist)
                {
                    shortestSqrDist = sqrDist;
                    closestShip = obj;
                }
            }

            if (closestShip == null)
            {
                OnShipTargetLost?.Invoke(this, EventArgs.Empty);
                continue;
            }

            if (!targetDead && this.closestShip != null) this.closestShip.GetComponent<Ship>().OnHealthChanged -= IsTargetDead;
            this.closestShip = closestShip;
            OnShipTargetFound?.Invoke(closestShip, Mathf.Sqrt(shortestSqrDist));

            targetDead = false;

            closestShip.GetComponent<Ship>().OnHealthChanged += IsTargetDead;

            shootCdTime += changedTargetShootDelay;
                    
            while (!shotReady && !targetDead)
            {
                RotateTowardsTarget(closestShip.transform);

                yield return null;
            }

            while (!targetDead)
            {
                Shoot(closestShip.transform, 0, transform.rotation.eulerAngles.z);
                shootCdTime = shootDelay;
                shotReady = false;
                
                while (!shotReady && !targetDead)
                {
                    RotateTowardsTarget(closestShip.transform);
                            
                    yield return null;
                }

                yield return null;
            }
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
    
    private bool IsTargetDead(int health)
    {
        if (health <= 0)
        {
            targetDead = true;
            return true;
        }
        return false;
    }

    private void OnDisable()
    {
       // if (!targetDead) closestShip.GetComponent<Ship>().OnHealthChanged -= IsTargetDead;
    }
}
