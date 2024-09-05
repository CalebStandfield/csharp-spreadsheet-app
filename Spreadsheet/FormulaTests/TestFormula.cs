// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright � 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Caleb Standfield] </authors>
// <date> [8/29/24] </date>

namespace CS3500.FormulaTests;

using CS3500.Formula;

/// <summary>
///   <para>
///     The following class shows the basics of how to use the MSTest framework,
///     including:
///   </para>
///   <list type="number">
///     <item> How to catch exceptions. </item>
///     <item> How a test of valid code should look. </item>
///   </list>
/// </summary>
[TestClass]
public class FormulaSyntaxTests
{
    // --- ps1 example and baseline test ---

    /// <summary>
    /// This tests the example given in ps1 in which all rules are followed and all tokens are valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestCompleteValidFormula_Valid()
    {
        _ = new Formula("(3 - 2) / x2 + 10");
    }

    /// <summary>
    /// Complex test that uses all valid operators, follows every rule, and employs all rules
    /// Makes use of valid variables, numbers, and scientific notation.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestComplexAndAllEncompassingFormula_Valid()
    {
        _ = new Formula("c2 + ((222 / 2) * 2e2) / 2 - (2 + 2.002)");
    }

    // --- Tests for One Token Rule ---

    /// <summary>
    ///   <para>
    ///     This test makes sure the right kind of exception is thrown
    ///     when trying to create a formula with no tokens.
    ///   </para>
    ///   <remarks>
    ///     <list type="bullet">
    ///       <item>
    ///         We use the _ (discard) notation because the formula object
    ///         is not used after that point in the method.  Note: you can also
    ///         use _ when a method must match an interface but does not use
    ///         some of the required arguments to that method.
    ///       </item>
    ///       <item>
    ///         string.Empty is often considered best practice (rather than using "") because it
    ///         is explicit in intent (e.g., perhaps the coder forgot to but something in "").
    ///       </item>
    ///       <item>
    ///         The name of a test method should follow the MS standard:
    ///         https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
    ///       </item>
    ///       <item>
    ///         All methods should be documented, but perhaps not to the same extent
    ///         as this one.  The remarks here are for your educational
    ///         purposes (i.e., a developer would assume another developer would know these
    ///         items) and would be superfluous in your code.
    ///       </item>
    ///       <item>
    ///         Notice the use of the attribute tag [ExpectedException] which tells the test
    ///         that the code should throw an exception, and if it doesn't an error has occurred;
    ///         i.e., the correct implementation of the constructor should result
    ///         in this exception being thrown based on the given poorly formed formula.
    ///       </item>
    ///     </list>
    ///   </remarks>
    ///   <example>
    ///     <code>
    ///        // here is how we call the formula constructor with a string representing the formula
    ///        _ = new Formula( "5+5" );
    ///     </code>
    ///   </example>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNoTokens_Invalid()
    {
        _ = new Formula(string.Empty);
    }

    /// <summary>
    /// Ensures a single valid number is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidSingleTokenNumber_Valid()
    {
        _ = new Formula("2");
    }

    /// <summary>
    /// Ensures a multi digit valid number is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestValidTokenMultiDigitNumber_Valid()
    {
        _ = new Formula("2222");
    }

    // --- Tests for Valid Token Rule ---

    /// <summary>
    /// Ensures the symbol "!" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenExclamationMark_Invalid()
    {
        _ = new Formula("2 ! 2");
    }

    /// <summary>
    /// Ensures the symbol "?" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenQuestionMark_Invalid()
    {
        _ = new Formula("2 ? 2");
    }

    /// <summary>
    /// Ensures the symbol "#" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenHash_Invalid()
    {
        _ = new Formula("2 # 2");
    }

    /// <summary>
    /// Ensures the symbol "$" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenDollar_Invalid()
    {
        _ = new Formula("2 $ 2");
    }

    /// <summary>
    /// Ensures the symbol "%" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenPercent_Invalid()
    {
        _ = new Formula("2 % 2");
    }

    /// <summary>
    /// Ensures the symbol "^" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenCarrot_Invalid()
    {
        _ = new Formula("2 ^ 2");
    }

    /// <summary>
    /// Ensures the symbol "&" is counted as an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenAndSign_Invalid()
    {
        _ = new Formula("2 & 2");
    }

    /// <summary>
    /// Ensures the symbols "(" and ")" are counted as valid tokens.
    /// <remarks>
    /// Test must have a closed parenthesis and a valid token after the opening parenthesis to not break other rules.
    /// </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenParenthesis_Valid()
    {
        _ = new Formula("(2)");
    }

