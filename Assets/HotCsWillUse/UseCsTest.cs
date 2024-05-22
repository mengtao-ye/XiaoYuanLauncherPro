using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.Audio;
using UnityEngine.AI;
using System.Reflection;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;
using UnityEngine.Animations;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.Rendering;
using UnityEditor;
//using UnityEngine.Rendering.HighDefinition;
using Color = UnityEngine.Color;
using System.IO;
using static UnityEngine.TerrainTools.PaintContext;
using Unity.Collections;
using UnityEngine.Jobs;
using UnityEngine.ParticleSystemJobs;
using UnityEngine.Pool;
using UnityEngine.TerrainTools;
using UnityEngine.TerrainUtils;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.TextCore;
using UnityEngine.Sprites;
using UnityEngine.Scripting;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;
using static UnityEngine.ParticleSystem;
using UnityEngine.U2D;
using System.Globalization;

public class UseCsTest : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

    // Start is called before the first frame update
    void Start()
    {

    }


    #region 1.Unity的网络库
    /// <summary>
    /// 时间获取和格式转换
    /// </summary>
    void WillUseTime()
    {
        DateTime.Now.ToShortTimeString();
        DateTime dt = DateTime.Now;
        dt.ToString();//2005-11-5 13:21:25
        dt.ToFileTime().ToString();//127756416859912816
        dt.ToFileTimeUtc().ToString();//127756704859912816
        dt.ToLocalTime().ToString();//2005-11-5 21:21:25
        dt.ToLongDateString().ToString();//2005年11月5日
        dt.ToLongTimeString().ToString();//13:21:25
        dt.ToOADate().ToString();//38661.5565508218
        dt.ToShortDateString().ToString();//2005-11-5
        dt.ToShortTimeString().ToString();//13:21
        dt.ToUniversalTime().ToString();//2005-11-5 5:21:25
        dt.Year.ToString();//2005
        dt.Date.ToString();//2005-11-5 0:00:00
        dt.DayOfWeek.ToString();//Saturday
        dt.DayOfYear.ToString();//309
        dt.Hour.ToString();//13
        dt.Millisecond.ToString();//441
        dt.Minute.ToString();//30
        dt.Month.ToString();//11
        dt.Second.ToString();//28
        dt.Ticks.ToString();//632667942284412864
        dt.TimeOfDay.ToString();//13:30:28.4412864
        dt.ToString();//2005-11-5 13:47:04
        dt.AddYears(1).ToString();//2006-11-5 13:47:04
        dt.AddDays(1.1).ToString();//2005-11-6 16:11:04
        dt.AddHours(1.1).ToString();//2005-11-5 14:53:04
        dt.AddMilliseconds(1.1).ToString();//2005-11-5 13:47:04
        dt.AddMonths(1).ToString();//2005-12-5 13:47:04
        dt.AddSeconds(1.1).ToString();//2005-11-5 13:47:05
        dt.AddMinutes(1.1).ToString();//2005-11-5 13:48:10
        dt.AddTicks(1000).ToString();//2005-11-5 13:47:04
        dt.CompareTo(dt).ToString();//0
        dt.Equals("2005 - 11 - 6 16:11:04").ToString();//False
        dt.Equals(dt).ToString();//True
        dt.GetHashCode().ToString();//1474088234
        dt.GetType().ToString();//System.DateTime
        dt.GetTypeCode().ToString();//DateTime
        dt.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25
        string.Format("{ 0:d}", dt);//2005-11-5
    }
    public RawImage my_Image;
    private IEnumerator LoadImageFromUrl(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);

                my_Image.texture = texture;
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }

    IEnumerator TypeWriting(string filePath)
    {
        string targetText;
        #region 云测直接加载策略
        var url = Path.Combine("vu", filePath).Replace("\\", "/");
        Debug.Log($"图片地址：{url}");
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                targetText = www.downloadHandler.text;
            }
            else
            {
                targetText = "暂无介绍";
                Debug.Log("Failed to load text: " + www.error);
            }
        }
        #endregion       
    }

    private IEnumerator LoadAudioFromUrl(string url)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                audioSource.spatialBlend = 0; // 将声音设置为2D
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.Play();
            }
            else
            {
                Debug.LogError("Failed to load image: " + www.error);
            }
        }
    }
    /// <summary>
    /// 2.网络模块
    /// </summary>
    void WillUseNet()
    {
        StartCoroutine(OnStartHttp());
    }
    /// <summary>
    /// Http 访问
    /// </summary>
    /// <returns></returns>
    IEnumerator OnStartHttp()
    {
        // 创建一个表单
        WWWForm form = new WWWForm();
        form.AddField("username", "john");
        form.AddField("password", "123456");

        // 发送 POST 请求
        using (UnityWebRequest www = UnityWebRequest.Post("http://www.example.com/login", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 获取服务器返回的数据
                Debug.Log(www.downloadHandler.text);
            }
        }

        // 发送 GET 请求
        using (UnityWebRequest www = UnityWebRequest.Get("http://www.example.com/leaderboard"))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 获取服务器返回的数据
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    #region 长连接
    private Socket socket;
    private Thread receiveThread;
    private bool isRunning = false;

    void OnStartSocket()
    {
        // 创建 Socket 对象
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        // 连接服务器
        socket.Connect("192.168.1.100", 8888);

        // 启动接收线程
        isRunning = true;
        receiveThread = new Thread(new ThreadStart(ReceiveMessage));
        receiveThread.Start();
        //
        SendMessage("测试");
    }

    void OnDestroy()
    {
        // 关闭连接
        isRunning = false;
        if (receiveThread != null)
        {
            receiveThread.Join();
        }
        if (socket != null)
        {
            socket.Close();
        }
    }

    void SendMessage(string message)
    {
        // 发送消息
        byte[] data = System.Text.Encoding.UTF8.GetBytes(message);
        socket.Send(data, data.Length, SocketFlags.None);
    }

    void ReceiveMessage()
    {
        // 接收消息
        while (isRunning)
        {
            if (socket.Available > 0)
            {
                byte[] buffer = new byte[1024];
                int length = socket.Receive(buffer, buffer.Length, SocketFlags.None);
                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, length);
                Debug.Log(message);
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
    #endregion
    #endregion

    #region 2. Unity的场景管理库
    void WillUseSceneManager()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MyScene");
        SceneManager.UnloadSceneAsync("MyScene");
    }
    #endregion

    #region 3.Unity的音频库
    public AudioClip jumpSound;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    // AudioClipPlayable AudioClipPlayable;
    AudioMixerGroup mixerGroup;
    // PlayableGraph PlayableGraph;
    void WillUseSceneAudio()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(jumpSound);
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
        AudioClipPlayable audioClipPlayable = AudioClipPlayable.Create(PlayableGraph, backgroundMusic, jumpSound);
        audioClipPlayable.GetClip();
        AudioMixer audioMixer = mixerGroup.audioMixer;
        AudioMixerSnapshot audioMixerSnapshot = audioMixer.FindSnapshot("");
        audioMixer = audioMixerSnapshot.audioMixer;
        AudioMixerGroup[] mixerGroupc = audioMixer.FindMatchingGroups("");
        string name = mixerGroupc[0].name;
        AudioPlayableOutput audioPlayableOutput = AudioPlayableOutput.Create(PlayableGraph, "", audioSource);
        bool mk = audioPlayableOutput.GetEvaluateOnSeek();
    }

    void WillUseAudioReverbZone()
    {
        AudioReverbZone audioReverbZone = new AudioReverbZone();
    }

    #endregion

    #region 4.Unity的视频库
    public VideoPlayer videoPlayer;
    public VideoClip newClip;

    PlayableGraph PlayableGraph;
    void WillUseSceneVideo()
    {
        int a1 = newClip.audioTrackCount;
        float a2 = newClip.frameCount;
        double a3 = newClip.frameRate;
        uint a4 = newClip.height;
        double a5 = newClip.length;
        string a6 = newClip.originalPath;
        uint a7 = newClip.pixelAspectRatioDenominator;
        uint a8 = newClip.pixelAspectRatioNumerator;
        bool a9 = newClip.sRGB;
        uint a10 = newClip.width;
        newClip.GetAudioChannelCount(new ushort());
        newClip.GetAudioLanguage(new ushort());
        newClip.GetAudioSampleRate(new ushort());

        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.clip = newClip;
        videoPlayer.Play();
        videoPlayer.Stop();


    }
    #endregion

    #region 5.Unity的物理检测库,射线检测
    private void OnCollisionEnter(Collision collision)
    {
        // 检测角色是否接触地面
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger event detected between " + gameObject.name + " and " + other.gameObject.name);
    }
    private void UpdateRaycast()
    {
        // 创建一条从摄像机发射的射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 检测射线是否与物体发生了交集
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Raycast detected between " + gameObject.name + " and " + hit.collider.gameObject.name);
        }
    }
    private Rigidbody rb;
    /// <summary>
    /// 力的使用
    /// </summary>
    private void UpdateRigidbody()
    {
        // 获取物体的刚体组件
        rb = GetComponent<Rigidbody>();
        // 在物体上施加一个向右的力
        rb.AddForce(Vector3.right * 10f, ForceMode.Force);
    }
    private float speed = 5f;
    private float jumpForce = 10f;
    private bool isGrounded = true;
    /// <summary>
    /// 角色控制器
    /// </summary>
    private void UpdateCharacter()
    {
        // 获取角色的刚体组件
        rb = GetComponent<Rigidbody>();
        // 获取用户的输入
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // 计算角色的移动向量
        Vector3 move = new Vector3(horizontalInput, 0f, verticalInput) * speed * Time.deltaTime;

        // 设置角色的朝向
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        // 在角色接触地面时允许跳跃
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    #endregion

    #region 6.Unity的渲染库，包含了许多渲染相关的类和函数，例如材质、纹理、光照等
    public Material mat; // 声明材质球
    public Texture tex; // 声明纹理
    public Texture2D tex2; // 声明纹理2
    public Light light; // 声明光源
    public Shader shader; // 声明着色器
    public Material mat2; // 声明材质球2
    //
    public MaterialPropertyBlock block; // 声明材质球属性块
    public Mesh mesh; // 声明网格
    public MeshRenderer meshRenderer; // 声明网格渲染器
    public MeshFilter meshFilter; // 声明网格过滤器
    public SkinnedMeshRenderer skinnedMeshRenderer; // 声明蒙皮网格渲染器

    void WillUseRendering()
    {
        mat = new Material(Shader.Find("Diffuse")); // 创建材质球，并指定着色器
        mat.color = Color.red; // 将材质球颜色修改为红色
        mat.mainTexture = tex; // 将纹理赋值给材质球
        tex = new Texture2D(256, 256, TextureFormat.RGB24, false); // 创建纹理，并指定大小和格式
        Color color = new Color(1.0f, 0.0f, 0.0f); // 创建颜色
        tex2.SetPixel(0, 0, color); // 修改纹理像素
        tex2.Apply(); // 应用修改
        light = gameObject.AddComponent<Light>(); // 添加光源组件
        light.type = LightType.Point; // 设置光源类型为点光源
        light.color = Color.red; // 将光源颜色修改为红色
        shader = Shader.Find("Custom/MyShader"); // 创建自定义着色器
        mat2.shader = shader; // 将自定义着色器赋值给材质球
        block = new MaterialPropertyBlock();
        mesh = new Mesh();
        meshRenderer = new MeshRenderer();
        meshFilter = new MeshFilter();
        skinnedMeshRenderer = new SkinnedMeshRenderer();

        _receiverRender = GetComponent<Renderer>();
        InvokeRepeating(nameof(SetReceiverMatrix), 0, invokeTime);
    }
    Renderer _receiverRender;
    bool usePLANARSHADOW = true;
    float invokeTime = 0.5f;
    public Transform receiver;

    int World2Ground = Shader.PropertyToID("_World2Ground");
    int Ground2World = Shader.PropertyToID("_Ground2World");
    void SetReceiverMatrix()
    {
        if (_receiverRender == null)
        {
            foreach (var mat in _receiverRender.materials)
            {
                mat.DisableKeyword("_PLANARSHADOW_ENABLE");
                usePLANARSHADOW = false;
            }

        }
        if (_receiverRender != null && usePLANARSHADOW)
        {
            foreach (var mat in _receiverRender.materials)
            {
                mat.SetMatrix(World2Ground, receiver.worldToLocalMatrix);
                mat.SetMatrix(Ground2World, receiver.localToWorldMatrix);
            }
        }
    }
    #endregion

    #region 7.Unity的人工智能库，包含了许多AI相关的类和函数，例如寻路、路径规划等
    public Transform target; // 目标点
    private NavMeshAgent agent; // 导航代理
    public float detectionRange = 10f; // 检测范围
    private bool isChasing = true; // 是否正在追逐
    void WillUseAI()
    {
        agent = GetComponent<NavMeshAgent>(); // 获取导航代理组件
        OnUpdate();
    }

    void OnUpdate()
    {
        if (isChasing)
        {
            ChaseTarget();
        }
        else
        {
            WanderAround();
        }
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        isChasing = true; // 进入追逐状态
    //    }
    //}

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false; // 退出追逐状态
        }
    }

    void WanderAround()
    {
        if (agent.remainingDistance < 0.5f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, 5f, -1); // 在半径为5的球形区域内随机生成一个新的位置
            agent.SetDestination(newPos); // 设置新的目标点
        }
    }

    void ChaseTarget()
    {
        if (target != null && Vector3.Distance(transform.position, target.position) < detectionRange)
        {
            agent.SetDestination(target.position); // 追逐目标
        }
        else
        {
            isChasing = false; // 目标超出检测范围，退出追逐状态
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDir = UnityEngine.Random.insideUnitSphere * dist; // 在球形区域内生成随机方向
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, dist, layermask); // 在随机方向上采样一个可行的位置
        return navHit.position;
    }
    #endregion

    #region 8. Unity中的模型动画控制，基础移动、旋转库
    //动画控制
    public Animator animator;
    public Animation animation;
    void WillUseAnimator()
    {
        animation = GetComponent<Animation>();
        animation.AddClip(new AnimationClip(), "");
        // 播放动画
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // 暂停/继续动画
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (animator.speed == 0)
            {
                animator.speed = 1;
            }
            else
            {
                animator.speed = 0;
            }
        }

        // 重置动画
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.Play("Idle");
        }
        AnimationClipPlayable animationClipPlayable = new AnimationClipPlayable();
        animationClipPlayable.Destroy();
        AimConstraint aimConstraint = GetComponent<AimConstraint>();
        string str = aimConstraint.name;
        AnimationHumanStream animationHumanStream = new AnimationHumanStream();
        Vector3 vector3 = animationHumanStream.bodyLocalPosition;
        AnimationPlayableBinding.Create("", new UnityEngine.Object());
    }

    public static void UseAmition()
    {
        AnimationPlayableBinding.Create("", new UnityEngine.Object());
    }

    public float speed2 = 5f;
    void WillUseMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical) * speed2 * Time.deltaTime;
        transform.Translate(movement, Space.Self);
    }
    public float rotationSpeed = 100f;
    void WillUseRotate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime);
    }
    #endregion

    #region 9.Unity中的反射，委托(delegate),事件(event),Action,Func使用
    //反射：
    public int myInt = 10;
    /// <summary>
    /// 反射
    /// </summary>
    void WillUseReflection()
    {
        //PropertyInfo propertyInfo = typeof(ReflectionExample).GetProperty("myInt");
        //Debug.Log("myInt = " + propertyInfo.GetValue(this));
    }

    //委托：
    public delegate void MyDelegate(string message);
    public MyDelegate myDelegate;
    /// <summary>
    /// 委托
    /// </summary>
    void WillUseDelegate()
    {
        myDelegate = LogMessage;
        myDelegate("Hello World");
    }
    private void LogMessage(string message)
    {
        Debug.Log(message);
    }

    //事件：
    public delegate void MyDelegate2();
    public event MyDelegate2 myEvent;
    void WillUseEvent()
    {
        myEvent += LogMessage2;
        myEvent();
    }
    private void LogMessage2()
    {
        Debug.Log("Event Fired");
    }

    //Action和Func：
    void WillUseAction()
    {
        Action myAction = LogMessage3;
        myAction();

        Func<int, int> myFunc = AddOne;
        int result = myFunc(10);
        Debug.Log("Result = " + result);
    }
    private void LogMessage3()
    {
        Debug.Log("Action Called");
    }

    private int AddOne(int number)
    {
        return number + 1;
    }
    #endregion

    #region 10.Unity中的事件系统库使用
    public UnityEvent onButtonClick;
    public UnityEvent onPointerEnter;
    public UnityEvent onPointerExit;
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;
    public UnityEvent onDrag;

    public void OnPointerClick(PointerEventData eventData)
    {
        onButtonClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        onDrag.Invoke();
    }
    #endregion

    #region 11.Unity中的TimeLine使用
    public TimelineAsset timelineAsset = new TimelineAsset();
    //TimelineAsset timelineAsset = new TimelineAsset();
    private PlayableDirector playableDirector;
    float my_time = 1;
    void WillUseTimeLine()
    {
        // timelineAsset = new TimelineAsset();
        playableDirector = GetComponent<PlayableDirector>();
        // playableDirector.playableAsset = timelineAsset;

        playableDirector.Play();
        playableDirector.Pause();
        playableDirector.Stop();
        playableDirector.time = my_time;
        // 设置时间轴的默认播放速度
        SetSpeed(1f);
        Playable playable = new Playable();
        ActivationControlPlayable activationControlPlayable = GetComponent<ActivationControlPlayable>();
        activationControlPlayable.OnGraphStart(playable);
        ActivationTrack activationTrack = GetComponent<ActivationTrack>();
        PlayableGraph playableGraph = new PlayableGraph();
        activationTrack.CreatePlayable(playableGraph, new GameObject());
        AnimationPlayableAsset animationPlayableAsset = new AnimationPlayableAsset();
        ClipCaps clipCaps = animationPlayableAsset.clipCaps;
        AudioBehaviour audioBehaviour = GetComponent<AudioBehaviour>();
        string str = audioBehaviour.name;
        AudioPlayableAsset audioPlayableAsset = new AudioPlayableAsset();
        _ = audioPlayableAsset.duration;
        audioPlayableAsset.CreatePlayable(playableGraph, new GameObject());
        AudioTrack audioTrack = new AudioTrack();
        AudioClipLoadType audioClipLoadType = new AudioClipLoadType();
        AudioClipPlayable audioClipPlayable = new AudioClipPlayable();
        PlayableBehaviour playableBehaviour = GetComponent<PlayableBehaviour>();
        playableBehaviour.OnPlayableCreate(playable);
        PlayableExtensions.SetDuration(playable, 1f);

        audioClipPlayable.Play();
        audioClipPlayable.Destroy();
        BasicPlayableBehaviour basicPlayableBehaviour = GetComponent<BasicPlayableBehaviour>();

        FrameData frameData = new FrameData();
        basicPlayableBehaviour.OnBehaviourPause(playable, frameData);
        ControlTrack controlTrack = new ControlTrack();
        controlTrack.CreatePlayable(playableGraph, new GameObject());
        CustomStyleAttribute customStyleAttribute = new CustomStyleAttribute("");
        string strr = customStyleAttribute.ussStyle;
        DirectorControlPlayable directorControlPlayable = new DirectorControlPlayable();
        directorControlPlayable.OnBehaviourPlay(playable, frameData);
        GroupTrack groupTrack = new GroupTrack();
        groupTrack.CreatePlayable(playableGraph, new GameObject());
        HideInMenuAttribute hideInMenuAttribute = new HideInMenuAttribute();
        hideInMenuAttribute.ToString();
        MarkerTrack markerTrack = new MarkerTrack();
        bool b = markerTrack.mutedInHierarchy;
        UnityEngine.Animations.NotKeyableAttribute notKeyableAttribute = new UnityEngine.Animations.NotKeyableAttribute();
        notKeyableAttribute.ToString();
        ParticleControlPlayable particleControlPlayable = new ParticleControlPlayable();
        particleControlPlayable.OnBehaviourPlay(playable, frameData);
        PlayableTrack playableTrack = new PlayableTrack();
        double d = playableTrack.start;
        PrefabControlPlayable prefabControlPlayable = new PrefabControlPlayable();
        prefabControlPlayable.OnBehaviourPlay(playable, frameData);
        SignalAsset signalAsset = new SignalAsset();
        int id = signalAsset.GetInstanceID();
        SignalEmitter signalEmitter = new SignalEmitter();
        signalEmitter.GetInstanceID();
        SignalReceiver signalReceiver = new SignalReceiver();
        signalReceiver.GetInstanceID();
        SignalTrack signalTrack = new SignalTrack();
        signalTrack.GetInstanceID();
        TimelineAsset timelineAsset = new TimelineAsset();
        playableDirector.playableAsset = timelineAsset;
        TimelineClip timelineClip = GetComponent<TimelineClip>();
        double V = timelineClip.start;
        TimelinePlayable timelinePlayable = new TimelinePlayable();
        timelinePlayable.OnBehaviourPlay(playable, frameData);
        TimeNotificationBehaviour timeNotificationBehaviour = new TimeNotificationBehaviour();
        timeNotificationBehaviour.OnBehaviourPlay(playable, frameData);
        TrackAsset trackAsset = GetComponent<TrackAsset>();
        double m = trackAsset.start;


    }
    AnimationPlayableAsset.LoopMode loopMode = AnimationPlayableAsset.LoopMode.UseSourceAsset;
    TimelineClip.ClipExtrapolation clipExtrapolation = TimelineClip.ClipExtrapolation.Continue;
    TrackOffset trackOffset = TrackOffset.ApplySceneOffsets;
    public static void UseVEG()
    {
        float x = VFXManager.fixedTimeStep;
        VFXExposedProperty vFXExposedProperty = new VFXExposedProperty();
        string str = vFXExposedProperty.name;

    }
    public void UseVex()
    {
        VFXExpressionValues vFXExpressionValues = GetComponent<VFXExpressionValues>();
        AnimationCurve animationCurve = vFXExpressionValues.GetAnimationCurve(2);
        int xx = animationCurve.length;
        VFXSpawnerCallbacks VEFMy = new VEFMy();
        VFXSpawnerState vFXSpawnerState = new VFXSpawnerState();
        VisualEffect visualEffect = new VisualEffect();
        VEFMy.OnPlay(vFXSpawnerState, vFXExpressionValues, visualEffect);
        VisualEffectAsset visualEffectAsset = new VisualEffectAsset();
        string str = visualEffectAsset.name;
        visualEffectAsset.GetTextureDimension(2);
        int id = visualEffectAsset.GetInstanceID();
        VisualEffectObject visualEffectObject = new MyFEX();
        string name = visualEffectObject.name;
        id = visualEffectObject.GetInstanceID();

    }

    // 设置时间轴的播放速度
    public void SetSpeed(float speed)
    {
        playableDirector.playableGraph.GetRootPlayable(0).SetSpeed(speed);
    }

    [System.Serializable]
    public class TimelinePlayableAsset : PlayableAsset, ITimelineClipAsset
    {
        public ExposedReference<PlayableDirector> director;
        public float loopStartTime;

        public ClipCaps clipCaps => throw new NotImplementedException();


        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            TimelinePlayableBehaviour behaviour = new TimelinePlayableBehaviour();
            behaviour.director = director.Resolve(graph.GetResolver());
            behaviour.loopStartTime = loopStartTime;

            return ScriptPlayable<TimelinePlayableBehaviour>.Create(graph, behaviour);
        }
    }

    public class TimelinePlayableBehaviour : PlayableBehaviour
    {
        public PlayableDirector director;
        public float loopStartTime;

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable)
        {

        }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable)
        {

        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            director.time = loopStartTime;
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {

        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {
            //Debug.Log("A");
        }
    }

    public class MarkerRecieverRewind : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            PlayableDirector director = (PlayableDirector)origin.GetGraph().GetResolver();

            SignalEmitter emitter = (SignalEmitter)notification;
            TimelineAsset ta = emitter.parent.timelineAsset;
            int markerCount = ta.markerTrack.GetMarkerCount();
            if (markerCount > 0)
            {
                IMarker marker = ta.markerTrack.GetMarker(0);
                director.Pause();
                director.time = marker.time;
                director.Play();
            }
        }
    }
    [TrackColor(255f / 255f, 255f / 255f, 100f / 255f)]
    [TrackClipType(typeof(PlayableAsset))]
    [TrackClipType(typeof(UnityEngine.Timeline.TimelineClip))]
    [TrackBindingType(typeof(Transform))]
    public class TrackDemo1 : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }

    public class CustomSignal : SignalEmitter
    {
        public int param;
    }

    #endregion

    #region 12.Unity中各种数学计算库的使用
    void WillUseMathf()
    {
        float sinValue = Mathf.Sin(30 * Mathf.Deg2Rad);
        Debug.Log(sinValue); //输出0.5

        int randomValue = UnityEngine.Random.Range(1, 10);
        Debug.Log(randomValue); //输出1到10之间的随机整数

        double cosValue = Math.Cos(Math.PI / 4);
        Debug.Log(cosValue); //输出0.7071067811865476

        Vector3 up = Vector3.up;
        Vector3 right = Vector3.right;
        float dotValue = Vector3.Dot(up, right);
        Debug.Log(dotValue); //输出0

        Vector3 position = transform.position;
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Vector3 rotatedPosition = rotation * position;
        Debug.Log(rotatedPosition); //输出绕Y轴旋转45度后的位置向量

        HeightMathf();
    }


    public Transform targetObject;
    public Transform otherObject;
    public Transform point1;
    public Transform point2;
    public Transform point3;
    void HeightMathf()
    {
        //点乘
        Vector3 direction = targetObject.position - otherObject.position;
        float cosAngle = Vector3.Dot(direction.normalized, otherObject.forward.normalized);
        Debug.Log("Cosine of angle between vectors: " + cosAngle);

        //叉乘
        Vector3 v1 = point2.position - point1.position;
        Vector3 v2 = point3.position - point1.position;
        Vector3 normal = Vector3.Cross(v1, v2);
        normal.Normalize();
        Debug.DrawRay(point1.position, normal, Color.green);
    }
    #endregion

    #region 13.Unity中各种集合
    // 定义List集合
    private List<int> numList = new List<int>();
    // 定义Dictionary集合
    private Dictionary<string, int> dict = new Dictionary<string, int>();
    // 定义Queue队列
    private Queue<string> msgQueue = new Queue<string>();
    // 定义LinkedList链表
    private LinkedList<string> nameList = new LinkedList<string>();
    // 定义Stack堆栈
    private Stack<float> numStack = new Stack<float>();
    void WillUseList()
    {
        // 向List集合中添加元素
        numList.Add(1);
        numList.Add(2);
        numList.Add(3);

        // 向Dictionary集合中添加键值对
        dict.Add("apple", 5);
        dict.Add("banana", 3);
        dict.Add("orange", 2);

        // 向Queue队列中添加元素
        msgQueue.Enqueue("Hello");
        msgQueue.Enqueue("World");

        // 向LinkedList链表中添加元素
        nameList.AddLast("Alice");
        nameList.AddLast("Bob");
        nameList.AddLast("Charlie");

        // 向Stack堆栈中添加元素
        numStack.Push(1.1f);
        numStack.Push(2.2f);
        numStack.Push(3.3f);


        // 遍历List集合
        foreach (int num in numList)
        {
            Debug.Log("num: " + num);
        }

        // 遍历Dictionary集合
        foreach (KeyValuePair<string, int> pair in dict)
        {
            Debug.Log(pair.Key + ": " + pair.Value);
        }

        // 弹出Queue队列中的元素
        if (msgQueue.Count > 0)
        {
            Debug.Log(msgQueue.Dequeue());
        }

        // 遍历LinkedList链表
        LinkedListNode<string> node = nameList.First;
        while (node != null)
        {
            Debug.Log(node.Value);
            node = node.Next;
        }

        // 弹出Stack堆栈中的元素
        if (numStack.Count > 0)
        {
            Debug.Log(numStack.Pop());
        }
    }
    #endregion

        #region 14.System.Linq：这个命名空间包含一些用于 LINQ 查询的扩展方法，如 Where、Select、OrderBy 等，我们可以使用这些方法来方便地对集合进行筛选、排序等操作。
    void WillUseLinq()
    {
        // 创建一个int类型的数组
        int[] nums = { 1, 2, 3, 4, 5 };

        // 使用Where方法筛选出数组中大于3的元素
        var filtered = nums.Where(x => x > 3);
        Debug.Log("Filtered: " + string.Join(",", filtered));

        // 使用Select方法将数组元素转为字符串
        var strArray = nums.Select(x => x.ToString()).ToArray();
        Debug.Log("String array: " + string.Join(",", strArray));

        // 使用OrderBy方法将数组元素按降序排序
        var sorted = nums.OrderByDescending(x => x);
        Debug.Log("Sorted: " + string.Join(",", sorted));

        // 使用FirstOrDefault方法获取数组中第一个大于3的元素，如果不存在则返回默认值0
        var firstOrDefault = nums.FirstOrDefault(x => x > 3);
        Debug.Log("FirstOrDefault: " + firstOrDefault);

        // 创建一个字符串列表
        List<string> names = new List<string>() { "Alice", "Bob", "Charlie", "David", "Eve" };

        // 使用Any方法判断列表中是否存在以'B'开头的字符串
        var hasBName = names.Any(x => x.StartsWith("B"));
        Debug.Log("Has B name: " + hasBName);

        // 使用All方法判断列表中的所有字符串是否均以字母开头
        var allLetters = names.All(x => char.IsLetter(x[0]));
        Debug.Log("All letters: " + allLetters);

        // 使用Count方法统计列表中以'A'开头的字符串数量
        var countA = names.Count(x => x.StartsWith("A"));
        Debug.Log("Count A: " + countA);

        // 使用Max方法获取列表中字符串长度的最大值
        var maxLen = names.Max(x => x.Length);
        Debug.Log("Max length: " + maxLen);

        // 使用Reverse方法将列表中的元素顺序反转
        names.Reverse();
        Debug.Log("Reversed: " + string.Join(",", names));

        // 创建一个字典
        Dictionary<string, int> dict = new Dictionary<string, int>()
        {
            { "apple", 5 },
            { "banana", 3 },
            { "orange", 2 }
        };

        // 使用Where方法筛选出字典中值大于等于3的键值对
        var filteredDict = dict.Where(x => x.Value >= 3);
        foreach (var item in filteredDict)
        {
            Debug.Log("Filtered dict: " + item.Key + " - " + item.Value);
        }

        // 使用OrderBy方法将字典中的键按升序排序
        var sortedKeys = dict.OrderBy(x => x.Key);
        foreach (var item in sortedKeys)
        {
            Debug.Log("Sorted keys: " + item.Key + " - " + item.Value);
        }

        // 使用ToDictionary方法将列表中的字符串转为字典，键为字符串，值为字符串长度
        var dictFromList = names.ToDictionary(x => x, x => x.Length);
        foreach (var item in dictFromList)
        {
            Debug.Log("Dict from list: " + item.Key + " - " + item.Value);
        }

        // 创建一个自定义类的列表
        List<Person> people = new List<Person>()
        {
            new Person() { Name = "Alice", Age = 20 },
            new Person() { Name = "Bob", Age = 25 },
            new Person() { Name = "Charlie", Age = 30 }
        };

        // 使用OrderByDescending方法将列表中的元素按年龄降序排序
        var sortedPeople = people.OrderByDescending(x => x.Age);
        foreach (var person in sortedPeople)
        {
            Debug.Log("Sorted people: " + person.Name + " - " + person.Age);
        }

    }
    // 自定义Person类
    private class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    #endregion

    #region 15.UnityEngine.Assertions
    
    void WillUseAssertions()
    {
        float a = FloatComparer.kEpsilon;

        FloatComparer floatComparer = FloatComparer.s_ComparerWithDefaultTolerance;

        Assert.AreApproximatelyEqual(0,0);
        Assert.AreEqual(FloatComparer.kEpsilon, a);
        Assert.AreNotApproximatelyEqual(-FloatComparer.kEpsilon, a);
        Assert.AreNotEqual(FloatComparer.kEpsilon, a);
        Assert.IsFalse(FloatComparer.kEpsilon == a);
        Assert.IsNotNull(FloatComparer.s_ComparerWithDefaultTolerance);
        Assert.IsNull(FloatComparer.s_ComparerWithDefaultTolerance);
        Assert.IsTrue(true);
    }
    
    public static ParentConstraint SetParentConstraint(Transform target, Transform parentSource,
  ParentConstraint constraint = null)
    {
        Assert.IsTrue(!constraint || target == constraint.transform);

        if (!constraint)
        {
            if (!parentSource) return null;
            constraint = target.gameObject.AddComponent<ParentConstraint>();
        }

        // 清空已有约束
        constraint.constraintActive = false;
        for (int i = constraint.sourceCount - 1; i >= 0; i--)
        {
            constraint.RemoveSource(i);
        }

        // 无约束
        if (!parentSource) return constraint;

        // 设置新约束
        constraint.AddSource(new ConstraintSource
        {
            sourceTransform = parentSource,
            weight = 1,
        });

        // 设置 Position offset
        var positionOffset = parentSource.InverseTransformPoint(target.position);
        constraint.SetTranslationOffset(0, positionOffset);

        // 设置 Rotation offset
        var localForward = parentSource.InverseTransformDirection(target.forward);
        var localUpward = parentSource.InverseTransformDirection(target.up);
        var rotationOffset = Quaternion.LookRotation(localForward, localUpward).eulerAngles;
        constraint.SetRotationOffset(0, rotationOffset);

        // 激活约束
        constraint.constraintActive = true;

        return constraint;
    }
    #endregion

    #region 16.UnityEngine.TextRenderinModule
    public static int GetStringLength(Text text, string message)
    {
        int totalLength = 0;
        UnityEngine.Font myFont = text.font;
        myFont.RequestCharactersInTexture(message, text.fontSize, text.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = message.ToCharArray();
        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, text.fontSize);

            totalLength += characterInfo.advance;
        }
        return totalLength;
    }
    #endregion

    #region 16.ParticleSystemMoudule
    public void WillUseParticleSystem()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Clear();
        particleSystem.Play();
        particleSystem.Pause();
        StartCoroutine(GetEnumerator("xx"));
        ParticleSystemRenderer particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
        ParticleSystem.MainModule mainModule = particleSystem.main;
        ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
        ParticleSystem.ShapeModule shapeModule = particleSystem.GetComponent<ParticleSystem.ShapeModule>();
        ParticleSystem.VelocityOverLifetimeModule velocityOverLifetimeModule = particleSystem.velocityOverLifetime;
        ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetimeModule = particleSystem.limitVelocityOverLifetime;
        ParticleSystem.CollisionModule collisionModule = particleSystem.collision;

    }
    #endregion

    #region 17.后处理相关
    /// <summary>
    /// 时间获取和格式转换
    /// </summary>
    //public class RenderFeature : ScriptableRendererFeature
    //{
    //    RenderPass _pass = new RenderPass();
    //    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    //    {
    //        throw new NotImplementedException();
    //        renderer.EnqueuePass(_pass);
    //    }
    //    public override void Create()
    //    {
    //        _pass = new RenderPass();

    //        _pass.renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
    //    }

    //    public class RenderPass : ScriptableRenderPass
    //    {
    //        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    //        {
    //        }
    //        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    //        {
    //            throw new NotImplementedException();
    //        }
    //        public override void OnCameraCleanup(CommandBuffer cmd)
    //        {
    //        }
    //    }
    //}
    #endregion

    #region 18.Terrain地形相关
    //public void WillUseTerrain()
    //{
    //    Terrain terrain = GetComponent<Terrain>();
    //    TerrainCollider collider = GetComponent<TerrainCollider>();

    //    TerrainData terrainData = new TerrainData();
    //    terrainData.heightmapResolution = 513;
    //    terrainData.baseMapResolution = 513;
    //    terrainData.size = new Vector3(50, 50, 50);
    //    terrainData.alphamapResolution = 512;
    //    terrainData.SetDetailResolution(32, 8);

    //    int width = terrainData.heightmapResolution;
    //    int height = terrainData.heightmapResolution;

    //    float[,] heightsBackups = terrainData.GetHeights(0, 0, width, height);

    //    terrainData.SetHeights(0, 0, heightsBackups);

    //    GameObject obj = Terrain.CreateTerrainGameObject(terrainData);
    //    AssetDatabase.CreateAsset(terrainData, "Assets/Terrain_ModifyHeight.asset");
    //    AssetDatabase.SaveAssets();
    //}

    #endregion

    #region 19.Ragdoll布娃娃系统
    public void WillUseRagdoll()
    {
        //RagdollConstructor ragdollConstructor = new RagdollConstructor();

        //RamecanMixer ramecanMixer = new RamecanMixer();
    }
    #endregion

    #region 20.wind风
    void WillUseWind()
    {
        WindZone windZone = GetComponent<WindZone>();
        windZone.windMain = 1;
        windZone.radius = 1;
        windZone.mode = WindZoneMode.Spherical;
    }
    #endregion

    #region 21.矩阵计算
    void WillUseMath()
    {
        Matrix4x4 matrix = new Matrix4x4();
        matrix.SetTRS(transform.position, transform.rotation, Vector3.one);

    }

    public Quaternion GetRotation(Matrix4x4 matrix4X4)
    {
        float qw = Mathf.Sqrt(1f + matrix4X4.m00 + matrix4X4.m11 + matrix4X4.m22) / 2;
        float w = 4 * qw;
        float qx = (matrix4X4.m21 - matrix4X4.m12) / w;
        float qy = (matrix4X4.m02 - matrix4X4.m20) / w;
        float qz = (matrix4X4.m10 - matrix4X4.m01) / w;
        return new Quaternion(qx, qy, qz, qw);
    }



    #endregion

    #region 22.TrailRender
    public void WillUseTrail()
    {
        TrailRenderer trailRenderer = GetComponent<TrailRenderer>();

    }
    #endregion

    #region 22.lineRender
    public void WillUseLineRenderer()
    {
        LineRenderer trailRenderer = GetComponent<LineRenderer>();
    }

    void GetLineRenderer()
    {
        LineRenderer _lr = new LineRenderer();
        if (_lr == null) { Debug.Log($"{this.transform.name}: line renderer 获取失败"); return; }

        //if (lineMat == null) { Debug.Log("line renderer材质不能为空"); return; }

        //line renderer 设置;
        //_lr.materials = lineMat;
        _lr.useWorldSpace = true;

        //颜色渐变，linerenderer会赋值给顶点色
        _lr.colorGradient = new Gradient();
        _lr.shadowCastingMode = ShadowCastingMode.Off;
        _lr.lightProbeUsage = LightProbeUsage.Off;
        _lr.reflectionProbeUsage = ReflectionProbeUsage.Off;
        _lr.textureMode = LineTextureMode.RepeatPerSegment;
        _lr.alignment = LineAlignment.TransformZ;
        _lr.widthCurve = AnimationCurve.EaseInOut(10,10,10,10);
        _lr.useWorldSpace = true;

    }

    //这步可省略，可以在inspector里手动设置
    private Gradient GetColorGradient()
    {
        var g = new Gradient();
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        var colorKey = new GradientColorKey[4];
        colorKey[0].color = UnityEngine.Color.black;
        colorKey[0].time = 0.1f;
        colorKey[1].color = UnityEngine.Color.white;
        colorKey[1].time = 0.2f;
        colorKey[2].color = UnityEngine.Color.white;
        colorKey[2].time = 0.8f;
        colorKey[3].color = UnityEngine.Color.black;
        colorKey[3].time = 0.9f;
        var alphaKey = new GradientAlphaKey[1];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;

        g.SetKeys(colorKey, alphaKey);
        return g;
    }

    //根据输入点重新分布并平滑曲线点
    List<Vector3> GetCurve(List<Vector3> pathArray, int smooth = 4)
    {
        if (pathArray.Count < 3)
        {
            return pathArray;
        }
        List<Vector3> pathss = new List<Vector3>();
        CatmullRomCurve curve;
        Vector3 pt, p0, p1, p2, p3;

        for (int i = 0; i <= pathArray.Count - 2; i++)
        {
            p1 = pathArray[i];

            p0 = i == 0 ? p1 : pathArray[i - 1];
            p2 = pathArray[i + 1];
            p3 = i == pathArray.Count - 2 ? p2 : pathArray[i + 2];
            curve = new CatmullRomCurve(p0, p1, p2, p3, 0.5f);
            int detail = smooth;

            for (int j = 1; j < detail; j++)
            {
                float t = j / (detail - 1f);
                pt = curve.GetPoint(t);
                pathss.Add(pt);
            }
        }

        return pathss;
    }
    public struct CatmullRomCurve
    {

        public Vector3 p0, p1, p2, p3;
        public float alpha;

        public CatmullRomCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float alpha)
        {
            (this.p0, this.p1, this.p2, this.p3) = (p0, p1, p2, p3);
            this.alpha = alpha;
        }

        // Evaluates a point at the given t-value from 0 to 1
        public Vector3 GetPoint(float t)
        {
            // calculate knots
            const float k0 = 0;
            float k1 = GetKnotInterval(p0, p1);
            float k2 = GetKnotInterval(p1, p2) + k1;
            float k3 = GetKnotInterval(p2, p3) + k2;

            // evaluate the point
            float u = Mathf.LerpUnclamped(k1, k2, t);
            Vector3 A1 = Remap(k0, k1, p0, p1, u);
            Vector3 A2 = Remap(k1, k2, p1, p2, u);
            Vector3 A3 = Remap(k2, k3, p2, p3, u);
            Vector3 B1 = Remap(k0, k2, A1, A2, u);
            Vector3 B2 = Remap(k1, k3, A2, A3, u);
            return Remap(k1, k2, B1, B2, u);
        }

        static Vector3 Remap(float a, float b, Vector3 c, Vector3 d, float u)
        {
            return Vector3.LerpUnclamped(c, d, (u - a) / (b - a));
        }

        float GetKnotInterval(Vector3 a, Vector3 b)
        {
            return Mathf.Pow(Vector3.SqrMagnitude(a - b), 0.5f * alpha);
        }

    }
    #endregion

    #region 23.Volume
    //public void WillUseVolume()
    //{
    //    Volume volume = GetComponent<Volume>();
    //    volume.priority = 1.0f;
    //    volume.enabled = true;
    //    volume.runInEditMode = true;

    //    VolumeProfile volumeProfile = new VolumeProfile();
    //    volume.profile = volumeProfile;
    //    //volumeProfile;
    //    //Projector projector = GetComponent<Projector>();
    //    //SortingGroup sortingGroup = GetComponent<SortingGroup>();

    //    //assetPath 为profile文件保存位置
    //    var newVolume = ScriptableObject.CreateInstance<VolumeProfile>(); // 创建profile对象
    //    AssetDatabase.CreateAsset(newVolume, "");//根据对象保存文件
    //    var uBloom = newVolume.Add<UnityEngine.Rendering.Universal.Bloom>();//给profile添加
    //                                                                        //指定此对象在文件中的显示模式，这里是在inspector和hierarchy都不显示
    //    uBloom.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
    //    uBloom.active = true;
    //    uBloom.name = "Bloom";//起个名字
    //    AssetDatabase.AddObjectToAsset(uBloom, "");//根据路径将其添加到文件里，！！关键点！！

    //    //if (volume.sharedProfile.TryGet<HDRISky>(out var Temp0))
    //    //{
    //    //    Temp0.hdriSky.overrideState = true;//overrideState的值就是Volume的组件属性其前面的勾选状态
    //    //    //Temp0.hdriSky.value = cubemap;

    //    //}

    //    List<VolumeComponent> list = volume.profile.components;
    //    list[0].parameters[2].SetValue(new FloatParameter(0.5f));
    //}

    #endregion

    #region 24.CanvasGroup组件使用
    public CanvasGroup my_ImageCanvasGroup;
    void OnCloseImage(bool status)
    {
        my_ImageCanvasGroup.alpha = status ? 1 : 0;
        //是否允许射线穿过
        my_ImageCanvasGroup.blocksRaycasts = status;
    }
    #endregion

    #region 25.陀螺仪使用
    Quaternion quatMult;

    private void Ssstart()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            this.enabled = false;

        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true,
        Input.gyro.enabled = true;
        Debug.Log($"开启陀螺仪");
        //判断移动设备是否有陀螺仪————亲自测试OPPO R9手机并没有陀螺仪
        if (Input.gyro.enabled)
        {
            Debug.Log($"陀螺仪有陀螺仪");
            //获取设备重力加速度向量
            Vector3 deviceGravity = Input.gyro.gravity;
            //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒）
            Vector3 rotationVelocity = Input.gyro.rotationRate;
            //获取更加精确的旋转
            Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
            //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次
            Input.gyro.updateInterval = 0.1f;
            //获取移除重力加速度后设备的加速度
            Vector3 acceleration = Input.gyro.userAcceleration;
            quatMult = new Quaternion(0, 0, 1, 0);
        }
        else
        {
            //提示用户使用手指滑动查看全景图——Toast.ShowText("请单指滑动查看全景图", 5);
        }
        //如果可以使用陀螺仪
        if (Input.gyro.enabled)
        {
            transform.rotation = Input.gyro.attitude * quatMult;
            Debug.Log($"当前rotation：{transform.rotation}");
        }
    }
    #endregion

    #region 26.UnityEngine.Jobs
    public void WillUseDevice()
    {
        TransformAccess transformAccess = GetComponent<TransformAccess>();
        TransformAccessArray transformAccessArray = GetComponent<TransformAccessArray>();
    }

    //public class JobParallel : IJobParallelForTransformExtensions 
    //{ 

    //}

    public class JobParallelForTransformExtensions : IJobParallelForTransform
    {
        public void Execute(int index, TransformAccess transform)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    #region 27.UnityEngine.ParticleSystemJobs
    public void WillUseParticleSystemJobs()
    {
        ParticleSystemJobData jobData = GetComponent<ParticleSystemJobData>();
        ParticleSystemNativeArray3 particleSystemNativeArray3 = GetComponent<ParticleSystemNativeArray3>();
        ParticleSystemNativeArray4 particleSystemNativeArray4 = GetComponent<ParticleSystemNativeArray4>();

        NativeArray<float> a = jobData.aliveTimePercent;
        particleSystemNativeArray3 = jobData.axisOfRotations;
        int count = jobData.count;
        particleSystemNativeArray4 = jobData.customData1;
        particleSystemNativeArray4 = jobData.customData2;
        a = jobData.inverseStartLifetimes;
        NativeArray<int> b = jobData.meshIndices;
        particleSystemNativeArray3 = jobData.positions;
        NativeArray<uint> c = jobData.randomSeeds;
        particleSystemNativeArray3 = jobData.rotationalSpeeds;
        particleSystemNativeArray3 = jobData.rotations;
        particleSystemNativeArray3 = jobData.sizes;
        NativeArray<Color32> colors = jobData.startColors;
        particleSystemNativeArray3 = jobData.velocities;

    }

    public class JobParticleSystem : IJobParticleSystem
    {
        public void Execute(ParticleSystemJobData jobData)
        {
            throw new NotImplementedException();
        }
    }

    public class JobParticleSystemParallelFor : IJobParticleSystemParallelFor
    {
        public void Execute(ParticleSystemJobData jobData, int index)
        {
            throw new NotImplementedException();
        }
    }

    public class JobParticleSystemParallelForBatch : IJobParticleSystemParallelForBatch
    {
        public void Execute(ParticleSystemJobData jobData, int startIndex, int count)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    #region 28.UnityEngine.Pool
    public List<Vector2> SimplifyLine(Vector2[] points)
    {
        // This version will only be returned to the pool if we call Release on it.
        var simplifiedPoints = CollectionPool<List<Vector2>, Vector2>.Get();

        // Copy the points into a temp list so we can pass them into the Simplify method
        // When the pooled object leaves the scope it will be Disposed and returned to the pool automatically.
        // This version is ideal for working with temporary lists.
        using (CollectionPool<List<Vector2>, Vector2>.Get(out List<Vector2> tempList))
        {
            for (int i = 0; i < points.Length; ++i)
            {
                tempList.Add(points[i]);
            }

            LineUtility.Simplify(tempList, 1.5f, simplifiedPoints);
        }
        return simplifiedPoints;
    }

    void GetPooled()
    {
        // Get a pooled instance
        var instance = UnityEngine.Pool.DictionaryPool<int, int>.Get();

        // Use the Dictionary

        // Return it back to the pool
        UnityEngine.Pool.DictionaryPool<int, int>.Release(instance);
    }

    class MyClass
    {
        public int someValue;
        public string someString;
    }

    void GetPooled2()
    {
        // Get an instance
        var instance = UnityEngine.Pool.GenericPool<MyClass>.Get();

        // Return the instance
        UnityEngine.Pool.GenericPool<MyClass>.Release(instance);
    }

    void GetPooled3()
    {
        // Get a pooled instance
        var instance = UnityEngine.Pool.HashSetPool<int>.Get();

        // Use the HashSet

        // Return it back to the pool
        UnityEngine.Pool.HashSetPool<int>.Release(instance);
    }

    // This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
    [RequireComponent(typeof(ParticleSystem))]
    public class ReturnToPool : MonoBehaviour
    {
        public ParticleSystem system;
        public IObjectPool<ParticleSystem> pool;

        void Start()
        {
            system = GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            // Return to the pool
            pool.Release(system);
        }
    }

    // This example spans a random number of ParticleSystems using a pool so that old systems can be reused.
    public class PoolExample : MonoBehaviour
    {
        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        IObjectPool<ParticleSystem> m_Pool;

        public IObjectPool<ParticleSystem> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new UnityEngine.Pool.ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }

        ParticleSystem CreatePooledItem()
        {
            var go = new GameObject("Pooled Particle System");
            var ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = ps.main;
            main.duration = 1;
            main.startLifetime = 1;
            main.loop = false;

            // This is used to return ParticleSystems to the pool when they have stopped.
            var returnToPool = go.AddComponent<ReturnToPool>();
            returnToPool.pool = Pool;

            return ps;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(ParticleSystem system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(ParticleSystem system)
        {
            Destroy(system.gameObject);
        }

        void OnGUI()
        {
            GUILayout.Label("Pool size: " + Pool.CountInactive);
            if (GUILayout.Button("Create Particles"))
            {
                var amount = UnityEngine.Random.Range(1, 10);
                for (int i = 0; i < amount; ++i)
                {
                    var ps = Pool.Get();
                    ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
                    ps.Play();
                }
            }
        }
    }

    // This example shows how both version of Get could be used to simplify a line of points.
    public class Simplify2DLine
    {
        public List<Vector2> SimplifyLine(Vector2[] points)
        {
            // This version will only be returned to the pool if we call Release on it.
            var simplifiedPoints = UnityEngine.Pool.ListPool<Vector2>.Get();

            // Copy the points into a temp list so we can pass them into the Simplify method
            // When the pooled object leaves the scope it will be Disposed and returned to the pool automatically.
            // This version is ideal for working with temporary lists.
            using (var pooledObject = UnityEngine.Pool.ListPool<Vector2>.Get(out List<Vector2> tempList))
            {
                for (int i = 0; i < points.Length; ++i)
                {
                    tempList.Add(points[i]);
                }

                LineUtility.Simplify(tempList, 1.5f, simplifiedPoints);
            }
            return simplifiedPoints;
        }
    }

    // This component returns the particle system to the pool when the OnParticleSystemStopped event is received.
    [RequireComponent(typeof(ParticleSystem))]
    public class ReturnToPool2 : MonoBehaviour
    {
        public ParticleSystem system;
        public IObjectPool<ParticleSystem> pool;

        void Start()
        {
            system = GetComponent<ParticleSystem>();
            var main = system.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }

        void OnParticleSystemStopped()
        {
            // Return to the pool
            pool.Release(system);
        }
    }

    // This example spans a random number of ParticleSystems using a pool so that old systems can be reused.
    public class PoolExample2 : MonoBehaviour
    {
        public enum PoolType
        {
            Stack,
            LinkedList
        }

        public PoolType poolType;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int maxPoolSize = 10;

        IObjectPool<ParticleSystem> m_Pool;

        public IObjectPool<ParticleSystem> Pool
        {
            get
            {
                if (m_Pool == null)
                {
                    if (poolType == PoolType.Stack)
                        m_Pool = new UnityEngine.Pool.ObjectPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, 10, maxPoolSize);
                    else
                        m_Pool = new LinkedPool<ParticleSystem>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, maxPoolSize);
                }
                return m_Pool;
            }
        }

        ParticleSystem CreatePooledItem()
        {
            var go = new GameObject("Pooled Particle System");
            var ps = go.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = ps.main;
            main.duration = 1;
            main.startLifetime = 1;
            main.loop = false;

            // This is used to return ParticleSystems to the pool when they have stopped.
            var returnToPool = go.AddComponent<ReturnToPool>();
            returnToPool.pool = Pool;

            return ps;
        }

        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(ParticleSystem system)
        {
            system.gameObject.SetActive(false);
        }

        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(ParticleSystem system)
        {
            system.gameObject.SetActive(true);
        }

        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(ParticleSystem system)
        {
            Destroy(system.gameObject);
        }

        void OnGUI()
        {
            GUILayout.Label("Pool size: " + Pool.CountInactive);
            if (GUILayout.Button("Create Particles"))
            {
                var amount = UnityEngine.Random.Range(1, 10);
                for (int i = 0; i < amount; ++i)
                {
                    var ps = Pool.Get();
                    ps.transform.position = UnityEngine.Random.insideUnitSphere * 10;
                    ps.Play();
                }
            }
        }
    }

    public class UnsafeGenericPoolPoolExample
    {
        class MyClass
        {
            public int someValue;
            public string someString;
        }

        void GetPooled()
        {
            // Get an instance
            var instance = UnityEngine.Pool.UnsafeGenericPool<MyClass>.Get();

            // Return the instance
            UnityEngine.Pool.UnsafeGenericPool<MyClass>.Release(instance);
        }
    }
    #endregion

    #region 29.UnityEngine.Sprites
    void WillUseSprites()
    {
        Sprite sprite = Sprite.Create(new Texture2D(512, 512), new Rect(), Vector2.one);
        UnityEngine.Sprites.DataUtility.GetMinSize(sprite);
        UnityEngine.Sprites.DataUtility.GetInnerUV(sprite);
        UnityEngine.Sprites.DataUtility.GetOuterUV(sprite);
        UnityEngine.Sprites.DataUtility.GetPadding(sprite);
        SpriteMaskInteraction spriteMaskInteraction = SpriteMaskInteraction.None;
        SpriteAtlas spriteAtlas = new SpriteAtlas();
        SpriteDrawMode spriteDrawMode = SpriteDrawMode.Sliced;
        SpriteMask spriteMask = new SpriteMask();
        SpriteRenderer spriteRenderer = new SpriteRenderer();
        SpriteBone spriteBone   = new SpriteBone();



    }

    #endregion

    #region 29.Terrain地形相关
    public void WillUseTerrain()
    {
        Terrain terrain = GetComponent<Terrain>();
        TerrainCollider collider = GetComponent<TerrainCollider>();

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = 513;
        terrainData.baseMapResolution = 513;
        terrainData.size = new Vector3(50, 50, 50);
        terrainData.alphamapResolution = 512;
        terrainData.SetDetailResolution(32, 8);

        int width = terrainData.heightmapResolution;
        int height = terrainData.heightmapResolution;

        float[,] heightsBackups = terrainData.GetHeights(0, 0, width, height);

        terrainData.SetHeights(0, 0, heightsBackups);

        GameObject obj = Terrain.CreateTerrainGameObject(terrainData);
        //AssetDatabase.CreateAsset(terrainData, "Assets/Terrain_ModifyHeight.asset");
        //AssetDatabase.SaveAssets();

        BrushTransform brushTransform = new BrushTransform();

        TerrainPaintUtility.BeginPaintHeightmap(new Terrain(), new Rect());

        TerrainBuiltinPaintMaterialPasses terrainBuiltinPaintMaterialPasses = new TerrainBuiltinPaintMaterialPasses();
    }

    public class TerrainInfo : ITerrainInfo
    {
        public Terrain terrain => throw new NotImplementedException();

        public RectInt clippedTerrainPixels => throw new NotImplementedException();

        public RectInt clippedPCPixels => throw new NotImplementedException();

        public RectInt paddedTerrainPixels => throw new NotImplementedException();

        public RectInt paddedPCPixels => throw new NotImplementedException();

        public bool gatherEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool scatterEnable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object userData { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }

    #endregion

    #region 30.TerrainMap
    void WillUseTerrainMap()
    {
        TerrainMap terrainMap = new TerrainMap();

        TerrainTileCoord terrainTileCoord = new TerrainTileCoord();

        TerrainUtility.AutoConnect();
    }

    #endregion

    #region 31.UnityEngine.TextCore

    void WillUseTextCoreLevel()
    {
        FontEngine.DestroyFontEngine();
        GlyphAdjustmentRecord glyphAdjustmentRecord = new GlyphAdjustmentRecord();
        GlyphPairAdjustmentRecord glyphPairAdjustmentRecord = new GlyphPairAdjustmentRecord();
        GlyphValueRecord glyphValueRecord = new GlyphValueRecord();
        FontEngineError fontEngineError = new FontEngineError();
        GlyphLoadFlags glyphLoadFlags = new GlyphLoadFlags();
        GlyphPackingMode glyphPackingMode = new GlyphPackingMode();
        GlyphRenderMode glyphRenderMode = new GlyphRenderMode();
        FaceInfo faceInfo = new FaceInfo();
        Glyph glyph = new Glyph();
        GlyphMetrics glyphMetrics = new GlyphMetrics();
        GlyphRect glyphRect = new GlyphRect();
    }

    #endregion

    #region 31.UnityEngine.VFX
    void WillUseVFX()
    {
        VFXCameraXRSettings vFXCameraXRSettings = new VFXCameraXRSettings();
        VFXExposedProperty property = new VFXExposedProperty();

    }

    class ConstantRateEquivalent : VFXSpawnerCallbacks
    {
        public class InputProperties
        {
            [Min(0), Tooltip("Sets the number of particles to spawn per second.")]
            public float Rate = 10;
        }

        static private readonly int rateID = Shader.PropertyToID("Rate");

        public sealed override void OnPlay(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
        {
        }

        public sealed override void OnUpdate(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
        {
            if (state.playing)
            {
                float currentRate = vfxValues.GetFloat(rateID);
                state.spawnCount += currentRate * state.deltaTime;
            }
        }

        public sealed override void OnStop(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
        {
        }
    }
    #endregion

    #region 32.字节转换工具
    public byte[] GetBytes(IList<byte[]> data)
    {
        if (data == null || data.Count == 0) return null;
        int length = 0;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] == null)
            {
                length += 2;
            }
            else
            {
                length += (data[i].Length + 2);
            }

        }
        byte[] concat = new byte[length];
        int index = 0;
        byte[] len = null;
        for (int i = 0; i < data.Count; i++)
        {
            if (data[i] == null)
            {
                concat[index++] = 0;
                concat[index++] = 0;
            }
            else
            {
                len = BitConverter.GetBytes((ushort)data[i].Length);
                concat[index++] = len[0];
                concat[index++] = len[1];
                for (int j = 0; j < data[i].Length; j++)
                {
                    concat[index++] = data[i][j];
                }
            }
        }
        return concat;
    }
    #endregion

    #region 33.File Info创建文件夹
    public void CreateFile()
    {
        System.IO.FileInfo file = new System.IO.FileInfo("/app");
        file.Directory.Create();
    }

    #endregion

    #region 34.RenderFeature
    public void WillUseRendererUtils()
    {
        UnityEngine.Rendering.RendererUtils.RendererList rendererList = UnityEngine.Rendering.RendererUtils.RendererList.nullRendererList;
        bool a = rendererList.isValid;

        UnityEngine.Rendering.RendererUtils.RendererListDesc rendererListDesc = new UnityEngine.Rendering.RendererUtils.RendererListDesc();
        rendererListDesc.excludeObjectMotionVectors = false;
        rendererListDesc.layerMask = 0;
        Material mat = rendererListDesc.overrideMaterial;
        int index = rendererListDesc.overrideMaterialPassIndex;
        rendererListDesc.rendererConfiguration = PerObjectData.ReflectionProbeData;
        rendererListDesc.renderQueueRange = UnityEngine.Rendering.RenderQueueRange.opaque;
        rendererListDesc.sortingCriteria = 0;
        rendererListDesc.stateBlock = new RenderStateBlock();

        RendererListStatus rendererListStatus = new RendererListStatus();
        rendererListStatus = RendererListStatus.kRendererListPopulated;
        rendererListStatus = RendererListStatus.kRendererListEmpty;
        rendererListStatus = RendererListStatus.kRendererListInvalid;
        rendererListStatus = RendererListStatus.kRendererListProcessing;

        //Todo
        //UnityEngine.Rendering.VirtualTexturing


    }

    public void WillUseRenderFeature()
    {
        UniversalRendererData URPData;
        UniversalRendererData URPData1;
        List<ScriptableRendererFeature> rendererFeatures;
        List<ScriptableRendererFeature> rendererFeatures1;

        FieldInfo propertyInfo = QualitySettings.renderPipeline.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);

        URPData = (UniversalRendererData)(((ScriptableRendererData[])propertyInfo?.GetValue(QualitySettings.renderPipeline))?[0]);
        URPData1 = (UniversalRendererData)(((ScriptableRendererData[])propertyInfo?.GetValue(QualitySettings.renderPipeline))?[1]);

        Debug.Log("URPDATA:" + URPData);
        Debug.Log("URPDATA:" + URPData1);

        rendererFeatures = URPData.rendererFeatures;
        rendererFeatures1 = URPData1.rendererFeatures;

        Debug.Log("ScriptableRendererFeature:" + rendererFeatures);
        Debug.Log("ScriptableRendererFeature:" + rendererFeatures1);

        //PandaGrab pandaGrab = new PandaGrab();
        //pandaGrab.name = "PandaGrab";
        //Debug.Log("PandaGrab:" + pandaGrab);
        //rendererFeatures.Add(pandaGrab);
        //rendererFeatures1.Add(pandaGrab);

        RenderObjects renderObjects = ScriptableObject.CreateInstance<RenderObjects>();
        renderObjects.name = "RenderObjects";
        renderObjects.settings.Event = RenderPassEvent.AfterRenderingTransparents;
        renderObjects.settings.filterSettings.RenderQueueType = RenderQueueType.Transparent;
        renderObjects.settings.filterSettings.LayerMask = -1;
        renderObjects.settings.filterSettings.PassNames = new string[] { "PandaPass" };

        Debug.Log("RenderObjects:" + renderObjects);
        rendererFeatures.Add(renderObjects);
        rendererFeatures1.Add(renderObjects);

        URPData.SetDirty();
        URPData1.SetDirty();
    }

    [Preserve]
    public class PandaGrabCopy : ScriptableRendererFeature
    {
        class CustomRenderPassCopy : ScriptableRenderPass
        {

            static string rt_name = "_PandaGrabTex";
            static int rt_ID = Shader.PropertyToID(rt_name);
            // This method is called before executing the render pass.
            // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
            // When empty this render pass will render to the active camera render target.
            // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
            // The render pipeline will ensure target setup and clearing happens in a performant manner.
            public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
            {
                Debug.Log("OnCameraSetup");
                RenderTextureDescriptor descriptor = new RenderTextureDescriptor(2560, 1440, RenderTextureFormat.DefaultHDR, 0);
                cmd.GetTemporaryRT(rt_ID, descriptor);
                ConfigureTarget(rt_ID);
                ConfigureClear(ClearFlag.Color, Color.black);


            }

            // Here you can implement the rendering logic.
            // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
            // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
            // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
            public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
            {
                Debug.Log("Execute");
                CommandBuffer cmd = CommandBufferPool.Get("PandaPass");
                cmd.Blit(renderingData.cameraData.renderer.cameraColorTarget, rt_ID);
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
                cmd.Release();
            }

            // Cleanup any allocated resources that were created during the execution of this render pass.
            public override void OnCameraCleanup(CommandBuffer cmd)
            {
                Debug.Log("OnCameraCleanup");
                cmd.ReleaseTemporaryRT(rt_ID);
            }
        }

        CustomRenderPassCopy m_ScriptablePass;

        /// <inheritdoc/>
        public override void Create()
        {
            Debug.Log("Create");
            m_ScriptablePass = new CustomRenderPassCopy();

            // Configures where the render pass should be injected.
            m_ScriptablePass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }

        // Here you can inject one or multiple render passes in the renderer.
        // This method is called when setting up the renderer once per-camera.
        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            Debug.Log("AddRenderPasses");
            renderer.EnqueuePass(m_ScriptablePass);
        }
    }
    #endregion

    #region 35.AinimatorStateInfo
    public void GetAnimatorStateInfo()
    {
        var animator = GetComponent<Animator>();
        var AnimState = animator.GetCurrentAnimatorStateInfo(0);
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        AnimationState animationState = new AnimationState();
        foreach (AnimationState state in GetComponent<Animation>())
        {
            state.time = UnityEngine.Random.value * state.length;
        }
    }
    #endregion

    #region 36.UnityEngine.InputLegacyModule
    /// <summary>
    /// 启动成功回调
    /// </summary>
    public event UnityAction<LocationInfo> SuccessCallback;
    public void Test111()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            LocationInfo locationInfo = Input.location.lastData;
        }
    }
    #endregion

    #region 37.UnityEngine.RectTransformUtility
    public void RectTransform()
    {
        RectTransformUtility.RectangleContainsScreenPoint(transform as RectTransform, Vector2.zero);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Vector2.zero, Camera.main, out Vector2 pos);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, Vector2.zero, Camera.main, out Vector3 worldPoint);
    }
    #endregion

    #region 38.System.Convert
    void TestConvert()
    {
        Int32 test32 = 1;
        Convert.ToBase64String(null);
        Convert.ToByte(test32);
        Convert.ToChar(test32);
        Convert.ToString("1");
        Convert.ToBase64CharArray(null, 0, 0, null, 0);
        Convert.ToSingle(test32);
        Convert.ToDouble(test32);
        Convert.ToUInt16(test32);
        Convert.ToUInt32(test32);
        Convert.ToUInt64(test32);
        Convert.ToBoolean(true);
        TypeCode c = new TypeCode();
        IConvertible a = null;
        IFormatProvider b = null;
        NumberStyles d = new NumberStyles();

    }
    #endregion

    #region 39.后处理 renderFeature
    private void SetRenderFeature()
    {
        var pipeline = UnityEngine.Rendering.GraphicsSettings.defaultRenderPipeline;
        var propertyInfo = pipeline?.GetType().GetField("m_RendererDataList", BindingFlags.Instance | BindingFlags.NonPublic);
        ScriptableRendererData scriptableRendererData = ((ScriptableRendererData[])propertyInfo?.GetValue(pipeline))?[0];
    }
    #endregion

    # region 40.GPU Instancing
    private void GraphicsForDrawMeshInstanced()
    {
        var mb = new MaterialPropertyBlock();
        Graphics.DrawMeshInstanced(new Mesh(), 0, new Material(shader), new Matrix4x4[] { Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale) }, 512, mb);
    }
    #endregion

    #region 41.UnityEngine.PlayerPrefs
    private void PlayerPref()
    {
        PlayerPrefs.SetInt("LeiTaiPass", 1);
        PlayerPrefs.GetInt("LeiTaiPass");
        Handheld.Vibrate();
    }
    #endregion
    #region 42 IEnumerator

    public IEnumerator GetEnumerator()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSecondsRealtime(0.1f);
    }

    public IEnumerator GetEnumerator(string xx)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(0.1f);
        yield return new WaitForSecondsRealtime(0.1f);
    }

    #endregion
    #region 43UnityScene
    public void SetScreen()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
    #endregion
    #region 44AssetBundle
    public void AssetBundleMethod()
    {
        AssetBundle.LoadFromFileAsync(null);
    } 
    #endregion
    #region Other

    public void SetRigdbody()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        RigidbodyConstraints rigidbodyConstraints = rigidbody.constraints;
        RigidbodyConstraints2D rigidbodyConstraints2D = rigidbody2D.constraints;
        RigidbodyType2D rigidbodyType2D = rigidbody2D.bodyType;
        CharacterController characterController = GetComponent<CharacterController>();
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.tag = "";
        BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
        SphereCollider Spher = GetComponent<SphereCollider>();
        Spher.tag = "";
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.tag = "";
        CapsuleCollider2D capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        capsuleCollider2D.tag = "";
        UnityEngine.PolygonCollider2D polygonCollider2D = GetComponent<UnityEngine.PolygonCollider2D>();
        polygonCollider2D.tag = "";
        HingeJoint2D hingeJoint2D = GetComponent<HingeJoint2D>();
        HingeJoint hingeJoint1 = GetComponent<HingeJoint>();
        CircleCollider2D circleCollider2D = GetComponent<CircleCollider2D>();
        EdgeCollider2D edgeCollider2D = GetComponent<EdgeCollider2D>();
        CompositeCollider2D compositeCollider2D = GetComponent<CompositeCollider2D>();
        PhysicMaterial physicMaterial = GetComponent<PhysicMaterial>();
        Collider collider = GetComponent<Collider>();
        Collider2D collider2D = GetComponent<Collider2D>();
        Collision collision = GetComponent<Collision>();
        ContactFilter2D contactFilter2D = GetComponent<ContactFilter2D>();
        ContactPoint contactPoint = GetComponent<ContactPoint>();
        ContactPoint2D contactPoint2D = GetComponent<ContactPoint2D>();
        ColliderData colliderData = GetComponent<ColliderData>();
        CollisionModule collisionModule = GetComponent<CollisionModule>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.tag = "";
        WheelCollider wheelCollider = GetComponent<WheelCollider>();
        wheelCollider.tag = "";
        Cloth cloth = GetComponent<Cloth>();
        cloth.tag = "";
        ArticulationBody articulationBody = GetComponent<ArticulationBody>();
        articulationBody.useGravity = false;
        HingeJoint hingeJoint = GetComponent<HingeJoint>();
        hingeJoint.useLimits = true;
        FixedJoint fixedJoint = GetComponent<FixedJoint>();
        int cc = fixedJoint.GetInstanceID();
        SpringJoint springJoint = GetComponent<SpringJoint>();
        cc = springJoint.GetInstanceID();
        ConfigurableJoint configurableJoint = GetComponent<ConfigurableJoint>();
        float mm = configurableJoint.highAngularXLimit.bounciness;
        ConstantForce constantForce = GetComponent<ConstantForce>();
        bool vb = constantForce.isActiveAndEnabled;
        ConstantForce2D constantForce2D = GetComponent<ConstantForce2D>();
        Vector2 vector = constantForce2D.relativeForce;
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        // 设置线速度
        rigidbody.velocity = transform.forward * (vertical * 1);
        // 设置角速度
        rigidbody.angularVelocity = transform.up * (horizontal * 0.2f);
        characterController.Move(Vector3.forward);
        characterController.radius = 0.5f;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit != null && hit.gameObject.name == "Cube")
        {
            hit.rigidbody.AddForce(hit.point * 500);
        }
    }

    public void AddPoint(Vector3 position)
    {
        LineRenderer xx = GetComponent<LineRenderer>();

        xx.positionCount = 10;

        // index 0 positionCount must be - 1
        xx.SetPosition(1, position);

        // applies simplification if reminder is 0
        if (xx.positionCount % 2 == 0)
        {
            xx.Simplify(15.0f);
        }
    }

    public void AddNewLineRenderer(Transform parent, Vector3 position)
    {
        Material material = GetComponent<Material>();
        GameObject go = new GameObject($"LineRenderer");

        go.transform.position = position;

        LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();
        goLineRenderer.startWidth = 1.0f;
        goLineRenderer.endWidth = 1.0f;
        goLineRenderer.startColor = UnityEngine.Color.black;
        goLineRenderer.endColor = Color.white;
        goLineRenderer.material = material;

        goLineRenderer.useWorldSpace = true;
        goLineRenderer.positionCount = 10;

        goLineRenderer.numCornerVertices = 1;
        goLineRenderer.numCapVertices = 10;



        goLineRenderer.SetPosition(0, position);
        goLineRenderer.SetPosition(1, position);

        GameObject tempLineObj = new GameObject("Minimap Route (" + this.gameObject.transform.name + ")");
        tempLineObj.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        LineRenderer tempLine = tempLineObj.AddComponent<LineRenderer>();
        tempLineObj.layer = LayerMask.NameToLayer("UI");
        tempLine.SetPositions(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });
        tempLine.alignment = LineAlignment.TransformZ;
        tempLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        tempLine.receiveShadows = false;
        tempLine.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        tempLine.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        tempLine.numCapVertices = 90;
        tempLine.numCornerVertices = 90;



        int positionCount = 2;
        goLineRenderer.startWidth = 1.0f;
        goLineRenderer.endWidth = 1.0f;
        goLineRenderer.startColor = UnityEngine.Color.black;
        goLineRenderer.endColor = UnityEngine.Color.white;
        goLineRenderer.material = null;

        goLineRenderer.useWorldSpace = true;
        goLineRenderer.positionCount = positionCount;

        goLineRenderer.numCornerVertices = 1;
        goLineRenderer.numCapVertices = 1;



        goLineRenderer.colorGradient = new Gradient();



        goLineRenderer.SetPosition(0, position);
        goLineRenderer.SetPosition(1, position);
    }

    public UnityEngine.Event onDragEnd;
    public UnityEngine.Events.UnityEventBase onDragEnter;

    //手指触碰
    private void WiellSZ()
    {
        int tapCount = Input.touchCount > 1 ? Input.touchCount : 1;

        for (int i = 0; i < tapCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector3 touchPosition = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 10));

            if (touch.phase == TouchPhase.Began)
            {

            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                int ss = touch.fingerId;
            }
        }
    }

}
class VEFMy : VFXSpawnerCallbacks
{
    public override void OnPlay(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
    {
        throw new NotImplementedException();
    }

    public override void OnStop(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
    {
        throw new NotImplementedException();
    }

    public override void OnUpdate(VFXSpawnerState state, VFXExpressionValues vfxValues, VisualEffect vfxComponent)
    {
        throw new NotImplementedException();
    }
}
class MyFEX : VisualEffectObject
{

}

//事件系统
namespace ARMP.GuideScenic
{
    ///事件的处理中心
    ///处理不同事件的参数的监听
    ///不同参数的移除监听
    ///广播
    using System;
    using System.Collections.Generic;


    /// <summary>
    /// 不需要继承MonoBehaviour
    /// </summary>
    public class BoardCastModule
    {
        //定义一个字典 来存放事件码
        private static Dictionary<int, Delegate> m_EventTable = new Dictionary<int, Delegate>();
        //*************************************************************************************************************************************
        /// <summary>
        /// 判断事件监听是不是有错误
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="callBack">委托</param>
        /// 主要是为了精简代码
        private static void OnListenerAdding(int eventType, Delegate callBack)
        {
            //先判断事件码是否存在于事件表中
            //如果不存在
            if (!m_EventTable.ContainsKey(eventType))
            {
                //先给字典添加事件码,委托设置为空
                m_EventTable.Add(eventType, null);
            }

            //当前事件码和委托是否一致
            //如果不一致,是不能绑定在一起的
            //先把事件码传进去,接收值是 Delegate
            //这句代码是先把事件码拿出来
            Delegate d = m_EventTable[eventType];
            //d为空或d 的参数如果和callBack参数不一样
            if (d != null && d.GetType() != callBack.GetType())
            {
                //抛出异常
                throw new Exception(string.Format("尝试为事件{0}添加不同事件的委托,当前事件所对应的委托是{1},要添加的委托类型{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }

        /// <summary>
        /// 移除监听时做的判断，主要为精简代码
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="callBack">委托</param>
        private static void OnListenerRemoving(int eventType, Delegate callBack)
        {
            //判断是否包含指定键
            if (m_EventTable.ContainsKey(eventType))
            {
                //先把事件码拿出来
                Delegate d = m_EventTable[eventType];
                if (d == null)
                {
                    throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
                }
                else if (d.GetType() != callBack.GetType())//判断移除的委托类型是否和d的一致
                {
                    throw new Exception(string.Format("移除监听错误，尝试为事件{}移除不同类型的委托，当前委托类型为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
                }
            }
            else  //不存在事件码的情况
            {
                throw new Exception(string.Format("移除监听错误;没有事件码", eventType));
            }
        }

        /// <summary>
        /// 移除监听后的判断，主要为精简代码
        /// </summary>
        /// <param name="eventType"></param>
        private static void OnListenerRemoved(int eventType)
        {
            //判断当前的事件码所对应的事件是否为空
            //如果为空，事件码就没用了，就将事件码移除
            if (m_EventTable[eventType] == null)
            {
                //移除事件码
                m_EventTable.Remove(eventType);
            }
        }


        //*********************************************************************************************************************************
        /// <summary>
        /// 添加监听 静态 无参
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="callBack">委托</param>
        public static void AddListener(int eventType, CallBack callBack)
        {
            //调用事件监听是不是有错误方法
            OnListenerAdding(eventType, callBack);

            //已经存在的委托进行关联,相当于链式关系,再重新赋值
            //两个类型不一致,要强转换
            //委托对象可使用 "+" 运算符进行合并。
            //一个合并委托调用它所合并的两个委托。只有相同类型的委托可被合并。"-" 运算符可用于从合并的委托中移除组件委托。
            //使用委托的这个有用的特点，您可以创建一个委托被调用时要调用的方法的调用列表。这被称为委托的 多播（multicasting），也叫组播。
            //下面的程序演示了委托的多播
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 添加监听 静态 一个参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventType"></param>
        /// <param name="callBack"></param>
        /// 因为有一个参数，方法后要加一个泛型“<T>”,大写的T来代表
        /// CallBack 也是一个泛型，方法是有参数的，所以CallBack也是有参数的
        /// 除此之外其它与无参方法基本一致
        /// 泛函数 T 可以指定为任意的类型，多参数也是
        public static void AddListener<T>(int eventType, CallBack<T> callBack)
        {
            //调用事件监听是不是有错误方法
            OnListenerAdding(eventType, callBack);

            //这里是有参方法需要更改的地方
            //强制转换类型要加一个泛型 "<T>"
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] + callBack;
        }



        /// <summary>
        /// 添加监听 静态 两个参数
        /// </summary>
        public static void AddListener<T, X>(int eventType, CallBack<T, X> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 添加监听 静态 三个参数
        /// </summary>
        public static void AddListener<T, X, Y>(int eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 添加监听 静态 四个参数
        /// </summary>
        public static void AddListener<T, X, Y, Z>(int eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] + callBack;
        }

        /// <summary>
        /// 添加监听 静态 五个参数
        /// </summary>
        public static void AddListener<T, X, Y, Z, W>(int eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerAdding(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] + callBack;
        }


        //*******************************************************************************************************************************
        /// <summary>
        /// 移除监听 静态 无参
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="callBack">委托</param>
        public static void RemoveListener(int eventType, CallBack callBack)
        {
            //移除监听前的判断
            OnListenerRemoving(eventType, callBack);

            //这句话是主要的
            //事件码对应的委托-callBack 然后再重新赋值，强转型首字母要大写
            //移除监听
            m_EventTable[eventType] = (CallBack)m_EventTable[eventType] - callBack;

            //移除监听后的判断
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 移除监听 静态 一个参数
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="callBack">委托</param>
        public static void RemoveListener<T>(int eventType, CallBack<T> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            //这里是有参方法需要更改的地方
            //强制转换类型要加一个泛型 "<T>"
            m_EventTable[eventType] = (CallBack<T>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 移除监听 静态 两个参数
        /// </summary>
        public static void RemoveListener<T, X>(int eventType, CallBack<T, X> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 移除监听 静态 三个参数
        /// </summary>
        public static void RemoveListener<T, X, Y>(int eventType, CallBack<T, X, Y> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }


        /// <summary>
        /// 移除监听 静态 四个参数
        /// </summary>
        public static void RemoveListener<T, X, Y, Z>(int eventType, CallBack<T, X, Y, Z> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        /// <summary>
        /// 移除监听 静态 五个参数
        /// </summary>
        public static void RemoveListener<T, X, Y, Z, W>(int eventType, CallBack<T, X, Y, Z, W> callBack)
        {
            OnListenerRemoving(eventType, callBack);
            m_EventTable[eventType] = (CallBack<T, X, Y, Z, W>)m_EventTable[eventType] - callBack;
            OnListenerRemoved(eventType);
        }

        //******************************************************************************************************************************
        /// <summary>
        /// 广播监听 静态 无参
        /// </summary>
        /// <param name="eventType">事件码</param>
        /// <param name="once">是否只执行一次</param>
        /// 把事件码所对应的委托从m_EventTable 字典表中取出来，然后调用这个委托
        public static void Broadcast(int eventType, bool once = false)
        {
            Delegate d;
            //如果拿到这个值成功了，对这个委托进行一个广播
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                //把d强转型CallBack类型
                CallBack callBack = d as CallBack;
                if (callBack != null)
                {
                    callBack();
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }


        /// <summary> 
        /// 广播监听 静态 一个参
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventType"></param>
        /// <param name="arg"></param>
        /// <param name="once">是否只执行一次</param>
        /// 以为有参数，所以在方法后面加一个参数 T arg
        public static void Broadcast<T>(int eventType, T arg, bool once = false)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                //带有一个参数的委托
                CallBack<T> callBack = d as CallBack<T>;
                if (callBack != null)
                {
                    //把参数传过去
                    callBack(arg);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 广播 静态 两个参数
        /// <param name="once">是否只执行一次</param>
        /// </summary>
        public static void Broadcast<T, X>(int eventType, T arg1, X arg2, bool once = false)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X> callBack = d as CallBack<T, X>;
                if (callBack != null)
                {
                    callBack(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 广播 静态 三个参数
        /// <param name="once">是否只执行一次</param>
        /// </summary>
        public static void Broadcast<T, X, Y>(int eventType, T arg1, X arg2, Y arg3, bool once = false)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y> callBack = d as CallBack<T, X, Y>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 广播 静态 四个参数
        /// <param name="once">是否只执行一次</param>
        /// 
        /// </summary>
        public static void Broadcast<T, X, Y, Z>(int eventType, T arg1, X arg2, Y arg3, Z arg4, bool once = false)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z> callBack = d as CallBack<T, X, Y, Z>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }

        /// <summary>
        /// 广播 静态 五个参数
        /// <param name="once">是否只执行一次</param>
        /// </summary>
        public static void Broadcast<T, X, Y, Z, W>(int eventType, T arg1, X arg2, Y arg3, Z arg4, W arg5, bool once = false)
        {
            Delegate d;
            if (m_EventTable.TryGetValue(eventType, out d))
            {
                CallBack<T, X, Y, Z, W> callBack = d as CallBack<T, X, Y, Z, W>;
                if (callBack != null)
                {
                    callBack(arg1, arg2, arg3, arg4, arg5);
                }
                else
                {
                    throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同类型", eventType));
                }
                if (once)
                {
                    callBack = null;
                    m_EventTable.Remove(eventType);
                }
            }
        }
        /// <summary>
        /// 清理所有注册的事件
        /// </summary>
        public void Clear()
        {

            m_EventTable.Clear();
        }

        #endregion
    }
}
#region 委托
///委托类型定义
///
namespace ARMP.GuideScenic
{
    ///封装里了系统所使用到的委托
    ///定义委托的类
    ///定义多少参数都是可以的
    ///定义了多少委托，EventCenter添加多少委托，过多添加会报错

    /// <summary>
    /// 无参委托
    /// </summary>
    public delegate void CallBack();
    /// <summary>
    /// 有参委托,一个参数,需要指定一个泛型，（ 泛型类型 变量名 ）
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="arg">变量名</param>
    public delegate void CallBack<T>(T arg);

    /// <summary>
    /// 多个参数
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <typeparam name="X">泛型类型</typeparam>
    /// <param name="arg1">变量名</param>
    /// <param name="arg2">变量名</param>
    /// 一般只用到5个，如果还需要可以往后加
    public delegate void CallBack<T, X>(T arg1, X arg2);

    public delegate void CallBack<T, X, Y>(T arg1, X arg2, Y arg3);
    public delegate void CallBack<T, X, Y, Z>(T arg1, X arg2, Y arg3, Z arg4);
    public delegate void CallBack<T, X, Y, Z, W>(T arg1, X arg2, Y arg3, Z arg4, W arg5);
} 
#endregion



