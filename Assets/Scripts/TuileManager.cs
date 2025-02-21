using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TuileManager : MonoBehaviour
{
    public int numberHorizontal;
    public int numberVertical;
    private GameObject[,] tileMap;
    public GameObject blandTile;
    public GameObject selected;
    public GameObject selectedTile;
    public List<GameObject> createdTiles;
    public int rotationAmount;
    public bool isPicking;
    public bool isPosing;
    public GameObject panel;
    public MultiplayerManager multiplayerManager;
    public List<GameObject> everyTile;
    public List<Transform> spawns;


    void Start()
    {
        tileMap = new GameObject[numberHorizontal, numberVertical];
        GenerateMapStart();
        StartPicking();
    }


    void Update()
    {
        if (multiplayerManager.players.Count > 0)
        {
            if (isPicking)
            {
                bool everyonePicked = true;
                foreach (GameObject g in multiplayerManager.players)
                {
                    if (g && g != null)
                    {
                        if (g.GetComponent<CarMovementPhysics>().selected == null)
                        {
                            everyonePicked = false; break;
                        }
                    }
                }
                if (everyonePicked)
                {
                    foreach (GameObject g in multiplayerManager.players)
                    {
                        createdTiles.Remove(g.GetComponent<CarMovementPhysics>().selected);

                        g.GetComponent<CarMovementPhysics>().cursor.SetActive(true);
                        g.GetComponent<PlayerInput>().SwitchCurrentActionMap("PoseTile");
                    }
                    isPosing = true;

                    isPicking = false;
                    panel.SetActive(false);
                    foreach (GameObject g in createdTiles)
                    {
                        Destroy(g);
                    }

                    createdTiles.Clear();

                }
            }
            else if (isPosing)
            {
                bool everyonePosed = true;

                foreach (GameObject g in multiplayerManager.players)
                {

                    if (g.GetComponent<CarMovementPhysics>().selected != null)
                    {
                        everyonePosed = false; break;
                    }
                    Debug.Log(g.GetComponent<CarMovementPhysics>().selected);
                }
                if (everyonePosed)
                {
                    foreach (GameObject g in multiplayerManager.players)
                    {
                        g.GetComponent<CarMovementPhysics>().cursor.SetActive(true);
                        g.GetComponent<PlayerInput>().SwitchCurrentActionMap("Car");
                    }
                    isPosing = false;
                    Debug.Log("start");
                }
            }
        }


    }


    void CreatePoolTiles()
    {
        foreach (Transform t in spawns)
        {
            int wichPiece = Random.Range(0, everyTile.Count);
            GameObject newPiece = Instantiate(everyTile[wichPiece].gameObject, t.position, everyTile[wichPiece].transform.rotation);
            createdTiles.Add(newPiece);
            foreach (Transform ti in newPiece.transform)
            {
                ti.gameObject.GetComponent<Collider>().enabled = true;
            }
        }
    }


    void GenerateMapStart()
    {
        for (int x = 0; x < numberHorizontal; x++)
        {
            for (int y = 0; y < numberVertical; y++)
            {
                GameObject instanciedTile = Instantiate(blandTile, new Vector3(x, -1, y), Quaternion.identity);
                tileMap[x, y] = instanciedTile;
                instanciedTile.transform.SetParent(transform);
            }
        }
    }










    public void StartPicking()
    {
        panel.SetActive(true);
        // GameObject newTile = Instantiate(allTiles,new Vector3(7, 13, 3),Quaternion.identity);
        CreatePoolTiles();

        selected = null;
        isPicking = true;

    }


    //tool functions 

    public void Apply(List<GameObject> allTiles, Transform t)
    {
        foreach (GameObject go in allTiles)
        {
            if (go.transform.position.x >= 0 && go.transform.position.z >= 0 && go.transform.position.x < numberHorizontal && go.transform.position.z < numberVertical)
            {
                GameObject gao = Instantiate(go);
                Destroy(tileMap[(int)(gao.transform.position.x + 0.1f), (int)(gao.transform.position.z + 0.1f)]);
                Destroy(go);
                tileMap[(int)(go.transform.position.x + 0.1f), (int)(go.transform.position.z + 0.1f)] = gao;
                gao.GetComponent<Collider>().enabled = true;
            }
        }
    }


    public void ChangeOpacity(float opacity, GameObject target)//change l'opacité seulement 
    {
        foreach (Material m in target.GetComponent<Renderer>().materials)
        {
            m.color = new Vector4(m.color.r, m.color.g, m.color.b, opacity);
        }
    }
    public void ChangeOpacity(float opacity, GameObject target, Vector3 color)//change l'opacité et la color
    {
        foreach (Material m in target.GetComponent<Renderer>().materials)
        {
            m.color = new Vector4(color.x, color.y, color.z, opacity);
        }
    }



    public void ChangeRender(bool targetState, GameObject Target)
    {
        if (Target.GetComponent<MeshRenderer>())
            Target.GetComponent<MeshRenderer>().enabled = targetState;


    }
}

