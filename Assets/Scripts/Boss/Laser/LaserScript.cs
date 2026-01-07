using System;
using System.Collections;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    [Header("Hierarchy")]  
    public Transform laserRoot;
    public Transform laserVisual;

    [Header("Laser Parts")]
    public Transform start;
    public Transform middle;
    public Transform end;
    public LayerMask collisionMask;

    [Header("Laser Settings")]
    private float maxLaserLength = 20f; // longueur max du laser

    [Header("Laser Rotation")]  
    public float sweepAngle = 45f; // angle de balayage
    public float sweepDuration = 2f; // durée d'un balayage complet
    public bool rotateLeft = true; // direction de rotation

    public bool IsRunning { get; private set; } = false;
    private float timer;
    private Quaternion startRotation;
    private Vector2 startPosition;

    void OnEnable()
    {
        timer = 0f;
        startRotation = laserRoot.rotation;
        startPosition = laserVisual.localPosition;
        IsRunning = true;
    }


    void Update()
    {
        timer += Time.deltaTime;

        UpdateRotation();
        UpdateLaser();
    }

    private void UpdateLaser()
    {
        Vector2 origin = laserRoot.position;
        Vector2 direction = -laserRoot.up;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxLaserLength, collisionMask);

        float laserLength = hit ? hit.distance : maxLaserLength;

        //DEBUG RAY
        if (hit.collider != null)
        {
            Debug.DrawRay(origin, direction * hit.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(origin, direction * maxLaserLength, Color.green);
        }

        //LaserVisual ne tourne pas, il reste aligné localement
        laserVisual.localPosition = Vector3.zero;
        laserVisual.localRotation = Quaternion.identity;

        // Start
        start.localPosition = Vector3.zero;

        // Middle
        middle.localPosition = new Vector3(0, -(laserLength / 2) , 0);
        middle.localScale = new Vector3( 1f, laserLength, 1f);

        // End
        end.localPosition = new Vector3(0, -laserLength, 0);

    }

    private void UpdateRotation()
    {
        if(sweepDuration <= 0) return;  

        float t = Mathf.Clamp01(timer / sweepDuration);

        float direction = rotateLeft ? 1f : -1f;
        float angle = sweepAngle * t * direction;

        laserRoot.rotation = startRotation * Quaternion.Euler(0, 0, angle);
    }


    // API FSM
    public void ActivateLaser(bool left)
    {
        rotateLeft = left;
        timer = 0f; 
        startRotation = laserRoot.rotation;
        gameObject.SetActive(true);
    }

    public bool IsSweepComplete()
    {
        return timer >= sweepDuration;
    }

    public void StopLaser()
    {
        gameObject.SetActive(false);
        laserRoot.rotation = startRotation;
        laserVisual.localPosition = startPosition;
        IsRunning = false;
    }

    public bool SidePlayer(Transform playerTransform)
    {
        Vector2 toPlayer = playerTransform.position - laserRoot.position;
        float cross = Vector3.Cross(-laserRoot.up, toPlayer).z;
        return cross > 0; // true si à gauche, false si à droite
    }
}
