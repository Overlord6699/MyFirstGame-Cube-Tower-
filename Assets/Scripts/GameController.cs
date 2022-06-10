using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

struct CubePos
{
    public int x, y, z;

    public CubePos(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3 getVector()
    {
        return new Vector3(x, y, z);
    }

    public void SetVector(Vector3 pos)
    {
        x = Convert.ToInt32(pos.x);
        y = Convert.ToInt32(pos.y);
        z = Convert.ToInt32(pos.z);
    }

}

public class GameController : MonoBehaviour
{
    public Color[] BGColors;
    public Color toCameraColor;

    private CubePos nowCube = new CubePos(0, 1, 0);
    public float cubeChangePlaceSpeed = 0.5f;
    public Transform cubeToPlace;
    private Transform MainCam;

    public Text ScoreTxt;
    private float camMoveToYPos, CamMoveSpeed = 2f, prevCountMaxHorizontal;

    public Rigidbody allCubesRB;

    public GameObject[] cubesToCreate;
    public GameObject allCubes, VFX;
    public GameObject[] canvasStartPage;

    private bool isLose, firstCube;

    private Coroutine showCubePlace;

    private List<GameObject> possibleCubesToCreate = new List<GameObject>();

    private List<Vector3> allCubesPositions = new List<Vector3>()
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(-1, 0, 0),
        new Vector3(0, 1, 0),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, -1),
        new Vector3(1, 0, 1),
        new Vector3(-1, 0, -1),
        new Vector3(-1, 0, 1),
        new Vector3(1, 0, -1),
    };

    private void Start()
    {
        if(PlayerPrefs.GetInt("Score") < 5)
        {
            AddPossibleCubes(1);
        }
        else if(PlayerPrefs.GetInt("Score") < 10)
        {
            AddPossibleCubes(2);
        }
        else if (PlayerPrefs.GetInt("Score") < 15)
        {
            AddPossibleCubes(3);
        }
        else if (PlayerPrefs.GetInt("Score") < 30)
        {
            AddPossibleCubes(4);
        }
        else if (PlayerPrefs.GetInt("Score") < 40)
        {
            AddPossibleCubes(5);
        }
        else if (PlayerPrefs.GetInt("Score") < 50)
        {
            AddPossibleCubes(6);
        }
        else if (PlayerPrefs.GetInt("Score") < 60)
        {
            AddPossibleCubes(7);
        }
        else if (PlayerPrefs.GetInt("Score") < 70)
        {
            AddPossibleCubes(8);
        }
        else if (PlayerPrefs.GetInt("Score") < 111)
        {
            AddPossibleCubes(9);
        }
        else
        {
            AddPossibleCubes(10);
        }

        ScoreTxt.text = "<size=40><color=#FF070A>best:</color></size> " + PlayerPrefs.GetInt("Score") + '\n' + "<size=30><color=#0000FF>now:</color></size> 0";
        toCameraColor = Camera.main.backgroundColor;
        MainCam = Camera.main.transform;
        camMoveToYPos = 5.9f + nowCube.y - 1f;

        allCubesRB = allCubes.GetComponent<Rigidbody>();
        showCubePlace = StartCoroutine(ShowCubePlace());
    }

    private void AddPossibleCubes(int MaxElem)
    {
        for (int i = 0; i < MaxElem; i++)
        {
            possibleCubesToCreate.Add(cubesToCreate[i]);
        }
    }

    private void MoveCameraChangeBG()
    {
        int MaxX = 0;
        int MaxY = 0;
        int MaxZ = 0;
        int maxHorizontal;

        foreach(Vector3 pos in allCubesPositions)
        {
            if (Mathf.Abs(Convert.ToInt32(pos.x)) > MaxX)
                MaxX = Convert.ToInt32(pos.x);
            if (Mathf.Abs(Convert.ToInt32(pos.y)) > MaxY)
                MaxY = Convert.ToInt32(pos.y);
            if (Mathf.Abs(Convert.ToInt32(pos.z)) > MaxZ)
                MaxZ = Convert.ToInt32(pos.z);
        }

        MaxY--;

        if(PlayerPrefs.GetInt("Score") < MaxY)
        {
            PlayerPrefs.SetInt("Score", MaxY);
        }

        ScoreTxt.text = "<size=40><color=#FF070A>best:</color></size>" + PlayerPrefs.GetInt("Score") + '\n' + "<size=30><color=#0000FF>now:</color></size>" + MaxY;

        camMoveToYPos = 5.9f + nowCube.y - 1f;

        maxHorizontal = MaxX > MaxZ ? MaxX : MaxZ;
        if(maxHorizontal % 3 == 0 && prevCountMaxHorizontal != maxHorizontal)
        {
            MainCam.localPosition -= new Vector3(0, 0, 2f);
            prevCountMaxHorizontal = maxHorizontal;
        }

        if (MaxY >= 10)
            toCameraColor = BGColors[0];
        else if (MaxY >= 5)
            toCameraColor = BGColors[1];
        else if (MaxY >= 2)
            toCameraColor = BGColors[2];
    }

    private void Update()
    {
        if (!isLose && (Input.GetMouseButtonDown(0) || Input.touchCount > 0) && cubeToPlace != null && allCubes != null && !EventSystem.current.IsPointerOverGameObject())
        {
#if !UNITY_EDITOR
                if(Input.GetTouch(0).phase != TouchPhase.Began)
                    return;
#endif

            if (!firstCube)
            {
                firstCube = true;
                foreach (GameObject obj in canvasStartPage)
                    Destroy(obj);
            }

            GameObject createCube = null;
            if(possibleCubesToCreate.Count == 1)
            {
                createCube = possibleCubesToCreate[0];
            }
            else
            {
                createCube = possibleCubesToCreate[UnityEngine.Random.Range(0, possibleCubesToCreate.Count)];
            }

            GameObject newCube = Instantiate(createCube, cubeToPlace.position, Quaternion.identity) as GameObject;

            newCube.transform.SetParent(allCubes.transform);
            nowCube.SetVector(cubeToPlace.position);
            allCubesPositions.Add(nowCube.getVector());

            GameObject VFXObj = Instantiate(VFX, newCube.transform.position, Quaternion.identity) as GameObject;

            if(PlayerPrefs.GetString("Music") != "No")
            {
                GetComponent<AudioSource>().Play();
            }

            Destroy(VFXObj, 1.5f);

            allCubesRB.isKinematic = true;
            allCubesRB.isKinematic = false;

            SpawnPositions();
            MoveCameraChangeBG();
        }

        if(!isLose && allCubesRB.velocity.magnitude > 0.1f) // если уже падает конструкция
        {
            Destroy(cubeToPlace.gameObject);
            isLose = true;
            StopCoroutine(showCubePlace);
        }

        MainCam.localPosition = Vector3.MoveTowards(MainCam.localPosition, new Vector3(MainCam.localPosition.x, camMoveToYPos, MainCam.localPosition.z), CamMoveSpeed * Time.deltaTime);

        if (Camera.main.backgroundColor != toCameraColor)
        {
            Camera.main.backgroundColor = Color.Lerp(Camera.main.backgroundColor, toCameraColor, Time.deltaTime / 1.5f);
        }
    }

    IEnumerator ShowCubePlace()
    {
        while (true)
        {
            SpawnPositions();
            yield return new WaitForSeconds(cubeChangePlaceSpeed);
        }
    }

    private void SpawnPositions()
    {
        List<Vector3> positions = new List<Vector3>();
        if(IsPositionEmpty(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z)) && nowCube.x + 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x + 1, nowCube.y, nowCube.z));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z)) && nowCube.x - 1 != cubeToPlace.position.x)
        {
            positions.Add(new Vector3(nowCube.x - 1, nowCube.y, nowCube.z));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z)) && nowCube.y + 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y + 1, nowCube.z));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z)) && nowCube.y - 1 != cubeToPlace.position.y)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y - 1, nowCube.z));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1)) && nowCube.z + 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z + 1));
        }
        if(IsPositionEmpty(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1)) && nowCube.z - 1 != cubeToPlace.position.z)
        {
            positions.Add(new Vector3(nowCube.x, nowCube.y, nowCube.z - 1));
        }

        if (positions.Count > 1)
            cubeToPlace.position = positions[UnityEngine.Random.Range(0, positions.Count)];
        else if (positions.Count == 0)
            isLose = true;
        else
            cubeToPlace.position = positions[0];

    }

    private bool IsPositionEmpty(Vector3 targetpos)
    {
        bool isCorrect = true;
        if (targetpos.y == 0)
        {
            isCorrect = false;
            return isCorrect;
        }

        foreach (Vector3 pos in allCubesPositions)
        {
            if (pos.x == targetpos.x && pos.y == targetpos.y && pos.z == targetpos.z)
            {
                isCorrect = false;
                return isCorrect;
            }
        }

        return isCorrect;
    }

}
