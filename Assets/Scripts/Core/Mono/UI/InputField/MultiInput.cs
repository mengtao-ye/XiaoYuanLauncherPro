using UnityEngine;
using UnityEngine.UI;
using YFramework;


/// <summary>
///多功能输入类型
/// </summary>
public enum MultiInputType
{
    Int,
    Float
}
/// <summary>
/// 多功能输入
/// </summary>
public class MultiInput
{
    public float value { get; private set; }
    private InputField mInput;
    private Slider mSlider;
    private Transform mTrans;
    private GameObject mGameObject;
    private MultiInputType mType;
    private float mMin;
    private float mMax;
    private float mPreInputData = float.MinValue;//上一个输入的参数
    public MultiInput(GameObject target, MultiInputType type, float minValue, float maxValue)
    {
        if (target == null) return;
        mGameObject = target;
        mTrans = target.transform;
        mType = type;
        mMin = minValue;
        mMax = maxValue;
        Awake();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Awake()
    {
        mInput = mTrans.FindObject<InputField>("InputField");
        mSlider = mTrans.FindObject<Slider>("Slider");
        mSlider.minValue = mMin;
        mSlider.maxValue = mMax;
        switch (mType)
        {
            case MultiInputType.Int:
                mInput.contentType = InputField.ContentType.IntegerNumber;
                mSlider.wholeNumbers = true;
                break;
            case MultiInputType.Float:
                mInput.contentType = InputField.ContentType.DecimalNumber;
                mSlider.wholeNumbers = false;
                break;
        }
        mInput.onValueChanged.AddListener((value) =>
        {
            float data = 0;
            if (float.TryParse(value, out data))
            {
                if (mType == MultiInputType.Int)
                {
                    SetValueInputField(Mathf.Clamp((int)data, mMin, mMax));
                }
                else
                {
                    SetValueInputField(Mathf.Clamp(data, mMin, mMax));
                }
            }
            else //输入格式错误时
            { 
            
            }
        });
        mSlider.onValueChanged.AddListener((value) =>
        {
            if (mType == MultiInputType.Int)
            {
                SetValueSlider(Mathf.Clamp((int)value, mMin, mMax));
            }
            else
            {
                SetValueInputField(Mathf.Clamp(value, mMin, mMax));
            }
            
        });
        mTrans.FindObject<Button>("Add").onClick.AddListener(()=> {
            value = Mathf.Clamp(value +1, mMin, mMax);
            SetValueInputField(value);
        });
        mTrans.FindObject<Button>("Desc").onClick.AddListener(() => {
            value = Mathf.Clamp(value - 1, mMin, mMax);
            SetValueInputField(value);
        });
        SetValueInputField(mMin);
    }
    /// <summary>
    /// 设置基础值
    /// </summary>
    /// <param name="value"></param>
    private void SetTargetValue(float value)
    {
        this.value = value;
    }

    /// <summary>
    /// 设置Silder跟InputField
    /// </summary>
    /// <param name="value"></param>
    private void SetValueInputField(float value)
    {
        SetTargetValue(value);
        mSlider.value = value;
        mInput.text = value.ToString();
    }

    /// <summary>
    /// 滑动条设置参数
    /// </summary>
    private void SetValueSlider(float value) {
        SetTargetValue(value);
        mInput.text = value.ToString();
    }
}
