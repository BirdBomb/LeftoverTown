using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShakeText : MonoBehaviour
{
    private TextMeshProUGUI _textTMP;
    private TMP_TextInfo _textInfo;

    public bool bool_Loop;
    public float dloat_Distance;
    public float float_ShakeDuration = 0.5f;
    public int int_ShakeTime = 6;

    private Sequence sequence_Loop;
    public void Start()
    {
        if (_textTMP == null)
        {
            TryGetComponent(out _textTMP);
        }
        DoShake();
    }
    public void DoShake(int seed = 0)
    {
        _textInfo = _textTMP.textInfo;

        sequence_Loop = DOTween.Sequence();
        if (seed == 0)
        {
            sequence_Loop.AppendInterval(0.2f);
        }
        else
        {
            sequence_Loop.AppendInterval(int_ShakeTime * float_ShakeDuration * 0.5f);
        }


        var count = Mathf.Min(_textInfo.characterCount, _textInfo.characterInfo.Length);
        for (int i = 0; i < count; i++)
        {
            var characterInfo = _textInfo.characterInfo[i];
            if (!characterInfo.isVisible)
            {
                continue;
            }
            var pos = Vector3.zero;

            _textTMP.ForceMeshUpdate();
            var materialIndex = characterInfo.materialReferenceIndex;
            var meshInfo = _textInfo.meshInfo[materialIndex];
            var vertexIndex = characterInfo.vertexIndex;

            var color = meshInfo.colors32[vertexIndex];
            color.a = 0;

            var oriPos = GetOriPos(meshInfo, vertexIndex);


            var sequence_Shake = DOTween.Sequence();
            UnityEngine.Random.InitState(i + seed);
            Vector3 endPos = UnityEngine.Random.insideUnitCircle * dloat_Distance;
            sequence_Shake.Insert(0,
                DOTween.To(() => pos, x => pos = x, endPos, float_ShakeDuration).SetEase(Ease.InOutSine).SetLoops((int)(int_ShakeTime * 0.5f), LoopType.Yoyo));
            sequence_Shake.OnUpdate(() =>
            { SetVertexPosition(meshInfo, vertexIndex, pos, oriPos); });

        }

        sequence_Loop.OnComplete(() =>
        {
            if (bool_Loop)
            {
                DoShake(seed + 1);
            }
        });
    }
    private void SetVertexPosition(TMP_MeshInfo meshInfo, int vertexIndex, Vector3 pos, IReadOnlyList<Vector3> oriPos)
    {
        for (int j = 0; j < 4; j++)
        {
            meshInfo.vertices[vertexIndex + j] = oriPos[j] + pos;
        }

        _textTMP.UpdateVertexData();
    }
    private Vector3[] GetOriPos(TMP_MeshInfo meshInfo, int vertexIndex)
    {
        var pos = new Vector3[4];
        for (int j = 0; j < 4; j++)
        {
            pos[j] = meshInfo.vertices[vertexIndex + j];
        }

        return pos;
    }
}
