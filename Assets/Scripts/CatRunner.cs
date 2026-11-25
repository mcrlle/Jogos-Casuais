using UnityEngine;
using UnityEngine.InputSystem;

/// Prototipo do movimento: pular, (girar), aterrissar pelo angulo, terreno so desce ou reto.
///
/// VELOCIDADE (DESIGN.md): velocidade = BASE + EXTRA DA DESCIDA + EXTRA DO BOOST
/// - Base: o gato comeca nela e nunca fica abaixo (gato de patins nao comeca se arrastando).
/// - Extra da descida: cresce na ladeira e volta pra base AOS POUCOS (queda percentual:
///   rapida no comeco, suave no fim). Como a queda age o tempo todo no chao, cada ladeira
///   tem sua "velocidade de cruzeiro": mais inclinada = mais rapido.
/// - Extra do boost: aterrissar com rotacao completa da um boost BEM TEMPORARIO:
///   fica cheio um tempo e depois cai rapido ate zero.
public class CatRunner : MonoBehaviour
{
    [Header("Velocidade base")]
    public float baseSpeed = 16f;    // comeca nela e nunca fica abaixo
    public bool limitSpeed = true;   // desligado = sem teto de velocidade
    public float maxSpeed = 50f;     // teto (so vale se limitSpeed estiver ligado)

    [Header("Extra da descida (volta aos poucos)")]
    public float slopeGain = 28f;    // quanto a descida soma por segundo (x inclinacao)
    [Range(0.01f, 3f)]
    public float slopeReturn = 0.35f; // fracao do extra que some por segundo (maior = volta mais rapido)

    [Header("Extra do boost (bem temporario)")]
    public float landBoost = 6f;     // quanto o boost soma ao aterrissar com rotacao completa
    public float boostHold = 0.8f;   // segundos com o boost cheio
    public float boostFall = 1.0f;   // segundos pra cair de cheio ate zero

    [Header("Pulo (DESIGN: tocou, pulou; altura sempre a mesma, nao carrega)")]
    public float jumpForce = 12f;    // velocidade vertical do pulo (sempre igual)
    public float gravity = 24f;      // menor = mais tempo no ar

    [Header("Manobra e aterrissagem")]
    public bool enableFlip = false;     // BLOQUEADO por enquanto (pedido do Olavo): segurar nao gira
    public float flipSpeed = 200f;      // graus/s do backflip (positivo = anti-horario = backflip indo pra direita)
    public float flipHoldDelay = 0.15f; // segundos segurando antes do flip comecar: toque rapido = so pula
    public float landTolerance = 45f;   // leniency: erro de angulo perdoado na aterrissagem, em graus

    // DESIGN.md TBD "consequencia da aterrissagem ruim": fica trocavel aqui ate o Olavo decidir.
    public enum BadLanding { Restart, LoseExtra }
    [Tooltip("Restart = aterrissou torto, volta pro inicio. LoseExtra = perde todo o extra e volta pra base.")]
    public BadLanding badLanding = BadLanding.Restart;

    float x, y, vy, angle, angVel, heldTime, airRotation;
    float slopeExtra, boostExtra, boostTimer;
    bool grounded = true, wasHeld;
    int falls;

    public float Speed
    {
        get
        {
            float s = baseSpeed + slopeExtra + boostExtra;
            return limitSpeed ? Mathf.Min(s, maxSpeed) : s;
        }
    }
    public bool Grounded => grounded;
    public float LastLandDiff { get; private set; }

