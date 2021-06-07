using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MimicParentTMPText : MonoBehaviour
{
    TMP_Text text;
    TMP_Text parentText;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        parentText = transform.parent.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = parentText.text;
    }
}
