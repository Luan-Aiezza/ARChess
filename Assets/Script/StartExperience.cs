using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class StartExperience : MonoBehaviour
{
    [SerializeField] private GameObject boardPrefab;
    private GameObject spawnedBoard;

    public void OnStartExperience(ARPlane plane)
    {
        spawnedBoard = Instantiate(
            boardPrefab,
            plane.center,
            Quaternion.identity
        );

        // Ajusta o tabuleiro para ficar sobre o plano
        spawnedBoard.transform.position = plane.center;
        spawnedBoard.transform.up = plane.normal;

        // Notifica o GameManager
        FindObjectOfType<GameManager>().OnBoardSpawned(spawnedBoard);
    }
}
