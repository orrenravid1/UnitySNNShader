using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoSwitcher : MonoBehaviour
{
    [SerializeField]
    private VideoClipManager clipManager;

    [SerializeField]
    private Vector2 randomRange = new Vector2(5, 10);

    [SerializeField]
    private bool randomNext;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SwitchVideo());
    }

    private IEnumerator SwitchVideo()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(Random.Range(randomRange.x, randomRange.y));
            if (!randomNext)
            {
                clipManager.Next();
            }
            else
            {
                clipManager.RandomChoice();
            }
        }
    }
}
