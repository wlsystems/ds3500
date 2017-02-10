// Skeleton written by Joe Zachary for CS 3500, January 2017
// Formula by Dustin Shiozaki u0054455 Feb 2017

using Formulas;
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
    public struct Formula
    {
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// /// This constructor still has the formula but it also has Validator and Normalizer. Throws a ArgumentNullException if
        /// any parameters are null.
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
        private string formula;
        

        /// <summary>
        /// The constructor throw exceptions if there is an invalid character at the beginning or end of the string
        /// It also detects if an Operator token is in the first token and throws an exception if so.
        /// </summary>
        /// <param name="_formula"></param>
        public Formula(String _formula)
            : this(_formula, s =>s, s=>true)
        {
            if (_formula.Equals(null))
                throw new ArgumentNullException("cannot be null");
        }
        /// <summary>
        /// This constructor still has the formula but it also has Validator and Normalizer. Throws a ArgumentNullException if
        /// any parameters are null.
        /// </summary>
        /// <param name="_formula"></param>

        public Formula(String _formula, Normalizer N, Validator V)
        {
            if (_formula==null | N == null | V == null)
                throw new ArgumentNullException();
            this.formula = _formula;
            verify();
            string str = null;
            foreach (string s in GetTokens(formula))
            {
                if (getType(s) == 5)
                {
                    try
                    {
                        str = str + N(s);
                        if (!V(N(s)))
                            throw new FormulaFormatException(s + " :failed validator");
                    }
                    catch
                    {
                        throw new FormulaFormatException(s + " :variable not found");
                    }
                }
                else
                    str = str + s;
            }
            formula = str;   
            verify();
        }

        /// <summary>
        /// This returns each distinct variable that appears in the formula.
        /// </summary>
        /// <returns></returns>
        public ISet<string> GetVariables()
        {
            ISet<string> iss = new HashSet<string>();
            iss.UnionWith(GetTokens(formula));
            return iss;
        }
        ///
        /// This Will override the toString method to return the normalized string of the formula
        ///
        public override string ToString()
        {
            return formula;
        }


        /// <summary>
        /// Checks the sequence of the tokens and throws exceptions according to the following rules
        /// There can be no invalid tokens.
        /// There must be at least one token.
        /// The total number of opening parentheses must equal the total number of closing parentheses.
        /// The first token of a formula must be a number, a variable, or an opening parenthesis.
        /// The first token of a formula must be a number, a variable, or an opening parenthesis.
        /// Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis. 
        /// Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
        /// When reading tokens from left to right, at no point should the number of closing parentheses seen so far be greater than the number of opening parentheses seen so far.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private void orderCheck(int current, string s, int previous, int counter2)
        {
            if (current == 1) //1=double. example 3: diallowed: 3 3, 3 z, ) 3  allowed: 3 *, (3 
            {
                if (previous == 1 | previous == 5 | previous == 4)
                    throw new FormulaFormatException(s);
            }
            if (current == 2 | current == 8 | current == 6 |current == 7)
            { //2 = operator not allowed: + +, ( +, 
                if (previous == 2 | previous == 3 | counter2 == 0)
                    throw new FormulaFormatException(s);
            }
            if (current == 3) //3 = (; previous not allowed: ), 1
            {
                if (previous == 1 | previous == 4)
                    throw new FormulaFormatException(s);
            }
            if (current == 4) //4 = ); not allowed: (, *
            {
                if (previous == 2 | previous == 3 | previous == 6 | counter2 == 0)
                    throw new FormulaFormatException(s);
            }
            if (current == 5) //5 = x; previous not allowed: ), *
            {
                if (previous == 1 | previous == 5 | previous == 4)
                    throw new FormulaFormatException(s);
            }
        }

        /// <summary>
        /// Verify does checking to ensure that there are no invalid formulas.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private void verify()
        {
            IEnumerable<String> ieb = GetTokens(formula);
            int counter=0; //counts parenthesis
            int counter2=0; //tracks the index of what the token is that is being checked
            int previous=0;  //the type of token; 1 = double, 2 = + , 3 = ( parenthesis, 4 = ) parenthesis, 5 = var, = 6 * , 7 = "/", 8 = "-"
            int currentType = 0;
            foreach (string s in ieb)
            {
                previous = currentType;
                counter2++;
                currentType = getType(s);
                //see the order method for a description of what tokens the integers map to. 
                if (currentType == 3)
                    counter = counter + 1;

                else if (currentType == 4)
                {
                    counter = counter - 1;
                    if (counter < 0)
                        throw new FormulaFormatException(s);
                }
                orderCheck(currentType, s, previous, counter2);
                if ((currentType == 2 | currentType == 4 | currentType == 7 | currentType == 8 | currentType == 6) && counter2 == 1)
                    throw new FormulaFormatException(formula); //detects problems with first char
            }
            if (counter > 0)
                throw new FormulaFormatException(formula); //throw exception if parenthesis unbalanced
            if (counter2 == 0)
                throw new FormulaFormatException(formula); //throw exception if contains all empty spaces
            if (currentType == 2 | currentType == 3 | currentType == 7 | currentType == 8 | currentType == 6)
                throw new FormulaFormatException(formula); //detects problems with last char
        }
        private int getType(string s)
        {
            double number = 0;
            if (Double.TryParse(s, out number))
            {
                if (number >= 0)
                    return 1;
                else
                    throw new UndefinedVariableException(s);
            }
            else if (s.Equals("+"))
                return 2;
            else if (s.Equals("("))
                return 3;
            else if (s.Equals(")"))
                return 4;
            else if (s.Equals("*"))
                return 6;
            else if (s.Equals("/"))
                return 7;
            else if (s.Equals("-"))
                return 8;
            else if (s != "+" | s != "-" | s != "*" | s != "/" | s != "(" | s != ")")
            {
                if (!s.All(Char.IsLetterOrDigit))
                    throw new FormulaFormatException(s);
                else
                    return 5;
            }
            else return 0;
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
            if (formula == null)
                formula = "0";
            List<string> sl = new List<string>();
            IEnumerable<String> ieb = GetTokens(formula);
            Stack<double> dbls = new Stack<double>();
            Stack<string> ops = new Stack<string>();
            int count = 0;
            string temp2 = "";
            string temp = "";
            Stack<double> sDBL = new Stack<double>();
            Stack<string> sOP = new Stack<string>();
            int last = 0;
            int previous = 0;
            foreach (string s in ieb)
            {
                last = previous;
                previous = getType(s);
                double dbl;
                temp = s;
                temp2 = "";
                count++;
                double var1, var2;
                if (previous == 1 | previous == 5) //if the variable in question is double or var
                {
                    if (previous == 1) //1 means double
                    {
                        dbl = Double.Parse(s);
                        if (last == 6) //*
                        {
                            sDBL.Push(dbl * sDBL.Pop());
                            sOP.Pop();
                        }

                        else if (last == 7)
                        { // "/"
                            if (dbl != 0)
                            {
                                sDBL.Push(sDBL.Pop() / dbl);
                                sOP.Pop();
                            }
                            else
                                throw new FormulaEvaluationException(s);
                        }
                        else
                        {
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
                            throw new FormulaEvaluationException("invalid formula");
                        }
                    }
                }
                else
                {
                    
                    if (temp.Equals("+") | temp.Equals("-"))
                    {
                        if (sOP.Count > 0)
                        {
                            temp2 = sOP.Peek();
                        }
                        if (temp2 == "+") //+
                        {
                            var1 = sDBL.Pop();
                            var2 = sDBL.Pop();
                            dbl = var1 + var2;
                            sOP.Pop();
                            sDBL.Push(dbl);
                            sOP.Push(temp);
                        }
                        else if (temp2 == "-") //-
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
                        if (sOP.Any())
                        {
                            temp2 = sOP.Peek();
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
                        }
                        sOP.Pop();
                        if (sOP.Any())
                        {
                            temp2 = sOP.Peek();
                            if (temp2.Equals("*")) //+
                            {
                                var1 = sDBL.Pop();
                                var2 = sDBL.Pop();
                                sDBL.Push(var2 * var1);
                                sOP.Pop();
                            }
                            else if (temp2.Equals("/")) //-
                            {
                                var1 = sDBL.Pop();
                                var2 = sDBL.Pop();
                                if (var1 != 0)
                                    sDBL.Push(var2 / var1);
                                else throw new FormulaEvaluationException(s);
                                sOP.Pop();
                            }
                        }
                    }
                }
            }

            if (sOP.Any())
            {
                temp2 = sOP.Peek();
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
                    if (var1 != 0)
                        return (var2 / var1);
                    else
                        throw new FormulaEvaluationException(formula);
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
    public delegate string Normalizer(string s);
    public delegate bool Validator(string s);
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
    /// Used to report errors that occur when evaluating a Formula..
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