﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

[ExecuteInEditMode]
public class MadSlider : MonoBehaviour, IResetable
{

    [SerializeField]
    RectTransform[] _points;
    [SerializeField]
    RectTransform _mover;

    [SerializeField]
    float _oneMoveTime = 0.3f;
    [SerializeField]
    float _widthDelta;

    [SerializeField, ReadOnly]
    int currentId;

    public FadingAudioSource MadSliderAudioSource;

    public void SetIsland(int id)
    {
        PlaySound();
        var tarPos = _points[id].position;

        print(" cur id: " + currentId);
        if (currentId == 0)
        {
            var firstPos = tarPos;
            firstPos.y = _mover.position.y;
            _mover.DOMove(firstPos, _oneMoveTime).OnComplete(() => _mover.DOMove(tarPos, _oneMoveTime).OnComplete(()=> StopSound()));
        }
        else
        {
            if (tarPos.x == _mover.position.x)
            {
                _mover.DOMove(tarPos, 1f);
            }
            else
            {
                var firstPos = _mover.position;
                firstPos.y = _points[0].position.y;
                var secPos = tarPos;
                secPos.y = _points[0].position.y;
                _mover.DOMove(firstPos, _oneMoveTime).
                    OnComplete(() => _mover.DOMove(secPos, _oneMoveTime).
                    OnComplete(() => _mover.DOMove(tarPos, _oneMoveTime).OnComplete(() => StopSound()) ));
            }
        }
        currentId = id;

        TableControlsManager.Instance.SetLever(id);

    }


    private void OnGUI()
    {
        foreach (var point in _points)
        {
            var pos = point.anchoredPosition;

            if (pos.x == 0f || pos.y == 0f)
                continue;

            pos.x /= Mathf.Abs(pos.x);
            pos.y /= Mathf.Abs(pos.y);

            pos *= _widthDelta / 2f;

            point.anchoredPosition = pos;
        }
    }

    public void Reset()
    {
        var tarPos = _points[0].position;
        if (currentId == 0)
        {
            var firstPos = tarPos;
            firstPos.y = _mover.position.y;
            _mover.DOMove(firstPos, _oneMoveTime).OnComplete(() => _mover.DOMove(tarPos, _oneMoveTime));
        }
        else
        {
            if (tarPos.x == _mover.position.x)
            {
                _mover.DOMove(tarPos, 1f);
            }
            else
            {
                var firstPos = _mover.position;
                firstPos.y = _points[0].position.y;
                var secPos = tarPos;
                secPos.y = _points[0].position.y;
                _mover.DOMove(firstPos, _oneMoveTime).
                    OnComplete(() => _mover.DOMove(secPos, _oneMoveTime).
                        OnComplete(() => _mover.DOMove(tarPos, _oneMoveTime)));
            }
        }
        
        currentId = 0;
    }

    public void PlaySound()
    {
        MadSliderAudioSource.Play(0.5f);
    }

    public void StopSound()
    {
        MadSliderAudioSource.StopByFade();
    }
}
