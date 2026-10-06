using UnityEngine;
using TMPro;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    [Header("Collectibles")]
    public int totalCollectibles = 5;
    public int collectedCollectibles = 0;

    [Header("UI")]
    public TMP_Text collectibleText;

    public bool allCollectiblesCollected
    {
        get { return collectedCollectibles >= totalCollectibles; }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUI();
    }

    public void Collect()
    {
        collectedCollectibles++;

        UpdateUI();
    }

    void UpdateUI()
    {
        if (collectibleText != null)
        {
            collectibleText.text =
                collectedCollectibles + " / " + totalCollectibles;
        }
    }
}