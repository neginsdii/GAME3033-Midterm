using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform player;
	[SerializeField]
	private float MaxdistanceZ;
	[SerializeField]
	private float MaxdistanceX;
	[SerializeField]
	private float offset;
	[SerializeField]
	private float Speed;
	private void Start()
	{
		int r = GridGenerator.Instance.numberOfRows / 2;
		int c = GridGenerator.Instance.numberOfColumns / 2;
		transform.position = new Vector3(GridGenerator.Instance.grid[r, c].transform.position.x, 6,0);
	

	}

	private void Update()
	{
		if(player.transform.position.z - transform.position.z>MaxdistanceZ + offset)
		{
			transform.position += Vector3.forward * Time.deltaTime * Speed;
		}
		else if(player.transform.position.z - transform.position.z < MaxdistanceZ - offset)
		{
			transform.position += Vector3.back * Time.deltaTime * Speed;
		}
		if (player.transform.position.x - transform.position.x > MaxdistanceX + offset)
		{
			transform.position += Vector3.right * Time.deltaTime * Speed;
		}
		else if (player.transform.position.x - transform.position.x < MaxdistanceX - offset)
		{
			transform.position += Vector3.left * Time.deltaTime * Speed;
		}
	}


}
