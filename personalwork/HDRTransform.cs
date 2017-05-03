using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class HDRTransformWindow : EditorWindow 
{
    [MenuItem("JwaooTools/转换图片")]
    private static void newWindow()
    {
        HDRTransformWindow window = EditorWindow.CreateInstance(typeof(HDRTransformWindow)) as HDRTransformWindow;
        window.minSize            = new Vector2(200, 450);
        window.maxSize            = window.minSize;
        window.position           = new Rect(400, 300, 0, 0);
        window.Show();
    }

    /// <summary>
    /// 以下是右手坐标系，和unity坐标系是有区别的
    /// </summary>
    private Texture2D _pos_x;
    private Texture2D _pos_y;
    private Texture2D _neg_y;
    private Texture2D _neg_x;
    private Texture2D _pos_z;
    private Texture2D _neg_z;



    private System.Type typ;
    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("右手坐标系,映射到unity的坐标系");
        _pos_x = EditorGUILayout.ObjectField("+Z front", _pos_x, typeof(Texture2D)) as Texture2D;
        _pos_y = EditorGUILayout.ObjectField("-X right", _pos_y, typeof(Texture2D)) as Texture2D;
        _neg_y = EditorGUILayout.ObjectField("+X left", _neg_y, typeof(Texture2D)) as Texture2D;
        _neg_x = EditorGUILayout.ObjectField("-Z back", _neg_x, typeof(Texture2D)) as Texture2D;
        _pos_z = EditorGUILayout.ObjectField("+Y up", _pos_z, typeof(Texture2D)) as Texture2D;
        _neg_z = EditorGUILayout.ObjectField("-Y down", _neg_z, typeof(Texture2D)) as Texture2D;
        


        GUILayout.EndVertical();

        if(GUILayout.Button("convert"))
        {
            transfomTex();
        }
    }

    private StreamWriter fs = null;

    private const int OUTPUT_SIZE = 4096;

    private const string LOG_FILE_PATH = "D://log.txt";
    private void transfomTex()
    {
        File.Delete(LOG_FILE_PATH);
        fs = File.CreateText(LOG_FILE_PATH);

        Texture2D outPut = new Texture2D(OUTPUT_SIZE, OUTPUT_SIZE);

        for(int i = 0; i < OUTPUT_SIZE; ++ i)
        {
            for(int j = 0; j < OUTPUT_SIZE; ++j)
            {
                Color col = sample(i, j);
                outPut.SetPixel(i, j, col);
            }
        }
        outPut.Apply();
        byte[] bytes = outPut.EncodeToJPG();
        File.WriteAllBytes(Application.dataPath + "/JwaooVR/Hdr/hdr.jpg", bytes);
        fs.Close();
        System.Diagnostics.Process.Start("NOTEPAD", LOG_FILE_PATH);
    }

    private Color sample(int i, int j)
    {
        float theta = (float)i / OUTPUT_SIZE * 360f;//[0,360]
        float fi    = (float)j / OUTPUT_SIZE * 180f - 90f;//[-90,90]
        if(fi == 90)
            return _pos_z.GetPixel(_pos_z.width / 2, _pos_z.height / 2);
        else if(fi == -90)
            return _neg_z.GetPixel(_neg_z.width / 2, _neg_z.height / 2);

        if(theta >= 315f || theta < 45f)
        {
            //
            return samplePosX(theta, fi);
        }
        if(theta >= 45f && theta < 135f)
        { 
            return samplePosY(theta, fi);
        }
        if(theta >= 135 && theta < 225f)
        { 
            return sampleNegX(theta, fi);
        }
        if(theta >=225 && theta < 315)
        {
            return sampleNegY(theta, fi);
        }
        return Color.black;
    }

    private Color samplePosX(float theta, float fi)
    {
        float tangentTheta = Mathf.Tan(Mathf.Deg2Rad * theta);

        float factor = Mathf.Sqrt(1 + tangentTheta * tangentTheta) * Mathf.Tan(Mathf.Deg2Rad * fi); 
        float z = factor;
        if (z >= -1 && z < 1)
        {   
            float u = 0f;
            float v = (z + 1f) / 2f;
            if (theta <= 45f || theta > 315f)//front
            {
                if (theta <= 45)
                    u = .5f * (45f - theta) / 45f;//[0, 45]->[.5f, 0]
                else if (theta > 315f)
                    u = 1f - (theta - 315f) / 45f * .5f;
                //fs.WriteLine(string.Format("theta:{0, -10} fi:{1,-10} u:{2,-10} v:{3,-10}", theta, fi, u, v, z));
            }
            return _pos_x.GetPixel((int)(u * _pos_x.width), (int)(v * _pos_x.height));
        }
        else if(z >= 1)
            return sampleUp(theta, fi);
        else// if(z < -1)
            return sampleDown(theta, fi);
    }

    private Color samplePosY(float theta, float fi)
    {
        float sinTheta = Mathf.Sin(theta * Mathf.Deg2Rad);
        float z = Mathf.Sqrt(1 / (sinTheta * sinTheta)) * Mathf.Tan(Mathf.Deg2Rad * fi);
        if (z >= -1 && z < 1)
        {
            float u = -(theta - 135f) / 90f;//(135, 45) - > (0, 1)
            float v = (z + 1) / 2f;//(-1, 1) -> (0, 1)
            return _pos_y.GetPixel((int)(_pos_y.width * u), (int)(_pos_y.height * v));
        }
        else if(z >= 1)
            return sampleUp(theta, fi);
        else //z <= -1
            return sampleDown(theta, fi);
    }

    private Color sampleNegX(float theta, float fi)
    {
        float tangentTheta = Mathf.Tan(Mathf.Deg2Rad * theta);

        float factor = Mathf.Sqrt(1 + tangentTheta * tangentTheta) * Mathf.Tan(Mathf.Deg2Rad * fi);
        float z = factor;
        if (z >= -1 && z < 1)
        {
            float u = - (theta - 225f) / 90f;
            float v = (z + 1f) / 2f;
            return _neg_x.GetPixel((int)(u * _neg_x.width), (int)(v * _neg_x.height));
        }
        else if (z > 1)
        {
            return sampleUp(theta, fi);
        }
        else// if(z < -1)
            return sampleDown(theta, fi);
    }

    private Color sampleNegY(float theta, float fi)
    {
        float sinTheta = Mathf.Sin(theta * Mathf.Deg2Rad);
        float z = Mathf.Sqrt(1 / (sinTheta * sinTheta)) * Mathf.Tan(Mathf.Deg2Rad * fi);
        if (z >= -1 && z < 1)
        {
            float u = -(theta - 315f) / 90f;//
            float v = (z + 1) / 2f;//
            return _neg_y.GetPixel((int)(_neg_y.width * u), (int)(_neg_y.height * v));
        }
        else if (z >= 1)
        {
            return sampleUp(theta, fi);
        }
        else //z <= -1
            return sampleDown(theta, fi);
    }

    private Color sampleUp(float theta, float fi)
    {
        float tanTheta = Mathf.Tan(Mathf.Deg2Rad * theta);
        float tanFi = Mathf.Tan(Mathf.Deg2Rad * fi);
        if(tanFi < 0)
            Debug.LogError("...");
        float factor = Mathf.Sqrt(1 + tanTheta * tanTheta) * tanFi; 
        //xiaoyu 180的是没问题的
        float x;
        float y;
        if(theta == 90f)
            x = 0f;
        else if (theta > 90f && theta <270f) x = - 1f / factor;
        else x = 1f / factor;
        y = x * tanTheta;
        

        float u = -.5f * (y - 1);
        float v = -.5f * (x - 1);
        //fs.WriteLine("theta:{0, -15} fi:{1, -15} x:{2, -15} y:{3, -15} u:{4, -15} v:{5, -15}", theta, fi, x, y, u, v);

        return _pos_z.GetPixel((int)(_pos_z.width * u), (int)(_pos_z.height * v));
    }

    private Color sampleDown(float theta, float fi)
    {
        float tangentTheta = Mathf.Tan(Mathf.Deg2Rad * theta);

        float factor = Mathf.Sqrt(1 + tangentTheta * tangentTheta) * Mathf.Tan(Mathf.Deg2Rad * fi);
        float x;
        float y;
        if (theta >= 90f && theta < 270f) x = 1f / factor;
        else x = -1f / factor;
        y = x * tangentTheta;

        float u = -.5f * ( y - 1);
        float v =.5f * (x + 1);
        //fs.WriteLine(string.Format("d  x:{0, -10}, y:{1, -10}, u:{2, -10} v:{3, -10}", x, y, u, v));
        return _neg_z.GetPixel((int)(_neg_z.width * u), (int)(_neg_z.height * v));
    }

}
