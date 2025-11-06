using UnityEngine;
using UnityEngine.InputSystem;
public class MusicPlayerTester : MonoBehaviour
{
    [SerializeField] private AudioClip _song01;
    // [SerializeField] private AudioClip _song03;


    private void Awake()
    {
        MusicManager.Instance.Play(_song01, 3);
    }


}