    static bool Pressed()
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed) return true;
        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) return true;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) return true;
        return false;
    }

    void Start() { y = CatTerrain.Height(x); angle = CatTerrain.AngleDeg(x); }

    void Update()
    {
        float dt = Time.deltaTime;
        bool held = Pressed();
        heldTime = held ? heldTime + dt : 0f;

        UpdateBoost(dt);

        if (grounded)
        {
            // extra da descida: ganha proporcional a inclinacao, perde uma porcentagem
            float downhill = Mathf.Max(0f, -CatTerrain.Slope(x));
            slopeExtra += downhill * slopeGain * dt;
            slopeExtra -= slopeExtra * slopeReturn * dt;
            slopeExtra = Mathf.Max(0f, slopeExtra);

            x += Speed * dt;
            y = CatTerrain.Height(x);
            angle = CatTerrain.AngleDeg(x);

            if (held && !wasHeld) Jump();               // tocou (borda do toque) = pula na hora
        }
        else
        {
            // no ar a velocidade horizontal se mantem (so o boost continua seu relogio)
            vy -= gravity * dt;
            x += Speed * dt;
            y += vy * dt;

            // DESIGN: soltar = para de girar NA HORA (sem inercia), pra o jogador controlar o angulo
            angVel = (enableFlip && held && heldTime >= flipHoldDelay) ? flipSpeed : 0f;
            angle += angVel * dt;
            airRotation += Mathf.Abs(angVel) * dt;

            float ground = CatTerrain.Height(x);
            if (y <= ground && vy < 0f) Land(ground);
        }

        wasHeld = held;
        // meia altura do quadrado: o "pe" fica no chao, qualquer que seja o tamanho
        transform.position = new Vector3(x, y + transform.localScale.y * 0.5f, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    /// Boost: fica cheio por boostHold segundos, depois cai em linha reta ate zero em boostFall.
    void UpdateBoost(float dt)
    {
        if (boostExtra <= 0f) return;
        boostTimer += dt;
        if (boostTimer > boostHold)
        {
            float rate = landBoost / Mathf.Max(0.01f, boostFall);
            boostExtra = Mathf.Max(0f, boostExtra - rate * dt);
        }
    }

    /// Pulo uniforme: sempre a mesma forca, sem carga e sem bonus de ladeira.
    void Jump()
    {
        vy = jumpForce; grounded = false; airRotation = 0f;
    }

    void Land(float ground)
    {
        y = ground; grounded = true; vy = 0f;
        float terrainAngle = CatTerrain.AngleDeg(x);
        float diff = Mathf.Abs(Mathf.DeltaAngle(angle, terrainAngle));
        LastLandDiff = diff;

        if (diff >= landTolerance)
        {
            if (badLanding == BadLanding.Restart) { Restart(); return; }
            slopeExtra = 0f; boostExtra = 0f;        // LoseExtra: volta pra base
        }
        else if (airRotation >= 360f - landTolerance)
        {
            // DESIGN: aterrissar com rotacao completa da boost (so acontece com o flip ligado)
            boostExtra = landBoost; boostTimer = 0f;
        }

        angle = terrainAngle; angVel = 0f;
    }

    /// Aterrissou torto (modo Restart): volta pro comeco da rua.
    void Restart()
    {
        falls++;
        x = 0f; y = CatTerrain.Height(x); vy = 0f;
        slopeExtra = 0f; boostExtra = 0f; grounded = true;
        angle = CatTerrain.AngleDeg(x); angVel = 0f;
        var cam = Camera.main != null ? Camera.main.GetComponent<CatCamera>() : null;
        if (cam != null) cam.Snap();
    }

    void OnGUI()
    {
        var st = new GUIStyle(GUI.skin.label) { fontSize = 20 };
        GUI.Label(new Rect(16, 12, 900, 30),
            $"velocidade  {Speed:0.0}   =  base {baseSpeed:0}  +  descida {slopeExtra:0.0}  +  boost {boostExtra:0.0}", st);
        GUI.Label(new Rect(16, 40, 600, 30), $"inclinacao  {CatTerrain.AngleDeg(x):0.0}deg", st);
        GUI.Label(new Rect(16, 68, 600, 30), grounded ? "" : "NO AR", st);
        GUI.Label(new Rect(16, 96, 900, 30), $"ultimo pouso  {LastLandDiff:0}deg de erro  (limite {landTolerance:0})   quedas {falls}", st);
        GUI.Label(new Rect(16, 132, 700, 30), "tocar = pular  ·  segurar no ar = backflip (bloqueado)", st);
    }
}
