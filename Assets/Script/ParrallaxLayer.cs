using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float speed = 1f;

    private Transform[] tiles;
    private float tileWidth;

    void Start()
    {
        int count = transform.childCount;
        tiles = new Transform[count];
        for (int i = 0; i < count; i++)
            tiles[i] = transform.GetChild(i);

        SpriteRenderer sr = tiles[0].GetComponent<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        System.Array.Sort(tiles, (a, b) => a.position.x.CompareTo(b.position.x));
    }

    void Update()
    {
        Vector3 delta = Vector3.left * speed * Time.deltaTime;
        foreach (var t in tiles)
            t.position += delta;

        float rightMostX = tiles[0].position.x;
        foreach (var t in tiles)
            if (t.position.x > rightMostX)
                rightMostX = t.position.x;

        foreach (var t in tiles)
        {
            float tileRightEdge = t.position.x + tileWidth * 0.5f;

            if (tileRightEdge < Camera.main.transform.position.x - Camera.main.orthographicSize * Camera.main.aspect)
            {
                t.position = new Vector3(
                    rightMostX + tileWidth,
                    t.position.y,
                    t.position.z
                );

                rightMostX = t.position.x;
            }
        }
    }
}
