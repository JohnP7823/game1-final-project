using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelHandler : MonoBehaviour
{
    public GameObject p1, p2;

    // Start is called before the first frame update
    void Start()
    {
        setPanel(0);
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Determines which panel is displayed 
    public void setPanel(int p)
    {
        switch (p)
        {
            case 0:
                p1.SetActive(true);
                p2.SetActive(false);
                break;
            case 1:
                p1.SetActive(false);
                p2.SetActive(true);
                break;
        }
    }
}
