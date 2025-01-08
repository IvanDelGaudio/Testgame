using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line_Compass : MonoBehaviour
{
    #region Public Variables
    public GameObject cam;
    public GameObject cube;
    #endregion
    #region Private Variables
    #endregion
    #region Lifecycle
    private void Update()
    {
        float distace = (cam.transform.position.x - cube.transform.position.x)+(cam.transform.position.z - cube.transform.position.z);
        Debug.Log(distace);
    }
    #endregion
    #region Public Methods
    #endregion
    #region Private Methods
    #endregion
}
