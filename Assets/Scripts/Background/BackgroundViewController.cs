using UnityEngine;

[RequireComponent(typeof(BackgroundScroller))]
public class BackgroundViewController : MonoBehaviour
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
        //scroller.Scroll();
    }
}
