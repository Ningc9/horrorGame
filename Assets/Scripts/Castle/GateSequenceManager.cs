using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;  // 添加 UI 命名空间

public class GateSequenceManager : MonoBehaviour
{
    private class SequencePoint
    {
        public string name;           // 位置点的名称
        public Transform location;     // 位置点的Transform
        public string hintText;       // 到达该点时的提示文本
        public GameObject visualEffect; // 可选的视觉效果
        public bool isReached = false; // 是否已经到达过
    }

    private float detectionRange = 2f;      // 检测范围
    private GameObject gate;                // 最终要开启的门
    private float gateOpenSpeed = 2f;       // 门开启速度
    private Vector3 gateOpenRotation = new Vector3(0, -90, 0); // 门开启后的旋转角度
    private float hintDisplayTime = 3f;     // 提示显示时间
    
    private List<SequencePoint> sequencePoints = new List<SequencePoint>();
    private TextMeshProUGUI hintText;
    private int currentIndex = 0;
    private bool sequenceStarted = false;   // 是否已经开始序列（到达第一个点）
    private bool sequenceCompleted = false;
    private ItemPickup itemPickup;
    private AudioSource audioSource;
    private Quaternion gateInitialRotation;
    private bool isGateOpening = false;
    private Transform playerTransform;  // 添加玩家Transform引用

    // 特效预制体的路径
    private readonly string correctEffectPath = "Effects/CorrectEffect";
    private readonly string wrongEffectPath = "Effects/WrongEffect";
    private GameObject correctEffectPrefab;
    private GameObject wrongEffectPrefab;

    // 音效的路径
    private readonly string correctSoundPath = "Audio/CorrectSound";
    private readonly string wrongSoundPath = "Audio/WrongSound";
    private AudioClip correctSound;
    private AudioClip wrongSound;

    private void Start()
    {
        SetupComponents();
        FindRequiredObjects();
        CreateSequencePoints();
        SetupUI();
        SetupEffects();

        // 输出调试信息
        Debug.Log("GateSequenceManager initialized");
        Debug.Log($"Found {sequencePoints.Count} sequence points");
        foreach (var point in sequencePoints)
        {
            Debug.Log($"Sequence point: {point.name} at position {point.location.position}");
        }
    }

    private void SetupComponents()
    {
        itemPickup = FindObjectOfType<ItemPickup>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            Debug.Log("Found player at: " + playerTransform.position);
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    private void FindRequiredObjects()
    {
        // 查找门（通常带有特定标签或名称）
        gate = GameObject.Find("Gate");
        if (gate == null)
        {
            Debug.LogWarning("Gate not found. Looking for objects containing 'gate' in name...");
            GameObject[] possibleGates = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject obj in possibleGates)
            {
                if (obj.name.ToLower().Contains("gate"))
                {
                    gate = obj;
                    break;
                }
            }
        }

        if (gate != null)
        {
            gateInitialRotation = gate.transform.rotation;
        }
        else
        {
            Debug.LogError("No gate found in the scene!");
        }
    }

    private void CreateSequencePoints()
    {
        // 按顺序查找所需的物体
        string[] objectNames = new string[] { "bed", "table.004", "furnace", "brick.008 (5)", "wall_passage.005" };
        string[] hints = new string[] {
            "The journey begins where dreams rest...",
            "Next, seek where thoughts are inked...",
            "The warmth guides your path...",
            "Ancient stones hold secrets...",
            "Finally, the passage reveals itself..."
        };

        for (int i = 0; i < objectNames.Length; i++)
        {
            GameObject obj = GameObject.Find(objectNames[i]);
            if (obj == null)
            {
                // 尝试模糊查找
                GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                {
                    if (go.name.ToLower().Contains(objectNames[i].ToLower().Split(' ')[0]))
                    {
                        obj = go;
                        break;
                    }
                }
            }

            if (obj != null)
            {
                SequencePoint point = new SequencePoint
                {
                    name = objectNames[i],
                    location = obj.transform,
                    hintText = hints[i],
                    visualEffect = CreateVisualEffect(obj.transform.position)
                };
                sequencePoints.Add(point);
                Debug.Log($"Added sequence point: {point.name}");
            }
            else
            {
                Debug.LogError($"Could not find object: {objectNames[i]}");
            }
        }
    }

    private GameObject CreateVisualEffect(Vector3 position)
    {
        // 创建一个简单的视觉效果（发光球体）
        GameObject effect = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        effect.transform.localScale = Vector3.one * 0.3f;
        // 将效果位置提高到玩家视线高度（约1.7米）
        effect.transform.position = position + Vector3.up * 1.7f;
        
        // 添加发光材质
        Material glowMaterial = new Material(Shader.Find("Standard"));
        glowMaterial.SetColor("_EmissionColor", Color.yellow * 2);
        glowMaterial.EnableKeyword("_EMISSION");
        effect.GetComponent<Renderer>().material = glowMaterial;
        
        // 移除碰撞体，这样不会影响玩家移动
        Destroy(effect.GetComponent<SphereCollider>());
        
        effect.SetActive(false);
        return effect;
    }

