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

public class FuzzyTest : MonoBehaviour {
    decimal _inputHeight;
    decimal _inputWeight;

    public double ans;

    public Text text;

    public void SetHeight(float _height)
    {
        _inputHeight = (decimal)_height;
        Calc();
    }
    public void SetWeight(float _weight)
    {
        _inputWeight = (decimal)_weight;
        Calc();
    }

    void Calc()
    {
        #region Definitions
        //Definition of dimensions on which we will measure the input values
        ContinuousDimension height = new ContinuousDimension("Height", "Personal height", "cm", 100, 250);
        ContinuousDimension weight = new ContinuousDimension("Weight", "Personal weight", "kg", 30, 200);

        //Definition of dimension for output value
        ContinuousDimension consequent = new ContinuousDimension("Suitability for basket ball", "0 = not good, 5 = very good", "grade", 0, 5);

        //Definition of basic fuzzy sets with which we will work
        //  input sets:
        FuzzySet tall = new LeftLinearSet(height, "Tall person", 170, 185);
        FuzzySet weighty = new LeftLinearSet(weight, "Weighty person", 80, 100);
        //  output set:
        FuzzySet goodForBasket = new LeftLinearSet(consequent, "Good in basket ball", 0, 5);

        //Definition of antedescent
        FuzzyRelation lanky = tall & !weighty;

        //Implication
        FuzzyRelation term = (lanky & goodForBasket) | (!lanky & !goodForBasket);
        #endregion

        #region Input values
        //Console.Write("Enter your height in cm:");
        //decimal inputHeight = decimal.Parse(Console.ReadLine());
        decimal inputHeight = _inputHeight;

        //Console.Write("Enter your weight in kg:");
        //decimal inputWeight = decimal.Parse(Console.ReadLine());
        decimal inputWeight = _inputWeight;
        #endregion

        #region Deffuzification of the output set
        Defuzzification result = new MeanOfMaximum(
            term,
            new Dictionary<IDimension, decimal>{
                    { height, inputHeight },
                    { weight, inputWeight }
                }
        );

        //Console.WriteLine(String.Format("Your disposition to be a basketball player is {0:F3} out of <0,...,5>", result.CrispValue));
//        Debug.Log("Your disposition to be a basketball player is {0:F3} out of <0,...,5>" + result.CrispValue);
        ans = (double)result.CrispValue;
        text.text = ans.ToString();

        //Console.WriteLine("Press any key to exit");
        //Console.ReadKey();
        #endregion
    }
}
