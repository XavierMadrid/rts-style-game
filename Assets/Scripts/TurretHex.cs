using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class TurretHex : EnergyHex
{
    [SerializeField] private GameObject bulletPrefab = null;
    private Color poweredColor = new(1, 0.8366f, 0, 1f);
    private SpriteRenderer sr;

    private bool shootingOn;
    private bool targetDead;
    private bool shotReady;
    private int damage = 1;
    private float shootDelay = 1f;
    private float changedTargetShootDelay = .5f;
    private float time;
    private float range = 50f;
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
    
    private WaitForSeconds searchDelayInterval = new(.05f);
    
    protected override void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        
        base.Start();

        StartCoroutine(SearchForTargets());
    }

    private void Update()
    {
        time -= Time.deltaTime;

        shotReady = time <= 0;
    }

    private IEnumerator SearchForTargets()
    {
        Debug.Log($"{HexActivated}, base: {base.HexActivated}");
        while (shootingOn && gameObject != null) // perhaps have a bool that is true when the correct ship list is nonempty.
        {
            yield return searchDelayInterval;
            
            float sqrRange = Range * Range;

            Vector3 currentPos = transform.position;

            ObservableCollection<GameObject> enemyShips = ManagerReferences.Instance.EnemyHandler.EnemyShips;

            float shortestSqrDist = sqrRange + 50;
            GameObject closestShip = null;

            foreach (var enemy in enemyShips)
            {
                float sqrDist = Vector3.SqrMagnitude(enemy.transform.position - currentPos);

                if (!(sqrDist < sqrRange)) continue;
                
                if (sqrDist < shortestSqrDist)
                {
                    shortestSqrDist = sqrDist;
                    closestShip = enemy;
                }
            }

            if (closestShip == null) continue;
            
            targetDead = false;

            closestShip.GetComponent<Ship>().OnHealthChanged += IsTargetDead;

            time += changedTargetShootDelay;
                    
            while (!shotReady && !targetDead) // delay upon switching targets of .5f sec
            {
                yield return null;
            }

            while (!targetDead)
            {
                Shoot(closestShip.transform);
                time = shootDelay;
                shotReady = false;
                
                while (!shotReady && !targetDead) // shooting delay of 2f sec
                {
                    yield return null;
                }

                yield return null;
            }
        }
    }

    private void Shoot(Transform target)
    {
        Vector3 pos = transform.position;
        Vector3 spawnPos = new Vector3(pos.x, pos.y, -2);
        
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        BulletBehavior bulletBehavior = bullet.GetComponent<BulletBehavior>();
        bulletBehavior.InitiateBulletValues(target.transform, transform.rotation.eulerAngles.z, damage);
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

    protected override void HexActivation(bool value)
    {
        shootingOn = value;
        if (shootingOn) StartCoroutine(SearchForTargets());
        
        StartCoroutine(value ? FadeToColor(HexBuilder.TURRET_HEX.HexColor, poweredColor) 
            : FadeToColor(poweredColor, HexBuilder.TURRET_HEX.HexColor));
    }
    
    private IEnumerator FadeToColor(Color startColor, Color targetColor)
    {
        float timeElapsed = 0;
        float lerpDuration = .75f;
        while (timeElapsed < lerpDuration)
        {
            sr.color = Color.Lerp(startColor, targetColor, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        sr.color = targetColor;
    }
}
