using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    
    public TextMeshPro scoreText;
    
    public void SetScore(int _score)
    {
        scoreText.text = _score.ToString();
    }



}
