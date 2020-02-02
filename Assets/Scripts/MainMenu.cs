using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Transform FloatingShip, RotatingStation;

    private Vector3 _rotationVector;

    void Start()
    {
        _rotationVector = new Vector3(0.0f, 0.05f, 0.0f);
    }

    void Update()
    {
        RotatingStation.Rotate(_rotationVector);
        FloatingShip.Translate(new Vector3(0.0f, Mathf.Sin(Time.time) * 0.01f, 0.0f));
    }

    public void OnBegin() {
        SceneManager.LoadScene("ProdGame");
    }

    public void OnTutorial() {
        // TODO
    }

    public void OnSettings() {
        // TODO
    }
}
