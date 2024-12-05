using System.Collections;
using System.Collections.Generic;
using RouletteSystem;
using UnityEngine;

public class Test : MonoBehaviour
{
    private RouletteController _controller;

    private void Start()
    {
        _controller = new RouletteController();

        _controller.ShowRouletteView();
    }
}
