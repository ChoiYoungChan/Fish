using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class FrogView : BaseFish
{
    [SerializeField] Animator animator;
    private readonly string SWIM_PARAM_NAME = "IsSwimming";

    public override void Gotcha()
    {
        base.Gotcha();
        animator.SetBool(SWIM_PARAM_NAME, false);
    }

    public override void Release()
    {
        base.Release();
        animator.SetBool(SWIM_PARAM_NAME, true);
    }
}