﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUID : MonoBehaviour
{
    public TextMeshProUGUI display;
    public String guid;
    public void Generate()
    {
        guid = System.Guid.NewGuid().ToString();
        display.text = guid;
        Application.OpenURL("https://docs.google.com/forms/d/e/1FAIpQLSeiNsqd3oWmlIzIBRKKtTuBBtD8sC5ZBHkWa7PCHF21_DSbJg/viewform?usp=pp_url&entry.670940261=" + guid);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}