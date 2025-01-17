﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    RectTransform rtBorder;
    RectTransform rtEmpty;
    RectTransform rtLost;
    RectTransform rtPoint;

    bool init = false;
    //RectTransform rtBorder

    Image imgBorder;
    Image imgEmpty;
    Image imgLost;
    Image imgPoint;
    void Init()
    {
        if (!init)
        {
            rtBorder = transform.Find("Border") as RectTransform;
            rtEmpty = transform.Find("Empty") as RectTransform;
            rtLost = transform.Find("Lost") as RectTransform;
            rtPoint = transform.Find("XPoint") as RectTransform;

            imgBorder = rtBorder.GetComponent<Image>();
            imgEmpty = rtEmpty.GetComponent<Image>();
            imgLost = rtLost.GetComponent<Image>();
            imgPoint = rtPoint.GetComponent<Image>();

            init = true;
        }
    }

    public void SetAlpha(float alpha)
    {
        Color c = new Color(1, 1, 1, alpha);
        imgBorder.color = c;
        imgEmpty.color = c;
        imgLost.color = c;
        imgPoint.color = c;
    }

    void Awake()
    {
        Init();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    [SerializeField]
    float delay = 0.5f;
    [SerializeField]
    float lostSpeed = 0.8f;

    Vector2 pointSize;

    bool inLost = false;
    float delayTimer = 0;
    // Update is called once per frame
    void Update()
    {
        //这种UI尺寸直接硬编码了,计算出point bar的最大尺寸
        Rect rectBorder = rtBorder.rect;
        pointSize.x = rectBorder.width - 12;
        pointSize.y = rectBorder.height - 7;

        if (inLost)
        { //带延迟的lost bar
            delayTimer -= Time.deltaTime;
            if (delayTimer < 0)
            {
                delayTimer = 0;
                if (lostRatio > pointRatio)
                {
                    lostRatio -= lostSpeed * Time.deltaTime;
                    if (lostRatio < pointRatio)
                    {
                        lostRatio = pointRatio;
                        inLost = false;
                    }
                    SetLostRatioView();
                }
            }
        }
    }

    public float pointRatio = 1.0f;
    void SetPointRatioView()
    {
        float toRight = pointSize.x * (1.0f - pointRatio) + 6;
        rtPoint.offsetMax = new Vector2(-toRight, -4);
    }

    float lostRatio = 1.0f;
    void SetLostRatioView()
    {
        float toRight = pointSize.x * (1.0f - lostRatio) + 6;
        rtLost.offsetMax = new Vector2(-toRight, -4);
    }

    public void SetRatio(float ratio)
    {
        if (ratio > 1.0f)
            ratio = 1.0f;
        else if (ratio < 0)
            ratio = 0;

        
        if (pointRatio > lostRatio)
        {
            pointRatio = ratio;
            SetPointRatioView();
            lostRatio = pointRatio;
            SetLostRatioView();
        }
        else
        {
            if (!CommonHelper.FloatEqual(pointRatio, ratio))
            {
                delayTimer = delay;
                inLost = true;
            }
            pointRatio = ratio;
            SetPointRatioView();
        }
    }

}
