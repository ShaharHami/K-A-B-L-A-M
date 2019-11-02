using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] string tagName;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tagName);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
