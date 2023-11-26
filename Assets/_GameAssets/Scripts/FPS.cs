using UnityEngine;

public class FPS : MonoBehaviour
{
    [SerializeField] private int _fps;


    private void Awake()
    {
        Application.targetFrameRate = _fps;
    }



}
