using System;

namespace miniCalc.Utilities
{
    public class Calculator
    {

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

        #region ---------------------------------------------------------------- Calculator Class 

        public class CalculationEngine
        {
            public double? TotalSum { get; set; } = 0;
            public double? Operand1 { get; set; } = 0;
            public double? Operand2 { get; set; } = 0;
            public Operation Operation { get; set; } = Operation.None;
            public Function Function { get; set; } = Function.None;

            public CalculationEngine()
            {
                ClearScreen();
            }

            public void ClearScreen()
            {
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
                                //UserText = $@"NaN Error";
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
                    //UserText = $@"{TotalSum}";
                }
                catch (Exception)
                {
                    //UserText = $@"Calculation Error";
                }
            }

            #endregion --------------------------------------------------------- Calculator Class
        }
    }
}