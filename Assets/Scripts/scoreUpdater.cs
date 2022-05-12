using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreUpdater : MonoBehaviour
{
    private TextMeshProUGUI textMesH;
    private int score;
    // Start is called before the first frame update
    void Start()
    {
        textMesH = GetComponent<TextMeshProUGUI>();
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        textMesH.text = score.ToString();
        score++;
    }
}
