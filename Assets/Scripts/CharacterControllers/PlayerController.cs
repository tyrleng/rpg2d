using System;
using System.Collections;
using System.Collections.Generic;
using CharacterControllers;
using UnityEngine;

public class PlayerController : CharacterBaseController
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public bool Selected
    {
        get { return _animator.GetBool("Selected"); }
        set { _animator.SetBool("Selected", value); }
    }
    
    public bool Moving
    {
        get { return _animator.GetBool("Movement"); }
        set { _animator.SetBool("Movement", value); }
    }

    public float MovementX
    {
        get { return _animator.GetFloat("MoveX"); }
        set { _animator.SetFloat("MoveX", value); }
    }

    public float MovementY
    {
        get { return _animator.GetFloat("MoveY"); }
        set {_animator.SetFloat("MoveY", value);}
    }

    // Update is called once per frame
    void Update()
    {
    }
}
