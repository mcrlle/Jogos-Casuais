using UnityEngine;

/// Terreno de rua: so DESCIDA e RETA, nunca sobe.
/// Trechos sorteados (reta ou descida com inclinacao variada), emendados com curva suave.
/// A altura e a inclinacao ficam numa tabela pre-calculada, entao consultar e barato
/// e a inclinacao sob o player e sempre exata.
public static class CatTerrain
{
    // area coberta pela tabela (o gato comeca em x = 0)
    const float Start = -60f, End = 4200f, Res = 0.1f;

    // tuning do terreno (teste)
    const int Seed = 7;
    const float FlatMin = 15f, FlatMax = 40f;        // comprimento das retas
    const float DownMin = 25f, DownMax = 70f;        // comprimento das descidas
    const float SlopeMin = 0.12f, SlopeMax = 0.45f;  // inclinacao da descida (~7 a ~24 graus)
    const float Blend = 8f;                          // comprimento da curva de transicao

    static float[] hTab, sTab;

    static void Ensure()
    {
        if (hTab != null) return;
        int n = Mathf.CeilToInt((End - Start) / Res) + 1;
        hTab = new float[n];
        sTab = new float[n];

        var rng = new System.Random(Seed);
        float Rand(float a, float b) => a + (float)rng.NextDouble() * (b - a);

        // comeca com uma reta, depois alterna; as vezes duas descidas seguidas
        float segStart = Start, segEnd = 30f, prevSlope = 0f, segSlope = 0f;
        bool lastWasFlat = true;

        for (int i = 0; i < n; i++)
        {
            float x = Start + i * Res;
            while (x >= segEnd)
            {
                prevSlope = segSlope;
                bool flat = !lastWasFlat && rng.NextDouble() < 0.6;
                segStart = segEnd;
                segEnd = segStart + (flat ? Rand(FlatMin, FlatMax) : Rand(DownMin, DownMax));
                segSlope = flat ? 0f : -Rand(SlopeMin, SlopeMax);
                lastWasFlat = flat;
            }
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((x - segStart) / Blend));
            sTab[i] = Mathf.Lerp(prevSlope, segSlope, t);
            hTab[i] = i == 0 ? 0f : hTab[i - 1] + (sTab[i - 1] + sTab[i]) * 0.5f * Res;
        }
    }

    static float Sample(float[] tab, float x)
    {
        float f = Mathf.Clamp((x - Start) / Res, 0f, tab.Length - 1.001f);
        int i = (int)f;
        return Mathf.Lerp(tab[i], tab[i + 1], f - i);
    }

    // Ensure() ANTES de ler hTab/sTab: senao a tabela ainda e null quando e passada
    public static float Height(float x) { Ensure(); return Sample(hTab, x); }

    /// dy/dx: negativo = descendo (indo pra +x); nunca positivo
    public static float Slope(float x) { Ensure(); return Sample(sTab, x); }

    public static float AngleDeg(float x) => Mathf.Atan(Slope(x)) * Mathf.Rad2Deg;

    /// Faixa que acompanha a ladeira: em cada x vai de Height(x)+bottom ate Height(x)+top.
    /// A textura se repete a cada `tile` unidades MEDIDAS AO LONGO DA LADEIRA (comprimento
    /// de arco), entao numa descida ela nao estica: so se repete mais vezes.
    /// Na vertical, repete a cada `tileV` unidades (se nao passar, usa `tile`), contando a
    /// partir de `vOrigin` (passe o `bottom` pra textura comecar na base da faixa).
    public static Mesh BuildBand(float from, float to, float step, float top, float bottom, float tile,
                                 float tileV = -1f, float vOrigin = 0f)
    {
        if (tileV <= 0f) tileV = tile;
        int n = Mathf.CeilToInt((to - from) / step) + 1;
        var v = new Vector3[n * 2];
        var uv = new Vector2[n * 2];
        var t = new int[(n - 1) * 6];
        float arc = 0f, prevX = from, prevY = Height(from);
        for (int i = 0; i < n; i++)
        {
            float x = from + i * step;
            float h = Height(x);
            if (i > 0) arc += Mathf.Sqrt((x - prevX) * (x - prevX) + (h - prevY) * (h - prevY));
            prevX = x; prevY = h;

            float u = arc / tile;
            v[i * 2]      = new Vector3(x, h + top, 0f);
            v[i * 2 + 1]  = new Vector3(x, h + bottom, 0f);
            uv[i * 2]     = new Vector2(u, (top - vOrigin) / tileV);
            uv[i * 2 + 1] = new Vector2(u, (bottom - vOrigin) / tileV);
        }
        for (int i = 0; i < n - 1; i++)
        {
            int o = i * 6, b = i * 2;
            t[o]=b; t[o+1]=b+2; t[o+2]=b+1;
            t[o+3]=b+1; t[o+4]=b+2; t[o+5]=b+3;
        }
        var m = new Mesh { name = "CatTerrainBand" };
        m.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        m.vertices = v; m.uv = uv; m.triangles = t; m.RecalculateNormals(); m.RecalculateBounds();
        return m;
    }
}
