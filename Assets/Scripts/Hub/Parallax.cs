using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    /*    public Camera camera;
        public Transform subject;

        Vector2 startPosition;
        float startZ;
        Vector2 travel => (Vector2)camera.transform.position - startPosition;

        float distanceFromSubject => transform.position.z - subject.position.z;
        float clippingPlane => (camera.transform.position.z + (distanceFromSubject >  0 ? camera.farClipPlane : camera.nearClipPlane));

        float parallaxFactor => Mathf.Abs(distanceFromSubject) / clippingPlane;
        private void Start()
        {
            startPosition = transform.position;
            startZ = transform.position.z;
        }
        private void Update()
        {
            Vector2 newPos = startPosition  + travel * parallaxFactor;
            transform.position = new Vector3(newPos.x, newPos.y, startZ);
        }*/

    private float length, height, startposX, startposY;
    public float parallaxFactor;
    public GameObject cam;
    public float pixelsPerUnit = 16;

    void Start()
    {
        startposX = transform.position.x;
        startposY = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    /* void Update()
     {
         float distance = cam.transform.position.x * parallaxFactor;
         Vector3 newPosition = new Vector3(startpos + distance, transform.position.y, transform.position.z);
         transform.position = newPosition;
     }*/
    void Update()
    {
        float tempX = cam.transform.position.x * (1 - parallaxFactor);
        float distanceX = cam.transform.position.x * parallaxFactor;
        float tempY = cam.transform.position.y * (1 - parallaxFactor);
        float distanceY = cam.transform.position.y * parallaxFactor;

        Vector3 newPosition = new Vector3(startposX + distanceX, startposY + distanceY, transform.position.z);

        transform.position = PixelPerfectClamp(newPosition, pixelsPerUnit);

        if (tempX > startposX + (length / 2)) startposX += length;
        else if (tempX < startposX - (length / 2)) startposX -= length;

        if (tempY > startposY + (height / 2)) startposY += height;
        else if (tempY < startposY - (height / 2)) startposY -= height;
    }

    private Vector3 PixelPerfectClamp(Vector3 locationVector, float pixelsPerUnit)
    {
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(locationVector.x * pixelsPerUnit), Mathf.CeilToInt(locationVector.y * pixelsPerUnit), Mathf.CeilToInt(locationVector.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}


