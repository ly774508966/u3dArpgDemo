﻿using UnityEngine;
using System.Collections;

[AddComponentMenu("FC/Logic/FCObject/PlayAndEnemy/Agent/AI/State/MeleeNormal/Born2")]
public class FCStateBorn2_meleeNormal : FCStateAgent
{

    public override void Run()
    {
        StartCoroutine(STATE());
    }

    void Awake()
    {
        _currentStateID = AIAgent.STATE.BORN;
    }

    IEnumerator STATE()
    {
        _inState = true;
        float timeCount = _stateOwner._timeGodForBorn;

        if (_stateOwner.ACOwner.IsPlayer)
        {
            PlayAnimation();
        }

        //_stateOwner.ACOwner._avatarController.RimFlashColor(true);
        _stateOwner.SetActionSwitchFlag(FC_ACTION_SWITCH_FLAG.CANT_MOVE);
        _stateOwner.SetActionSwitchFlag(FC_ACTION_SWITCH_FLAG.CANT_ROTATE);
        _stateOwner.SetActionSwitchFlag(FC_ACTION_SWITCH_FLAG.IN_SUPER_SAIYAJIN);
        _stateOwner.SetActionSwitchFlag(FC_ACTION_SWITCH_FLAG.IN_RIGIDBODY2);

        _stateOwner.BornTaskChange(FCCommand.CMD.STATE_ENTER);

        StateIn();

        while (_inState)
        {
            if (timeCount > 0)
            {
                timeCount -= Time.deltaTime;
                if (timeCount <= 0)
                {
                    if (!_stateOwner.ACOwner.IsPlayer && _stateOwner.TargetAC != null)
                    {
                        _stateOwner.FaceToTarget(_stateOwner.TargetAC, true);
                    }

                    _stateOwner.ClearActionSwitchFlag(FC_ACTION_SWITCH_FLAG.CANT_MOVE);
                    _stateOwner.ClearActionSwitchFlag(FC_ACTION_SWITCH_FLAG.CANT_ROTATE);
                    _stateOwner.ClearActionSwitchFlag(FC_ACTION_SWITCH_FLAG.IN_SUPER_SAIYAJIN);
                    _stateOwner.ClearActionSwitchFlag(FC_ACTION_SWITCH_FLAG.IN_RIGIDBODY2); 
                    //_stateOwner.ACOwner._avatarController.RimFlashColor(false);
                }
            }
            _stateOwner.BornTaskChange(FCCommand.CMD.STATE_UPDATE);
            StateProcess();
            yield return null;
        }
        //_stateOwner.ACOwner._avatarController.RimFlashColor(false);
        StateOut();
        _stateOwner.BornTaskChange(FCCommand.CMD.STATE_QUIT);
        if (!_stateOwner.ACOwner.IsPlayer)
        {
            GlobalEffectManager.Instance.PlayEffect(_stateOwner._effectForBorn, _stateOwner.ACOwner.ThisTransform.localPosition);
        }
        _stateOwner.ChangeState();

    }
}
