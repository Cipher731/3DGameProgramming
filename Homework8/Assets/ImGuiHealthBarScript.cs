using UnityEngine;

public class ImGuiHealthBarScript : MonoBehaviour
{
    public int CurentHealth;
    
    private readonly Vector3 _offset = new Vector3(-50, -50, 0);
    private Texture2D _emptyTexture;
    private Texture2D _filledTexture;
    private GUIStyle _emptyStyle;
    private GUIStyle _filledStyle;

    private void Awake()
    {
        _emptyTexture = new Texture2D(1, 1);
        _filledTexture = new Texture2D(1, 1);

        _emptyTexture.SetPixel(0, 0, Color.grey);
        _filledTexture.SetPixel(0, 0, Color.green);
        _emptyTexture.wrapMode = TextureWrapMode.Repeat;
        _filledTexture.wrapMode = TextureWrapMode.Repeat;

        _emptyTexture.Apply();
        _filledTexture.Apply();

        var barStyle = new GUIStyle
        {
            border = new RectOffset(),
            margin = new RectOffset(),
            padding = new RectOffset(),
            normal = new GUIStyleState(),
            fixedHeight = 10,
            fixedWidth = 100
        };

        _emptyStyle = new GUIStyle(barStyle)
        {
            normal =
            {
                background = _emptyTexture
            }
        };

        _filledStyle = new GUIStyle(barStyle)
        {
            normal =
            {
                background = _filledTexture
            }
        };
    }

    private void OnGUI()
    {
        _filledStyle.fixedWidth = CurentHealth;

        var barPosition = Camera.main.WorldToScreenPoint(transform.position);
        var screenHeight = Screen.height;

        GUI.Box(new Rect(barPosition.x + _offset.x,
            screenHeight - barPosition.y + barPosition.z + _offset.y,
            100, 10), "", _emptyStyle);

        if (CurentHealth == 0)
        {
            return;
        } 
        
        GUI.Box(new Rect(barPosition.x + _offset.x,
            screenHeight - barPosition.y + barPosition.z + _offset.y,
            100, 10), "", _filledStyle);
    }
}