    private void SetupUI()
    {
        // 查找或创建UI画布
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("HintCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }

        // 创建提示文本
        GameObject textObj = new GameObject("HintText");
        textObj.transform.SetParent(canvas.transform, false);
        hintText = textObj.AddComponent<TextMeshProUGUI>();
        hintText.fontSize = 36;
        hintText.alignment = TextAlignmentOptions.Center;
        hintText.color = Color.white;
        
        // 设置文本位置在屏幕上方
        RectTransform rectTransform = hintText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        rectTransform.anchoredPosition = new Vector2(0, -50);
        rectTransform.sizeDelta = new Vector2(800, 100);
    }

    private void SetupEffects()
    {
        // 加载特效预制体（如果存在）
        correctEffectPrefab = Resources.Load<GameObject>(correctEffectPath);
        wrongEffectPrefab = Resources.Load<GameObject>(wrongEffectPath);

        // 加载音效（如果存在）
        correctSound = Resources.Load<AudioClip>(correctSoundPath);
        wrongSound = Resources.Load<AudioClip>(wrongSoundPath);

        // 如果没有找到预制体，使用简单的粒子系统
        if (correctEffectPrefab == null)
        {
            correctEffectPrefab = CreateSimpleParticleSystem(Color.green);
        }
        if (wrongEffectPrefab == null)
        {
            wrongEffectPrefab = CreateSimpleParticleSystem(Color.red);
        }
    }

    private GameObject CreateSimpleParticleSystem(Color color)
    {
        GameObject effectObj = new GameObject("ParticleEffect");
        ParticleSystem ps = effectObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.startColor = color;
        main.startSize = 0.2f;
        main.startSpeed = 2f;
        main.maxParticles = 100;
        main.duration = 1f;
        main.loop = false;

        var emission = ps.emission;
        emission.rateOverTime = 20;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.2f;

        effectObj.SetActive(false);
        return effectObj;
    }

    private void Update()
    {
        if (sequenceCompleted || isGateOpening) return;

        if (playerTransform == null)
        {
            Debug.LogError("Player transform is missing!");
            return;
        }

        // 检查玩家是否拿着权杖
        if (itemPickup == null || !itemPickup.isHoldingItem || 
            itemPickup.currentItem == null || !itemPickup.currentItem.name.Contains("Scepter"))
        {
            if (sequenceStarted)
            {
                ResetSequence(); // 如果序列已经开始但玩家放下了权杖，重置序列
            }
            return;
        }

        // 检查当前位置点
        CheckCurrentLocation();

        // 如果门正在开启，更新门的旋转
        if (isGateOpening && gate != null)
        {
            gate.transform.rotation = Quaternion.Slerp(
                gate.transform.rotation,
                Quaternion.Euler(gateOpenRotation),
                Time.deltaTime * gateOpenSpeed
            );
        }
    }

    private void CheckCurrentLocation()
    {
        // 如果序列还没开始，只检查第一个点
        if (!sequenceStarted)
        {
            CheckFirstPoint();
            return;
        }

        // 如果序列已经开始，检查下一个点
        if (currentIndex < sequencePoints.Count)
        {
            CheckNextPoint();
        }
    }

    private void CheckFirstPoint()
    {
        SequencePoint firstPoint = sequencePoints[0];
        float distance = Vector3.Distance(playerTransform.position, firstPoint.location.position);

        if (distance <= detectionRange)
        {
            sequenceStarted = true;
            HandleSequencePoint(firstPoint);
            Debug.Log("Sequence started with first point: bed");
        }
    }

    private void CheckNextPoint()
    {
        SequencePoint currentPoint = sequencePoints[currentIndex];
        float distance = Vector3.Distance(playerTransform.position, currentPoint.location.position);

        if (distance <= detectionRange && !currentPoint.isReached)
        {
            HandleSequencePoint(currentPoint);
        }
        else
        {
            // 检查是否触碰了后面的点（错误顺序）
            for (int i = currentIndex + 1; i < sequencePoints.Count; i++)
            {
                float wrongDistance = Vector3.Distance(playerTransform.position, sequencePoints[i].location.position);
                if (wrongDistance <= detectionRange)
                {
                    Debug.Log($"Wrong sequence! Reached {sequencePoints[i].name} before {currentPoint.name}");
                    ResetSequence();
                    return;
                }
            }
        }
    }

    private void HandleSequencePoint(SequencePoint point)
    {
        point.isReached = true;
        ShowHint(point.hintText);

        if (point.visualEffect != null)
        {
            point.visualEffect.SetActive(true);
        }

        if (correctSound != null)
        {
            audioSource.PlayOneShot(correctSound);
        }

        if (correctEffectPrefab != null)
        {
            Instantiate(correctEffectPrefab, point.location.position, Quaternion.identity);
        }

        Debug.Log($"Completed point {currentIndex + 1} of {sequencePoints.Count}: {point.name}");
        currentIndex++;

        if (currentIndex >= sequencePoints.Count)
        {
            Debug.Log("Sequence completed! Opening gate...");
            StartOpeningGate();
        }
    }

    private void ResetSequence()
    {
        Debug.Log("Resetting sequence");
        if (wrongSound != null)
        {
            audioSource.PlayOneShot(wrongSound);
        }

        if (wrongEffectPrefab != null)
        {
            Instantiate(wrongEffectPrefab, playerTransform.position, Quaternion.identity);
        }

        foreach (var point in sequencePoints)
        {
            if (point.visualEffect != null)
            {
                point.visualEffect.SetActive(false);
            }
            point.isReached = false;
        }

        ShowHint("Wrong sequence! Start over from the beginning.");
        currentIndex = 0;
        sequenceStarted = false;
    }

    private void StartOpeningGate()
    {
        sequenceCompleted = true;
        isGateOpening = true;
        ShowHint("The gate is opening...");
    }

    private void ShowHint(string message)
    {
        if (hintText != null)
        {
            hintText.text = message;
            Invoke(nameof(ClearHint), hintDisplayTime);
        }
    }

    private void ClearHint()
    {
        if (hintText != null)
        {
            hintText.text = "";
        }
    }
} 