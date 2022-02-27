using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float WalkSpeed = 5;
    [SerializeField]
    private float runSpeed = 10;
    [SerializeField]
    private float JumpForce = 5;
    [SerializeField]
    private float rotationSpeed = 5;
    public LayerMask ObstacleLayerMask;
    public bool isJumping;
    public bool isMoving;
    //Components

    private Rigidbody rigidbody;
    private Animator PlayerAnimator;
    public GameObject followTarget;

    Vector2 inputVector = Vector2.zero;
    Vector3 MoveDirection = Vector3.zero;
    public Vector2 lookInput = Vector2.zero;
    public float AimSensetivity = 1;


    private Vector3 targetPosition;
    private Vector3 startingPosition;

    public readonly int isMovingHash = Animator.StringToHash("IsMoving");

    private void Awake()
    {

      
        rigidbody = GetComponent<Rigidbody>();
        PlayerAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        int r = GridGenerator.Instance.numberOfRows / 2;
        int c = GridGenerator.Instance.numberOfColumns / 2;
        transform.position = new Vector3(GridGenerator.Instance.grid[r,c].transform.position.x, 3, GridGenerator.Instance.grid[r, c].transform.position.z);
    }

    void Update()
    {
        //Movement
        if (isJumping) return;
    
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPosition)<0.1)
            {
                transform.position = targetPosition;
                isMoving = false;
               
            }
            MoveDirection = targetPosition - startingPosition;
            transform.position += (MoveDirection) * WalkSpeed * Time.deltaTime;
            
        }
        if(MoveDirection!=Vector3.zero)
		{
            Vector3 dir = MoveDirection.normalized;
            Quaternion toRotation = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

		}

        if (!isMoving)
        {
            if (inputVector.x > 0)
            {
                if (!Physics.Raycast(transform.position, Vector3.right, GridGenerator.Instance.tileSize,ObstacleLayerMask))
                {
                    targetPosition = transform.position + Vector3.right *GridGenerator.Instance.tileSize;
                    startingPosition = transform.position;
                    isMoving = true;
                }
            }
            else if (inputVector.x < 0)
            {
                if (!Physics.Raycast(transform.position, Vector3.left, GridGenerator.Instance.tileSize, ObstacleLayerMask))
                {
                    targetPosition = transform.position + Vector3.left*GridGenerator.Instance.tileSize;
                    startingPosition = transform.position;
                    isMoving = true;
                }
            }
            else if (inputVector.y < 0)
            {
                if (!Physics.Raycast(transform.position, Vector3.back, GridGenerator.Instance.tileSize, ObstacleLayerMask))
                {
                    targetPosition = transform.position + Vector3.back* GridGenerator.Instance.tileSize;
                    startingPosition = transform.position;
                    isMoving = true;
                }
            }
            else if (inputVector.y > 0)
            {
                if (!Physics.Raycast(transform.position, Vector3.forward, GridGenerator.Instance.tileSize, ObstacleLayerMask))
                {
                    targetPosition = transform.position + Vector3.forward* GridGenerator.Instance.tileSize;
                    startingPosition = transform.position;
                    isMoving = true;
                }
            }
        }

        PlayerAnimator.SetBool(isMovingHash, isMoving);

    }

    public void OnMovement(InputValue value)
    {
        inputVector = value.Get<Vector2>();
        
    }

    public void OnJump(InputValue value)
    {
        if (isJumping)
        {
            return;
        }
        isJumping = value.isPressed;
        rigidbody.AddForce((transform.up + transform.forward) * JumpForce, ForceMode.Impulse);
      

    }
  

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground") && !isJumping) return;

       isJumping = false;

    }

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Bomb"))
		{
            GridGenerator.Instance.NumOfBombs++;
            Tile tmp = other.gameObject.GetComponentInParent<Tile>();
            tmp.ActivateBomb(false);
            int i = GridGenerator.Instance.emptyTileList.IndexOf(tmp.coordinates);
            if(i!=-1)
            GridGenerator.Instance.emptyTileList.RemoveAt(i);

        }
    }
}
