using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyPlayerView : MonoBehaviour
{
    [SerializeField]
    private int playerId;

    [SerializeField]
    private SpriteSheet[] knightSpriteSheets;

    [SerializeField]
    private SpriteRenderer[] knightSpriteRenderers;

    [SerializeField]
    private SpriteSheet[] horseSpriteSheets;

    [SerializeField]
    private SpriteRenderer horseSpriteRenderer;

    [SerializeField]
    private TMP_InputField nameInputField;

    [SerializeField]
    private TMP_Text readyText;

    [SerializeField]
    private SpriteRenderer readySpriteRenderer;

    [SerializeField]
    private TMP_Text connectText;

    [SerializeField]
    private GameObject arrows;

    public string Name
    {
        get => string.IsNullOrEmpty(nameInputField.text) ? (nameInputField.placeholder as TMP_Text).text : nameInputField.text;
        set
        {
            if (string.IsNullOrEmpty(nameInputField.text))
                (nameInputField.placeholder as TMP_Text).text = value;
            else
                nameInputField.text = value;
        }
    }

    private int knightIndex;
    public int KnightIndex
    {
        get => knightIndex;
        set
        {
            knightIndex = (value + knightSpriteSheets.Length) % knightSpriteSheets.Length;
            for (int i = 0; i < knightSpriteRenderers.Length; i++)
            {
                knightSpriteRenderers[i].sprite = Knight[i];
            }

        }
    }
    public SpriteSheet Knight => knightSpriteSheets[knightIndex];

    private int horseIndex;
    public int HorseIndex
    {
        get => horseIndex;
        set
        {
            horseIndex = (value + horseSpriteSheets.Length) % horseSpriteSheets.Length;
            horseSpriteRenderer.sprite = Horse[0];
        }
    }

    public SpriteSheet Horse => horseSpriteSheets[horseIndex];

    private bool isReady;
    public bool IsReady
    {
        get => isReady;
        set
        {
            Color color;
            string text;

            if (value)
            {
                color = Color.green;
                text = "Ready";
            }
            else
            {
                color = Color.red;
                text = "Not\nReady";
            }

            readySpriteRenderer.color = color;
            readyText.text = text;
            isReady = value;
        }
    }

    private bool isConnected;
    public bool IsConnected
    {
        get => isConnected;
        set
        {
            isConnected = value;
            connectText.enabled = !value;
            nameInputField.gameObject.SetActive(value);
            readyText.enabled = value;
            readySpriteRenderer.enabled = value;
            for (int i = 0; i < knightSpriteRenderers.Length; i++)
            {
                knightSpriteRenderers[i].gameObject.SetActive(value);
            }
            horseSpriteRenderer.gameObject.SetActive(value);
            arrows.SetActive(value);
        }
    }

    void Start()
    {
        IsConnected = false;
        IsReady = false;
        KnightIndex = 0;
        HorseIndex = 0;
    }
}