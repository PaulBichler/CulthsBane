using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    public int size = 5;
    private MapNode[][] map;
    private bool hasheal = false;
    private int currentPosition = 0;

    public GameObject MapIcon;
    public GameObject Line;
    public Sprite[] sprite;


    public GameObject dungeonCanvas;
    public GameObject mapCanvas;

    void Start()
    {
        createMap();
    }

    private void createMap()
    {
        createDiffrentWaysInMap();
        assignARoomToEveryWay();
        createIcons();
        drawLines();
    }

    private void drawLines()
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                List<MapNode> mapNodes = map[i][j].getChildren();
                for (int k = 0; k < mapNodes.Count; k++)
                {
                    drawLine(map[i][j].getGameObject().transform.position, mapNodes[k].getGameObject().transform.position);
                }
            }
        }
    }

    private void createIcons()
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                GameObject MyFatherObj = GameObject.Find("Map");
                GameObject uiIcon = null;
                switch (map[i][j].getLevel())
                {
                    case 0:
                        InstantiateIcon(uiIcon, 0, i, j, MyFatherObj);
                        break;
                    case 1:
                        InstantiateIcon(uiIcon, 1, i, j, MyFatherObj);
                        break;
                    case 2:
                        InstantiateIcon(uiIcon, 2, i, j, MyFatherObj);
                        break;
                    case 3:
                        InstantiateIcon(uiIcon, 3, i, j, MyFatherObj);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void assignARoomToEveryWay()
    {
        for (int i = 0; i < map.Length; i++)
        {
            for (int j = 0; j < map[i].Length; j++)
            {
                if (i == 0)
                {
                    map[i][j] = new MapNode(0);
                }
                else if (i == (map.Length - 1))
                {
                    map[i][j] = new MapNode(3);
                }
                else if (!hasheal)
                {
                    map[i][j] = new MapNode();
                    if (map[i][j].getLevel() == 2)
                    {
                        hasheal = true;
                    }
                }
                else
                {
                    map[i][j] = new MapNode(1);
                }
            }
            if (i != 0)
            {
                for (int j = 0; j < map[i - 1].Length; j++)
                {
                    if (map[i - 1][j].getLevel() == 2)
                    {
                        hasheal = false;
                    }
                }

                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i - 1].Length == 1)
                    {
                        map[i - 1][0].setChildren(map[i][j]);
                    }
                    else if (map[i - 1].Length == map[i].Length)
                    {
                        map[i - 1][j].setChildren(map[i][j]);
                    }
                    else if (map[i - 1].Length == 2)
                    {
                        if (map[i].Length == 1)
                        {
                            map[i - 1][0].setChildren(map[i][j]);
                            map[i - 1][1].setChildren(map[i][j]);
                        }
                        else
                        {
                            if (j == 0)
                            {
                                int ran = Random.Range(0, 2);
                                if (ran == 0)
                                {
                                    map[i - 1][0].setChildren(map[i][0]);
                                    map[i - 1][1].setChildren(map[i][2]);

                                    map[i - 1][0].setChildren(map[i][1]);
                                }
                                else
                                {
                                    map[i - 1][0].setChildren(map[i][0]);
                                    map[i - 1][1].setChildren(map[i][2]);

                                    map[i - 1][1].setChildren(map[i][1]);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (map[i].Length == 1)
                        {
                            map[i - 1][0].setChildren(map[i][j]);
                            map[i - 1][1].setChildren(map[i][j]);
                            map[i - 1][2].setChildren(map[i][j]);
                        }
                        else
                        {
                            if (j == 0)
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    map[i - 1][0].setChildren(map[i][0]);
                                    map[i - 1][2].setChildren(map[i][1]);

                                    map[i - 1][1].setChildren(map[i][0]);
                                }
                                else
                                {
                                    map[i - 1][0].setChildren(map[i][0]);
                                    map[i - 1][2].setChildren(map[i][1]);

                                    map[i - 1][1].setChildren(map[i][1]);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void createDiffrentWaysInMap()
    {
        map = new MapNode[size][];

        for (int i = 0; i < size; i++)
        {
            if (i != 0 && i != size - 1)
            {
                map[i] = new MapNode[Random.Range(1, 4)];
            }
            else
            {
                map[i] = new MapNode[1];
            }
        }
    }

    private void InstantiateIcon(GameObject uiIcon, int n, int i, int j, GameObject MyFatherObj)
    {
        uiIcon = Instantiate(MapIcon);
        uiIcon.GetComponent<Image>().sprite = sprite[n];
        uiIcon.transform.position = new Vector3((i + 1) * Screen.width / (size + 1), (j + 1) * Screen.height / (map[i].Length + 1), 0);
        uiIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 50);
        Button button = uiIcon.GetComponent<Button>();

        button.onClick.AddListener(delegate { Clicked(i, j); });

        uiIcon.transform.SetParent(MyFatherObj.transform);

        uiIcon.GetComponent<Button>().interactable = map[i][j].getPlayable();

        map[i][j].setGameObject(uiIcon);
    }

    public void Clicked(int i, int j)
    {
        for (int k = 0; k < map[i].Length; k++)
        {
            Button button = map[i][k].getGameObject().GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }

        if (i != size - 1)
        {
            List<MapNode> mapNodes = map[i][j].getChildren();
            for (int k = 0; k < mapNodes.Count; k++)
            {
                mapNodes[k].setPlayable(true);

            }
            for (int k = 0; k < map[i + 1].Length; k++)
            {
                map[i + 1][k].getGameObject().GetComponent<Button>().interactable = map[i + 1][k].getPlayable();
            }
            currentPosition++;
        }
        if (map[i][j].getLevel() == 0 || map[i][j].getLevel() == 1)
        {
            mapCanvas.SetActive(false);
            dungeonCanvas.SetActive(true);
            Game.GameLogic.EnterBattle(1);
        }
        if (map[i][j].getLevel() == 2)
        {
            Game.Player.ReceiveHealing(10);
        }
    }

    private void drawLine(Vector3 pointA, Vector3 pointB)
    {
        GameObject MyFatherObj = GameObject.Find("Map");
        GameObject uiLine = Instantiate(Line);



        uiLine.transform.SetParent(MyFatherObj.transform);

        Vector3 differenceVector = pointB - pointA;

        uiLine.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 3);
        uiLine.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
        uiLine.GetComponent<RectTransform>().position = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        uiLine.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, angle);
    }

}
