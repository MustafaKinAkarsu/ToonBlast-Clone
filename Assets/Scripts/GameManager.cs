using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject goalImage;
    public string goalCubeTag;
    public int goalCount;
    public int movesCount;
    public int quadHeight;
    public int quadWidth;
    [SerializeField] Sprite goalImageSprite;
    [SerializeField] TextMeshProUGUI goalCountText;
    [SerializeField] TextMeshProUGUI movesText;
    GridGenerator gridGenerator;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        goalCountText.text = goalCount.ToString();
        movesText.text = movesCount.ToString();
        goalImage.GetComponent<Image>().sprite = goalImageSprite;
        gridGenerator = GridGenerator.Instance;
        //gridGenerator.ChangeQuadSize(quadWidth, quadHeight);

    }
    private void Start()
    {
        Instance = this;
    }
    public void UpdateMove()
    {
        if(movesCount > 0)
        {
            movesCount--;
            movesText.text = movesCount.ToString();
        }
    }
    public void UpdateGoal()
    {
        if (goalCount > 0)
        {
            goalCount--;
            goalCountText.text = goalCount.ToString();
        }
    }
}
