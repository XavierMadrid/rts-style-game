using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipBehavior : ShipBehavior
{
    private float speed = .01f;

    private bool currentlyMoving = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 mousePos = ManagerReferences.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, -1);
            Vector3 dir = mousePos - transform.position;
            float dist = dir.magnitude;
            ShipRotationRaw = Mathf.Atan2(dir.y, dir.x);
            transform.rotation = Quaternion.Euler(0, 0, ShipRotationRaw * Mathf.Rad2Deg - 90);

            if (currentlyMoving) currentlyMoving = false;
            StartCoroutine(MoveShip(mousePos, dist, speed));
        }
    }

    private IEnumerator MoveShip(Vector3 targetPos, float dist, float speed)
    {
        yield return null;
        
        currentlyMoving = true;
        float endingSpeed = 1;
        float duration = dist * speed;
        float timeElapsed = 0;
        
        while (currentlyMoving)
        {
            Vector3 currentPos = transform.position;

            if (Vector3.SqrMagnitude(transform.position - targetPos) <= .00125f) // slowing at the end
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, endingSpeed * speed);
                endingSpeed -= 2 * Time.deltaTime;

                if (endingSpeed < 0) // stop
                {
                    transform.position = targetPos;
                    break;
                }
            }
            else // default lerp
            {
                transform.position = Vector3.Lerp(currentPos, targetPos, timeElapsed * speed / duration);
            }
            
            if (timeElapsed >= 30)
            {
                Debug.LogError("Ship failed to meet its destination within 30 seconds.");
                break;
            }
            
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        currentlyMoving = false;
    }
}
