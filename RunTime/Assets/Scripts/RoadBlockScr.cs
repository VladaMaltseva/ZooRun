using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockScr : MonoBehaviour
{
    GameManager GM;
    Vector3 moveVec;

    public GameObject CoinsObj;

    public int CoinChance;
    // Start is called before the first frame update
    void Start()
    {
        GM = FindObjectOfType<GameManager>();
        moveVec = new Vector3(0, 1, 0);

        CoinsObj.SetActive(Random.Range(0, 101) <= CoinChance);
    }

    // Update is called once per frame
    void Update()
    {
        if (GM.CanPlay)
            transform.Translate(moveVec * Time.deltaTime * GM.CurrentMoveSpeed);
    }
}
