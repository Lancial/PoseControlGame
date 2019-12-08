using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public Sprite[] backgrounds;
    public GameObject[] positions;
    public GameObject player;

    private SpriteRenderer currentSR;
    void Start()
    {
        currentSR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeBackground();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        transform.localScale = new Vector3(
            worldScreenWidth / sr.sprite.bounds.size.x,
            worldScreenHeight / sr.sprite.bounds.size.y, 1);
    }

    private void ChangeBackground()
    {
        Vector2 playerPos = player.transform.position;
        for (int i = 0; i < positions.Length; i++) {
            Vector2 pos = positions[i].transform.position;
            //Debug.Log("player pos: " + playerPos + " bg pos: " + pos);
            if(playerPos.x > pos.x)
            {
                //Debug.Log("Change bg : " + i);
                currentSR.sprite = backgrounds[i];
                //break;
            } else
            {
                break;
            }
        }
        
    }
}