    /// <summary>
    /// Ensures the symbol "+" is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenPlus_Valid()
    {
        _ = new Formula("2 + 2");
    }

    /// <summary>
    /// Ensures the symbol "-" is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenMinus_Valid()
    {
        _ = new Formula("2 - 2");
    }

    /// <summary>
    /// Ensures the symbol "*" is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenStar_Valid()
    {
        _ = new Formula("2 * 2");
    }

    /// <summary>
    /// Ensures the symbol "/" is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenDivide_Valid()
    {
        _ = new Formula("2 / 2");
    }

    /// <summary>
    /// Ensures a malformed variable with number than letter is an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenVariableNumberFirst_Invalid()
    {
        _ = new Formula("2c");
    }

    /// <summary>
    /// Ensures a malformed variable with number than letter then number is an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenVariableNumberLetterNumber_Invalid()
    {
        _ = new Formula("2c2");
    }

    /// <summary>
    /// Ensures a repeated variable squished together throws exception.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ValidTokenVariableRepeated_Valid()
    {
        _ = new Formula("c2c2");
    }

    /// <summary>
    /// Ensures a malformed variable with no number is an invalid token.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_InvalidTokenVariableLetterOnly_Invalid()
    {
        _ = new Formula("c");
    }

    /// <summary>
    /// Ensures a letter than number variable is counted as a valid token.
    /// Also ensures a single valid variable is counted as a valid token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenVariableLetterNumber_Valid()
    {
        _ = new Formula("c2");
    }

    /// <summary>
    /// Ensures that variables can be operated on together.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidVariableTokenOperation_Valid()
    {
        _ = new Formula("c2 + c2");
    }

    /// <summary>
    /// Ensures that multiple sequential letters can be present in a valid variable token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenVariableMultiLetter_Valid()
    {
        _ = new Formula("cccc2");
    }

    /// <summary>
    /// Ensures that multiple sequential numbers can be present in a valid variable token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenVariableMultiNumber_Valid()
    {
        _ = new Formula("c2222");
    }

    /// <summary>
    /// Ensures that uppercase and lowercase letters can be in a valid variable token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidTokenVariableUpperLowerCase_Valid()
    {
        _ = new Formula("CCccCC2");
    }

    /// <summary>
    /// Ensures that a simple scientific notation variable is valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidScientificToken_Valid()
    {
        _ = new Formula("2e8");
    }

    /// <summary>
    /// Ensures that scientific notations can be operated on together.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidScientificTokenOperation_Valid()
    {
        _ = new Formula("2e8 + 2e8");
    }

    /// <summary>
    /// Ensures that a scientific notation with a negative exponent notation variable is valid.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidScientificNegativeExponentToken_Valid()
    {
        _ = new Formula("2e-8");
    }

    /// <summary>
    /// Ensures that a decimal places are valid for numbers.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidDecimalPlace_Valid()
    {
        _ = new Formula("2.0");
        _ = new Formula("2.002");
        _ = new Formula("2.202");
    }

    /// <summary>
    /// Ensures that decimal places can be operated on together.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidDecimalPlaceOperation_Valid()
    {
        _ = new Formula("2.0 + 2.0");
    }

    /// <summary>
    /// Ensures that a decimal places are valid for scientific notation.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ValidDecimalPlaceScientific_Valid()
    {
        _ = new Formula("2.0e8");
    }

    /// <summary>
    /// Space between a what would be valid token is invalid.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_SpaceInValidTokenThrowsException_Invalid()
    {
        _ = new Formula("c 22");
    }

    // --- Tests for Closing Parenthesis Rule

    /// <summary>
    /// Simple balanced parenthesis formula.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ClosingParenthesisRuleBalancedParenthesis_Valid()
    {
        _ = new Formula("(2 + 2)");
    }

    /// <summary>
    /// Surrounded Expression with double set of parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ClosingParenthesisRuleDoubleSurrounded_Valid()
    {
        _ = new Formula("((2 + 2) + 2)");
    }

    /// <summary>
    /// Opening parenthesis missing.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ClosingParenthesisRuleOpeningParenthesisMissing_Invalid()
    {
        _ = new Formula("2 + 2)");
    }

    /// <summary>
    /// Double surrounded expression opening unbalanced.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ClosingParenthesisRuleSurroundedExpressionUnbalanced_Invalid()
    {
        _ = new Formula("(2 + 2))");
    }

