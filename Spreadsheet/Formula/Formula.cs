// <copyright file="Formula_PS2.cs" company="UofU-CS3500">
// Copyright (c) 2024 UofU-CS3500. All rights reserved.
// </copyright>
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

using System.Text;

namespace CS3500.Formula;

using System.Text.RegularExpressions;

/// <summary>
///   <para>
///     This class represents formulas written in standard infix notation using standard precedence
///     rules.  The allowed symbols are non-negative numbers written using double-precision
///     floating-point syntax; variables that consist of one or more letters followed by
///     one or more numbers; parentheses; and the four operator symbols +, -, *, and /.
///   </para>
///   <para>
///     Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
///     a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable;
///     and "x 23" consists of a variable "x" and a number "23".  Otherwise, spaces are to be removed.
///   </para>
///   <para>
///     For Assignment Two, you are to implement the following functionality:
///   </para>
///   <list type="bullet">
///     <item>
///        Formula Constructor which checks the syntax of a formula.
///     </item>
///     <item>
///        Get Variables
///     </item>
///     <item>
///        ToString
///     </item>
///   </list>
/// </summary>
public class Formula
{
    /// <summary>
    ///   All variables are letters followed by numbers.  This pattern
    ///   represents valid variable name strings.
    /// </summary>
    private const string VariableRegExPattern = @"[a-zA-Z]+\d+";

    // The normalized list of tokens
    private readonly List<string> _tokens;

    // The normalized string of the tokens
    private readonly string _formulaString;

