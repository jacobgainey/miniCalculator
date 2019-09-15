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

        public override void WillActivate()
        {
            // This method is called when the watch view controller is about to be visible to the user.
            Console.WriteLine("{0} will activate", this);

            obuttoncollection = new List<WKInterfaceButton>
            {
                 btnZero, btnOne, btnTwo, btnThree, btnThree, btnFour,
                 btnFive, btnSix, btnSeven, btnEight, btnNine
            };
            obuttoncollection = new List<WKInterfaceButton>
            {
                 btnAdd, btnSubtract, btnDivide, btnMultiply,
            };
            abuttoncollection = new List<WKInterfaceButton>
            {
                 btnClear, btnPercent, btnPlusMinus, btnSum
            };
        }

        public override void DidDeactivate()
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
        }

        // --------------------------------------------------------------------- start

        public Calculationbject calculator = new Calculationbject();

        public List<WKInterfaceButton> nbuttoncollection;
        public List<WKInterfaceButton> obuttoncollection;
        public List<WKInterfaceButton> abuttoncollection;

        //public void ResetButtonColors()
        //{
        //    foreach (var b in buttons)
        //    {
        //        b.SetBackgroundColor(button.NormalColor);
        //    }
        //}

        private void OnButtonPress(NumberButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            lblScreen.SetText(calculator.SetScreen(button.Symbol));

            //ResetButtonColors(nbuttoncollection, button);
            //button.Button.SetBackgroundColor(button.PressedColor);
        }

        private void OnButtonPress(OperationButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            calculator.Operand1 = calculator.ScreenNumber;
            calculator.Operation = button.Operation;
            lblScreen.SetText(calculator.ClearScreen());

            //ResetButtonColors(obuttoncollection, button);
            //button.Button.SetBackgroundColor(button.PressedColor);
        }

        private void OnButtonPress(ActionButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            switch (button.Action)
            {
                case Action.Equals:
                    calculator.Operand2 = calculator.ScreenNumber;
                    calculator.Calculate();
                    calculator.Operand1 = calculator.Total;
                    lblScreen.SetText($@"{calculator.Total}");
                    break;
                case Action.Clear:
                    calculator.Reset();
                    lblScreen.SetText(calculator.SetScreen($@"0"));
                    //ResetButtonColors(nbuttoncollection, button);
                    //ResetButtonColors(obuttoncollection, button);
                    break;
            }

            //ResetButtonColors(abuttoncollection, button);
            //button.Button.SetBackgroundColor(button.PressedColor);
        }

        #region -------------------------------------------------------------- Button Events

        partial void OnButtonPressZero()
        {
            Console.WriteLine("{0} button press zero", this);
            NumberButton button = new NumberButton { Symbol = "0", Button = btnZero };
            OnButtonPress(button);
        }

        partial void OnButtonPressOne()
        {
            Console.WriteLine("{0} button press one", this);
            NumberButton button = new NumberButton { Symbol = "1", Button = btnOne };
            OnButtonPress(button);
        }

        partial void OnButtonPressTwo()
        {
            Console.WriteLine("{0} button press two", this);
            NumberButton button = new NumberButton { Symbol = "2", Button = btnTwo };
            OnButtonPress(button);
        }

        partial void OnButtonPressThree()
        {
            Console.WriteLine("{0} button press three", this);
            NumberButton button = new NumberButton { Symbol = "3", Button = btnThree };
            OnButtonPress(button);
        }

        partial void OnButtonPressFour()
        {
            Console.WriteLine("{0} button press four", this);
            NumberButton button = new NumberButton { Symbol = "4", Button = btnFour };
            OnButtonPress(button);
        }

        partial void OnButtonPressFive()
        {
            Console.WriteLine("{0} button press five", this);
            NumberButton button = new NumberButton { Symbol = "5", Button = btnFive };
            OnButtonPress(button);
        }

        partial void OnButtonPressSix()
        {
            Console.WriteLine("{0} button press six", this);
            NumberButton button = new NumberButton { Symbol = "6", Button = btnSix };
            OnButtonPress(button);
        }

        partial void OnButtonPressSeven()
        {
            Console.WriteLine("{0} button press seven", this);
            NumberButton button = new NumberButton { Symbol = "7", Button = btnSeven };
            OnButtonPress(button);
        }

        partial void OnButtonPressEight()
        {
            Console.WriteLine("{0} button press eight", this);
            NumberButton button = new NumberButton { Symbol = "8", Button = btnEight };
            OnButtonPress(button);
        }

        partial void OnButtonPressNine()
        {
            Console.WriteLine("{0} button press nine", this);
            NumberButton button = new NumberButton { Symbol = "9", Button = btnNine };
            OnButtonPress(button);
        }

        partial void OnButtonPressPeriod()
        {
            Console.WriteLine("{0} button press period", this);
            NumberButton button = new NumberButton{ Symbol = ".", Button = btnPeriod };
            OnButtonPress(button);
        }

        partial void OnButtonPressAdd()
        {
            Console.WriteLine("{0} button press add", this);
            OperationButton button = new OperationButton { Operation = Operation.Add, Button = btnAdd };
            OnButtonPress(button);
        }

        partial void OnButtonPressSubtract()
        {
            Console.WriteLine("{0} button press subtract", this);
            OperationButton button = new OperationButton { Operation = Operation.Subtract, Button = btnSubtract };
            OnButtonPress(button);
        }

        partial void OnButtonPressMultiply()
        {
            Console.WriteLine("{0} button press multiply", this);
            OperationButton button = new OperationButton { Operation = Operation.Multiply, Button = btnMultiply };
            OnButtonPress(button);
        }

        partial void OnButtonPressDivide()
        {
            Console.WriteLine("{0} button press divide", this);
            OperationButton button = new OperationButton { Operation = Operation.Divide, Button = btnDivide };
            OnButtonPress(button);
        }

        partial void onButtonPressSum()
        {
            Console.WriteLine("{0} button press sum", this);
            ActionButton button = new ActionButton { Action = Action.Equals,  Button = btnSum };
            OnButtonPress(button);
        }

        partial void OnButtonPressClear()
        {
            Console.WriteLine("{0} button press clear", this);
            ActionButton button = new ActionButton { Action = Action.Clear, Button = btnClear };
            OnButtonPress(button);
        }

        #endregion -------------------------------------------------------------- Button Events

        public enum Operation
        {
            None = 0, Add = 1, Subtract = 2, Multiply = 3, Divide = 4,
        }

        public enum Action
        {
            None = 0, Percent = 1, PluMinus = 2, Clear = 3, Equals = 4,
        }

        public class CalculatorButton
        {
            public UIColor NormalColor { get; set; }
            public UIColor PressedColor { get; set; }
            public WKInterfaceButton Button { get; set; }
        }

        public class OperationButton : CalculatorButton
        {
            public Operation Operation { get; set; } = Operation.None;
            public OperationButton()
            {
                NormalColor = UIColor.FromRGB(127, 255, 44);
                PressedColor = UIColor.FromRGB(255, 255, 0);
            }
        }

        public class ActionButton : CalculatorButton
        {
            public Action Action { get; set; } = Action.None;
            public ActionButton()
            {
                NormalColor = UIColor.FromRGB(127, 255, 44);
                PressedColor = UIColor.FromRGB(255, 255, 0);
            }
        }

        public class NumberButton : CalculatorButton
        {
            public string Symbol { get; set; } = null;
            public NumberButton()
            {
                NormalColor = UIColor.FromRGB(165, 33, 255);
                PressedColor = UIColor.FromRGB(212, 158, 255);
            }
        }

        public class Calculationbject
        {
            public string ScreenText { get; set; } = "0";
            public double ScreenNumber { get; set; } = 0;
            public double Total { get; set; } = 0;
            public double Operand1 { get; set; } = 0;
            public double Operand2 { get; set; } = 0;
            public Operation Operation { get; set; } = Operation.None;

            public string SetScreen(string text)
            {
                ScreenText += text.TrimStart('0');
                ScreenNumber = Convert.ToDouble(ScreenText);
                return $@"{ScreenNumber}";
            }

            public string ClearScreen()
            {
                ScreenText = "";
                return ScreenText;
            }

            public void Reset()
            {
                ScreenText = "0";
                ScreenNumber = 0;
                Total = 0;
                Operand1 = 0;
                Operand2 = 0;
                Operation = Operation.None;
            }

            public double Calculate()
            {
                switch (Operation)
                {
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
                            Total = this.Operand1 / this.Operand2;
                        break;
                }
                return Total;
            }
        }
    }
}