    /// <summary>
    /// Double surrounded expression opening unbalanced.
    /// <remarks>
    /// Note: test also breaks first token rule, however still satisfies balanced parenthesis rule.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ClosingParenthesisRuleCLosingAtBeginning_Invalid()
    {
        _ = new Formula(")2 + 2(");
    }

    // --- Tests for Balanced Parenthesis Rule

    /// <summary>
    /// Too many opening parenthesis at beginning.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_BalancedParenthesisOpeningOffCount_Invalid()
    {
        _ = new Formula("(((2 + 2))");
    }

    /// <summary>
    /// Multi surrounded expression with equal opening and closing.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_BalancedParenthesisMultiSurrounded_Valid()
    {
        _ = new Formula("(((((2 + 2)))))");
    }

    /// <summary>
    /// Lots of Parenthesis encapsulate expression and are counted correctly in order.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_BalancedParenthesisEncapsulateParenthesisOrderCorrect_Valid()
    {
        _ = new Formula("(((2 + 2) + 2) / (2 * 2))");
    }

    // --- Tests for First Token Rule

    /// <summary>
    ///   <para>
    ///     Make sure a simple well-formed formula is accepted by the constructor (the constructor
    ///     should not throw an exception).
    ///   </para>
    ///   <remarks>
    ///     This is an example of a test that is not expected to throw an exception, i.e., it succeeds.
    ///     In other words, the formula "1+1" is a valid formula which should not cause any errors.
    ///   </remarks>
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenNumber_Valid()
    {
        _ = new Formula("1 + 1");
    }

    /// <summary>
    /// First token is a valid variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenVariable_Valid()
    {
        _ = new Formula("c2 + 2");
    }

    /// <summary>
    /// First token is a valid scientific notation token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_FirstTokenRuleScientificNotation_Valid()
    {
        _ = new Formula("2e4 + 2");
    }

    /// <summary>
    /// First token is a valid scientific notation token.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_FirstTokenRuleDecimal_Valid()
    {
        _ = new Formula("2.0 + 2");
    }

    /// <summary>
    /// First token is an opening parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestFirstTokenOpenParenthesis_Valid()
    {
        _ = new Formula("(2 + 2)");
    }

    /// <summary>
    /// First token is an opening parenthesis.
    /// <remarks>
    /// Note: also breaks closing parenthesis rule and balanced parenthesis rule.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosingParenthesis_Invalid()
    {
        _ = new Formula(")c2 + 2)");
    }

    /// <summary>
    /// First token is an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenOperator_Invalid()
    {
        _ = new Formula("+ 2 + 2");
    }

    /// <summary>
    /// The first token is a valid operator.
    /// <remarks>
    /// This test should fail as an operator is not a valid first token.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_FirstTokenSoloValidOperator_Invalid()
    {
        _ = new Formula("+");
    }

    /// <summary>
    /// The first token is an invalid operator, but does not follow the first token rule.
    /// <remarks>
    /// This test should fail as a symbol is not a valid first token or a valid token at all.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_FirstTokenSoloInvalidToken_Invalid()
    {
        _ = new Formula("#");
    }

    // --- Tests for Last Token Rule ---

    /// <summary>
    /// Last token is a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleNumber_Valid()
    {
        _ = new Formula("2 + 2");
    }

    /// <summary>
    /// Last token is a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleVariable_Valid()
    {
        _ = new Formula("2 + c2");
    }

    /// <summary>
    /// Last token is a closing parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleClosingParenthesis_Valid()
    {
        _ = new Formula("(2 + c2)");
    }

    /// <summary>
    /// Last token is a scientific notation number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_LastTokenRuleScientificNotation_Valid()
    {
        _ = new Formula("2 + 2e4");
    }

    /// <summary>
    /// last token is an opening parenthesis.
    /// <remarks>
    /// Note: also breaks balanced parenthesis rule.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenAccidentalOpeningParenthesis_Invalid()
    {
        _ = new Formula("(c2 + 2(");
    }
    
    /// <summary>
    /// last token is an opening parenthesis.
    /// <remarks>
    /// Note: also breaks balanced parenthesis rule.
    /// </remarks>
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOpeningParenthesis_Invalid()
    {
        _ = new Formula("(c2 + 2) + (");
    }

