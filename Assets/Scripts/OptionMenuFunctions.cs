/* OptionMenuFunctions.cs
 * ------------------------------
 * This class holds public functions for use in the main menu, when the user selects an option
 * 
 * Author(s):
 *      - Jay Ganguli
 *      - 
 *      - 
 * 
 * Last Edited: 2021-02-18
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenuFunctions : MonoBehaviour
{
    public Canvas previousMenu;

    public void ExitOptionMenu()
    {
        gameObject.SetActive(false);
        previousMenu.gameObject.SetActive(true);
    }
}
