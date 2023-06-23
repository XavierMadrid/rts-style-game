using System.Collections.ObjectModel;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] private GameObject enemyShipUnitPrefab = null;

    public ObservableCollection<GameObject> EnemyShips = new();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 spawnPos = ManagerReferences.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, -1f);
            GameObject enemyShipClone = Instantiate(enemyShipUnitPrefab, spawnPos, Quaternion.identity);
            
            EnemyShips.Add(enemyShipClone);
        }
    }
}