    /// <summary>
    /// Last token is an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        _ = new Formula("2 + 2 +");
    }

    // --- Tests for Parenthesis/Operator Following Rule ---

    /// <summary>
    /// Number following open parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleNumber_Valid()
    {
        _ = new Formula("(2 + 2) / (2 + 2)");
    }

    /// <summary>
    /// Double parenthesis with no operator in between throws a FormulaFormatException.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ParenthesisFollowingRuleDoubleParenthesis_Valid()
    {
        _ = new Formula("(2 + 2)(2 + 2)");
    }

    /// <summary>
    /// Variable following open parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleVariable_Valid()
    {
        _ = new Formula("(c2 + 2)");
    }

    /// <summary>
    /// Scientific notation following open parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleScientific_Valid()
    {
        _ = new Formula("(2e8 + 2)");
    }

    /// <summary>
    /// Open parenthesis following open parenthesis.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ParenthesisFollowingRuleParenthesis_Valid()
    {
        _ = new Formula("((c2 + 2))");
    }

    /// <summary>
    /// Operator follows open parenthesis.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ParenthesisFollowingRuleOperatorFollows_Invalid()
    {
        _ = new Formula("( + 2)");
    }

    /// <summary>
    /// Closed parenthesis follows open parenthesis.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ParenthesisFollowingRuleClosingParenthesisFollows_Invalid()
    {
        _ = new Formula("()");
    }

    /// <summary>
    /// Number following operator.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_OperatorFollowingRuleNumber_Valid()
    {
        _ = new Formula("(2 + 2) / (2 + 2)");
    }

    /// <summary>
    /// Variable following operator.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_OperatorFollowingRuleVariable_Valid()
    {
        _ = new Formula("2 + c2");
    }

    /// <summary>
    /// Open parenthesis following operator.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_OperatorFollowingRuleParenthesis_Valid()
    {
        _ = new Formula("2 + 2 / (16 / 2)");
    }

    /// <summary>
    /// Closed parenthesis follows operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_OperatorFollowingRuleClosingParenthesisFollows_Invalid()
    {
        _ = new Formula("(2 + )");
    }

    /// <summary>
    /// Operator follows an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_OperatorFollowingRuleOperatorOperator_Invalid()
    {
        _ = new Formula(" 2 ++ 2");
    }

    // --- Tests for Extra Following Rule ---

    /// <summary>
    /// Operator follows number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ExtraFollowingNumberOperator_Valid()
    {
        _ = new Formula("2 + 2 * 2 / 2");
    }

    /// <summary>
    /// Closing Parenthesis follows a number.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ExtraFollowingNumberParenthesis_Valid()
    {
        _ = new Formula("2 + 2 * (2 / 2)");
    }

    /// <summary>
    /// Opening parenthesis follows number.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingNumberOpeningParenthesis_Invalid()
    {
        _ = new Formula("2(2 + 2)");
    }

    /// <summary>
    /// Number follows number.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingNumberNumber_Invalid()
    {
        _ = new Formula("2 2");
    }

    /// <summary>
    /// Scientific follows Scientific.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingScientificScientific_Invalid()
    {
        _ = new Formula("2e8 2e8");
    }

    /// <summary>
    /// Decimal number follows decimal number.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingDecimalNumbDecimalNumb_Invalid()
    {
        _ = new Formula("2.0 2.0");
    }

    /// <summary>
    /// Variable follows variable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_ExtraFollowingVariableVariable_Invalid()
    {
        _ = new Formula("c2 c2");
    }
}

[TestClass]
public class FormulaRulesAndPublicMethodsTests
{
    // --- Test ToString ---
    
    // --- Test GetVariables ---
    
    [TestMethod]
    public void GetVariables_NoVariables_Count0()
    {
        var x = new Formula("2 + 2");
        var y = x.GetVariables();
        Assert.IsTrue(y.Count == 0);
    }
    
    [TestMethod]
    public void GetVariables_DifferentVariables_Count2()
    {
        var x = new Formula("c2 + n2");
        var y = x.GetVariables();
        Assert.IsTrue(y.Count == 2);
    }
    
    [TestMethod]
    public void ToString_NormalExpression_CorrectString()
    {
        var x = new Formula("c2 + n2");
        var str = "C2+N2";
        Assert.IsTrue(x.ToString().Equals(str));
    }

    [TestMethod] public void ToString_DoesNotIncludeSpaces_CorrectString()
    {
        var x = new Formula("C2+N2 + 5/ C8");
        var str = "C2+N2+5/C8";
        Assert.IsTrue(x.ToString().Equals(str));
    }
}