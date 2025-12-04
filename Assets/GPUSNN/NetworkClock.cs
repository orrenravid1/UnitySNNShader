using UnityEngine;

public class NetworkClock : MonoBehaviour
{
    public float TimeStep;

    public float DeltaTime { get; private set; }
    public float CurrentTime { get; private set; }
    [field: SerializeField]
    public bool On { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (On)
        {
            Tick();
        }
        else
        {
            DeltaTime = 0;
        }
    }

    public void ResetTime()
    {
        DeltaTime = 0;
        CurrentTime = 0;
    }

    public void Tick()
    {
        DeltaTime = TimeStep * Time.deltaTime;
        CurrentTime += DeltaTime;
    }
}
