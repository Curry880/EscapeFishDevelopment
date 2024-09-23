using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject[] HUDs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.isPlaying)
        {
            return;
        }
        foreach (GameObject h in HUDs)
        {
            h.SetActive(false);
        }
    }
}
