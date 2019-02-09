using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject block;
    public GameObject lastBlock;
    public GameObject stackedBlock;
    
    int spawnPosition = 0;
    bool isGameOver = false;

    public float error = 0.2f;


    void Start()
    {
        
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            StopBlock();

            if (!isGameOver)
            {
                SpawnBlock();
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
        newBlock.SetActive(true);
        newBlock.transform.position = lastBlock.transform.position + Vector3.up;
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

            if (deltaX > error && deltaX < stackedBlock.transform.localScale.x )
            {
                float transCenter = (stackedBlock.transform.position.x + lastBlock.transform.position.x) / 2;
                lastBlock.transform.position = new Vector3(transCenter, lastBlock.transform.position.y, lastBlock.transform.position.z);
                lastBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x - deltaX, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);

                if (lastBlock.transform.position.x > 0)
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(transCenter + 1.5f, lastBlock.transform.position.y, lastBlock.transform.position.z),Quaternion.identity);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(3 - lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }
                else
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(transCenter - 1.5f, lastBlock.transform.position.y, lastBlock.transform.position.z), Quaternion.identity);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(3 - lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }

            }
            else if(deltaX > stackedBlock.transform.localScale.x)
            {
                //TODO :: GameOver
                isGameOver = true;
                Debug.Log("GAME OVER!!");
            }
            else if(deltaX <= error)
            {
                if(!isGameOver)
                    lastBlock.transform.position = stackedBlock.transform.position + Vector3.up;
            }

        }
        else
        {
            float deltaZ = stackedBlock.transform.position.z - lastBlock.transform.position.z;
            deltaZ = Mathf.Abs(deltaZ);

            if (deltaZ > error && deltaZ < stackedBlock.transform.localScale.z)
            {
                float transCenter = (stackedBlock.transform.position.z + lastBlock.transform.position.z) / 2;
                lastBlock.transform.position = new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter);
                lastBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, lastBlock.transform.localScale.z - deltaZ);

                if (lastBlock.transform.position.z > 0)
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter + 1.5f), Quaternion.identity);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, 3-lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }
                else
                {
                    GameObject fallingBlock = Instantiate(block, new Vector3(lastBlock.transform.position.x, lastBlock.transform.position.y, transCenter - 1.5f), Quaternion.identity);
                    fallingBlock.SetActive(true);
                    fallingBlock.transform.localScale = new Vector3(lastBlock.transform.localScale.x, lastBlock.transform.localScale.y, 3- lastBlock.transform.localScale.z);
                    fallingBlock.AddComponent<Rigidbody>();
                }

            }
            else if (deltaZ > stackedBlock.transform.localScale.z)
            {
                //TODO :: GameOver
                isGameOver = true;
                Debug.Log("GAME OVER!!");
            }
            else if (deltaZ <= error)
            {
                if (!isGameOver)
                    lastBlock.transform.position = stackedBlock.transform.position + Vector3.up;
            }
        }
    }




}
