using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	[SerializeField]
	private Color OccupiedColor;
	[SerializeField]
	private Color FreeColor;
	public bool IsOccupied;
	public GameObject TileCrate;
	public GameObject TileBomb;
	public Vector2 coordinates = new Vector2(0,0);
	public void ChangeTileColor(bool isOccupied)
	{
		if(isOccupied)
		{
			GetComponent<Renderer>().material.color = OccupiedColor;
		}
		else
		{
			GetComponent<Renderer>().material.color = FreeColor;
		}
		IsOccupied = IsOccupied;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			ChangeTileColor(true);
			IsOccupied = true;
			int i = GridGenerator.Instance.emptyTileList.IndexOf(coordinates);
			if(i!=-1)
			GridGenerator.Instance.emptyTileList.RemoveAt(i);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			ChangeTileColor(false);
			IsOccupied = false;
			GridGenerator.Instance.emptyTileList.Add(coordinates);
		}
	}

	public void ActivateCrate(bool isActive)
	{
		TileCrate.SetActive(isActive);
		IsOccupied = isActive;
	}
	public void ActivateBomb(bool isActive)
	{
		TileBomb.SetActive(isActive);
		IsOccupied = isActive;
	}
}
