using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TuileManager : MonoBehaviour
{
    public int numberHorizontal;
    public int numberVertical;
    public GameObject[,] tileMap;
    public GameObject blandTile;
    public GameObject selected;
    public GameObject selectedTile;
    public List<GameObject> previewTiles;
    void Start()
    {
        tileMap = new GameObject[numberHorizontal, numberVertical];
        GenerateMapStart();


    }

    // Update is called once per frame
    void Update()
    {
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

                    foreach(Transform t in selected.transform)
                    { 
                        GameObject tuilePose = tileMap[x + (int)t.localPosition.x, y+ (int)t.localPosition.z];
                        GameObject cree = Instantiate(t.gameObject, tuilePose.transform.position, t.transform.rotation);
                        previewTiles.Add(cree);

                        ChangeRender(false, tuilePose);
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

    public void ChangeRender(bool targetState, GameObject Target)
    {
            if (Target.GetComponent<MeshRenderer>())
                Target.GetComponent<MeshRenderer>().enabled = targetState;
            
        
    }
}
