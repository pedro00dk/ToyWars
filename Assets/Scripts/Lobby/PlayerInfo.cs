using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class PlayerInfo : MonoBehaviour {
    
    //User setable============================
    public Text NickNameInputField;


    //InternalUse=============================
    //Scenes Settings
    string NickName;
    GameObject PlayerCharacter;

    //Internal Settings
    private LobbyPlayer _lp;
    private PlayerSelector _ps;
    private string _defaultNickname;

	// Use this for initialization
	void Start () {
        NickName = "";

        _ps = FindObjectOfType<PlayerSelector>();

    }
	
	// Update is called once per frame
	void Update () {

        if(_lp == null)
        {
            _lp = FindObjectOfType<LobbyPlayer>();
            if(_lp)
                _defaultNickname = _lp.playerName;
        }

        verifyNickname();
        getPlayerCharacter();

    }

    public void verifyNickname()
    {
        if (NickNameInputField)
            NickName = NickNameInputField.text;
        
        //if is a valid nickname, the windows desappear
        if(NickName != "" && NickName != null && !NickName.Contains(" "))
        {
            NickNameInputField.color = Color.black;
            if(_lp != null)
            {
                _lp.OnMyName(NickName);
            }
        }
        else
        {
            NickNameInputField.color = Color.red;

            if (_lp != null)
                _lp.OnMyName(_defaultNickname);
        }
    }

    public void getPlayerCharacter()
    {
        PlayerCharacter = _ps.PlayerCharacter;
    }
}
