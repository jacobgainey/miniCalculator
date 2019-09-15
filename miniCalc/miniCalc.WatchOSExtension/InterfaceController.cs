using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using WatchKit;

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

        public UIColor NumberButtonNormalColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 1.0f);
        public UIColor OperationButtonNormalColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 1.0f);
        public UIColor ActionButtonNormalColor = new UIColor(red:0.28f, green:0.01f, blue:1.00f, alpha:1.0f);
        public UIColor NumberButtonPressedColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 0.9f);
        public UIColor OperationButtonPressedColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 0.9f);
        public UIColor ActionButtonPressedColor = new UIColor(red: 0.28f, green: 0.01f, blue: 1.00f, alpha: 0.9f);

        public override void WillActivate() 
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);

            nbuttoncollection = new List<WKInterfaceButton>
            {
                 btnZero, btnOne, btnTwo, btnThree, btnThree, btnFour,
                 btnFive, btnSix, btnSeven, btnEight, btnNine, btnPeriod
            };
            obuttoncollection = new List<WKInterfaceButton>
            {
                 btnAdd, btnSubtract, btnDivide, btnMultiply
            };
            abuttoncollection = new List<WKInterfaceButton>
            {
                 btnClear, btnPercent, btnPlusMinus, btnSum
            };

            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(obuttoncollection, OperationButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
        }

        public override void DidDeactivate() 
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
        }

        // --------------------------------------------------------------------- start

        public CalculationObject calculator = new CalculationObject();
        public List<WKInterfaceButton> nbuttoncollection;
        public List<WKInterfaceButton> obuttoncollection;
        public List<WKInterfaceButton> abuttoncollection;

        public void ResetButtonColors(List<WKInterfaceButton> buttons, UIColor color)
        {
            foreach (var b in buttons)
            {
                b.SetBackgroundColor(color);
            }
        }

        // --------------------------------------------------------------------- number button press (0 - 9, .)
        private void OnButtonPress(NumberButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            calculator.ProcessScreenText(button.Symbol);
            lblScreen.SetText(calculator.ScreenText);

            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
            button.Button.SetBackgroundColor(button.PressedColor);
        }

        // --------------------------------------------------------------------- operation button press (+, -, * , /)
        private void OnButtonPress(OperationButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            if (calculator.ScreenText !="") { calculator.Operand1 = Convert.ToDouble(calculator.ScreenText); }
            
            calculator.Operation = button.Operation;
            calculator.SaveResults();
            calculator.ScreenText = $@"";

            lblResult.SetText(calculator.ScreenResult);

            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(obuttoncollection, OperationButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
            button.Button.SetBackgroundColor(button.PressedColor);
        }

        // --------------------------------------------------------------------- action button press (C, +/-, %, =)
        private void OnButtonPress(ActionButton button)
        {
            switch (button.Action)
            {
                case Action.Clear:
                    calculator.ClearScreen();
                    break;
                case Action.PluMinus:
                    break;
                case Action.Equals:
                    WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.DirectionUp);

                    calculator.Operand2 = Convert.ToDouble(calculator.ScreenText);
                    calculator.SaveResults();
                    calculator.Calculate();
                    calculator.Operand1 = calculator.Total;
                    calculator.SaveResults($@"[{calculator.Operation}] ----------------");
                    calculator.SaveResults();
                    calculator.SaveResults($@"[Total] ----------------");
                    break;
            }

            lblScreen.SetText($@"{calculator.Total}");
            lblResult.SetText($@"{calculator.ScreenResult}");

            ResetButtonColors(nbuttoncollection, NumberButtonNormalColor);
            ResetButtonColors(obuttoncollection, OperationButtonNormalColor);
            ResetButtonColors(abuttoncollection, ActionButtonNormalColor);
            button.Button.SetBackgroundColor(button.PressedColor);
        }

        #region -------------------------------------------------------------- Button Events

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
            OperationButton button = new OperationButton { Operation = Operation.Add, Button = btnAdd };
            OnButtonPress(button);
        }

        partial void OnButtonPressSubtract()
        {
            OperationButton button = new OperationButton { Operation = Operation.Subtract, Button = btnSubtract };
            OnButtonPress(button);
        }

        partial void OnButtonPressMultiply()
        {
            OperationButton button = new OperationButton { Operation = Operation.Multiply, Button = btnMultiply };
            OnButtonPress(button);
        }

        partial void OnButtonPressDivide()
        {
            OperationButton button = new OperationButton { Operation = Operation.Divide, Button = btnDivide };
            OnButtonPress(button);
        }

        partial void OnButtonPressPercent()
        {
            OperationButton button = new OperationButton { Operation = Operation.Percent, Button = btnPercent };
            OnButtonPress(button);
        }

        partial void onButtonPressSum()
        {
            ActionButton button = new ActionButton { Action = Action.Equals,  Button = btnSum };
            OnButtonPress(button);
        }

        partial void OnButtonPressClear()
        {
            ActionButton button = new ActionButton { Action = Action.Clear, Button = btnClear };
            OnButtonPress(button);
        }

        #endregion -------------------------------------------------------------- Button Events

        public enum Operation 
        {
            None = 0, Add = 1, Subtract = 2, Multiply = 3, Divide = 4, Percent = 5
        }

        public enum Action 
        {
            None = 0, PluMinus = 2, Clear = 3, Equals = 4,
        }

        public class BaseCalculatorButton 
        {
            public UIColor NormalColor { get; set; }
            public UIColor PressedColor { get; set; }
            public WKInterfaceButton Button { get; set; }
        }

        public class NumberButton : BaseCalculatorButton
        {
            public string Symbol { get; set; } = null;
            public NumberButton()
            {
                //NormalColor = UIColor.FromRGB(165, 33, 255);
                //PressedColor = UIColor.FromRGB(212, 158, 255);
                NormalColor = UIColor.Purple;
                PressedColor = UIColor.LightGray;
            
            }

        }

        public class OperationButton : BaseCalculatorButton
        {
            public Operation Operation { get; set; } = Operation.None;
            public OperationButton()
            {
                //NormalColor = UIColor.FromRGB(127, 255, 44);
                //PressedColor = UIColor.FromRGB(255, 255, 0);
                NormalColor = UIColor.Green;
                PressedColor = UIColor.LightGray;
            }
        }

        public class ActionButton : BaseCalculatorButton
        {
            public Action Action { get; set; } = Action.None;
            public ActionButton()
            {
                //NormalColor = UIColor.FromRGB(127, 255, 44);
                //PressedColor = UIColor.FromRGB(255, 255, 0);
                NormalColor = UIColor.DarkGray;
                PressedColor = UIColor.LightGray;
            }
        }

        public class CalculationObject
        {
            public string ScreenResult { get; set; } = "*** Scratch Pad ***";
            public string ScreenText { get; set; } = "0";
            public double Total { get; set; } = 0;
            public double Operand1 { get; set; } = 0;
            public double Operand2 { get; set; } = 0;
            public Operation Operation { get; set; } = Operation.None;

            public CalculationObject()
            {
                ClearScreen();
            }

            public void ProcessScreenText(string text)
            {
                ScreenText += text;
                ScreenText = ScreenText.TrimStart('0');
            }

            public void SaveResults()
            {
                ScreenResult += ScreenText + '\n';
            }

            public void SaveResults(string text)
            {
                ScreenResult += text + '\n' ;
            }

            public void ClearScreen()
            {
                ScreenText = $@"0";
                ScreenResult = "*** Scratch Pad ***" + '\n';
                Total = 0;
                Operand1 = 0;
                Operand2 = 0;
                Operation = Operation.None;
            }

            public void Calculate()
            {
                switch (Operation)
                {
                    case Operation.Percent:
                        Total = Operand1 / 100; ;
                        break;
                    case Operation.Add:
                        Total = Operand1 + Operand2;
                        break;
                    case Operation.Subtract:
                        Total = Operand1 - Operand2;
                        break;
                    case Operation.Multiply:
                        Total = Operand1 * Operand2;
                        break;
                    case Operation.Divide:
                        if (Convert.ToInt32(Operand2) != 0)
                        {
                            Total = this.Operand1 / this.Operand2;
                        }
                        else
                        {
                            // todo: return nan
                        }
                        break;
                }
                ScreenText = $@"{Total}";
            }

        }
    }
}





