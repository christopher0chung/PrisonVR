using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BookTextureManager : MonoBehaviour {

    private Texture[] myPages = new Texture[0];
    private Texture[] activePages = new Texture[2];

    [SerializeField] private Material leftPageMat;
    [SerializeField] private Material rightPageMat;

    private int _leftPageNum;
    private int leftPageNum
    {
        get
        {
            return _leftPageNum;
        }
        set
        {
            if (value != _leftPageNum)
            {
                _leftPageNum = value;

                activePages[0] = myPages[_leftPageNum];
                activePages[1] = myPages[_leftPageNum + 1];

                leftPageMat.SetTexture(0, activePages[0]);
                rightPageMat.SetTexture(0, activePages[1]);
            }
        }
    }

	void Start () {
        myPages = Resources.LoadAll<Texture>("BookTextures");
        leftPageNum = 0;
	}
}
