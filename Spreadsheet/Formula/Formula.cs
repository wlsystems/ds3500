// Skeleton written by Joe Zachary for CS 3500, January 2017
// Formula by Dustin Shiozaki u0054455 Feb 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unarry operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        private string formula = "";
        int counter = 0;
        int previous = 0;  //the type of token; 1 = double, 2 = + , 3 = ( parenthesis, 4 = ) parenthesis, 5 = var, = 6 * , 7 = "/", 8 = "-"
        
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
        public Formula(String _formula)
        {
            this.formula = _formula;
            Lookup lk = new Lookup(Lookup4);
        }

        private bool orderCheck(int current, string s)
        {
            if (current == 1) //1=double. example 3: diallowed: 3 3, 3 z, ) 3  allowed: 3 *, (3 
            {
                if (previous == 1 | previous == 5 | previous == 4)
                    throw new FormulaFormatException(s);
                else
                {
                    previous = 1;
                    return true;
                }
                    
            }
            if (current == 2) { //2 = operator not allowed: + +, ( +, 
                if (previous == 2 | previous == 3)
                    throw new FormulaFormatException(s);
                else
                {
                    if (s.Equals("+"))
                        previous = 2;
                    else if (s.Equals("-"))
                        previous = 8;
                    else if (s.Equals("*"))
                        previous = 6;
                    else if (s.Equals("/"))
                        previous = 7;
                    return true;
                }
            }
            if (current == 3) //3 = (; not allowed: ), 1
            {
                if (previous == 1 | previous == 4)
                    throw new FormulaFormatException(s);
                else
                {
                    previous = 3;
                    return true;
                }
            }
            if (current == 4) //4 = ); not allowed: (, *
            {
                if (previous == 2 | previous == 3 | previous ==6)
                    throw new FormulaFormatException(s);
                else
                {
                    previous = 4;
                    return true;
                }
            }
            if (current == 5) //5 = x; previous not allowed: ), *
            {
                if (previous == 1 | previous == 5 | previous == 4)
                    throw new FormulaFormatException(s);
                else
                {
                    previous = 5;
                    return true;
                }
            }
            throw new FormulaFormatException(s);
        }

        private string verify(string s)
        {
            {
                int number = 0;
                Boolean result2;
                bool result = Int32.TryParse(s, out number);
                {
                    if (result)
                    {
                        if (number > 0) { 
                            if (orderCheck(1, s))
                            {
                                return s;
                            }
                                                         
                            else
                            {
                                throw new UndefinedVariableException(s);
                            }
                        }                   
                    }
                    else 
                    {
                        if (s.Equals("("))
                        {
                            counter = counter + 1;
                            if (orderCheck(3, s))
                            {
                                return s;
                            }
                        }
                        else if (s.Equals(")"))
                        {
                            counter = counter - 1;
                            if (counter < 0)
                            {
                                throw new FormulaFormatException(s);
                            }
                            else if (orderCheck(4,s))
                            {
                                return s;
                            }
                        }
                        else if (s.Equals("+") | s.Equals("-") | s.Equals("*") | s.Equals("/"))
                            {
                                if (orderCheck(2,s))
                                {
                                    return s;
                                }
                                else
                                {
                                    throw new FormulaFormatException(s);
                                }
                            }
                        
                        else if ((result2 = s.Any(x => !char.IsLetter(x)) && (s != "+" | s != "-" | s != "*" | s != "/" | s != "(" | s != ")" )))
                        {
                            throw new FormulaFormatException(s);
                        }
                        previous = 5;
                    }
                }
                return s;
            }
        }


        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            List<string> sl = new List<string>();
            IEnumerable<String> ieb = GetTokens(formula);
            Stack<double> dbls = new Stack<double>();
            Stack<string> ops = new Stack<string>();
            int count = 0;
            int length = 0;
            string temp2 = "";
            string temp = "";
            Stack<double> sDBL = new Stack<double>();
            Stack<string> sOP = new Stack<string>();
            foreach (string s in ieb) //get length
                length++;

            foreach (string s in ieb)
            {
                double last;
                double dbl;
                last = previous;
                temp = verify(s);
                count++;
                double var1, var2;
                if (previous == 1 | previous == 5)
                {

                    if (previous == 1)
                    {
                        dbl = Int32.Parse(s);
                        if (last == 6) //*
                        {
                            sDBL.Push(dbl * sDBL.Pop());
                            sOP.Pop();
                        }

                        else if (last == 7) { // "/"
                            sDBL.Push(sDBL.Pop()/dbl);
                            sOP.Pop();
                        }
                        else {                           
                            sDBL.Push(dbl);
                        }
                            
                    }
                    if (previous == 5)
                    {
                        try
                        {
                            dbl = lookup(s);
                            if (last == 6)
                            {
                                sDBL.Push(dbl * sDBL.Pop());
                                sOP.Pop();
                            }

                            else if (last == 7)
                            {
                                sDBL.Push(sDBL.Pop() / dbl);                             
                                sOP.Pop();                                
                            }
                                
                            else
                            {
                                sDBL.Push(dbl);
                            }
                            
                        }
                        catch (UndefinedVariableException)
                        {
                            throw new UndefinedVariableException("invalid formula");
                        }
                        
                    }
                }
                else
                {
                    if ((previous == 2 | previous == 4 | previous == 7 | previous == 8) && count == 1)
                    {
                        throw new UndefinedVariableException("invalid formula"); //detects problems with first char
                    }
                    else if ((count == length) && (previous == 2 | previous == 3 | previous == 7 | previous == 8 | previous == 6))
                    {
                        throw new UndefinedVariableException("invalid formula"); //detects problems with last char
                    }

                    if (temp.Equals("+") | temp.Equals("-"))
                    {
                        if(sOP.Count > 0)
                        {
                            temp2 = sOP.Peek();
                        }
                        if (temp2 == "+") //+
                        {
                            Console.WriteLine(sDBL.Count);
                            var1 = sDBL.Pop();
                            var2 = sDBL.Pop();
                            dbl = var1 + var2;
                            sOP.Pop();
                            sDBL.Push(dbl);
                            sOP.Push(temp);
                        }
                        else if (last.Equals((8))) //-
                        {
                            var1 = sDBL.Pop();
                            var2 = sDBL.Pop();
                            dbl = var2 - var1;
                            sOP.Pop();
                            sOP.Push(temp);
                            sDBL.Push(dbl);
                        }
                        else
                        {
                            sOP.Push(temp);
                        }
                            
                    }
                    else if (temp.Equals("(") | temp.Equals("*") | temp.Equals("/"))
                    {
                        
                        sOP.Push(temp);
                    }

                    if (temp.Equals(")"))
                    {
                        temp2 = (sOP.Peek());
                        if (temp2.Equals("+")) //+
                        {                           
                            var1 = sDBL.Pop();
                            var2 = sDBL.Pop();
                            sDBL.Push(var2 + var1);                         
                            sOP.Pop();
                        }
                        else if (temp2.Equals("-")) //-
                        {
                            var1 = sDBL.Pop();
                            var2 = sDBL.Pop();
                            sDBL.Push(var2 - var1);
                            sOP.Pop();
                        }
                        sOP.Pop();
                        Console.WriteLine(sOP.Count + "C");
                    }                 
                } 
            }
            if (counter > 0)
                throw new FormulaEvaluationException("paranthesis not balanced");
            if (sOP.Any())
            {
                if (temp2.Equals("+"))
                {
                    double var1 = sDBL.Pop();
                    double var2 = sDBL.Pop();
                    return (var2 + var1);
                }
                if (temp2.Equals("-"))
                {
                    double var1 = sDBL.Pop();
                    double var2 = sDBL.Pop();
                    return (var2 - var1);
                }
                if (temp2.Equals("*"))
                {
                    double var1 = sDBL.Pop();
                    double var2 = sDBL.Pop();
                    return (var2 * var1);
                }
                if (temp2.Equals("/"))
                {
                    double var1 = sDBL.Pop();
                    double var2 = sDBL.Pop();
                    return (var2 / var1);
                }
            }

            else
            {
                return sDBL.Pop();
            }

            return sDBL.Pop();
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            // PLEASE NOTE:  I have added white space to this regex to make it more readable.
            // When the regex is used, it is necessary to include a parameter that says
            // embedded white space should be ignored.  See below for an example of this.
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern.  It contains embedded white space that must be ignored when
            // it is used.  See below for an example of this.
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            // PLEASE NOTE:  Notice the second parameter to Split, which says to ignore embedded white space
            /// in the pattern.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }


    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string var);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    [Serializable]
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    [Serializable]
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    [Serializable]
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}