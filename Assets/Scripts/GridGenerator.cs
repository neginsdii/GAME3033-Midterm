using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GridGenerator : MonoBehaviour
{
    private static GridGenerator instance;
    public static GridGenerator Instance { get { return instance; } }
    private System.Random rand = new System.Random();


    public int numberOfRows;
    public int numberOfColumns;
    [SerializeField]
    private int MaxNumberOfCrates;
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject CratePrefab;
    [SerializeField]
    private GameObject exitPrefab;
    public GameObject[,] grid = new GameObject[32, 32];
    public List<Vector2> emptyTileList = new List<Vector2>();
    public TextMeshProUGUI bombText;

    public float tileSize;
    private float numberOfCrates;
    private float timer;
    public float maxTimer;

    [Header("Bomb properties")]
    [SerializeField]
    private GameObject bomb;
    public float MaxNumOfBombs;
    public float NumOfBombs;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        generateGrid();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfCrates < MaxNumberOfCrates)
        {
            timer += Time.deltaTime;
            if (timer >= maxTimer)
            {
                PlaceCrateOnTiles();
                timer = 0;
            }
        }

        bombText.SetText(NumOfBombs.ToString());
    }

    private void generateGrid()
    {
        Vector3 pos = Vector3.zero;
        for (int r = 0; r < numberOfRows; r++)
        {
            for (int c = 0; c < numberOfColumns; c++)
            {
                grid[r, c] = Instantiate(tilePrefab, this.transform);
                grid[r, c].gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z + c * tileSize);
                grid[r, c].GetComponent<Tile>().coordinates = new Vector2(r, c);
                //  if (r!= numberOfRows/2 && c!=numberOfColumns/2)
                emptyTileList.Add(new Vector2(r, c));
            }
            pos.x += tileSize;
        }

        emptyTileList.Remove(new Vector2(numberOfRows / 2, numberOfColumns / 2));

        GameObject LastTilePos = grid[numberOfRows - 1, numberOfColumns - 1];
        GameObject tmp = Instantiate(exitPrefab, new Vector3(LastTilePos.transform.position.x, LastTilePos.transform.position.y + 0.5f, LastTilePos.transform.position.z), Quaternion.identity);
        emptyTileList.Remove(new Vector2(numberOfRows -1, numberOfColumns-1));
        LastTilePos.GetComponent<Tile>().IsOccupied = true;

        for (int i = 0; i < MaxNumOfBombs; i++)
        {
            int indx = rand.Next(0, emptyTileList.Count - 1);
            grid[(int)emptyTileList[indx].x, (int)emptyTileList[indx].y].GetComponent<Tile>().ActivateBomb(true);
            emptyTileList.RemoveAt(indx);


        }
       
    }
    public void PlaceCrateOnTiles()
    {
        int indx = rand.Next(1, emptyTileList.Count - 1);

        grid[(int)emptyTileList[indx].x, (int)emptyTileList[indx].y].GetComponent<Tile>().ActivateCrate(true);
        numberOfCrates++;
        emptyTileList.RemoveAt(indx);
    }

}
