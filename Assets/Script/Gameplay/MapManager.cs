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
    private int currTreeLevel = 0;
    private TreeButton currTreeButton;
	
	[SerializeField]
	private TextAsset txtFile;


    private bool[,] tileBitmap;
    private GameObject[,] treeObjs;
    private GameObject[,] availableTiles;
    private int[,] treeScore;
    private int row = 0;
    private int col = 0;
    private bool showingTiles = false;


    [SerializeField]
    private SpriteRenderer cursorShadow;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private EnergyBar energyBar;

    [SerializeField]
    private Sprite shovelImage;
    [SerializeField]
    private GameObject coinPrefab;
    private GameObject coinContainer;


    private List<IvyInterface> attackTreeObservers = new();

    // -------------------------------------------------------------------------

    public SpriteRenderer GetCursorShadow()
    {
        return cursorShadow;
    }

    public Sprite GetShovel()
    {
        return shovelImage;
    }

    public Object GetTree(int i, int j)
    {
        return treeObjs[i, j];
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
        newObj.GetComponent<IvyInterface>().SetCoord(j, i);
        newObj.GetComponent<IvyInterface>().SetTreeLevel(currTreeLevel);

        treeObjs[i, j] = newObj;
        treeScore[i, j] = currEnergyScore;
        cursorShadow.sprite = null;
        currTreeInstance = null;

        ShowAvaiableTiles(false);

        // If the tree can attack
        if (newObj.GetComponent<IvyInterface>() is AttackTreeInterface)
            AddAttackObserver(newObj.GetComponent<IvyInterface>());
    }


    // Remove with shovel
    public void RemoveTree(int i, int j)
    {
        if (showingTiles)
        {
            Debug.Log("Trying to show/hide at (" + i + ", " + j + "): " + (cursorShadow.sprite.name != shovelImage.name).ToString());
            availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(cursorShadow.sprite.name != shovelImage.name);
        }
        Destroy(treeObjs[i, j]);
        treeObjs[i, j] = null;

        for (int coin = 0; coin < treeScore[i, j] / 2; coin += 25)
        {
            GameObject coinObj = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            coinObj.transform.parent = coinContainer.GetComponent<Transform>().transform;
            coinObj.transform.localPosition = new Vector3(
                availableTiles[i, j].transform.position.x + Random.Range(-0.10f, 0.10f),
                availableTiles[i, j].transform.position.y - 0.15f + Random.Range(-0.05f, 0.05f),
                0.0f);
        }

        treeScore[i, j] = 0;

        // Clear the shovel
        ShowAvaiableTiles(false);
        cursorShadow.sprite = null;
    }



    public void RestoreCell(int i, int j)
    {
        treeObjs[i, j] = null;

        if (showingTiles)
        {
            availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(cursorShadow.sprite.name != shovelImage.name);
        }
    }




    // -------------------------------------------------------------------------

    private void Start()
    {
        // Load tiles
        coinContainer = GameObject.Find("CoinContainer");

        string[] lines = txtFile.text.Split('\n');
		string[] rowCol = lines[0].Split(' ');
        
        row = int.Parse(rowCol[0]);
        col = int.Parse(rowCol[1]);

        tileBitmap = new bool[row, col];
        treeObjs = new GameObject[row, col];
        availableTiles = new GameObject[row, col];
        treeScore = new int[row, col];

        for (int i = 0; i < row; i++)
        {
			string[] bits = lines[i + 1].Split(' ');

            for (int j = 0; j < col; j++)
            {
                var newObj = Instantiate(availableTileInstance, Vector3.zero, Quaternion.identity);
                newObj.transform.SetParent(transform);
                newObj.transform.localPosition = GetMapPosition(i, j);
                AvailableTile newScript = newObj.GetComponent<AvailableTile>();
                newScript.SetPressable(false);
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
    }



    private void Update()
    {
        // Get ghost tree image following the cursor
        if (cursorShadow.sprite != null)
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            cursorShadow.transform.position = new Vector3(pos.x, pos.y, 0f);
        }
    }

    // -------------------------------------------------------------------------

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
    public void OnTreeButtonPressed(GameObject treeInstance,
        Sprite newTreeSprite, int energyScore, TreeButton selectedButton, int level)
    {
        // Same type => unselect the tree
        if ((cursorShadow.sprite != null) && (cursorShadow.sprite.name == newTreeSprite.name))
        {
            ShowAvaiableTiles(false);
            cursorShadow.sprite = null;
            currTreeInstance = null;
            currEnergyScore = 0;
            currTreeLevel = 0;
            currTreeButton = null;
        }
        // Different type / not selecting => select the tree
        else
        {
            if ((cursorShadow.sprite == null) || (cursorShadow.sprite.name == shovelImage.name))
                ShowAvaiableTiles(true);
            cursorShadow.sprite = newTreeSprite;
            currTreeInstance = treeInstance;
            currEnergyScore = energyScore;
            currTreeLevel = level;
            currTreeButton = selectedButton;
        }
    }



    // From Shovel.cs
    public void OnShovelPressed()
    {
        // Like above
        if ((cursorShadow.sprite != null) && (cursorShadow.sprite.name == shovelImage.name))
        {
            ShowAvaiableTiles(false);
            cursorShadow.sprite = null;
        }
        else
        {
            if ((cursorShadow.sprite == null) || (cursorShadow.sprite.name != shovelImage.name))
                ShowAvaiableTiles(true, false);
            cursorShadow.sprite = shovelImage;
        }

        currTreeInstance = null;
        currEnergyScore = 0;
        currTreeButton = null;
    }



    // Show available tiles to put the tree
    public void ShowAvaiableTiles(bool showing = true, bool showEmpty = true)
    {
        showingTiles = showing;

        if (showing)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    // Legal tilemap and no tree was put there
                    if (tileBitmap[i, j])
                    {
                        if (showEmpty && (treeObjs[i, j] == null))
                            availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(true);
                        else if (!showEmpty && (treeObjs[i, j] != null))
                            availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(true);
                        else
                            availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(false);
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
                    availableTiles[i, j].GetComponent<AvailableTile>().SetPressable(false);
                }
            }
        }
    }


    public void AddAttackObserver(IvyInterface tree)
    {
        attackTreeObservers.Add(tree);
    }
}
