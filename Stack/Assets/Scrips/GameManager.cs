using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    public GameObject block;
    public GameObject lastBlock;
    public GameObject stackedBlock;
    
    int spawnPosition = 0;
    bool isGameOver = false;

    public float error = 0.2f;

    int currentCombo = 0;
    int maxCombo;

    bool isSizeUp = false;
    Vector3 sizeUpVec = new Vector3(1.1f, 1, 1.1f);

    public Color prevColor;
    public Color nextColor;

    public int score;

    void Start()
    {
        prevColor = RandomColor();
        nextColor = RandomColor();
        ChangeColor(stackedBlock);
        UIManager.instance.SetScore(score);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            StopBlock();

            if (!isGameOver)
            {
                SpawnBlock();
                score++;
                UIManager.instance.SetScore(score);
                Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 8 + spawnPosition, Camera.main.transform.position.z);
            }
            else
            {
                Debug.Log("GAME OVER");
            }
        }


    }

    void SpawnBlock()
    {
        GameObject newBlock = Instantiate(block, new Vector3(0, spawnPosition, 0), Quaternion.identity);
        ChangeColor(newBlock);
        newBlock.SetActive(true);
        newBlock.transform.position = lastBlock.transform.position + Vector3.up;

        if (isSizeUp)
        {
            newBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x * 1.1f, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z * 1.1f);
            isSizeUp = false;
            currentCombo = 0;
        }
        else
            newBlock.transform.localScale = lastBlock.transform.localScale;

        if (spawnPosition % 2 == 0)
        {
            newBlock.GetComponent<BlockController>().currentState = MOVESTATE.MOVEX;
        }
        else
        {
            newBlock.GetComponent<BlockController>().currentState = MOVESTATE.MOVEZ;
        }
        spawnPosition++;
        lastBlock = newBlock;
    }

    void StopBlock()
    {
        try{
            lastBlock.GetComponent<BlockController>().currentState = MOVESTATE.STOP;
                       
            CutBlock();

            stackedBlock = lastBlock;
        }
        catch {
        }
    }

    void CutBlock()
    {
        if(spawnPosition%2 == 1)
        {
            float deltaX = stackedBlock.transform.position.x - lastBlock.transform.position.x;
            deltaX = Mathf.Abs(deltaX);

            if (deltaX > error*lastBlock.transform.localScale.x  && deltaX < stackedBlock.transform.localScale.x )
            {
                currentCombo = 0;
                float transCenter = (stackedBlock.transform.position.x + lastBlock.transform.position.x) / 2;
                lastBlock.transform.position = new Vector3(transCenter, lastBlock.transform.position.y, lastBlock.transform.position.z);
                lastBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x - deltaX, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);

                if (lastBlock.transform.position.x > 0)
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(transCenter + stackedBlock.transform.localScale.x/2, lastBlock.transform.position.y, lastBlock.transform.position.z),Quaternion.identity);
                    ChangeColor(fallingBlock);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(stackedBlock.transform.localScale.x - lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }
                else
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(transCenter - stackedBlock.transform.localScale.x/2, lastBlock.transform.position.y, lastBlock.transform.position.z), Quaternion.identity);
                    ChangeColor(fallingBlock);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(stackedBlock.transform.localScale.x - lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }

            }
            else if(deltaX > stackedBlock.transform.localScale.x)
            {
                //TODO :: GameOver
                isGameOver = true;
                Debug.Log("GAME OVER!!");
            }
            else if(deltaX <= error*lastBlock.transform.localScale.x)
            {
                if (!isGameOver)
                {
                    lastBlock.transform.position = stackedBlock.transform.position + Vector3.up;
                    CheckCombo();
                }
            }

        }
        else
        {
            float deltaZ = stackedBlock.transform.position.z - lastBlock.transform.position.z;
            deltaZ = Mathf.Abs(deltaZ);

            if (deltaZ > error*lastBlock.transform.localScale.z && deltaZ < stackedBlock.transform.localScale.z)
            {
                currentCombo = 0;
                float transCenter = (stackedBlock.transform.position.z + lastBlock.transform.position.z) / 2;
                lastBlock.transform.position = new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter);
                lastBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z - deltaZ);

                if (lastBlock.transform.position.z > 0)
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter + stackedBlock.transform.localScale.z/2), Quaternion.identity);
                    ChangeColor(fallingBlock);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, stackedBlock.transform.localScale.z-lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }
                else
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter - stackedBlock.transform.localScale.z/2), Quaternion.identity);
                    ChangeColor(fallingBlock);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, stackedBlock.transform.localScale.z- lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }

            }
            else if (deltaZ > stackedBlock.transform.localScale.z)
            {
                //TODO :: GameOver
                isGameOver = true;
                Debug.Log("GAME OVER!!");
            }
            else if (deltaZ <= error*lastBlock.transform.localScale.z)
            {
                if (!isGameOver)
                {
                    lastBlock.transform.position = stackedBlock.transform.position + Vector3.up;
                    CheckCombo();
                }
            }
        }
    }

    void CheckCombo()
    {
        currentCombo++;
        if(currentCombo >= 7)
        {
            isSizeUp = true;
        }
    }

    Color RandomColor()
    {
        float r = Random.Range(100f, 200f) / 255f;
        float g = Random.Range(100f, 200f) / 255f;
        float b = Random.Range(100f, 200f) / 255f;

        return new Color(r, g, b);
    }

    void ChangeColor(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (score % 11) / 10f);
        Renderer renderer = go.GetComponent<Renderer>();
        renderer.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if(applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = RandomColor();
        }

    }
    



}
