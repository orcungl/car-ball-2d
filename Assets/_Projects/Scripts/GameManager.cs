using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private Rigidbody2D ballRb;
    [SerializeField] private Transform ballSpawn;
    [SerializeField] private Rigidbody2D carRb;
    [SerializeField] private Transform carSpawn;
    [SerializeField] private CarController carController;
    [SerializeField] private TMP_Text kickoffText;

    [SerializeField] float resetDelayBall = 0.7f;
    [SerializeField] float resetDelayCar = 0.8f;

    private int kickoffCounter = 3;
    private bool isKickoff = false;
    private int scoreLeft = 0;
    private int scoreRight = 0;
    private bool isResetting;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {

            Destroy(gameObject);
            return;

        }

        Instance = this;
    }
    private void Start()
    {
        UpdateScoreUI();
    }

    public void ScoreLeft()
    {
        scoreLeft++;
        OnGoalScored();
    }
    public void ScoreRight()
    {
        scoreRight++;
        OnGoalScored();
    }
    public void OnGoalScored()
    {

        if (isResetting == true) return;

        if (ballRb == null || ballSpawn == null) return;
        ballRb.linearVelocity = Vector2.zero;
        ballRb.angularVelocity = 0f;
        carController.canDrive = false;
        isResetting = true;
        UpdateScoreUI();
        Invoke("ResetBall", resetDelayBall);
        Invoke("ResetCar", resetDelayCar);
    }
    private void ResetBall()
    {
        ballRb.position = ballSpawn.position;
        ballRb.rotation = 0f;
    }
    private void ResetCar()
    {
        carRb.position = carSpawn.position;
        carRb.rotation = 0f;
        StartKickoff();
        isResetting = false;
    }

    private void UpdateScoreUI()
    {
        if (scoreText == null) return;
        scoreText.text = scoreLeft + " - " + scoreRight;
    }

    private void StartKickoff()
    {
        isKickoff = true;
        kickoffCounter = 3;
        kickoffText.gameObject.SetActive(true);
        kickoffText.text = "3";
        carController.canDrive = false;
        Invoke("KickoffTick", 1f);
    }
    private void KickoffTick()
    {
        kickoffCounter--;
        if(kickoffCounter > 0 )
        {
            kickoffText.text = kickoffCounter.ToString();
            Invoke("KickoffTick", 1f);
        }

        if( kickoffCounter == 0 )
        {
            kickoffText.text = "START";
            Invoke("EndKickoff", 0.7f);
        }
    }
    private void EndKickoff()
    {
        kickoffText.gameObject.SetActive(false);
        isKickoff = false;
        carController.canDrive = true;
    }
}

        
   

