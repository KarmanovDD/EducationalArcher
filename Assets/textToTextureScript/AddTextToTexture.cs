using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddTextToTexture: MonoBehaviour
{
    public Font customFont;
    public int fontCountX;
    public int fontCountY;
    public string text;
    public int textPlacementY;
    public PerCharacterKerning[] perCharacterKerning; 
    public float lineSpacing = 1;
    public bool useSharedMaterial = true;
    public int decalTextureSize = 1024;
    public float characterSize = 1; 
    public string textureID = "_DecalTex"; 

    private const string TEXT_TEXTURE_ID = "_DecalTex";
    private Material mat;

    private bool hasTextTextureBeenSet = false;
    private int maxMugTextWidth = 1024;

    public void Start()
    {
        maxMugTextWidth = decalTextureSize;
    }

    public void Update()
    {
        if (!hasTextTextureBeenSet)
        {
            CreateAndAddTextToTexture();
        }
    }

    public void CreateAndAddTextToTexture()
    {
        if (useSharedMaterial)
        {
            mat = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else
        {
            mat = gameObject.GetComponent<MeshRenderer>().material;
        }
        //wrong textToTexture with 3-digits numbers, perhaps because of stupid using of perCharacterKerning
        TextToTexture textToTexture = new TextToTexture(customFont, fontCountX, fontCountY, DefaultCharacterKerning(),/*perCharacterKerning,*/ false);
        int textWidthPlusTrailingBuffer = textToTexture.CalcTextWidthPlusTrailingBuffer(text, decalTextureSize, characterSize);
       
        int posX = (decalTextureSize - (textWidthPlusTrailingBuffer + 1)) -  Mathf.Clamp(((maxMugTextWidth-textWidthPlusTrailingBuffer)/2),0,decalTextureSize);
        mat.SetTexture(/*TEXT_TEXTURE_ID*/textureID, textToTexture.CreateTextToTexture(text, posX, textPlacementY, decalTextureSize, characterSize, lineSpacing));
        hasTextTextureBeenSet = true;
        if (useSharedMaterial)
        {
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = mat;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material = mat;
        }
        

    }

    public void OnApplicationQuit()
    {
        if (hasTextTextureBeenSet)
        {
            //cleans up material so it appears correctly in editor after application has quit
            mat.SetTexture(TEXT_TEXTURE_ID, null);
        }
    }

    private float[] DefaultCharacterKerning()
    {
        double[] perCharKerningDouble = new double[] {
 .201 /* */
,.201 /*!*/
,.256 /*"*/
,.401 /*#*/
,.401 /*$*/
,.641 /*%*/
,.481 /*&*/
,.138 /*'*/
,.24 /*(*/
,.24 /*)*/
,.281 /***/
,.421 /*+*/
,.201 /*,*/
,.24 /*-*/
,.201 /*.*/
,.201 /*/*/
,.401 /*0*/
,.353 /*1*/
,.401 /*2*/
,.401 /*3*/
,.401 /*4*/
,.401 /*5*/
,.401 /*6*/
,.401 /*7*/
,.401 /*8*/
,.401 /*9*/
,.201 /*:*/
,.201 /*;*/
,.421 /*<*/
,.421 /*=*/
,.421 /*>*/
,.401 /*?*/
,.731 /*@*/
,.481 /*A*/
,.481 /*B*/
,.52  /*C*/
,.481 /*D*/
,.481 /*E*/
,.44  /*F*/
,.561 /*G*/
,.52  /*H*/
,.201 /*I*/
,.36  /*J*/
,.481 /*K*/
,.401 /*L*/
,.6   /*M*/
,.52  /*N*/
,.561 /*O*/
,.481 /*P*/
,.561 /*Q*/
,.52  /*R*/
,.481 /*S*/
,.44  /*T*/
,.52  /*U*/
,.481 /*V*/
,.68  /*W*/
,.481 /*X*/
,.481 /*Y*/
,.44  /*Z*/
,.201 /*[*/
,.201 /*\*/
,.201 /*]*/
,.338 /*^*/
,.401 /*_*/
,.24  /*`*/
,.401 /*a*/
,.401 /*b*/
,.36  /*c*/
,.401 /*d*/
,.401 /*e*/
,.189 /*f*/
,.401 /*g*/
,.401 /*h*/
,.16  /*i*/
,.16  /*j*/
,.36  /*k*/
,.16  /*l*/
,.6   /*m*/
,.401 /*n*/
,.401 /*o*/
,.401 /*p*/
,.401 /*q*/
,.24  /*r*/
,.36  /*s*/
,.201 /*t*/
,.401 /*u*/
,.36  /*v*/
,.52  /*w*/
,.36  /*x*/
,.36  /*y*/
,.36  /*z*/
,.241 /*{*/
,.188 /*|*/
,.241 /*}*/
,.421 /*~*/
};
        float[] perCharKerning = new float[perCharKerningDouble.Length];

        for (int x = 0; x < perCharKerning.Length; x++)
        {
            perCharKerning[x] = (float)perCharKerningDouble[x];
        }
        return perCharKerning;
    }
}