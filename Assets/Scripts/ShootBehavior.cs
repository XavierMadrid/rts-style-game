using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootBehavior : MonoBehaviour
{
    [SerializeField] private bool isEnemy = false;

    private bool targetDead = false;
    private int damage = 1; // is set in Start()
    private float time = 0;
    private bool shotReady;
    private float rotSpeed = 200f;
    private float range = 30f;
    public float Range
    {
        get => range;
        set => range = Mathf.Clamp(value, 0, 100);
    }
    
    [SerializeField] private GameObject bulletPrefab = null;
    
    private WaitForSeconds searchDelayInterval = new(.05f);
    private WaitForSeconds shootDelay = new(4f);
    
    private void Start()
    {
        damage = GetComponent<Ship>().DamageStrength;
        StartCoroutine(SearchForTargets());
    }

    private void Update()
    {
        time -= Time.deltaTime;

        if (time <= 0)
        {
            shotReady = true;
        }
        else
        {
            shotReady = false;
        }
    }

    private IEnumerator SearchForTargets()
    {
        float sqrRange = Range * Range;
        
        ObservableCollection<GameObject> ships;

        while (true)
        {
            yield return searchDelayInterval;
            
            Vector3 currentPos = transform.position;

            ships = isEnemy
                ? ManagerReferences.Instance.ShipController.ShipUnits
                : ManagerReferences.Instance.EnemyHandler.EnemyShips;
            
            foreach (var ship in ships)
            {
                if (Vector3.SqrMagnitude(ship.transform.position - currentPos) < sqrRange)
                {
                    targetDead = false;

                    ship.GetComponent<Ship>().OnHealthChanged += IsTargetDead;

                    time += .5f;
                    
                    while (!shotReady && !targetDead)
                    {
                        RotateShipTowardsTarget(ship.transform);

                        yield return null;
                    }

                    while (!targetDead)
                    {
                        Shoot(ship.transform);
                        time = 2f;
                        
                        while (!shotReady && !targetDead)
                        {
                            RotateShipTowardsTarget(ship.transform);
                            
                            yield return null;
                        }

                        yield return null;
                    }

                    break;
                }
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
