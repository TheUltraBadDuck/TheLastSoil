using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IvyInterface : MonoBehaviour
{
    // Tree's status
    [SerializeField]
    protected int treeId = -1;
    [SerializeField]
    protected string treeName = "[None]";
    [SerializeField]
    protected float hp = 0;
    [SerializeField]
    protected float maxhp = 0;
    [SerializeField]
    protected Vector2 position = Vector2.zero;

    // Tree's support
    protected Animator animator;
    private SpriteRenderer whiteTreeRenderer;

    // When attacked
    private float hurtCD = 0.25f;
    private float hurtTimer = 0f;
    protected bool attacked = false;
    public GameObject blood;


    public int currentLevel = 0;
    public string[] treeLevel = { "Basic", "Evolution", "Legendary" };
    public string[] levelDescription;
    public Sprite sprite;

    protected int coordX = 0;
    protected int coordY = 0;

    private SpriteRenderer spriteRenderer;
    private Color startColor = Color.white;

    public void UpdateSpriteColor()
    {
        if (spriteRenderer != null && sprite != null)
        {
            if (currentLevel == 2)
            {
                startColor = new Color(0.8f, 0.0f, 0.0f); // Red
            }
            else if (currentLevel == 3)
            {
                startColor = new Color(0.7f, 0.0f, 1.0f); // Purple
            }

            StartCoroutine(BreatheColor());
        }
    }

    private IEnumerator BreatheColor()
    {
        Color targetColor = Color.white;

        float duration = 2.0f; // Adjust the duration of the breathing
        float t = 0.0f;

        while (true && startColor != Color.white)
        {
            t += Time.deltaTime;

            // Use Mathf.Sin to create a smooth breathing effect
            float lerpFactor = Mathf.Sin(t / duration * Mathf.PI);

            // Lerp between the original color and the target color based on the sine wave
            Color lerpedColor = Color.Lerp(startColor, targetColor, lerpFactor);

            spriteRenderer.color = lerpedColor;

            // Check if the current level has changed
            if (currentLevel == 2 && startColor != new Color(0.8f, 0.0f, 0.0f))
            {
                startColor = new Color(0.8f, 0.0f, 0.0f);
            }
            else if (currentLevel == 3 && startColor != new Color(0.7f, 0.0f, 1.0f))
            {
                startColor = new Color(0.7f, 0.0f, 1.0f);
            }

            yield return null;
        }
    }
    public virtual void Start()
    {
         animator = GetComponent<Animator>();
        whiteTreeRenderer = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            sprite = spriteRenderer.sprite;
        }
        else
        {
            Debug.LogWarning("SpriteRenderer component not found in children.");
        }
        animator = GetComponent<Animator>();
        whiteTreeRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();


    public virtual void Update()
    {
        // Blink
        if (attacked)
        {
            hurtTimer += Time.deltaTime;
            if (hurtTimer < hurtCD / 2)
            {
                whiteTreeRenderer.color = new Color(1, 1, 1, hurtTimer / hurtCD * 2);
            }
            else if (hurtTimer < hurtCD)
            {
                whiteTreeRenderer.color = new Color(1, 1, 1, (hurtCD - hurtTimer) / hurtCD * 2);
            }
            else
            {
                hurtTimer = 0f;
                attacked = false;
            }
        }
    }



    public virtual void HandleEnter2D(Collider2D coll)
    {
        Debug.Log(treeName + " is ready to attack.");
    }


    public virtual void HandleExit2D(Collider2D coll)
    {
        Debug.Log(treeName + " stops attacking.");
    }


    public virtual void RemoveEnemy(Behavior enemy)
    {

    }
    // If the tree is attacked
    public virtual void BeAttacked(int damage)
    {
        hp -= damage;
        Instantiate(blood, gameObject.transform.position, Quaternion.identity);
        if (hp <= 0)
        {
            // Restore the button to the map
            GameObject.Find("MapManager").GetComponent<MapManager>().RestoreCell(coordY, coordX);
            Destroy(gameObject);
        }
        else
        {
            attacked = true;
        }
    }


    public void BeHealed(int heal)
    {
        hp = Mathf.Min(hp + heal, maxhp);
    }


    public virtual void BeBuff(float extraDamage, float extraSpeed)
    {
        
    }


    public void SetCoord(int x, int y)
    {
        coordX = x;
        coordY = y;
    }
}
