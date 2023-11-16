using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class ScoreTable : MonoBehaviour {
    private Transform entryContainer;
    private Transform entryTemplate;
    private Highscores highscores;

    private List<Transform> scoreEntryTransforms;

    private void Awake() {
        entryContainer = transform.Find("ScoreEntryContainer");
        entryTemplate = entryContainer.Find("ScoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        string jsonStr = PlayerPrefs.GetString("scoreTable");
        if (jsonStr == null || jsonStr == "") {
            string json = JsonUtility.ToJson(new Highscores());
            PlayerPrefs.SetString("scoreTable", json);
            PlayerPrefs.Save();
        }

        //string jsonStr = PlayerPrefs.GetString("scoreTable");
        highscores = JsonUtility.FromJson<Highscores>(jsonStr);
        SortHighscore(highscores);

        scoreEntryTransforms = new List<Transform>();
        for (int i = 0; i < 6; i++) {
            if (i < highscores.scoreEntryList.Count) {
                CreateScoreEntryTransform(highscores.scoreEntryList[i], entryContainer, scoreEntryTransforms);
            }
        }
    }

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if(GameManager.Instance.IsGameOver()) {
            ScoreEntry entry = new ScoreEntry() { name = "Player" + scoreEntryTransforms.Count, score = ScoreCardUI.Instance.GetScore() };
            AddScoreEntry(entry.score, entry.name);
            SortHighscore(highscores);
            for (int i = 0; i < 6; i++) {
                if (i < highscores.scoreEntryList.Count) {
                    CreateScoreEntryTransform(highscores.scoreEntryList[i], entryContainer, scoreEntryTransforms);
                }
            }
        } else if(GameManager.Instance.IsCountdownToStartActive()) {
            foreach(Transform transform in scoreEntryTransforms) {
                Destroy(transform.gameObject);
            }
            scoreEntryTransforms.Clear();
        }
    }

    private void SortHighscore(Highscores highscores) {
        for (int i = 0; i < highscores.scoreEntryList.Count; i++) {
            for (int j = i + 1; j < highscores.scoreEntryList.Count; j++) {
                if (highscores.scoreEntryList[j].score > highscores.scoreEntryList[i].score) {
                    //  Swap
                    ScoreEntry tmp = highscores.scoreEntryList[i];
                    highscores.scoreEntryList[i] = highscores.scoreEntryList[j];
                    highscores.scoreEntryList[j] = tmp;
                }
            }
        }
    }

    private void CreateScoreEntryTransform(ScoreEntry entry, Transform container, List<Transform> transformList) {
        float templateHeight = 50f;
        Transform entryTransform = Instantiate(entryTemplate, entryContainer);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = transformList.Count + 1;
        string rankString = rank.ToString() + ".";
        entryTransform.Find("Pos").GetComponent<TextMeshProUGUI>().text = rankString;

        int score = entry.score;
        entryTransform.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();

        string name = entry.name;
        entryTransform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;

        transformList.Add(entryTransform);
    }

    private void AddScoreEntry(int score, string name) {
        ScoreEntry entry = new ScoreEntry() { score = score, name = name };

        string jsonStr = PlayerPrefs.GetString("scoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonStr);

        if(highscores.scoreEntryList == null) {
            highscores.scoreEntryList = new List<ScoreEntry>();
        }

        highscores.scoreEntryList.Add(entry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("scoreTable", json);
        PlayerPrefs.Save();
    }

    private class Highscores {
        public List<ScoreEntry> scoreEntryList;
    }

    [Serializable]
    private class ScoreEntry {
        public int score;
        public string name;
    }
}