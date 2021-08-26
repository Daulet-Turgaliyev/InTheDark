using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Effector : MonoBehaviour, IPunObservable
{

    public SpriteRenderer Avatar;
    public PhotonView pv;
    public GameObject[] BTN_Effector;

    private int _colorCount;

    /*
     *  0 - Damage
     *  1 - Swamp
     *  2 - Heal
     *  3 - Refuel
     *  4 - Fuel
     *  5 - SpeedBoost
     */

    private void Start()
    {
        Paint(-1);
    }

    #region Effects

    public void SetActive(int numEff, bool isActive)
    {
        if (pv.IsMine)
        {
            BTN_Effector[numEff].SetActive(isActive);
            if (isActive) { Paint(numEff); }
            else          { Paint(-1); }
        }
        else
        {
            Paint(_colorCount);
        }

    }


    private void Paint(int num)
    {
        _colorCount = num;
        switch (num)
        {
            case -1:
                Avatar.color = Color.white;
                break;
            case 0:
                Avatar.color = Color.red;
                break;
            case 1:
                break;
            case 2:
                Avatar.color = Color.green;
                break;
            case 3:
                Avatar.color = Color.yellow;
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            stream.SendNext(_colorCount);
        }
        else if (stream.IsReading)
        {
            Paint((int)stream.ReceiveNext());
        }

    }

    #endregion
}
