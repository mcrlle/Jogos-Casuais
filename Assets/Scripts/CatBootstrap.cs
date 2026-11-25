using UnityEngine;

/// Monta a cena inteira em runtime. Nao precisa arrastar nada no editor:
/// abre qualquer cena e aperta Play.
public static class CatBootstrap
{
    // tamanho (em unidades do mundo) de UM pedaco da textura antes de ela se repetir
    const float SidewalkTile = 2f;   // distancia entre as juntas das placas da calcada
    const float StreetTile = 6f;     // um pedaco da rua = 3 faixas amarelas
    const float StreetHeight = 3.5f; // altura da faixa da rua
    const float SidewalkFar = 1.6f;  // quanto da calcada aparece ATRAS do gato, ate o meio-fio
    const float CurbHeight = 0.3f;   // espessura do meio-fio

    /// Textura de teste da calcada: concreto claro com uma junta escura na borda
    /// (a divisa entre uma placa e outra). Repetida, vira uma fileira de placas.
    static Texture2D Slabs(int px = 64)
    {
        var tex = new Texture2D(px, px, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Repeat,
            filterMode = FilterMode.Bilinear,
        };
        var concrete = new Color(0.80f, 0.78f, 0.74f);
        var joint = new Color(0.58f, 0.56f, 0.53f);
        for (int y = 0; y < px; y++)
        for (int x = 0; x < px; x++)
            tex.SetPixel(x, y, x < 2 ? joint : concrete);
        tex.Apply();
        return tex;
    }

    /// Textura de teste da rua: asfalto com 3 retangulos amarelos no meio (linha
    /// tracejada). Emenda nela mesma, entao repetida vira uma faixa continua.
    /// Na vertical cobre a rua inteira uma vez so (sem repetir).
    static Texture2D LaneLines(int w = 192, int h = 64)
    {
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Repeat,
            filterMode = FilterMode.Bilinear,
        };
        var asphalt = new Color(0.25f, 0.26f, 0.30f);
        var yellow = new Color(0.98f, 0.80f, 0.15f);
        int seg = w / 3;                 // cada traco + espaco
        int dash = seg * 5 / 8;          // comprimento do traco
        int y0 = h / 2 - 3, y1 = h / 2 + 3; // espessura do traco
        for (int y = 0; y < h; y++)
        for (int x = 0; x < w; x++)
        {
            int xs = x % seg;
            bool line = y >= y0 && y < y1 && xs >= (seg - dash) / 2 && xs < (seg + dash) / 2;
            tex.SetPixel(x, y, line ? yellow : asphalt);
        }
        tex.Apply();
        return tex;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Boot()
    {
        var sh = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");

        Material Mat(Color c, Texture2D tex = null)
        {
            var m = new Material(sh);
            if (m.HasProperty("_BaseColor")) m.SetColor("_BaseColor", c);
            if (m.HasProperty("_Color")) m.SetColor("_Color", c);
            if (tex != null)
            {
                m.mainTexture = tex;
                if (m.HasProperty("_BaseMap")) m.SetTexture("_BaseMap", tex);
            }
            return m;
        }

        // TESTE DE TEXTURA: faixas geradas por codigo, seguindo a ladeira (so desce ou reta).
        // De tras pra frente: rua -> meio-fio -> calcada (onde o gato anda).
        // O gato anda na linha Height(x), que fica DENTRO da calcada, abaixo do meio-fio.
        // Quando a artista entregar as tiras de verdade, troca Slabs()/LaneLines() pelas dela.
        const float from = -40f, to = 4100f, step = 0.3f;

        // calcada: da beira do meio-fio ate bem embaixo da tela
        var sidewalk = new GameObject("Sidewalk");
        sidewalk.AddComponent<MeshFilter>().sharedMesh =
            CatTerrain.BuildBand(from, to, step, SidewalkFar, -60f, SidewalkTile);
        var swr = sidewalk.AddComponent<MeshRenderer>();
        swr.sharedMaterial = Mat(Color.white, Slabs());
        swr.sortingOrder = -1;

        // meio-fio: faixa fina entre a calcada e a rua
        var curb = new GameObject("Curb");
        curb.transform.position = new Vector3(0f, 0f, 0.5f);
        curb.AddComponent<MeshFilter>().sharedMesh =
            CatTerrain.BuildBand(from, to, step, SidewalkFar + CurbHeight, SidewalkFar, 1f);
        var cr = curb.AddComponent<MeshRenderer>();
        cr.sharedMaterial = Mat(new Color(0.90f, 0.89f, 0.86f));
        cr.sortingOrder = -2;

        // rua: atras do meio-fio, segue a mesma ladeira
        var street = new GameObject("Street");
        street.transform.position = new Vector3(0f, 0f, 1f);
        float streetBottom = SidewalkFar + CurbHeight;
        street.AddComponent<MeshFilter>().sharedMesh =
            CatTerrain.BuildBand(from, to, step, streetBottom + StreetHeight, streetBottom,
                                 StreetTile, StreetHeight, streetBottom);
        var str = street.AddComponent<MeshRenderer>();
        str.sharedMaterial = Mat(Color.white, LaneLines());
        str.sortingOrder = -3;

        // player
        var player = GameObject.CreatePrimitive(PrimitiveType.Quad);
        player.name = "Cat";
        Object.Destroy(player.GetComponent<Collider>());
        player.transform.localScale = new Vector3(2f, 2f, 1f);
        player.GetComponent<MeshRenderer>().sharedMaterial = Mat(new Color(0.98f, 0.72f, 0.35f));
        var runner = player.AddComponent<CatRunner>();

        // camera
        var cam = Camera.main;
        if (cam == null)
        {
            var go = new GameObject("Main Camera") { tag = "MainCamera" };
            cam = go.AddComponent<Camera>();
        }
        cam.orthographic = true;
        cam.orthographicSize = 11f;
        cam.backgroundColor = new Color(0.90f, 0.55f, 0.42f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        var follow = cam.gameObject.AddComponent<CatCamera>();
        follow.target = player.transform;
    }
}

public class CatCamera : MonoBehaviour
{
    public Transform target;
    [Tooltip("Quanto a camera olha a frente do gato. Maior = gato mais pra esquerda da tela.")]
    public float ahead = 10f;
    [Tooltip("Quanto a camera mira acima do CHAO. Maior = rua e calcada mais pra baixo na tela " +
             "(menos calcada aparecendo). Da pra ajustar no Inspector durante o Play.")]
    public float height = 5.5f;
    [Tooltip("Rapidez com que a camera acompanha a descida do chao. Maior = mais grudada.")]
    public float followY = 5f;

    bool snapNext;
    /// Pula direto pra posicao certa no proximo frame (usado quando o gato volta pro inicio).
    public void Snap() => snapNext = true;

    void LateUpdate()
    {
        if (target == null) return;
        var p = transform.position;
        float tx = target.position.x + ahead;
        // segue o CHAO embaixo do gato, nao o gato: no pulo a camera nao sobe junto
        float ty = CatTerrain.Height(target.position.x) + height;
        if (snapNext) { snapNext = false; transform.position = new Vector3(tx, ty, -20f); return; }
        transform.position = new Vector3(
            tx, // horizontal travado: o gato fica sempre no mesmo lugar da tela, em qualquer velocidade
            Mathf.Lerp(p.y, ty, Time.deltaTime * followY),
            -20f);
    }
}
