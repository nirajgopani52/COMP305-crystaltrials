/* MainMenuFunctions.cs
 * ------------------------------
 * This class holds public functions for use in the main menu, when the user selects an option
 * 
 * Author(s):
 *      - Jay Ganguli
 *      - 
 *      - 
 * 
 * Last Edited: 2021-02-17
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuFunctions : MonoBehaviour
{
    public Canvas optionMenu;

    public void NewGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    public void OpenOptionsMenu()
    {
        gameObject.SetActive(false);
        optionMenu.gameObject.SetActive(true);
    }
}