    /// <summary>
    ///   Initializes a new instance of the <see cref="Formula"/> class.
    ///   <para>
    ///     Creates a Formula from a string that consists of an infix expression written as
    ///     described in the class comment.  If the expression is syntactically incorrect,
    ///     throws a FormulaFormatException with an explanatory Message.  See the assignment
    ///     specifications for the syntax rules you are to implement.
    ///   </para>
    ///   <para>
    ///     Non-Exhaustive Example Errors:
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
        _tokens = GetTokens(formula);
        _formulaString = StandardizedStringCreation(_tokens);
        var differenceInPar = 0;
        // Ensures that all 8 rules are followed
        CheckRulesOfFormula(_tokens, ref differenceInPar);
    }

    /// <summary>
    /// Represents the different states each token and subsequent state can exist in.
    /// </summary>
    private enum StateOfFormula
    {
        First,
        NumberOrVariable,
        Operator,
        OpenParenthesis,
        CloseParenthesis,
        Invalid
    }

    /// <summary>
    ///   Ensures the formula follows each and every rule.
    /// </summary>
    /// <param name="tokens">The tokenized formula</param>
    /// <param name="differenceInPar">The difference in opening vs closing parenthesis</param>
    /// <exception cref="FormulaFormatException">Throws if the formula does not follow each rule</exception>
    private static void CheckRulesOfFormula(List<string> tokens, ref int differenceInPar)
    {
        // Check for at least one token
        OneToken(tokens.Count);
        // Start on First 
        var currentState = StateOfFormula.First;
        foreach (var token in tokens)
        {
            currentState = GetNextState(currentState, token, ref differenceInPar);
            if (currentState == StateOfFormula.Invalid)
            {
                // Call GetNextState method to throw exception as the currentState is invalid
                GetNextState(currentState, token, ref differenceInPar);
            }
        }

        // Check if the last token was an operator as that is invalid
        // If the last token was an opening parenthesis it will be caught by balance check
        if (currentState == StateOfFormula.Operator)
        {
            // Set state to invalid
            currentState = StateOfFormula.Invalid;
            // Call GetNextState method to throw exception as the currentState is invalid
            GetNextState(currentState, string.Empty, ref differenceInPar);
        }

        // Closing and opening parenthesis check
        BalanceCheck(differenceInPar);
    }

    /// <summary>
    ///   A helper method that uses the state of the formula via the enum to determine what paths are valid or invalid.
    /// </summary>
    /// <param name="currentState">The value of the enum</param>
    /// <param name="token">The string to be checked against</param>
    /// <param name="differenceInPar">The difference in opening vs closing parenthesis</param>
    /// <returns></returns>
    /// <exception cref="FormulaFormatException">Throws if the formula does not follow each rule</exception>
    private static StateOfFormula GetNextState(StateOfFormula currentState, string token, ref int differenceInPar)
    {
        switch (currentState)
        {
            case StateOfFormula.First: // Must be either number, variable, or open parenthesis
                if (ValidNumber(token) || IsVar(token)) return StateOfFormula.NumberOrVariable;
                if (!OpenPar(token)) return StateOfFormula.Invalid;
                differenceInPar++;
                return StateOfFormula.OpenParenthesis;
            case StateOfFormula.NumberOrVariable: // Must be either an operator or closing parenthesis
                if (ValidOp(token)) return StateOfFormula.Operator;
                if (!ClosingPar(token)) return StateOfFormula.Invalid;
                differenceInPar--;
                CheckClosingVsOpen(differenceInPar);
                return StateOfFormula.CloseParenthesis;
            case StateOfFormula.Operator: // Must be either a valid number, variable or open parenthesis
                if (ValidNumber(token) || IsVar(token)) return StateOfFormula.NumberOrVariable;
                if (!OpenPar(token)) return StateOfFormula.Invalid;
                differenceInPar++;
                return StateOfFormula.OpenParenthesis;
            case StateOfFormula.OpenParenthesis: // Must be either a valid number, variable or open parenthesis
                if (ValidNumber(token) || IsVar(token)) return StateOfFormula.NumberOrVariable;
                if (!OpenPar(token)) return StateOfFormula.Invalid;
                differenceInPar++;
                return StateOfFormula.OpenParenthesis;
            case StateOfFormula.CloseParenthesis: // must be either an operator or closing parenthesis
                if (ValidOp(token)) return StateOfFormula.Operator;
                if (!ClosingPar(token)) return StateOfFormula.Invalid;
                differenceInPar--;
                CheckClosingVsOpen(differenceInPar);
                return StateOfFormula.CloseParenthesis;
            case StateOfFormula.Invalid: // Fall through to throw the exception
            default:
                throw new FormulaFormatException("Invalid formula.");
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
        var variables = new HashSet<string>();
        // Filter _tokens based on IsVar
        foreach (var str in _tokens.Where(IsVar))
        {
            variables.Add(str);
        }

        return variables;
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
    ///     All the variables in the string will be normalized.  This
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
    ///   </para>
    /// </summary>
    /// <returns>
    ///   A canonical version (string) of the formula. All "equal" formulas
    ///   should have the same value here.
    /// </returns>
    public override string ToString()
    {
        return _formulaString;
    }

    /// <summary>
    ///   <para>
    ///     Makes a standardized string expression of the tokens in a passed in list.
    ///   </para>
    /// </summary>
    /// <param name="tokens">The list of tokens used to create the string</param>
    /// <returns>A string of the standardized formula</returns>
    private static string StandardizedStringCreation(List<string> tokens)
    {
        var builder = new StringBuilder();
        // Note: tokens has already been filtered and standardized
        foreach (var str in tokens)
        {
            builder.Append(str);
        }

        return builder.ToString();
    }

    /// <summary>
    ///   Normalizes the given token..
    /// </summary>
    /// <param name="token">The token to be normalized.</param>
    /// <returns>
    ///   A normalized string representation of the token. If the token is valid and can be normalized.
    ///   If a token is not valid an exception will be thrown.
    /// </returns>
    /// <exception cref="FormulaFormatException">Will be thrown when an invalid token is passed through</exception>
    private static string NormalizedToken(string token)
    {
        // Number normalizer
        if (double.TryParse(token, out var number))
        {
            return number.ToString("G");
        }

        // Make each char in token uppercase
        if (IsVar(token))
        {
            // If a token is a letter then make uppercase
            return new string(token.Select(c => char.IsLetter(c) ? char.ToUpper(c) : c).ToArray());
        }

        // Check if its operator or parenthesis; can return without modification 
        if (OpenPar(token) || ClosingPar(token) || ValidOp(token))
        {
            return token;
        }

        // If reached then it is an invalid token
        throw new FormulaFormatException("Invalid token:" + token);
    }

    /// <summary>
    ///   Checks if there is at least 1 token in the tokens list.
    /// </summary>
    /// <param name="tokenCount">The list of tokens to pass in for a size check</param>
    /// <exception cref="FormulaFormatException">Will be thrown if there are no tokens in the list</exception>
    private static void OneToken(int tokenCount)
    {
        if (tokenCount < 1)
        {
            throw new FormulaFormatException("There must be at least one token in the formula.");
        }
    }

    /// <summary>
    ///   Checks to make sure that the difference in parentheses at the end of the formula is 0.
    /// </summary>
    /// <param name="differenceInPar">The difference of opening vs closing parenthesis</param>
    /// <exception cref="FormulaFormatException">Will be thrown if the difference is positive or negative</exception>
    private static void BalanceCheck(int differenceInPar)
    {
        if (differenceInPar != 0)
        {
            throw new FormulaFormatException(
                "The amount of opening parenthesis must be equal to the amount closing parenthesis.");
        }
    }

    /// <summary>
    ///   Checks that there have been more closing parenthesis than opening parenthesis so far in the formula.
    /// </summary>
    /// <param name="differenceInPar">The difference of opening vs closing parenthesis; if negative then there are more closing parenthesis</param>
    /// <exception cref="FormulaFormatException">Will be thrown if the current amount of closing parenthesis is greater than opening parenthesis</exception>
    private static void CheckClosingVsOpen(int differenceInPar)
    {
        if (differenceInPar < 0)
        {
            throw new FormulaFormatException(
                "There can not be more closing parenthesis seen before opening parenthesis.");
        }
    }

    /// <summary>
    ///   Reports whether "token" is a valid number, decimal number, or scientific notation number.
    /// </summary>
    /// <param name="token">A token that may be a valid number</param>
    /// <returns>True if the string matches the requirements</returns>
    private static bool ValidNumber(string token)
    {
        // out value not needed
        return double.TryParse(token, out _);
    }

    /// <summary>
    ///   Reports whether "token" is a variable.  It must be one or more letters
    ///   followed by one or more numbers.
    /// </summary>
    /// <param name="token">A token that may be a variable. </param>
    /// <returns>True if the string matches the requirements, e.g., A1 or a1. </returns>
    private static bool IsVar(string token)
    {
        var standaloneVarPattern = $"^{VariableRegExPattern}$";
        return Regex.IsMatch(token, standaloneVarPattern);
    }

    /// <summary>
    ///   Reports whether "token" is a valid operator.
    ///   Valid operators are +,-,/,*.
    /// </summary>
    /// <param name="token">A token that may be a valid operator. </param>
    /// <returns>True if the string matches the requirements</returns>
    private static bool ValidOp(string token)
    {
        return token switch
        {
            "+" or "-" or "/" or "*" => true, _ => false
        };
    }

    /// <summary>
    ///   Reports whether "token" is an opening parenthesis.  
    /// </summary>
    /// <param name="token">A token that may be an opening Parenthesis. </param>
    /// <returns>True if the string matches the requirements</returns>
    private static bool OpenPar(string token)
    {
        return token.Equals("(");
    }

    /// <summary>
    ///   Reports whether "token" is a closing parenthesis.  
    /// </summary>
    /// <param name="token">A token that may be a closing Parenthesis</param>
    /// <returns>True if the string matches the requirements</returns>
    private static bool ClosingPar(string token)
    {
        return token.Equals(")");
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
        foreach (var s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
        {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
                results.Add(NormalizedToken(s));
            }
        }

        return results;
    }
}

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