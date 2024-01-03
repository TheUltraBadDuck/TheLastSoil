using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class TreeButton : MonoBehaviour
{
    [SerializeField]
    private GameObject treeInstance;    // Load a certain tree
    [SerializeField]
    private Texture2D newTreeImage;
    [SerializeField]
    private Sprite newTreeSprite;
    [SerializeField]
    public int energyScore;             // A number of energy score to consume
    [SerializeField]
    private float pressableCD = 5.0f;   // Cooldown for the button
    private float pressableTimer = 5.0f;


    private RawImage treeImage;
    private Text eneryScoreText;
    private Image waitingBlack;         // If pressableCD is done, disable the image to make the button pressable
    private Image inactiveBlack;        // If enough score, disable the image to make the button pressable
    private Text treeDescriptionTitle;
    private Text treeDescriptionText;

    private bool enoughEnergy = false;  // Activates inactiveBlack
    private bool available = false;     // Depends on waitingBlack


<<<<<<< Updated upstream


=======
    public void SetLevel(int level)
    {
        treeLevel = level;
    }



    // Always run before Start
>>>>>>> Stashed changes
    private void Awake()
    {
        // Load components
        treeImage = transform.GetChild(0).GetComponent<RawImage>();
        eneryScoreText = transform.GetChild(1).GetComponent<Text>();
        waitingBlack = transform.GetChild(2).GetComponent<Image>();
        inactiveBlack = transform.GetChild(3).GetComponent<Image>();
        treeDescriptionTitle = GameObject.Find("TreeDescription/Title").GetComponent<Text>();
        treeDescriptionText = GameObject.Find("TreeDescription/Text").GetComponent<Text>();

        // Update button
        // Image
        treeImage.texture = newTreeImage;

        // Score
        eneryScoreText.text = energyScore.ToString();

        // Pressable
        UpdatePressableByTotalEnergy(enoughEnergy);
        waitingBlack.fillAmount = 0f;

        // Pressable
        available = pressableTimer >= pressableCD;
    }


    private void Update()
    {
        if (pressableTimer >= pressableCD)
            return;

        pressableTimer += Time.deltaTime;
        waitingBlack.fillAmount = 1f - pressableTimer / pressableCD;
        if (pressableTimer > pressableCD)
        {
            available = true;
            waitingBlack.fillAmount = 0f;
        }
    }


    // If the Energy Coin UI's energy score is not enough
    // Disable the button
    public void UpdatePressableByTotalEnergy(bool active)
    {
        inactiveBlack.gameObject.SetActive(!active);
        enoughEnergy = active;
    }


    public void AddCD()
    {
        pressableTimer = 0;
        available = false;
        waitingBlack.fillAmount = 1f;
    }


    public void OnPressed()
    {
        if (enoughEnergy && available && EventSystem.current.IsPointerOverGameObject())
        {
            GameObject.Find("MapManager").GetComponent<MapManager>().OnTreeButtonPressed(
<<<<<<< Updated upstream
                treeInstance, newTreeSprite, energyScore, this);
=======
                treeInstance, newTreeSprite, energyScore, this, treeLevel);

            treeDescriptionTitle.text = treeInstance.GetComponent<IvyInterface>().GetTreeName();
            treeDescriptionText.text = treeInstance.GetComponent<IvyInterface>().GetTreeDescription();
>>>>>>> Stashed changes
        }
    }
}
