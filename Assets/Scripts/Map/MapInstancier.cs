using UnityEngine;

public class MapInstancier : MonoBehaviour
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject tileWin;
    [SerializeField] private GameObject tileStart;
    private GameObject[,] map;
    private GameObject parent;

    private void Awake()
    {
        map = new GameObject[10, 10];
    }


    void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int i = 0; i < 10; i++)
        {
            parent = new GameObject("ligne " + i);
            for (int j = 0; j < 10; j++)
            {
                if (j == 0 && i == 0)
                {
                    map[i, j] = Instantiate(tileStart, new Vector3(j * 2, 0, i * 2), tile.transform.rotation, parent.transform);
                }
                else if (j == 9 && i == 9)
                {
                    map[i, j] = Instantiate(tileWin, new Vector3(j * 2, 0, i * 2), tile.transform.rotation, parent.transform);
                }
                else
                {
                    map[i, j] = Instantiate(tile, new Vector3(j * 2, 0, i * 2), tile.transform.rotation, parent.transform);
                }
            }
        }
    }

}


