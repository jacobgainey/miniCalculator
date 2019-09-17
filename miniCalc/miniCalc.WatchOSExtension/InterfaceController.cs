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
            ConfigureButtonCollections();
        }

        public override void DidDeactivate() 
        {
            // This method is called when the watch view controller is no longer visible to the user.
            Console.WriteLine("{0} did deactivate", this);
        }

        #region ---------------------------------------------------------------- Globals 

        // colors
        public UIColor NumberButtonNormalColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 1.0f);
        public UIColor OperationButtonNormalColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 1.0f);
        public UIColor FunctionButtonNormalColor = new UIColor(red: 0.28f, green: 0.01f, blue: 1.00f, alpha: 1.0f);
        public UIColor ActionButtonNormalColor = new UIColor(red: 0.78f, green: 1.00f, blue: 0.21f, alpha: 1.0f);
        public UIColor NumberButtonPressedColor = new UIColor(red: 0.65f, green: 0.13f, blue: 1.00f, alpha: 0.9f);
        public UIColor OperationButtonPressedColor = new UIColor(red: 0.50f, green: 1.00f, blue: 0.17f, alpha: 0.9f);
        public UIColor ActionButtonPressedColor = new UIColor(red: 0.28f, green: 0.01f, blue: 1.00f, alpha: 0.9f);
        public UIColor FunctionButtonPressedColor = new UIColor(red:0.78f, green:1.00f, blue:0.21f, alpha:0.9f);

        // calculator 
        public CalculationEngine calculator = new CalculationEngine();

        // button collections
        public List<WKInterfaceButton> nbuttoncollection;
        public List<WKInterfaceButton> obuttoncollection;
        public List<WKInterfaceButton> fbuttoncollection;
        public List<WKInterfaceButton> abuttoncollection;

        #endregion ------------------------------------------------------------- Globals  

        #region ---------------------------------------------------------------- Button Collection Methods 

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

        #endregion ------------------------------------------------------------- Button Collection Methods

        #region ---------------------------------------------------------------- Button OnPress Events

        // **********************************
        // *** number button press (0 - 9, .)
        // **********************************
        private void OnButtonPress(NumberButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);

            try
            {
                calculator.AppendScreenText(button.Symbol);
                lblScreen.SetText(calculator.UserText);
            }
            catch (Exception)
            {
                lblScreen.SetText($@"Number Error");
            }
            finally
            {
                SetButtonColors();
                button.Button.SetBackgroundColor(button.PressedColor);
            }
        }

        // ****************************************
        // *** operation button press (+, -, * , /)
        // ****************************************
        private void OnButtonPress(OperationButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);
            calculator.Function = Function.None;

            try
            {
                calculator.Operand1 = Convert.ToDouble(calculator.UserText);
                calculator.Operation = button.Operation;
                calculator.UserText = $@"";

                //lblResult.SetText(calculator.ScreenResult);
            }
            catch (Exception)
            {
                lblScreen.SetText($@"Function Error");
            }
            finally
            {
                SetButtonColors();
                button.Button.SetBackgroundColor(button.PressedColor);
            }
        }

        // ****************************************
        // *** function button press (+/-, %)
        // ****************************************
        private void OnButtonPress(FunctionButton button)
        {
            WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.Click);
            calculator.Operation = Operation.None;

            try
            {
                calculator.Operand1 = Convert.ToDouble(calculator.UserText);
                calculator.Function = button.Function;
                switch (button.Function)
                {
                    case Function.Percent:
                        calculator.Calculate();
                        break;

                    case Function.PluMinus:
                        break;

                }

                lblScreen.SetText($@"{calculator.TotalSum}");
            }
            catch (Exception)
            {
                lblScreen.SetText($@"Operation Error");
            }
            finally
            {
                SetButtonColors();
                button.Button.SetBackgroundColor(button.PressedColor);
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
                    case Action.Clear:
                        calculator.ClearScreen();
                        break;

                    case Action.Equals:
                        WKInterfaceDevice.CurrentDevice.PlayHaptic(WKHapticType.DirectionUp);

                        calculator.Operand2 = Convert.ToDouble(calculator.UserText);
                        //calculator.SaveResults();
                        calculator.Calculate();
                        calculator.Operand1 = calculator.TotalSum;
                        //calculator.SaveResults($@"[{calculator.Operation}] ----------------");
                        //calculator.SaveResults();
                        //calculator.SaveResults($@"[Total] ----------------");
                        break;

                }

                lblScreen.SetText($@"{calculator.TotalSum}");
            }
            catch (Exception)
            {
                lblScreen.SetText($@"Action Error");
            }
            finally
            {
                SetButtonColors();
                button.Button.SetBackgroundColor(button.PressedColor);
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

        partial void OnButtonPressPlusMinus()
        {
            FunctionButton button = new FunctionButton { Function = Function.PluMinus, Button = btnPlusMinus };
            OnButtonPress(button);
        }

        partial void OnButtonPressPercent()
        {
            FunctionButton button = new FunctionButton { Function = Function.Percent, Button = btnPercent };
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

        #endregion ------------------------------------------------------------- Button Events 

        #region ---------------------------------------------------------------- Enums 

        public enum Operation 
        {
            None = 0, Add = 1, Subtract = 2, Multiply = 3, Divide = 4
        }

        public enum Function
        {
            None = 0, Percent = 1, PluMinus = 2
        }

        public enum Action
        {
            None = 0, Clear = 1, Equals = 2
        }

        #endregion ------------------------------------------------------------- Enums 

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
            public Operation Operation { get; set; } = Operation.None;
        }

        public class FunctionButton : BaseCalculatorButton
        {
            public Function Function  { get; set; } = Function.None;
        }

        public class ActionButton : BaseCalculatorButton
        {
            public Action Action { get; set; } = Action.None;
        }

        #endregion ------------------------------------------------------------- Button Classes 

        #region ---------------------------------------------------------------- Calculator Class 

        public class CalculationEngine
        {
            //public string ScreenResult { get; set; } = "*** Scratch Pad ***";
            public string UserText { get; set; } = "0";
            public double TotalSum { get; set; } = 0;
            public double Operand1 { get; set; } = 0;
            public double Operand2 { get; set; } = 0;
            public Operation Operation { get; set; } = Operation.None;
            public Function Function { get; set; } = Function.None;

            public CalculationEngine()
            {
                ClearScreen();
            }

            public void AppendScreenText(string text)
            {
                UserText += text;
                UserText = UserText.TrimStart('0');
                if (UserText.StartsWith('.')) UserText = $@"0{UserText}";
            }

            //public void SaveResults()
            //{
            //    ScreenResult += ScreenText + '\n';
            //}

            //public void SaveResults(string text)
            //{
            //    ScreenResult += text + '\n' ;
            //}

            public void ClearScreen()
            {
                UserText = $@"0";
                //ScreenResult = "*** Scratch Pad ***" + '\n';
                TotalSum = 0;
                Operand1 = 0;
                Operand2 = 0;
                Operation = Operation.None;
            }

            public void Calculate()
            {
                try
                {
                    switch (Operation)
                    {
                        case Operation.Add:
                            TotalSum = Operand1 + Operand2;
                            break;

                        case Operation.Subtract:
                            TotalSum = Operand1 - Operand2;
                            break;

                        case Operation.Multiply:
                            TotalSum = Operand1 * Operand2;
                            break;

                        case Operation.Divide:
                            if (Convert.ToInt32(Operand2) != 0)
                            {
                                TotalSum = this.Operand1 / this.Operand2;
                            }
                            else
                            {
                                // todo: return nan
                            }
                            break;
                    }

                    switch (Function)
                    {
                        case Function.Percent:
                            TotalSum = Operand1 / 100;
                            break;

                        case Function.PluMinus:
                            TotalSum = Operand1;
                            TotalSum *= -1;
                            break;

                    }
                    UserText = $@"{TotalSum}";
                }
                catch (Exception)
                {
                    UserText = $@"Calculation Error";
                }
            }

            #endregion --------------------------------------------------------- Calculator Class

        }
    }
}





