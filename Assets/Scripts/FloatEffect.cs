using UnityEngine;
using System.Collections;

public class FloatEffect : MonoBehaviour
{
    public bool isActive = false;
    private float curTime = 0;
    private float period = 4;
    private float amplitude = .07f;
    private float activeLerp;

    void Start()
    {
        curTime = Random.Range(0, period);
    }

    void Update()
    {
        //Update current time
        activeLerp = Mathf.Lerp(activeLerp, (isActive ? 1f : 0), 0.08f);
        curTime = (curTime + Time.deltaTime * (1 + 9f * activeLerp)) % period;
        float value = amplitude * Mathf.Sin(((1 / period) * 2 * Mathf.PI) * curTime);
        transform.position = new Vector3(transform.position.x, -.3f + .5f * activeLerp + value, transform.position.z);
    }
}

