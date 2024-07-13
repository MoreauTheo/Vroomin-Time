using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class TuileManager : MonoBehaviour
{
    public int numberHorizontal;
    public int numberVertical;
    private GameObject[,] tileMap;
    public GameObject blandTile;
    public GameObject selected;
    public GameObject selectedTile;
    public List<GameObject> previewTiles;
    public int rotationAmount;
    public bool isPicking;
    public bool isPosing;
    public GameObject panel;
    public MultiplayerManager multiplayerManager;
    public GameObject allTiles;
    void Start()
    {
        tileMap = new GameObject[numberHorizontal, numberVertical];
        GenerateMapStart();


    }

    // Update is called once per frame
    void Update()
    {
        if (isPicking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = CastRay();
                if (hit.collider.tag != "panel")
                {
                    selected = hit.collider.transform.parent.gameObject;
                    hit.collider.transform.parent.position = new Vector3(-10, -10, -10);
                    panel.SetActive(false);
                    isPicking = false;
                    isPosing = true;

                    foreach (Transform t in hit.collider.transform.parent)
                    {
                        t.gameObject.GetComponent<Collider>().enabled = false;
                    }

                }
            }
        }
        else if (isPosing)
        {
            if (Input.GetMouseButtonDown(1))
            {
                rotationAmount++;
                if (rotationAmount > 3)
                    rotationAmount = 0;
                ActualizePreview();

            }
            RaycastHit hit = CastRay();
            if (selectedTile)
            {
                if (selectedTile != hit.collider.gameObject)
                {
                    ActualizePreview();
                    Debug.Log("reset");
                }
            }

            Preview();




            if (Input.GetMouseButtonDown(0))
            {
                ApplyPreview();

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




        void Preview()
        {
            RaycastHit hit = CastRay();
            selectedTile = hit.collider.gameObject;
            for (int x = 0; x < numberHorizontal; x++)
            {
                for (int y = 0; y < numberVertical; y++)
                {
                    if (tileMap[x, y] == hit.collider.gameObject)
                    {

                        foreach (Transform t in selected.transform)
                        {
                            if (previewTiles.Count < 3)//trouver meilleur condition
                            {

                                //GameObject tuilePose = tileMap[x + (int)t.localPosition.x, y + (int)t.localPosition.z];
                                GameObject cree = Instantiate(t.gameObject, tileMap[x,y].transform.position+ t.localPosition, t.transform.rotation);
                                cree.transform.RotateAround(hit.collider.transform.position, Vector3.up, 90 * rotationAmount);
                                previewTiles.Add(cree);
                                if(cree.transform.position.x <= numberHorizontal-1 && cree.transform.position.x+0.1f >=0 && cree.transform.position.z <= numberVertical-1 && cree.transform.position.z >= 0)
                                {
                                
                                    GameObject tuilePose = tileMap[(int)(cree.transform.position.x+0.1f), (int)cree.transform.position.z];

                                    ChangeOpacity(0.5f, cree);
                                    ChangeRender(false, tuilePose);
                                
                                }
                                else
                                {
                                    Destroy(cree);
                                }

                            /*
                                GameObject tuilePose = tileMap[x + (int)t.localPosition.x, y + (int)t.localPosition.z];
                                GameObject cree = Instantiate(t.gameObject, tuilePose.transform.position, t.transform.rotation);
                                cree.transform.RotateAround(hit.collider.transform.position, Vector3.up, 90 * rotationAmount);
                                tuilePose = tileMap[(int)cree.transform.position.x, (int)cree.transform.position.z];
                                //cree.transform.Rotate(0, 90 * rotationAmount, 0);
                                previewTiles.Add(cree);
                                ChangeOpacity(0.5f, cree);
                                ChangeRender(false, tuilePose);*/
                            }   


                        }

                    }
                }
            }

        }

        void ActualizePreview()
        {
            for (int x = 0; x < numberHorizontal; x++)
            {
                for (int y = 0; y < numberVertical; y++)
                {
                    ChangeRender(true, tileMap[x, y]);
                }

            }

            foreach (GameObject g in previewTiles)
            {
                Destroy(g);

            }

            previewTiles.Clear();
        }



        void ApplyPreview()
        {
            foreach (GameObject g in previewTiles)
            {
                if (g & g != null)
                {
                    if (g.transform.position.x <= numberHorizontal - 1 && g.transform.position.x + 0.1f >= 0 && g.transform.position.z <= numberVertical - 1 && g.transform.position.z >= 0)
                    {
                        Destroy(tileMap[(int)(g.transform.position.x + 0.1f), (int)g.transform.position.z]);
                        g.GetComponent<Collider>().enabled = true;
                        tileMap[(int)(g.transform.position.x + 0.1f), (int)g.transform.position.z] = g;
                        ChangeOpacity(1f, g);
                    }

                }
              
            }
        previewTiles.Clear();
        isPosing = false;
        multiplayerManager.StartRaceM();
    }


        public void StartPicking()
        {
            panel.SetActive(true);
            GameObject newTile = Instantiate(allTiles,new Vector3(7, 13, 3),Quaternion.identity);
            selected = null;
            isPicking = true;
            foreach(Transform t in newTile.transform)
            {
                t.gameObject.GetComponent<Collider>().enabled = true;
            }
        }


        //tool functions 

        private RaycastHit CastRay()//permet de tirer un rayon la ou le joueur clic peut importe la direction de la camera
        {
            Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
            Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

            //transformation des position ecran en position monde
            Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
            Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

            //tir du raycast
            RaycastHit hit;
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
            return hit;
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

