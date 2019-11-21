using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnClick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(loadlevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadlevel()
    {
        SceneManager.LoadScene("Map");
    }
}
