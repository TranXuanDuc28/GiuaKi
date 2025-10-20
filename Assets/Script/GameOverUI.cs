using UnityEngine;
using TMPro; // <- thêm dòng này để dùng TextMeshPro

public class GameOverUI : MonoBehaviour
{
    void Start()
    {
        // Lấy dữ liệu điểm và kết quả lưu trong GameManager
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        string result = PlayerPrefs.GetString("GameResult", "Lose");

        // Tìm TextMeshPro có tag "Score"
        GameObject scoreObj = GameObject.FindGameObjectWithTag("Score");

        if (scoreObj != null)
        {
            TMP_Text scoreText = scoreObj.GetComponent<TMP_Text>();
            if (scoreText != null)
            {
                scoreText.text = "Đạt điểm: " + finalScore.ToString() + "/" + GameManager.Instance.scoreGoal.ToString();
            }
            else
            {
                Debug.LogWarning("Không tìm thấy TMP_Text trên object có tag 'Score'");
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy object nào có tag 'Score'");
        }

        // Nếu bạn có Text hiển thị kết quả (Thắng/Thua)
        GameObject resultObj = GameObject.FindGameObjectWithTag("Results"); // hoặc đặt tag riêng là "Result"
        if (resultObj != null)
        {
            TMP_Text resultText = resultObj.GetComponent<TMP_Text>();
            if (resultText != null)
            {
                resultText.text = (result == "Win") ? "CHIẾN THẮNG" : "THẤT BẠI";
            }
        }
    }
}
