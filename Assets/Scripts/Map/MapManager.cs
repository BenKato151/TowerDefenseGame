using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    #region Vars
    public GameObject path_parent;

    private static Dictionary<Vector2Int, GameObject> grid = new Dictionary<Vector2Int, GameObject>();
    private Transform spawnpoint;
    private GameObject[] mapPrefabs;
    private Vector3 fieldSize;
    private List<Transform> enemyAliveList = new List<Transform>();
    private List<Transform> enemyFullList = new List<Transform>();
    private int maxEnemies = 3;
    private float spawnTime = 0.5f;
    private IEnumerator coroutine;
    #endregion

    private void Start()
    {
        spawnpoint = GameObject.Find("Spawnpoint").transform;
        DatabaseController.Connection();
        mapPrefabs = Resources.LoadAll<GameObject>("Prefabs/Islands/");
        CreateMap();
        coroutine = SpawningCoroutine();
    }

    #region CreateMap
    private void CreateMap()
    {
        try
        {
            string[] rows = DatabaseController.SelectMap("1");
            int x_size = 9;
            int z_size = rows.Length;

            for (int z = 0; z < z_size; z++)
            {
                string row = rows[z];
                string[] ids = row.Split(',');

                for (int x = 0; x < x_size; x++)
                {
                    PlaceField(ids[x], x, z);
                }
            }

            foreach (KeyValuePair<Vector2Int, GameObject> item in grid)
            {
                if (item.Key.y == 0)
                {
                    if (item.Value.gameObject.GetComponent<Field>().fieldid == 16)
                    {
                        GameObject gate = Instantiate(Resources.Load<GameObject>("Prefabs/Other/Gate_SEV_001"), item.Value.gameObject.transform);
                        gate.transform.position = new Vector3(item.Value.transform.position.x, item.Value.transform.position.y + 5, item.Value.transform.position.z);
                        gate.transform.localEulerAngles = new Vector3(0, 0, 90);
                        item.Value.gameObject.GetComponent<Field>().isOccupied = true;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error:\n" + e);
            throw;
        }
    }
    #endregion

    #region PlaceField
    private void PlaceField(string fieldstring, int x, int z)
    {
        int fieldIndex = int.Parse(fieldstring);
        Vector2Int gridposition = new Vector2Int(x, z);
        GameObject newField = Instantiate(mapPrefabs[fieldIndex]);
        
        newField.transform.SetParent(this.gameObject.transform);
        fieldSize = newField.GetComponent<Renderer>().bounds.size;
        newField.transform.position = new Vector3(fieldSize.x * x, 0, fieldSize.z * z);
        newField.GetComponent<Field>().fieldid = fieldIndex;
        newField.GetComponent<Field>().position = gridposition;
        grid.Add(gridposition, newField);
    }
    #endregion

    public static Dictionary<Vector2Int, GameObject> GetGrid()
    {
        return grid;
    }

    #region Spawning Enemies
    public void StartSpawnEnemies()
    {
        StartCoroutine(coroutine);
    }

    public void EndSpawnEnemies()
    {
        StopCoroutine(coroutine);
    }

    public void SetMaxEnemy(int enemyNumber)
    {
        this.maxEnemies = enemyNumber;
    }

    private void SpawnEnemy()
    {
        GameObject enemies = Resources.Load<GameObject>("Prefabs/Enemies/Demon Enemy");
        GameObject enemy = Instantiate(enemies, spawnpoint.position, new Quaternion(), GameObject.Find("Enemies").transform);
        enemy.GetComponent<Enemy>();
        enemyAliveList.Add(enemy.transform);
        enemyFullList.Add(enemy.transform);
    }

    private IEnumerator SpawningCoroutine()
    {
        while (PhaseManager.isInFight)
        {
            List<Transform> tempList = new List<Transform>();
            foreach (var item in enemyAliveList)
            {
                if (item != null && item.gameObject.activeInHierarchy)
                {
                    //SN: Man kann sich nicht einfach Elemente aus der gerade zu durchsuchenden Liste wegnehmen... Ast auf dem man sitzt.
                    tempList.Add(item); 
                }
            }
            // die neue Liste enthält nur die noch lebenden Enemies.
            enemyAliveList = tempList; 
            if (enemyAliveList.Count < maxEnemies)
            {
                if (enemyFullList.Count > maxEnemies)
                {
                    // Möglicherweise Bug?
                    foreach (Transform item in enemyAliveList)
                    {
                        Destroy(item.gameObject);
                    }
                    enemyAliveList.Clear();
                    enemyFullList.Clear();
                    PhaseManager.EndFightPhase();
                    yield return null;
                }
                else
                {
                    Invoke("SpawnEnemy", spawnTime);
                }
            }
            yield return new WaitForSeconds(5);
        }
    }
    #endregion

}