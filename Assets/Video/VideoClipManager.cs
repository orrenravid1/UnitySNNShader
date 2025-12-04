using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using NaughtyAttributes;
using static MathUtils;

public class VideoClipManager : MonoBehaviour
{
    [SerializeField]
    VideoPlayer videoPlayer;
    [SerializeField]
    List<VideoClip> clipList;
    [SerializeField]
    private int minClip;
    [SerializeField]
    private int maxClip;

    Dictionary<int, double> clipTimes = new Dictionary<int, double>();
    int currVideoClip = 0;

    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < clipList.Count; i++)
        {
            clipTimes[i] = 0;
        }
        videoPlayer.prepareCompleted += OnPrepareCompleted;
        videoPlayer.clip = clipList[currVideoClip];
        videoPlayer.Play();
    }

    public int MinClip { get { return Mathf.Clamp(minClip, 0, clipList.Count - 1); } }

    public int MaxClip { get { return Mathf.Clamp(maxClip, 0, clipList.Count - 1); } }

    [Button]
    public void Next()
    {
        ChooseClip(currVideoClip + 1);
    }

    [Button]
    public void RandomChoice()
    {
        ChooseClip(Random.Range(MinClip, MaxClip + 1));
    }

    public void ChooseClip(int clipIdx)
    {
        clipTimes[currVideoClip] = videoPlayer.time;
        print($"clipIdx = {clipIdx}, MinClip = {MinClip}, MaxClip = {MaxClip}");

        currVideoClip = RangeMod(clipIdx, MinClip, MaxClip + 1);
        print(currVideoClip);

        videoPlayer.Stop();
        videoPlayer.clip = clipList[currVideoClip];
        videoPlayer.Play();
    }

    private void OnPrepareCompleted(VideoPlayer _)
    {
        videoPlayer.time = clipTimes[currVideoClip];
    }
}
