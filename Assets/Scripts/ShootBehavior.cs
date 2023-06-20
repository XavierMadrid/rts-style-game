using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Serialization;

public class ShootBehavior : MonoBehaviour
{
    [SerializeField] private GameObject DEV_trailDot = null;
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

        StartCoroutine(MoveBullet(bullet.transform, target.transform));
    }
    
    private IEnumerator MoveBullet(Transform bulletTransform, Transform targetTransform)
    {
        float t = 0;
        float angle = GetComponent<ShipBehavior>().ShipRotationRaw;
        float x = Mathf.Cos(angle);
        float y = Mathf.Sin(angle);

        Vector3 initialDir = new Vector3(x, y, 0); // direction the ship is facing

        Vector3 initialPos = transform.position;
        float initialDist = Vector3.Distance(targetTransform.position , initialPos); // total distance from ship to target

        Vector3 bulletPos = new Vector3(initialPos.x, initialPos.y, -2);
        
        bool targetReached = false;
        
        while (!targetReached)
        {
            if (targetDead)
            {
                targetReached = true;
                Destroy(bulletTransform.gameObject);
                yield break;
            }
            
            bulletTransform.position = bulletPos;
            
            Vector3 dist = targetTransform.position - bulletTransform.position;

            float straightSpeed = .1f; // speed of straight ejection from ship
            float curveSpeed = 10f; // speed of curving 
            float curveStopT = 1f; // how long in seconds until the bullet curving stops
            
            if (Vector2.Distance(bulletTransform.position, targetTransform.position) <= .1)
            {
                targetReached = true;

                Destroy(bulletTransform.gameObject);

                int remainingHealth = targetTransform.gameObject.GetComponent<Ship>().DamageShip(damage);
                Debug.Log($"RemainingHealth: {remainingHealth}");

                if (remainingHealth <= 0) targetDead = true;
            }
            else
            {
                bulletPos += new Vector3(curveSpeed * dist.x / initialDist * t * t * t + straightSpeed * initialDir.x * (curveStopT - t), 
                    curveSpeed * dist.y / initialDist * t * t * t + straightSpeed * initialDir.y * (curveStopT - t), 0); // cubic with linear term that falls off at curveStopT
            }

            // DEV_BulletTrail(bulletPos, t); // DEV

            if (t > 5)
            {
                Debug.LogError("Bullet failed to reach the within 0.1f units of the target within 5 seconds.");
                break;
            }

            t += Time.deltaTime;
            yield return null;
        }
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
    
    private void DEV_BulletTrail(Vector3 pos, float t) // DEV
    {
        GameObject trailDot = Instantiate(DEV_trailDot, pos, Quaternion.identity);

        t *= 1.6f;
        float tRed = 0;
        float tGreen = 0;
        float tBlue = 0;
        
        if (t > 6) t %= 6;
        t += 1;
        switch (t)
        {
            case > 1 and < 2:
                tRed = 1;   
                tGreen = t - 1;
                break;
            case > 2 and < 3:
                tRed = 1 - t % 2;
                tGreen = 1;
                break;
            case > 3 and < 4:
                tGreen = 1;
                tBlue = t - 3;
                break;
            case > 4 and < 5 :
                tGreen = 1 - t % 4;
                tBlue = 1;
                break;
            case > 5 and < 6:
                tBlue = 1;
                tRed = t - 5;
                break;
            case > 6 and < 7:
                tBlue = 1 - t % 6;
                tRed = 1;
                break;
        }

        trailDot.GetComponent<SpriteRenderer>().color = new Color(tRed, tGreen, tBlue, 1);
    }

    private void OnDestroy()
    {
        
    }
}
