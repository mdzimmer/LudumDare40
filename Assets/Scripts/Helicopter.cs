using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {
    GameManager gm;
    Vector3 goal;

    int PAYLOAD = 10;
    float SPEED = 12.0f;
    float ARRIVAL_DISTANCE = 0.1f;

    public void Initialize()
    {
        gm = GameManager.GetManager();
        goal = gm.grid.TileToPosition(new Vector2(0.0f, 0.0f));
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            Vector3 delta = goal - transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(delta.x, 2.0f) + Mathf.Pow(delta.z, 2.0f));
            if (distance <= ARRIVAL_DISTANCE)
            {
                gm.currency.IncrementValue(PAYLOAD);
                Destroy(this.gameObject);
            }
            float angleTowards = Mathf.Atan2(delta.x, delta.z);
            transform.localRotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, -angleTowards * Mathf.Rad2Deg));
            Vector3 translation = new Vector3(
                Mathf.Sin(angleTowards) * Time.deltaTime,
                0.0f,
                Mathf.Cos(angleTowards) * Time.deltaTime
                );
            transform.position = transform.position + translation;
            yield return new WaitForEndOfFrame();
        }
    }
}
