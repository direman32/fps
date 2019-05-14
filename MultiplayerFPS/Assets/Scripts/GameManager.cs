using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public MatchSettings MatchSettings;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one game manager in the scene");
        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer (string _netID, Player _player)
    {
        //Registers player with netID so the server knows which player is which
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static Player getPlayer(string _playerID)
    {
        return players[_playerID];
    }

    //    private void OnGUI()
    //   {
    //        GUILayout.BeginArea(new Rect(200,200,200,500));
    //        GUILayout.BeginVertical();
    //
    //        foreach (string _playerID in players.Keys)
    //        {
    //            GUILayout.Label(_playerID + "  -  " + players[_playerID].transform.name);
    //        }
    //
    //        GUILayout.EndVertical();
    //        GUILayout.EndArea();
    //    }
    #endregion
}
