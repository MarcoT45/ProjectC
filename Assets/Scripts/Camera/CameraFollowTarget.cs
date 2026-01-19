using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * Script à attacher à la VCAM
 * Nécessaire pour éviter le jittering comme c'est du pixel perfect
 */
public class CameraFollowTarget : MonoBehaviour
{
    public GameObject target;
    public float PixelsPerUnit;

    private Vector3 startPos;
    //private Vector3 velocity;

    void Update()
    {
        startPos = transform.position;
        //velocity = startPos;
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        //transform.position = PixelPerfectClamp(target.transform.position, PixelsPerUnit);
        //transform.position = Vector3.SmoothDamp(startPos, target.transform.position, ref velocity, 0.05f);
    }

    private Vector3 PixelPerfectClamp(Vector3 moveVector, float pixelsPerUnit)
    {
        //Ligne de départ, seulement comme on change également le Z de la caméra pour celui de la target. La caméra n'affiche plus les objets sur le même Z ( perso, tiles, etc).
        //Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(moveVector.z * pixelsPerUnit));
        Vector3 vectorInPixels = new Vector3(Mathf.CeilToInt(moveVector.x * pixelsPerUnit), Mathf.CeilToInt(moveVector.y * pixelsPerUnit), Mathf.CeilToInt(startPos.z * pixelsPerUnit));
        return vectorInPixels / pixelsPerUnit;
    }
}
