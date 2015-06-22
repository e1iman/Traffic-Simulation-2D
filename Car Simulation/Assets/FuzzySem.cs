using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Text;

using FuzzyFramework;
using FuzzyFramework.Dimensions;
using FuzzyFramework.Sets;
using FuzzyFramework.Defuzzification;
using UnityEngine.UI;

public class FuzzySem : MonoBehaviour
{
    [Range(0, 1200)]
    public float inpVol;
    [Range(0, 30)]
    public float inpQue;
    decimal _inputVolume;
    decimal _inputQueue;

    public float TMPANSW;

    public double ans;

    public static FuzzySem fs;
    [SerializeField]
    GameObject haha;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("FUZZY TECHING " + Time.time);
            _inputVolume = (decimal)inpVol;
            _inputQueue = (decimal)inpQue;
            TMPANSW = Calc(inpVol, inpQue);
        }
    }

    #region Definitions
    //Definition of dimensions on which we will measure the input values
    ContinuousDimension volume;
    ContinuousDimension queue;


    //Definition of dimension for output value
    ContinuousDimension greenLight;

    //Definition of basic fuzzy sets with which we will work
    //  input sets:
    FuzzySet Volume_small;
    FuzzySet Volume_middle;
    FuzzySet Volume_large;
    FuzzySet Volume_extraLarge;

    FuzzySet Queue_small;
    FuzzySet Queue_middle;
    FuzzySet Queue_large;
    FuzzySet Queue_extraLarge;

    //  output set:
    FuzzySet Time_veryShort;
    FuzzySet Time_short;
    FuzzySet Time_middle;
    FuzzySet Time_long;
    FuzzySet Time_veryLong;

    //Definition of antedescent
    FuzzyRelation relation;
    #endregion

    void Start()
    {
        if (fs == null)
        {
            fs = this;
        }
        #region Definitions
        //Definition of dimensions on which we will measure the input values
        volume = new ContinuousDimension("Volume", "Vehicle per hour", "veh/hr", 0, 1200);
        queue = new ContinuousDimension("Weight", "Vehicle lenth on proper direction", "veh", 0, 30);


        //Definition of dimension for output value
        greenLight = new ContinuousDimension("Green Time", "green phase duration of Traffic Light", "s", 5, 60);

        //Definition of basic fuzzy sets with which we will work
        //  input sets:
        Volume_small = new RightLinearSet(volume, "Small", 100, 300);
        Volume_middle = new TrapezoidalSet(volume, "Middle", 300, 400, 100, 600);
        Volume_large = new TrapezoidalSet(volume, "Large", 600, 700, 400, 900);
        Volume_extraLarge = new LeftLinearSet(volume, "Extra Large", 700, 900);

        Queue_small = new RightLinearSet(queue, "Small", 0, 7);
        Queue_middle = new TriangularSet(queue, "Middle", 10, 14);
        Queue_large = new TriangularSet(queue, "Large", 17, 14);
        Queue_extraLarge = new LeftLinearSet(queue, "Extra Large", 17, 23);

        //  output set:
        Time_veryShort = new RightLinearSet(greenLight, "veryshort", 7, 15);
        Time_short = new TrapezoidalSet(greenLight, "short", 15, 23, 7, 30);
        Time_middle = new TrapezoidalSet(greenLight, "middle", 30, 38, 23, 45);
        Time_long = new TrapezoidalSet(greenLight, "long", 45, 53, 38, 60);
        Time_veryLong = new LeftLinearSet(greenLight, "verylong", 53, 60);

        //Definition of antedescent
        //relation =
        //    ((Volume_small & Queue_small) & Time_veryShort) |
        //    ((Volume_small & Queue_middle) & Time_short) |
        //    ((Volume_small & Queue_large) & Time_short) |
        //    ((Volume_small & Queue_extraLarge) & Time_middle) |
        //    ((Volume_middle & Queue_small) & Time_short) |
        //    ((Volume_middle & Queue_middle) & Time_middle) |
        //    ((Volume_middle & Queue_large) & Time_middle) |
        //    ((Volume_middle & Queue_extraLarge) & Time_long) |
        //    ((Volume_large & Queue_small) & Time_middle) |
        //    ((Volume_large & Queue_middle) & Time_long) |
        //    ((Volume_large & Queue_large) & Time_long) |
        //    ((Volume_large & Queue_extraLarge) & Time_veryLong) |
        //    ((Volume_extraLarge & Queue_small) & Time_middle) |
        //    ((Volume_extraLarge & Queue_middle) & Time_long) |
        //    ((Volume_extraLarge & Queue_large) & Time_veryLong) |
        //    ((Volume_extraLarge & Queue_extraLarge) & Time_veryLong);
        relation =
    ((Volume_small & Queue_small) & Time_veryShort) |
    ((Volume_small & Queue_middle) & Time_short) |
    ((Volume_small & Queue_large) & Time_short) |
    ((Volume_small & Queue_extraLarge) & Time_middle) |
    ((Volume_middle & Queue_small) & Time_veryShort) |
    ((Volume_middle & Queue_middle) & Time_short) |
    ((Volume_middle & Queue_large) & Time_middle) |
    ((Volume_middle & Queue_extraLarge) & Time_long) |
    ((Volume_large & Queue_small) & Time_short) |
    ((Volume_large & Queue_middle) & Time_middle) |
    ((Volume_large & Queue_large) & Time_long) |
    ((Volume_large & Queue_extraLarge) & Time_veryLong) |
    ((Volume_extraLarge & Queue_small) & Time_middle) |
    ((Volume_extraLarge & Queue_middle) & Time_long) |
    ((Volume_extraLarge & Queue_large) & Time_veryLong) |
    ((Volume_extraLarge & Queue_extraLarge) & Time_veryLong);
        #endregion

        
        //for (int iv = 0; iv < 110; iv++)
        //{
        //    for (int iq = 0; iq < 25; iq++)
        //    {

        //        Instantiate(haha, new Vector3((float)iv * 0.25f, Calc((float)(iv * 10), (float)(iq)) * 0.5f, (float)iq), Quaternion.identity);
        //        Debug.Log("=!=");
        //    }
        //}
        //Debug.Log("=)");

    }

    public float Calc(float inputVolume, float inputQueue)
    {
        inputVolume = Mathf.Clamp(inputVolume, 0, 1200);
        inputQueue = Mathf.Clamp(inputQueue, 0, 30);
        //#region Definitions
        ////Definition of dimensions on which we will measure the input values
        //ContinuousDimension volume = new ContinuousDimension("Volume", "Vehicle per hour", "veh/hr", 0, 1200);
        //ContinuousDimension queue = new ContinuousDimension("Weight", "Vehicle lenth on proper direction", "veh", 0, 30);


        ////Definition of dimension for output value
        //ContinuousDimension greenLight = new ContinuousDimension("Green Time", "green phase duration of Traffic Light", "s", 5, 60);

        ////Definition of basic fuzzy sets with which we will work
        ////  input sets:
        //FuzzySet Volume_small = new RightLinearSet(volume, "Small", 100, 300);
        //FuzzySet Volume_middle = new TrapezoidalSet(volume, "Middle", 300, 400, 100, 600);
        //FuzzySet Volume_large = new TrapezoidalSet(volume, "Large", 600, 700, 400, 900);
        //FuzzySet Volume_extraLarge = new LeftLinearSet(volume, "Extra Large", 700, 900);

        //FuzzySet Queue_small = new RightLinearSet(queue, "Small", 0, 7);
        //FuzzySet Queue_middle = new TriangularSet(queue, "Middle", 10, 14);
        //FuzzySet Queue_large = new TriangularSet(queue, "Large", 17, 14);
        //FuzzySet Queue_extraLarge = new LeftLinearSet(queue, "Extra Large", 17, 23);

        ////  output set:
        //FuzzySet Time_veryShort = new RightLinearSet(greenLight, "veryshort", 7, 15);
        //FuzzySet Time_short = new TrapezoidalSet(greenLight, "short", 15, 23, 7, 30);
        //FuzzySet Time_middle = new TrapezoidalSet(greenLight, "middle", 30, 38, 23, 45);
        //FuzzySet Time_long = new TrapezoidalSet(greenLight, "long", 45, 53, 38, 60);
        //FuzzySet Time_veryLong = new LeftLinearSet(greenLight, "verylong", 53, 60);

        ////Definition of antedescent
        //FuzzyRelation relation =
        //    ((Volume_small & Queue_small) & Time_veryShort) |
        //    ((Volume_small & Queue_middle) & Time_short) |
        //    ((Volume_small & Queue_large) & Time_short) |
        //    ((Volume_small & Queue_extraLarge) & Time_middle) |
        //    ((Volume_middle & Queue_small) & Time_short) |
        //    ((Volume_middle & Queue_middle) & Time_middle) |
        //    ((Volume_middle & Queue_large) & Time_middle) |
        //    ((Volume_middle & Queue_extraLarge) & Time_long) |
        //    ((Volume_large & Queue_small) & Time_middle) |
        //    ((Volume_large & Queue_middle) & Time_long) |
        //    ((Volume_large & Queue_large) & Time_long) |
        //    ((Volume_large & Queue_extraLarge) & Time_veryLong) |
        //    ((Volume_extraLarge & Queue_small) & Time_middle) |
        //    ((Volume_extraLarge & Queue_middle) & Time_long) |
        //    ((Volume_extraLarge & Queue_large) & Time_veryLong) |
        //    ((Volume_extraLarge & Queue_extraLarge) & Time_veryLong);
        //#endregion
        
        #region Deffuzification of the output set
        Defuzzification myRes = new CenterOfGravity(relation, new Dictionary<IDimension, decimal>{
                    { volume, (decimal)inputVolume },
                    { queue, (decimal)inputQueue }
                });
        
        ans = (float)myRes.CrispValue;
        #endregion

        return (float)ans;
    }
}


