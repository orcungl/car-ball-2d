using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum GoalSide {Left, Right}
public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private GoalSide side;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (!other.CompareTag("Ball")) return;
         
        if (side == GoalSide.Left) {GameManager.Instance.ScoreRight(); }
        if (side == GoalSide.Right) {GameManager.Instance.ScoreLeft(); }
    }
}
