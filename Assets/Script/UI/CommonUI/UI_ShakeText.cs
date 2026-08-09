using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ShakeText : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;
    private TMP_TextInfo _textInfo;

    public bool bool_Loop;
    public float dloat_Distance;
    public float float_ShakeDuration = 0.5f;
    public int int_ShakeTime = 6;

    private Sequence sequence_Loop;
    private int int_LastTextCount;
    public void Start()
    {
        if (textMeshProUGUI == null)
        {
            TryGetComponent(out textMeshProUGUI);
        }
        DoShake();
    }
    public void DoShake(int seed = 0)
    {
        textMeshProUGUI.ForceMeshUpdate();
        _textInfo = textMeshProUGUI.textInfo;

        // 如果当前没有文本或没有字符，直接返回
        if (_textInfo == null || _textInfo.characterCount == 0)
        {
            if (bool_Loop)
            {
                // 延迟后重试
                DOVirtual.DelayedCall(0.1f, () => DoShake(seed + 1));
            }
            return;
        }


        sequence_Loop = DOTween.Sequence();
        if (seed == 0)
        {
            sequence_Loop.AppendInterval(0f);
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
            int currentIndex = i;
            var pos = Vector3.zero;

            textMeshProUGUI.ForceMeshUpdate();
            var materialIndex = characterInfo.materialReferenceIndex;
            var meshInfo = _textInfo.meshInfo[materialIndex];
            var vertexIndex = characterInfo.vertexIndex;

            var color = meshInfo.colors32[vertexIndex];
            color.a = 0;

            var oriPos = GetOriPos(meshInfo, vertexIndex);

            // 重要：保存当前字符的网格信息和索引，用于在OnUpdate中验证
            var capturedMeshInfo = meshInfo;
            var capturedVertexIndex = vertexIndex;
            var capturedOriPos = oriPos;

            var sequence_Shake = DOTween.Sequence();
            UnityEngine.Random.InitState(i + seed);
            Vector3 endPos = UnityEngine.Random.insideUnitCircle * dloat_Distance;
            sequence_Shake.Insert(0,
                DOTween.To(() => pos, x => pos = x, endPos, float_ShakeDuration).SetEase(Ease.InOutSine).SetLoops((int)(int_ShakeTime * 0.5f), LoopType.Yoyo));
            sequence_Shake.OnUpdate(() =>
            {
                // 验证：当前索引是否还在有效范围内
                if (currentIndex >= textMeshProUGUI.textInfo.characterCount)
                    return;

                var currentCharInfo = textMeshProUGUI.textInfo.characterInfo[currentIndex];
                if (!currentCharInfo.isVisible)
                    return;

                // 重新获取当前的网格信息（防止文本变化）
                var currentMeshInfo = textMeshProUGUI.textInfo.meshInfo[currentCharInfo.materialReferenceIndex];
                var currentVertexIndex = currentCharInfo.vertexIndex;

                // 重新获取原始位置（如果文本内容变了，位置也会变）
                var currentOriPos = GetOriPos(currentMeshInfo, currentVertexIndex);

                SetVertexPosition(meshInfo, vertexIndex, pos, oriPos); 
            });

        }

        sequence_Loop.OnComplete(() =>
        {
            if (bool_Loop)
            {
                DoShake(seed + 1);
            }
        });
    }
    public void ReShake()
    {
        if (sequence_Loop != null && sequence_Loop.IsActive())
        {
            sequence_Loop.Kill();
            sequence_Loop = null;
        }
        DOTween.Kill(textMeshProUGUI); 
        ResetTextVertices();
        DoShake();
    }
    private void SetVertexPosition(TMP_MeshInfo meshInfo, int vertexIndex, Vector3 pos, IReadOnlyList<Vector3> oriPos)
    {
        for (int j = 0; j < 4; j++)
        {
            meshInfo.vertices[vertexIndex + j] = oriPos[j] + pos;
        }

        textMeshProUGUI.UpdateVertexData();
    }
    private void ResetTextVertices()
    {
        // 强制刷新并重置所有顶点位置
        textMeshProUGUI.ForceMeshUpdate();
        var textInfo = textMeshProUGUI.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            var meshInfo = textInfo.meshInfo[charInfo.materialReferenceIndex];
            var vertexIndex = charInfo.vertexIndex;
            var oriPos = GetOriPos(meshInfo, vertexIndex);

            // 重置到原始位置
            for (int j = 0; j < 4; j++)
            {
                meshInfo.vertices[vertexIndex + j] = oriPos[j];
            }
        }
        textMeshProUGUI.UpdateVertexData();
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
