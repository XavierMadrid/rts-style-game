using System.Collections;
using System.Collections.ObjectModel;
using UnityEngine;

public class ShootBehavior : MonoBehaviour
{
    [SerializeField] private bool isEnemy = false;

    private bool targetDead;
    private bool shotReady;
    private float shootDelay = 2f;
    private float changedTargetShootDelay = .5f;
    private float time;
    private readonly float rotSpeed = 200f;
    private float range = 30f;
    public float Range
    {
        get => range;
        set
        {
            if (value <= 0)
            {
                Debug.LogError($"Range has been changed to an invalid range: {value}");
            }
            else
            {
                range = value;
            }
        }
    }
    
    [SerializeField] private GameObject bulletPrefab = null;
    
    private WaitForSeconds searchDelayInterval = new(.05f);
    
    private void Start()
    {
        StartCoroutine(SearchForTargets());
    }

    private void Update()
    {
        time -= Time.deltaTime;

        shotReady = time <= 0;
    }

    private IEnumerator SearchForTargets()
    {
        float sqrRange = Range * Range;
        
        ObservableCollection<GameObject> ships;

        while (gameObject != null) // perhaps have a bool that is true when the correct ship list is nonempty.
        {
            yield return searchDelayInterval;
            
            Vector3 currentPos = transform.position;

            ships = isEnemy
                ? ManagerReferences.Instance.ShipController.ShipUnits
                : ManagerReferences.Instance.EnemyHandler.EnemyShips;

            float shortestSqrDist = sqrRange + 50;
            GameObject closestShip = null;

            foreach (var ship in ships)
            {
                float sqrDist = Vector3.SqrMagnitude(ship.transform.position - currentPos);

                if (!(sqrDist < sqrRange)) continue;
                
                if (sqrDist < shortestSqrDist)
                {
                    shortestSqrDist = sqrDist;
                    closestShip = ship;
                }
            }

            if (closestShip == null) continue;
            
            targetDead = false;

            closestShip.GetComponent<Ship>().OnHealthChanged += IsTargetDead;

            time += changedTargetShootDelay;
                    
            while (!shotReady && !targetDead)
            {
                RotateShipTowardsTarget(closestShip.transform);

                yield return null;
            }

            while (!targetDead)
            {
                Shoot(closestShip.transform);
                time = shootDelay;
                shotReady = false;
                
                while (!shotReady && !targetDead)
                {
                    RotateShipTowardsTarget(closestShip.transform);
                            
                    yield return null;
                }

                yield return null;
            }
        }
    }

    private void RotateShipTowardsTarget(Transform targetTransform)
    {
        Vector3 dir = (targetTransform.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(dir.y, dir.x);
        Quaternion targetRot = Quaternion.Euler(0, 0, targetAngle * Mathf.Rad2Deg - 90);
        Quaternion currentRot = transform.rotation;

        transform.rotation = Quaternion.RotateTowards(currentRot, targetRot, Time.deltaTime * rotSpeed);
    }
    
    private void Shoot(Transform target)
    {
        Vector3 pos = transform.position;
        Vector3 spawnPos = new Vector3(pos.x, pos.y, -2);
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.InitiateBulletValues(target.transform, transform.rotation.eulerAngles.z, GetComponent<Ship>().DamageStrength);
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
}
