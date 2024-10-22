// <copyright file="Formula_PS2.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Ethan Perkins] </authors>
// <date> [9/20/2024] </date>
// <summary>
//   <para>
//     This code is provides to start your assignment.  It was written
//     by Profs Joe, Danny, and Jim.  You should keep this attribution
//     at the top of your code where you have your header comment, along
//     with the other required information.
//   </para>
//   <para>
//     You should remove/add/adjust comments in your file as appropriate
//     to represent your work and any changes you make.
//   </para>
// </summary>


namespace CS3500.Formula;

using System.Collections;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one ore more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     This class also includes an evaluate function, which will calculate the end result of an 
///     inputted formula provided the formula is constructed without error.
///   </para>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    /// <summary>
    /// List of the tokens generated from the inputted formula.
    /// </summary>
    private List<string> tokens;

    /// <summary>
    /// String form of the input with proper capitalization, numbers, and no spaces.
    /// </summary>
    private string formulaString;

    /// <summary>
    /// An array of string operators, used to check if a token is an operator.
    /// </summary>
    private static readonly string[] operators = ["+", "-", "*", "/"];

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non Exhaustive Example Errors:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>
    ///        Invalid variable name, e.g., x, x1x  (Note: x1 is valid, but would be normalized to X1)
    ///     </item>
    ///     <item>
    ///        Empty formula, e.g., string.Empty
    ///     </item>
    ///     <item>
    ///        Mismatched Parentheses, e.g., "(("
    ///     </item>
    ///     <item>
    ///        Invalid Following Rule, e.g., "2x+5"
    ///     </item>
    ///   </list>
    /// </summary>
    /// <param name="formula"> The string representation of the formula to be created.</param>
    public Formula(string formula)
    {
        //Initializing fields.
        List<string> tempTokens = GetTokens(formula);
        formulaString = "";
        tokens = new();

        //Checks for empty formulas.
        if (tempTokens.Count == 0) {
            throw new FormulaFormatException("Formulas must have at least one token.");
        }

        //Runs code to check the first token.
        string prev = tempTokens.First();
        prev = CheckFirstToken(prev, out int parenCount);
        formulaString += prev;
        tokens.Add(prev);
        //Checks every token following the first token, based off of what the token is
        string current = "";
        foreach (string token in tempTokens.Skip(1))
        {
            current = token;
            if (current == "(")
            {
                parenCount++;
                OPareOperFollower(current, prev);
            }

            else if (current == ")")
            {
                parenCount--;
                NumVarCPareFollower(current, prev);
                if (parenCount < 0)
                {
                    throw new FormulaFormatException("There are more closing parentheses than open at some point in the formula.");
                }
            }

            else if (operators.Contains(current))
            {
                NumVarCPareFollower(current, prev);
            }

            else if (IsValue(current, out current))
            {
                OPareOperFollower(current, prev);
            }

            else
            {
                throw new FormulaFormatException($"Invalid token {current} in formula.");
            }

            //update the previous token and add the current token to the end of the formula string.
            formulaString += current;
            tokens.Add(current);
            prev = token;
        }

        //Checks if the formula ends on a valid token and that the parentheses are balanced.
        if (parenCount != 0)
        {
            throw new FormulaFormatException("Parentheses are imbalanced.");
        }
        if (operators.Contains(current) || current == "(")
        {
            throw new FormulaFormatException($"Invalid last token {current}.");
        }
    }

    /// <summary>
    /// Function written to check the first token of the list of tokens, and sets the current number of open v.s. closed parentheses. 
    /// </summary>
    /// <param name="str">The first token to be checked.</param>
    /// <param name="parenCount">The current number of open v.s. closed parentheses</param>
    /// <returns>Returns the first token in canonical form.</returns>
    /// <exception cref="FormulaFormatException">Thrown if the first token invalidates the attempted formula.</exception>
    private static string CheckFirstToken(string str, out int parenCount)
    {
        parenCount = 0;
        if (str == ")" || operators.Contains(str))
        {
            throw new FormulaFormatException($"Invalid first token {str}.");
        }
        else if (str == "(")
        {
            parenCount = 1;
        }
        else if (!IsValue(str, out str))
        {
            throw new FormulaFormatException($"Invalid token {str} in formula.");
        }
        return str;
    }

    /// <summary>
    /// Function used to check whether a token is a numerical value, in either double or variable form. 
    /// </summary>
    /// <param name="str">The token being checked.</param>
    /// <param name="output">Ouputs the token in canonical form.</param>
    /// <returns>True if the token is a number/variable.</returns>
    private static bool IsValue(string str, out string output) {
        output = str;
        if (IsVar(str)) {
            output = str.ToUpper();
            return true;
        }
        if (double.TryParse(str, out double num))
        {
            output = num.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Function used to check whether a token is a numerical value, and outputs a double if successful.
    /// This function requires a Lookup function that will provide values to variables in the string.
    /// </summary>
    /// <param name="str">The token being checked.</param>
    /// <param name="lookup">The function used to evaluate variables into doubles.</param>
    /// <param name="output">Ouputs the token in double form, or a FormulaError if an undefined variable was inputted.</param>
    /// <returns>True if the token is a number/variable.</returns>
    private static bool IsValue(string str, Lookup lookup, ref object output)
    {
        if (IsVar(str))
        {
            try
            {
                output = lookup(str);
            }
            catch
            {
                output = new FormulaError($"Variable {str} is non-operatable.");
            }
            return true;
        }
        if (double.TryParse(str, out double num))
        {
            output = num;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks that the current token (a value or an open parentheses) is allowed to follow the previous token (an open parentheses or an operator).
    /// </summary>
    /// <param name="current">The current token.</param>
    /// <param name="prev">The previous token.</param>
    /// <exception cref="FormulaFormatException">Thrown if an invalid sequence of tokens is detected.</exception>
    private static void OPareOperFollower(string current, string prev)
    {
        if (prev != "(" && !operators.Contains(prev))
        {
            throw new FormulaFormatException($"Invalid token sequence: {prev} was followed by {current}.");
        }
    }

    /// <summary>
    /// Checks that the current token (an operator or a closed parentheses) is allowed to follow the previous token (a value or a closed parentheses).
    /// </summary>
    /// <param name="current">The current token.</param>
    /// <param name="prev">The previous token.</param>
    /// <exception cref="FormulaFormatException">Thrown if an invalid sequence of tokens is detected.</exception>
    private static void NumVarCPareFollower(string current, string prev)
    {
        if (prev != ")" && !IsValue(prev, out _))
        {
            throw new FormulaFormatException($"Invalid token sequence: {prev} was followed by {current}.");
        }
    }

    /// <summary>
    ///   <para>
    ///     Returns a set of all the variables in the formula.
    ///   </para>
    ///   <remarks>
    ///     Important: no variable may appear more than once in the returned set, even
    ///     if it is used more than once in the Formula.
    ///   </remarks>
    ///   <para>
    ///     For example, if N is a method that converts all the letters in a string to upper case:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>new("x1+y1*z1").GetVariables() should enumerate "X1", "Y1", and "Z1".</item>
    ///     <item>new("x1+X1"   ).GetVariables() should enumerate "X1".</item>
    ///   </list>
    /// </summary>
    /// <returns> the set of variables (string names) representing the variables referenced by the formula. </returns>
    public ISet<string> GetVariables()
    {
        HashSet<string> hash = new();
        foreach (string token in tokens)
        {
            if (Regex.IsMatch(token, VariableRegExPattern))
            {
                hash.Add(token.ToUpper());
            }
        }
        return hash;
    }

    /// <summary>
    ///   <para>
    ///     Returns a string representation of a canonical form of the formula.
    ///   </para>
    ///   <para>
    ///     The string will contain no spaces.
    ///   </para>
    ///   <para>
    ///     If the string is passed to the Formula constructor, the new Formula f 
    ///     will be such that this.ToString() == f.ToString().
    ///   </para>
    ///   <para>
    ///     All of the variables in the string will be normalized.  This
    ///     means capital letters.
    ///   </para>
    ///   <para>
    ///       For example:
    ///   </para>
    ///   <code>
    ///       new("x1 + y1").ToString() should return "X1+Y1"
    ///       new("X1 + 5.0000").ToString() should return "X1+5".
    ///   </code>
    ///   <para>
    ///     This code should execute in O(1) time.
    ///   <para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return formulaString;
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token"> A token that may be a variable. </param>
    /// <returns> true if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        // notice the use of ^ and $ to denote that the entire string being matched is just the variable
        string standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    ///   <para>
    ///     Given an expression, enumerates the tokens that compose it.
    ///   </para>
    ///   <para>
    ///     Tokens returned are:
    ///   </para>
    ///   <list type="bullet">
    ///     <item>left paren</item>
    ///     <item>right paren</item>
    ///     <item>one of the four operator symbols</item>
    ///     <item>a string consisting of one or more letters followed by one or more numbers</item>
    ///     <item>a double literal</item>
    ///     <item>and anything that doesn't match one of the above patterns</item>
    ///   </list>
    ///   <para>
    ///     There are no empty tokens; white space is ignored (except to separate other tokens).
    ///   </para>
    /// </summary>
    /// <param name="formula"> A string representing an infix formula such as 1*B1/3.0. </param>
    /// <returns> The ordered list of tokens in the formula. </returns>
    private static List<string> GetTokens(string formula)
    {
        List<string> results = [];

        string lpPattern = @"\(";
        string rpPattern = @"\)";
        string opPattern = @"[\+\-*/]";
        string doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
        string spacePattern = @"\s+";

        // Overall pattern
        string pattern = string.Format(
                                        "({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                        lpPattern,
                                        rpPattern,
                                        opPattern,
                                        VariableRegExPattern,
                                        doublePattern,
                                        spacePattern);

        // Enumerate matching tokens that don't consist solely of white space.
        foreach (string s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(s);
            }
        }

        return results;
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 == f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are the same.</returns>
    public static bool operator ==(Formula f1, Formula f2)
    {
        return f1.Equals(f2);
    }

    /// <summary>
    ///   <para>
    ///     Reports whether f1 != f2, using the notion of equality from the <see cref="Equals"/> method.
    ///   </para>
    /// </summary>
    /// <param name="f1"> The first of two formula objects. </param>
    /// <param name="f2"> The second of two formula objects. </param>
    /// <returns> true if the two formulas are not equal to each other.</returns>
    public static bool operator !=(Formula f1, Formula f2)
    {
        return !f1.Equals(f2);
    }

    /// <summary>
    ///   <para>
    ///     Determines if two formula objects represent the same formula.
    ///   </para>
    ///   <para>
    ///     By definition, if the parameter is null or does not reference 
    ///     a Formula Object then return false.
    ///   </para>
    ///   <para>
    ///     Two Formulas are considered equal if their canonical string representations
    ///     (as defined by ToString) are equal.  
    ///   </para>
    /// </summary>
    /// <param name="obj"> The other object.</param>
    /// <returns>
    ///   True if the two objects represent the same formula.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is Formula f && this.ToString().Equals(f.ToString());
    }

    /// <summary>
    ///   <para>
    ///     Evaluates this Formula, using the lookup delegate to determine the values of
    ///     variables.
    ///   </para>
    ///   <remarks>
    ///     When the lookup method is called, it will always be passed a normalized (capitalized)
    ///     variable name.  The lookup method will throw an ArgumentException if there is
    ///     not a definition for that variable token.
    ///   </remarks>
    ///   <para>
    ///     If no undefined variables or divisions by zero are encountered when evaluating
    ///     this Formula, the numeric value of the formula is returned.  Otherwise, a 
    ///     FormulaError is returned (with a meaningful explanation as the Reason property).
    ///   </para>
    ///   <para>
    ///     This method should never throw an exception.
    ///   </para>
    /// </summary>
    /// <param name="lookup">
    ///   <para>
    ///     Given a variable symbol as its parameter, lookup returns the variable's value
    ///     (if it has one) or throws an ArgumentException (otherwise).  This method will expect 
    ///     variable names to be normalized.
    ///   </para>
    /// </param>
    /// <returns> Either a double or a FormulaError, based on evaluating the formula.</returns>
    public object Evaluate(Lookup lookup)
    {
        //create two stacks of tokens, and an object to hold the result of IsValue.
        Stack<double> values = new();
        Stack<string> operators = new();
        object result = 0;
        //begins operating on each token in the formula.
        foreach (string token in tokens)
        {
            if (token == "*" || token == "/" || token == "(")
            {
                operators.Push(token);
            }
            else if (IsValue(token, lookup, ref result))
            {
                if(result is FormulaError)
                {
                    return result;
                }
                values.Push((double)result);
                if (CheckMultDivide(values, operators))
                {
                    return new FormulaError("Division by 0 deteced.");
                }
            }
            else if(token == "+" || token == "-")
            {
                CheckPlusMinus(values, operators);
                operators.Push(token);
            }
            else
            {
                CheckPlusMinus(values, operators);
                operators.Pop();
                if(CheckMultDivide(values, operators))
                {
                    return new FormulaError("Division by 0 deteced.");
                }
            }
        }
        //checks the operator stack for tokens, and evaluates one last time if necessary.
        if(operators.Count != 0)
        {
            Operate(values, operators);
        }
        return values.Pop();
    }

    /// <summary>
    /// Checks if the most recently detected operator is a + or - symbol, then operates using two values and said operator if true.
    /// </summary>
    /// <param name="values">Stack of values</param>
    /// <param name="operators">Stack of operators</param>
    private static void CheckPlusMinus(Stack<double> values, Stack<string> operators)
    {
        if (operators.IsOnTop("+", "-"))
        {
            Operate(values, operators);
        }
    }

    /// <summary>
    /// Checks if the most recently detected operator is a * or / symbol, then operates using two values and said operator if true.
    /// </summary>
    /// <param name="values">Stack of values</param>
    /// <param name="operators">Stack of operators</param>
    /// <returns>True if operation was successful.</returns>
    private static bool CheckMultDivide(Stack<double> values, Stack<string> operators)
    {
        return operators.IsOnTop("*", "/") && !Operate(values, operators);
    }

    /// <summary>
    /// Performs one correct operation between two values and an operator, from the stack of values and operators provided.
    /// The function returns false if a division by 0 is detected.
    /// </summary>
    /// <param name="values">Stack of values</param>
    /// <param name="operators">Stack of operators</param>
    /// <returns>True if operation was successful.</returns>
    private static bool Operate(Stack<double> values, Stack<string> operators)
    {
        double result = 0;
        double second = values.Pop();
        double first = values.Pop();
        switch (operators.Pop())
        {
            case ("+"):
                result = first + second; break;
            case ("-"):
                result = first - second; break;
            case ("*"):
                result = first * second; break;
            default:
                if (second.Equals(0))
                {
                    return false;
                }
                result = first / second;
                break;
        }
        values.Push(result);
        return true;
    }

    /// <summary>
    ///   <para>
    ///     Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
    ///     case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two
    ///     randomly-generated unequal Formulas have the same hash code should be extremely small.
    ///   </para>
    /// </summary>
    /// <returns> The hashcode for the object. </returns>
    public override int GetHashCode()
    {
        return this.ToString().GetHashCode();
    }
}

/// <summary>
/// Creates a new method for the Stack class to reduce duplicate code.
/// </summary>
internal static class StackExtensions
{
    internal static bool IsOnTop(this Stack<string> stack, string str1, string str2)
    {
        return stack.Count > 0 && (stack.Peek() == str1 || stack.Peek() == str2);
    }
}

/// <summary>
/// Used as a possible return value of the Formula.Evaluate method.
/// </summary>
public class FormulaError
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaError"/> class.
    ///   <para>
    ///     Constructs a FormulaError containing the explanatory reason.
    ///   </para>
    /// </summary>
    /// <param name="message"> Contains a message for why the error occurred.</param>
    public FormulaError(string message)
    {
        Reason = message;
    }

    /// <summary>
    ///  Gets the reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
}

/// <summary>
///   Any method meeting this type signature can be used for
///   looking up the value of a variable.
/// </summary>
/// <exception cref="ArgumentException">
///   If a variable name is provided that is not recognized by the implementing method,
///   then the method should throw an ArgumentException.
/// </exception>
/// <param name="variableName">
///   The name of the variable (e.g., "A1") to lookup.
/// </param>
/// <returns> The value of the given variable (if one exists). </returns>
public delegate double Lookup(string variableName);

/// <summary>
///   Used to report syntax errors in the argument to the Formula constructor.
/// </summary>
public class FormulaFormatException : Exception
{
    /// <summary>
    ///   Initializes a new instance of the <see cref="FormulaFormatException"/> class.
    ///   <para>
    ///      Constructs a FormulaFormatException containing the explanatory message.
    ///   </para>
    /// </summary>
    /// <param name="message"> A developer defined message describing why the exception occured.</param>
    public FormulaFormatException(string message)
        : base(message)
    {
        // All this does is call the base constructor. No extra code needed.
    }
}
