using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    
    private GameObject character;
    private Rigidbody2D physics;
    private Vector2 input;
    public float moveSpeed;
    public Transform movePoint;

    public LayerMask stopMovement;

    // Start is called before the first frame update
    void Start()
    {
        character = this.gameObject;
        physics = character.GetComponent<Rigidbody2D>();
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movePointDirection; 
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        if( Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                movePointDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                if(!Physics2D.OverlapCircle(movePoint.position + movePointDirection, .2f, stopMovement ))
                {
                    movePoint.position += movePointDirection;
                }
                
            }else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                movePointDirection = new Vector3( 0f, Input.GetAxisRaw("Vertical"), 0f);
                if(!Physics2D.OverlapCircle(movePoint.position + movePointDirection, .2f, stopMovement ))
                {
                    movePoint.position += movePointDirection;
                }
            }
        }
    }
}
