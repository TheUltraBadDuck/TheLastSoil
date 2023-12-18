using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapManager : MonoBehaviour
{
    [SerializeField]
    private GameObject hoffenTree;

    [SerializeField]
    private GameObject availableTileInstance;   // Tree distribution
    private GameObject currTreeInstance;
    private int currEnergyScore = 0;
    private TreeButton currTreeButton;


    private bool[,] tileBitmap;
    private Object[,] treeObjs;
    private GameObject[,] availableTiles;
    private int row = 0;
    private int col = 0;


    [SerializeField]
    private SpriteRenderer cursorShadow;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private EnergyBar energyBar;

    private List<IvyInterface> attackTreeObservers = new();



    private void Start()
    {
        // Load tiles
        StreamReader stream = new(Application.dataPath + "/Resources/PlaceableTiles.txt");

        string text = stream.ReadLine();
        string[] bits = text.Split(' ');

        row = int.Parse(bits[0]);
        col = int.Parse(bits[1]);

        tileBitmap = new bool[row, col];
        treeObjs = new Object[row, col];
        availableTiles = new GameObject[row, col];

        for (int i = 0; i < row; i++)
        {
            text = stream.ReadLine();
            bits = text.Split(' ');

            for (int j = 0; j < col; j++)
            {
                var newObj = Instantiate(availableTileInstance, Vector3.zero, Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.transform.localPosition = GetMapPosition(i, j);
                newObj.GetComponent<SpriteRenderer>().enabled = false;
                AvailableTile newScript = newObj.GetComponent<AvailableTile>();
                newScript.pressable = false;
                newScript.this_i = i;
                newScript.this_j = j;

                availableTiles[i, j] = newObj;

                if (bits[j] == "1")
                {
                    tileBitmap[i, j] = true;
                    newScript.SetMapManager(this);
                }
                else
                {
                    tileBitmap[i, j] = false;
                }
            }
        }
        stream.Close();
    }



    private void Update()
    {
        // Get ghost tree image following the cursor
        if (currTreeInstance != null)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            cursorShadow.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }



    public float GetDistanceToHoffen(Behavior enemy)
    {
        return Vector3.Distance(enemy.transform.position, hoffenTree.transform.position);
    }


    public static Vector3 GetMapPosition(int i, int j)
    {
        return new Vector3(j * 0.48f - 2.88f, i * 0.48f - 2.88f, 0.0f);
    }



    // Remove enemy from a list of each tree
    public void RemoveEnemyDetection(Behavior enemy)
    {
        for (int i = 0; i < attackTreeObservers.Count; i++)
        {
            IvyInterface component = attackTreeObservers[i].GetComponent<IvyInterface>();
            if (component is AttackTreeInterface)
            {
                component.RemoveEnemy(enemy);
            }
        }
    }


    // From TreeButton.cs
    public void OnTreeButtonPressed(GameObject treeInstance, Sprite newTreeSprite, int energyScore, TreeButton selectedButton)
    {
        Debug.Log(newTreeSprite);

        // Same type => unselect the tree
        if ((cursorShadow.sprite != null) && (cursorShadow.sprite.name == newTreeSprite.name))
        {
            ShowAvaiableTiles(false);
            cursorShadow.sprite = null;
            currTreeInstance = null;
            currEnergyScore = 0;
            currTreeButton = null;
        }
        // Different type / not selecting => select the tree
        else
        {
            if (cursorShadow.sprite == null)
                ShowAvaiableTiles(true);
            cursorShadow.sprite = newTreeSprite;
            currTreeInstance = treeInstance;
            currEnergyScore = energyScore;
            currTreeButton = selectedButton;
        }
    }



    // Show available tiles to put the tree
    public void ShowAvaiableTiles(bool showing = true)
    {
        if (showing)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    // Legal tilemap and no tree was put there
                    if (tileBitmap[i, j] && (treeObjs[i, j] == null))
                    {
                        availableTiles[i, j].GetComponent<AvailableTile>().pressable = true;
                        availableTiles[i, j].GetComponent<SpriteRenderer>().enabled = true;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    availableTiles[i, j].GetComponent<AvailableTile>().pressable = false;
                    if (tileBitmap[i, j] && availableTiles[i, j].GetComponent<SpriteRenderer>().enabled)
                    {
                        availableTiles[i, j].GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
            }
        }
    }


    public void PutTree(int i, int j)
    {
        // Subtract energy
        energyBar.SubtractScore(currEnergyScore);

        // Add CD to a specific button
        currTreeButton.AddCD();

        var newObj = Instantiate(currTreeInstance, new Vector3(i, j), Quaternion.identity);
        newObj.transform.SetParent(GameObject.Find("TreeContainer").transform);
        newObj.transform.localPosition = GetMapPosition(i, j);

        treeObjs[i, j] = newObj;
        cursorShadow.sprite = null;
        currTreeInstance = null;

        ShowAvaiableTiles(false);

        // If the tree can attack
        if (newObj.GetComponent<IvyInterface>() is AttackTreeInterface)
        {
            AddAttackObserver(newObj.GetComponent<IvyInterface>());
        }
    }


    public void AddAttackObserver(IvyInterface tree)
    {
        attackTreeObservers.Add(tree);
    }


    public void RemoveAttackObserver(IvyInterface tree)
    {
        // Remove enemy that can be attacked by looking for the id
        int index = attackTreeObservers.FindIndex(e => e.name == tree.name);
        if (index == -1)
            return;

        attackTreeObservers.RemoveAt(index);
    }
}
