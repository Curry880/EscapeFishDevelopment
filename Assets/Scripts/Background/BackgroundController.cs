using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BackgroundScroller))]
public class BackgroundController : MonoBehaviour
{
    private BackgroundScroller scroller;
    // Start is called before the first frame update
    void Start()
    {
        scroller = GetComponent<BackgroundScroller>();
    }

    // Update is called once per frame
    void Update()
    {
        scroller.Scroll();
    }
}
