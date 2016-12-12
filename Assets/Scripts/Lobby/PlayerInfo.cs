using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class PlayerInfo : MonoBehaviour {
    string NickName;

    public Text NickNameInputField;

    LobbyPlayer lp;

	// Use this for initialization
	void Start () {
        NickName = "";
	}
	
	// Update is called once per frame
	void Update () {

        if(lp == null)
            lp = FindObjectOfType<LobbyPlayer>();

        verifyNickname();

    }

    public void verifyNickname()
    {
        if (NickNameInputField)
            NickName = NickNameInputField.text;
        
        //if is a valid nickname, the windows desappear
        if(lp != null && NickName != "" && !NickName.Contains(" "))
        {
            lp.OnMyName(NickName);
        }
    }
}
