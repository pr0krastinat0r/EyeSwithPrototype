using UnityEngine;

//This script was created by maxhusak.wordpress.com. If you have any feedback intend on using it, please contact me first.

public class HoverEffect : MonoBehaviour
{
    public float hoverHeight = 0.5f;
    public float hoverSpeed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * hoverSpeed) * hoverHeight + startPos.y;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}
