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

    private IEnumerator SearchForTargets()
    {
        float sqrRange = range * range;
        while (true)
        {
            yield return searchDelayInterval;
            
            Vector3 currentPos = transform.position;

            ObservableCollection<GameObject> ships = isEnemy
                ? ManagerReferences.Instance.ShipController.ShipUnits
                : ManagerReferences.Instance.EnemyHandler.EnemyShips;
            
            foreach (var ship in ships)
            {
                if (Vector3.SqrMagnitude(ship.transform.position - currentPos) < sqrRange)
                {
                    yield return null;
                        
                    targetDead = false;
                    while (!targetDead)
                    {
                        Shoot(ship);

                        yield return shootDelay;
                    }

                    break;
                }
            }
        }
    }

    private void Shoot(GameObject target)
    {
        if (targetDead) return;
        target.GetComponent<Ship>().OnHealthChanged += IsTargetDead;
        
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.transform.position = new Vector3(bullet.transform.position.x, bullet.transform.position.y, -2);
        
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.InitiateBulletValues(target.transform, GetComponent<ShipBehavior>().ShipRotationRaw, GetComponent<Ship>().DamageStrength);
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
