using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    [SerializeField] private GameObject DEV_trailDot = null;

    private int damage;
    private float t;
    private float initialDist;
    private float maxTravelTime = 5f;
    private bool targetDead;
    private bool valuesInitiated;
    
    private Vector3 initialDir;
    
    private float straightSpeed = .1f; // speed of straight ejection from ship
    private float curveSpeed = 10f; // speed of curving
    private float curveStopT = 1f; // how long in seconds until the bullet curving stops

    private Transform targetTransform;
    
    public void InitiateBulletValues(Transform targetTransform, float shipRotationRadians, int damage)
    {
        targetTransform.GetComponent<Ship>().OnHealthChanged += IsTargetDead;

        float x = Mathf.Cos(shipRotationRadians);
        float y = Mathf.Sin(shipRotationRadians);

        initialDir = new Vector3(x, y, 0); // direction the ship is facing

        initialDist = Vector3.Distance(targetTransform.position , transform.position); // total distance from ship to target
        
        this.targetTransform = targetTransform;
        this.damage = damage;
        
        valuesInitiated = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!valuesInitiated) return;
        
        if (targetDead)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dist = targetTransform.position - transform.position;

        if (Vector2.Distance(transform.position, targetTransform.position) <= .2)
        {
            TargetReached();
            return;
        }
            
        // cubic with linear term that falls off at curveStopT
        transform.position += new Vector3(curveSpeed * dist.x / initialDist * t * t * t + straightSpeed * initialDir.x * (curveStopT - t), 
            curveSpeed * dist.y / initialDist * t * t * t + straightSpeed * initialDir.y * (curveStopT - t), 0); 
        
        if (t > maxTravelTime)
        {
            Debug.LogError("Bullet failed to reach the within 0.1f units of the target within 5 seconds.");
            Destroy(gameObject);
        }
        
        // DEV_BulletTrail(bulletPos, t); // DEV

        t += Time.deltaTime;
    }

    private void TargetReached()
    {
        int remainingHealth = targetTransform.GetComponent<Ship>().DamageShip(damage);

        if (remainingHealth <= 0) targetDead = true;
        
        targetTransform.GetComponent<Ship>().OnHealthChanged -= IsTargetDead;

        Destroy(gameObject);
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
}
