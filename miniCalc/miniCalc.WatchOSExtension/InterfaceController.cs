using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using WatchKit;
using miniCalc.Utilities;

namespace miniCalc.WatchOSExtension
{
    public partial class InterfaceController : WKInterfaceController
    {

        protected InterfaceController(IntPtr handle) : base(handle) 
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void Awake(NSObject context)  
        {
            base.Awake(context);

            // Configure interface objects here.
            Console.WriteLine("{0} awake with context", this);
        }

        public override void WillActivate() 
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);
            ConfigureButtonCollections();
            WriteTextToScreen("miniCalc!");
        }

        public override void DidDeactivate() 
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
        }

        #region ---------------------------------------------------------------- Globals 

        // colors
        //public UIColor NumberButtonNormalColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 1.0f);
        //public UIColor OperationButtonNormalColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 1.0f);
        //public UIColor FunctionButtonNormalColor = new UIColor(red: 0.28f, green: 0.01f, blue: 1.00f, alpha: 1.0f);
        //public UIColor ActionButtonNormalColor = new UIColor(red: 0.78f, green: 1.00f, blue: 0.21f, alpha: 1.0f);
        //public UIColor NumberButtonPressedColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 0.9f);
        //public UIColor OperationButtonPressedColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 0.9f);
        //public UIColor ActionButtonPressedColor = new UIColor(red: 0.28f, green: 0.01f, blue: 1.00f, alpha: 0.9f);
        //public UIColor FunctionButtonPressedColor = new UIColor(red:0.78f, green:1.00f, blue:0.21f, alpha:0.9f);

        public UIColor NumberButtonNormalColor = UIColor.FromRGB(125, 0, 255);
        public UIColor OperationButtonNormalColor = UIColor.FromRGB(100, 255, 50);
        public UIColor FunctionButtonNormalColor = UIColor.FromRGB(0, 150, 50);
        public UIColor ActionButtonNormalColor = UIColor.FromRGB(200, 0, 255);

        public UIColor NumberButtonPressedColor = UIColor.FromRGB(125, 130, 255);
        public UIColor OperationButtonPressedColor = UIColor.FromRGB(100, 100, 50);
        public UIColor ActionButtonPressedColor = UIColor.FromRGB(0, 100, 50);
        public UIColor FunctionButtonPressedColor = UIColor.FromRGB(200, 125,255);

        // calculator 
        public Calculator.CalculationEngine calculator = new Calculator.CalculationEngine();

        // screen test 
        public string CalculatorText = string.Empty;

        // button collections
        public List<WKInterfaceButton> nbuttoncollection;
        public List<WKInterfaceButton> obuttoncollection;
        public List<WKInterfaceButton> fbuttoncollection;
        public List<WKInterfaceButton> abuttoncollection;

        #endregion ------------------------------------------------------------- Globals  

        #region ---------------------------------------------------------------- UI Methods 

        private void ConfigureButtonCollections()
        {

            nbuttoncollection = new List<WKInterfaceButton>
            {
                 btnZero, btnOne, btnTwo, btnThree, btnThree, btnFour,
                 btnFive, btnSix, btnSeven, btnEight, btnNine, btnPeriod
            };
            obuttoncollection = new List<WKInterfaceButton>
            {
                 btnAdd, btnSubtract, btnDivide, btnMultiply
            };
            fbuttoncollection = new List<WKInterfaceButton>
            {
                 btnPercent, btnPlusMinus
            };
            abuttoncollection = new List<WKInterfaceButton>
            {
                 btnClear, btnSum
            };

            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(obuttoncollection, OperationButtonNormalColor);
            ResetButtonColors(fbuttoncollection, FunctionButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
        }

        public void ResetButtonColors(List<WKInterfaceButton> buttons, UIColor color)
        {
            foreach (var b in buttons)
            {
                b.SetBackgroundColor(color);
            }

        }

        public void SetButtonColors()
        {
            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(obuttoncollection, OperationButtonNormalColor);
            ResetButtonColors(fbuttoncollection, FunctionButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
        }

        public void WriteTextToScreen(string text, bool append = false)
        {
            if (append)
            {
                CalculatorText += text;
                CalculatorText = CalculatorText.TrimStart('0');

                if (CalculatorText.StartsWith('.'))
                {
                    CalculatorText = $@"0{CalculatorText}";
                }
            }
            else
            {
                CalculatorText = text;
            }
            lblScreen.SetText(CalculatorText);
        }

        #endregion ------------------------------------------------------------- UI Methods

        #region ---------------------------------------------------------------- Button OnPress Events

        // **********************************
        // *** number button press (0 - 9, .)
        // **********************************
        private void OnButtonPress(NumberButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            try
            {
                WriteTextToScreen(button.Symbol, true);
                calculator.Operand1 = null;
            }
            catch (Exception)
            {
                WriteTextToScreen("Number Error");
            }
            finally
            {
                SetButtonColors();
                //button.Button.SetBackgroundColor(button.PressedColor);
                button.Button.SetBackgroundColor(UIColor.Gray);
            }
        }

        // ****************************************
        // *** operation button press (+, -, * , /)
        // ****************************************
        private void OnButtonPress(OperationButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);
            calculator.Function = Calculator.Function.None;

            try
            {
                calculator.Operation = button.Operation;

                if (calculator.Operand1 != null)
                {
                    calculator.Operand2 = Convert.ToDouble(CalculatorText);
                    calculator.Calculate();
                    calculator.Operand1 = calculator.TotalSum;
                }
                else
                {

                    var isNumeric = double.TryParse(CalculatorText, out double number);
                    if (isNumeric)
                    {
                        calculator.Operand1 = number;
                        CalculatorText = string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                WriteTextToScreen("Operation Error");
            }
            finally
            {
                SetButtonColors();
                //button.Button.SetBackgroundColor(button.PressedColor);
                button.Button.SetBackgroundColor(UIColor.Gray);
            }
        }

        // ****************************************
        // *** function button press (+/-, %)
        // ****************************************
        private void OnButtonPress(FunctionButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);
            calculator.Operation = Calculator.Operation.None;

            try
            {
                calculator.Operand1 = Convert.ToDouble(CalculatorText);
                calculator.Function = button.Function;

                switch (button.Function)
                {
                    case Calculator.Function.Percent:
                        calculator.Calculate();
                        CalculatorText = string.Empty;
                        break;

                    case Calculator.Function.PluMinus:
                        break;

                }

                WriteTextToScreen($@"{calculator.TotalSum}");
            }
            catch (Exception)
            {
                WriteTextToScreen($@"Operation Error");
            }
            finally
            {
                SetButtonColors();
                //button.Button.SetBackgroundColor(button.PressedColor);
                button.Button.SetBackgroundColor(UIColor.Gray);
            }
        }

        // **************************************
        // *** action button press (C, =)
        // **************************************
        private void OnButtonPress(ActionButton button)
        {
            try
            {
                switch (button.Action)
                {
                    case Calculator.Action.Clear:
                        calculator.ClearScreen();
                        break;

                    case Calculator.Action.Equals:
                        WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.DirectionUp);

                        calculator.Operand2 = Convert.ToDouble(CalculatorText);
                        calculator.Calculate();
                        calculator.Operand1 = calculator.TotalSum;

                        //calculator.SaveResults($@"[{calculator.Operation}] ----------------");
                        //calculator.SaveResults();
                        //calculator.SaveResults($@"[Total] ----------------");
                        break;

                }

                WriteTextToScreen($@"{calculator.TotalSum}");
            }
            catch (Exception)
            {
                WriteTextToScreen("Action Error");
            }
            finally
            {
                SetButtonColors();
                //button.Button.SetBackgroundColor(button.PressedColor);
                button.Button.SetBackgroundColor(UIColor.Gray);
            }
        }

        #endregion ------------------------------------------------------------- Button OnPress Events

        #region ---------------------------------------------------------------- Button Events 

        partial void OnButtonPressZero()
        {
            NumberButton button = new NumberButton { Symbol = "0", Button = btnZero };
            OnButtonPress(button);
        }

        partial void OnButtonPressOne()
        {
            NumberButton button = new NumberButton { Symbol = "1", Button = btnOne };
            OnButtonPress(button);
        }

        partial void OnButtonPressTwo()
        {
            NumberButton button = new NumberButton { Symbol = "2", Button = btnTwo };
            OnButtonPress(button);
        }

        partial void OnButtonPressThree()
        {
            NumberButton button = new NumberButton { Symbol = "3", Button = btnThree };
            OnButtonPress(button);
        }

        partial void OnButtonPressFour()
        {
            NumberButton button = new NumberButton { Symbol = "4", Button = btnFour };
            OnButtonPress(button);
        }

        partial void OnButtonPressFive()
        {
            NumberButton button = new NumberButton { Symbol = "5", Button = btnFive };
            OnButtonPress(button);
        }

        partial void OnButtonPressSix()
        {
            NumberButton button = new NumberButton { Symbol = "6", Button = btnSix };
            OnButtonPress(button);
        }

        partial void OnButtonPressSeven()
        {
            NumberButton button = new NumberButton { Symbol = "7", Button = btnSeven };
            OnButtonPress(button);
        }

        partial void OnButtonPressEight()
        {
            NumberButton button = new NumberButton { Symbol = "8", Button = btnEight };
            OnButtonPress(button);
        }

        partial void OnButtonPressNine()
        {
            NumberButton button = new NumberButton { Symbol = "9", Button = btnNine };
            OnButtonPress(button);
        }

        partial void OnButtonPressPeriod()
        {
            NumberButton button = new NumberButton{ Symbol = ".", Button = btnPeriod };
            OnButtonPress(button);
        }

        partial void OnButtonPressAdd()
        {
            OperationButton button = new OperationButton { Operation = Calculator.Operation.Add, Button = btnAdd };
            OnButtonPress(button);
        }

        partial void OnButtonPressSubtract()
        {
            OperationButton button = new OperationButton { Operation = Calculator.Operation.Subtract, Button = btnSubtract };
            OnButtonPress(button);
        }

        partial void OnButtonPressMultiply()
        {
            OperationButton button = new OperationButton { Operation = Calculator.Operation.Multiply, Button = btnMultiply };
            OnButtonPress(button);
        }

        partial void OnButtonPressDivide()
        {
            OperationButton button = new OperationButton { Operation = Calculator.Operation.Divide, Button = btnDivide };
            OnButtonPress(button);
        }

        partial void OnButtonPressPlusMinus()
        {
            FunctionButton button = new FunctionButton { Function = Calculator.Function.PluMinus, Button = btnPlusMinus };
            OnButtonPress(button);
        }

        partial void OnButtonPressPercent()
        {
            FunctionButton button = new FunctionButton { Function = Calculator.Function.Percent, Button = btnPercent };
            OnButtonPress(button);
        }

        partial void onButtonPressSum()
        {
            ActionButton button = new ActionButton { Action = Calculator.Action.Equals,  Button = btnSum };
            OnButtonPress(button);
        }

        partial void OnButtonPressClear()
        {
            ActionButton button = new ActionButton { Action = Calculator.Action.Clear, Button = btnClear };
            OnButtonPress(button);
        }

        #endregion ------------------------------------------------------------- Button Events 

        #region ---------------------------------------------------------------- Button Classes 

        public class BaseCalculatorButton 
        {
            public UIColor NormalColor { get; set; }
            public UIColor PressedColor { get; set; }
            public WKInterfaceButton Button { get; set; }
        }

        public class NumberButton : BaseCalculatorButton
        {
            public string Symbol { get; set; } = null;
        }

        public class OperationButton : BaseCalculatorButton
        {
            public Calculator.Operation Operation { get; set; } = Calculator.Operation.None;
        }

        public class FunctionButton : BaseCalculatorButton
        {
            public Calculator.Function Function  { get; set; } = Calculator.Function.None;
        }

        public class ActionButton : BaseCalculatorButton
        {
            public Calculator.Action Action { get; set; } = Calculator.Action.None;
        }

        #endregion ------------------------------------------------------------- Button Classes 

 }
   public static class UIColorExtensions
    {
        public static UIColor FromHex(this UIColor color, int hexValue)
       {
            return UIColor.FromRGB(
                (((float)((hexValue & 0xFF0000) >> 16)) / 255.0f),
                (((float)((hexValue & 0xFF00) >> 8)) / 255.0f),
                (((float)(hexValue & 0xFF)) / 255.0f)
            );
        }
    }
}





