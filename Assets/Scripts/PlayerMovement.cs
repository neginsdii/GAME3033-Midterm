using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    public Vector2 TileCoordinates;
    public LayerMask ObstacleLayerMask;
    public bool isJumping;
    public bool isMoving;
    //Components

    private Rigidbody rigidbody;
    private Animator PlayerAnimator;
    public GameObject followTarget;
    public AudioSource audioSource;
    public GameObject Bomb;
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
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        int r = 0;// GridGenerator.Instance.numberOfRows / 2;
        int c = 0;// GridGenerator.Instance.numberOfColumns / 2;
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

    public void OnDrop(InputValue value)
    {


        bool isDropping = value.isPressed;
        if(isDropping && GridGenerator.Instance.NumOfBombs>0)
		{
            Vector3 tilePos = GridGenerator.Instance.grid[(int)TileCoordinates.x, (int)TileCoordinates.y].transform.position;
            GameObject tmp = Instantiate(Bomb,new Vector3  (tilePos.x,tilePos.y+0.5f,tilePos.z ),Quaternion.identity);
            GridGenerator.Instance.NumOfBombs--;

            StartCoroutine(RemoveCratesOnBombDrop());
        }

    }

    public IEnumerator RemoveCratesOnBombDrop()
    {
        yield return new WaitForSeconds(1.0f);
        int r = (int)TileCoordinates.x;
        int c = (int)TileCoordinates.y;
        for (int i = r - 1; i < r + 2; i++)
        {
            for (int j = c - 1; j < c + 2; j++)
            {
                if((i>=0 && i<GridGenerator.Instance.numberOfRows) && (j >= 0 && j < GridGenerator.Instance.numberOfColumns) )
                GridGenerator.Instance.grid[i, j].GetComponent<Tile>().ActivateCrate(false);

            }
        }
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
            if(!audioSource.isPlaying)
            audioSource.Play();
            GridGenerator.Instance.NumOfBombs++;
            Tile tmp = other.gameObject.GetComponentInParent<Tile>();
            tmp.ActivateBomb(false);
            int i = GridGenerator.Instance.emptyTileList.IndexOf(tmp.coordinates);
            if(i!=-1)
            GridGenerator.Instance.emptyTileList.RemoveAt(i);

        }
        else if(other.gameObject.CompareTag("Exit"))
		{
            SceneManager.LoadScene("EndScene");
		}
		else if (other.gameObject.CompareTag("DeathPlane"))
		{
            SceneManager.LoadScene("GameOver");

        }
    }
}
