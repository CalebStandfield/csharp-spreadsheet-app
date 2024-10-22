// <copyright file="FormulaSyntaxTests.cs" company="UofU-CS3500">
//   Copyright 2024 UofU-CS3500. All rights reserved.
// </copyright>
// <authors> [Ethan Perkins] </authors>
// <date> [9/20/2024] </date>

namespace CS3500.FormulaTests;

using CS3500.Formula;
using System.Linq;

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
    #region --- PS1,PS2 Tests ---

    #region --- Formula Construction Tests ---

    #region --- Tests for One Token Rule ---

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

    #endregion

    #region --- Tests for Valid Token Rule ---

    /// <summary>
    /// Testing standard numbers.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNumberTokens_Valid()
    {
        _ = new Formula("1");
    }

    /// <summary>
    /// Testing standard numbers with more than one digit.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLongNumberTokens_Valid()
    {
        _ = new Formula("15");
    }

    /// <summary>
    /// Testing numbers with decimal points.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestDecimalNumberTokens_Valid()
    {
        _ = new Formula("3.14");
    }

    /// <summary>
    /// Testing numbers with positive exponentials.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestPositiveExponentNumberTokens_Valid()
    {
        _ = new Formula("7e2");
    }

    /// <summary>
    /// Testing numbers with negative exponentials.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestNegativeExponentNumberTokens_Valid()
    {
        _ = new Formula("8e-4");
    }

    /// <summary>
    /// Testing valid operator sum.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorSum_Valid()
    {
        _ = new Formula("1+1");
    }

    /// <summary>
    /// Testing valid operator subtract.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorSubtract_Valid()
    {
        _ = new Formula("1-1");
    }

    /// <summary>
    /// Testing valid operator multiply.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorMultiply_Valid()
    {
        _ = new Formula("1*1");
    }

    /// <summary>
    /// Testing valid operator divide.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorDivide_Valid()
    {
        _ = new Formula("1/1");
    }

    /// <summary>
    /// Testing dividing by 0 NOT being affected.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestDivideByZero_Valid()
    {
        _ = new Formula("1/0");
    }

    /// <summary>
    /// Testing valid variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestLongVariableTokens_Valid()
    {
        _ = new Formula("aaaasdaa3333 + biidshiodsoj9990112");
    }

    /// <summary>
    /// Testing invalid letter tokens.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLetterTokens_Invalid()
    {
        _ = new Formula("a");
    }

    /// <summary>
    /// Testing invalid variable tokens. "a1a1" should be invalid because two variables are next to each other.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidVariableTokens_Invalid()
    {
        _ = new Formula("a1a1");
    }

    /// <summary>
    /// Testing invalid variable tokens. "1a1a" should be invalid because a number and a variable are next to each other.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidVariableTokensNumberStart_Invalid()
    {
        _ = new Formula("1a1a");
    }

    /// <summary>
    /// Testing invalid symbol token !.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenExclamationPoint_Invalid()
    {
        _ = new Formula("!");
    }

    /// <summary>
    /// Testing invalid symbol token @.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenAtSign_Invalid()
    {
        _ = new Formula("@");
    }

    /// <summary>
    /// Testing invalid symbol token #.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenHashtag_Invalid()
    {
        _ = new Formula("#");
    }

    /// <summary>
    /// Testing invalid symbol token $.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenDollarSign_Invalid()
    {
        _ = new Formula("$");
    }

    /// <summary>
    /// Testing invalid symbol token %.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenPercent_Invalid()
    {
        _ = new Formula("%");
    }

    /// <summary>
    /// Testing invalid symbol token ^.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenCaret_Invalid()
    {
        _ = new Formula("^");
    }

    /// <summary>
    /// Testing invalid symbol token ampersand.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestSymbolTokenAmpersand_Invalid()
    {
        _ = new Formula("&");
    }

    /// <summary>
    /// Testing invalid symbol after a valid symbol.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestInvalidSymbolFollowValidSymbol_Invalid()
    {
        _ = new Formula("1 &");
    }

    #endregion
    
    #region --- Tests for Closing Parenthesis Rule ---

    /// <summary>
    /// Testing properly ordered parentheses.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOrderedParentheses_Valid()
    {
        _ = new Formula("(1)");
    }

    /// <summary>
    /// Testing unordered parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestUnorderedParentheses_Invalid()
    {
        _ = new Formula("(1))+(1");
    }

    #endregion

    #region --- Tests for Balanced Parentheses Rule ---

    /// <summary>
    /// Testing equal numbers of parentheses.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestEqualParentheses_Valid()
    {
        _ = new Formula("((((1))))");
    }

    /// <summary>
    /// Testing unequal numbers of parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestUnequalParentheses_Invalid()
    {
        _ = new Formula("((1)");
    }

    #endregion

    #region --- Tests for First Token Rule ---

    /// <summary>
    /// Testing the first token being an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenOperator_Invalid()
    {
        _ = new Formula("+1+1");
    }

    /// <summary>
    /// Testing the first token being a closing parentheses (balanced).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosingParenthesesBalanced_Invalid()
    {
        _ = new Formula(")+((1)");
    }

    /// <summary>
    /// Testing the first token being a closing parentheses (imbalanced).
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestFirstTokenClosingParenthesesImbalanced_Invalid()
    {
        _ = new Formula(")");
    }

    #endregion

    #region --- Tests for  Last Token Rule ---

    /// <summary>
    /// Testing the last token being an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOperator_Invalid()
    {
        _ = new Formula("1+1+");
    }

    /// <summary>
    /// Testing the last token being an open parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestLastTokenOpenParentheses_Invalid()
    {
        _ = new Formula("1+(");
    }

    #endregion

    #region --- Tests for Parentheses/Operator Following Rule ---

    /// <summary>
    /// Testing an operator being followed by a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorToVariable_Valid()
    {
        _ = new Formula("1+a1");
    }

    /// <summary>
    /// Testing an operator being followed by an open parentheses.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOperatorToOpenParentheses_Valid()
    {
        _ = new Formula("1+(1)");
    }

    /// <summary>
    /// Testing an operator being followed by a closed parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorToClosedParentheses_Invalid()
    {
        _ = new Formula("(1+)");
    }

    /// <summary>
    /// Testing an operator being followed by an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorToOperator_Invalid()
    {
        _ = new Formula("1++1");
    }

    /// <summary>
    /// Testing several operators in a row.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOperatorString_Invalid()
    {
        _ = new Formula("1+*/-1");
    }

    /// <summary>
    /// Testing an open parentheses being followed by a variable.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOpenParenthesesToVariable_Valid()
    {
        _ = new Formula("(a1)");
    }

    /// <summary>
    /// Testing an open parentheses being followed by an open parentheses.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestOpenParenthesesToOpenParentheses_Valid()
    {
        _ = new Formula("((1)+1)");
    }

    /// <summary>
    /// Testing an open parentheses being followed by a closed parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpenParenthesesToClosedParentheses_Invalid()
    {
        _ = new Formula("()");
    }

    /// <summary>
    /// Testing an open parentheses being followed by an operator.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestOpenParenthesesToOperator_Invalid()
    {
        _ = new Formula("(+)");
    }

    #endregion

    #region --- Tests for Extra Following Rule ---

    /// <summary>
    /// Testing a closed parentheses being followed by an operator.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosedParenthesesToOperator_Valid()
    {
        _ = new Formula("(1)+1");
    }

    /// <summary>
    /// Testing a closed parentheses being followed by a closed parentheses.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_TestClosedParenthesesToClosedParentheses_Valid()
    {
        _ = new Formula("(1+(1))");
    }

    /// <summary>
    /// Testing a closed parentheses being followed by a number.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosedParenthesesToNumber_Invalid()
    {
        _ = new Formula("(1)1");
    }

    /// <summary>
    /// Testing a closed parentheses being followed by a variable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosedParenthesesToVariable_Invalid()
    {
        _ = new Formula("(1)a1");
    }

    /// <summary>
    /// Testing a closed parentheses being followed by an open parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestClosedParenthesesToOpenParentheses_Invalid()
    {
        _ = new Formula("(1)(1)");
    }

    /// <summary>
    /// Testing a number being followed by a number.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberToNumber_Invalid()
    {
        _ = new Formula("1 1");
    }

    /// <summary>
    /// Testing a number being followed by a variable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberToVariable_Invalid()
    {
        _ = new Formula("1 a1");
    }

    /// <summary>
    /// Testing a variable being followed by a variable.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestVariableToVariable_Invalid()
    {
        _ = new Formula("a1 a1");
    }

    /// <summary>
    /// Testing a number being followed by an open parentheses.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(FormulaFormatException))]
    public void FormulaConstructor_TestNumberToOpenParentheses_Invalid()
    {
        _ = new Formula("1(1)");
    }

    #endregion

    #endregion

    # region --- Testing ToString ---

    /// <summary>
    /// Testing that ToString() functions in the simplest form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringBase_Valid()
    {
        Assert.AreEqual(new Formula("1").ToString(), "1");
    }

    /// <summary>
    /// Testing that ToString() removes spaces.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringSpaces_Valid()
    {
        Assert.AreEqual(new Formula("1    +   1").ToString(), "1+1");
    }

    /// <summary>
    /// Testing that ToString() changes decimals to canonical form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringDecimalChange_Valid()
    {
        Assert.AreEqual(new Formula("1.00100").ToString(), "1.001");
    }

    /// <summary>
    /// Testing that ToString() does not change decimals already in canonical form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringDecimalNoChange_Valid()
    {
        Assert.AreEqual(new Formula("1.03").ToString(), "1.03");
    }

    /// <summary>
    /// Testing that ToString() changes positive exponents into canonical form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringPositiveExponent_Valid()
    {
        Assert.AreEqual(new Formula("1e3").ToString(), "1000");
    }

    /// <summary>
    /// Testing that ToString() changes negative exponents into canonical form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringNegativeExponent_Valid()
    {
        Assert.AreEqual(new Formula("1e-3").ToString(), "0.001");
    }

    /// <summary>
    /// Testing that ToString() capitalizes variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringVariableCapitalization_Valid()
    {
        Assert.AreEqual(new Formula("aa111").ToString(), "AA111");
    }

    /// <summary>
    /// Testing that ToString() capitalizes multiple variables.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_ToStringMultipleVariableCapitalization_Valid()
    {
        Assert.AreEqual(new Formula("aa111+b222+aa111").ToString(), "AA111+B222+AA111");
    }

    #endregion

    # region --- Testing GetVariables ---

    /// <summary>
    /// Testing that GetVariables() functions in the simplest form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_GetVariablesOneVariable_Valid()
    {
        Formula formula = new Formula("a1");
        Assert.IsTrue(formula.GetVariables().Contains("A1"));
    }

    /// <summary>
    /// Testing that GetVariables() functions in the simplest form.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_GetVariablesMultipleVariables_Valid()
    {
        Formula formula = new Formula("a1+b2+c3");
        Assert.IsTrue(formula.GetVariables().Contains("A1"));
        Assert.IsTrue(formula.GetVariables().Contains("B2"));
        Assert.IsTrue(formula.GetVariables().Contains("C3"));
    }

    /// <summary>
    /// Testing that GetVariables() does not count duplicates.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_GetVariablesDuplicates_Valid()
    {
        Formula formula = new Formula("a1+a1+a1");
        Assert.IsTrue(formula.GetVariables().Count == 1);
    }

    /// <summary>
    /// Testing that GetVariables() can return empty sets.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_GetVariablesEmpty_Valid()
    {
        Formula formula = new Formula("1+1");
        Assert.IsTrue(formula.GetVariables().Count == 0);
    }

    #endregion

    # region --- Testing changing Formula variables ---

    /// <summary>
    /// Testing that ToString() changes when a new formula is defined.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_DefinitionChangeToString_Valid()
    {
        Formula formula = new Formula("3+3");
        formula = new Formula("5+8");
        Assert.AreEqual(formula.ToString(), "5+8");
    }

    /// <summary>
    /// Testing that ToString() changes when formula is reassigned.
    /// </summary>
    [TestMethod]
    public void FormulaConstructor_AssignmentChangeToString_Valid()
    {
        Formula formula1 = new Formula("3+3");
        Formula formula2 = new Formula("5+8");
        formula1 = formula2;
        Assert.AreEqual(formula1.ToString(), "5+8");
    }

    #endregion

    #endregion

    #region --- PS4 Tests ---

    #region --- .Equals() Tests ---

    /// <summary>
    /// Testing that Formula can only equal other Formulas.
    /// </summary>
    [TestMethod]
    public void Equals_NonFormula_Invalid()
    {
        Formula a = new Formula("1+1");
        Assert.IsFalse(a.Equals(2));
    }

    /// <summary>
    /// Testing that Formulas with the same formula are equal.
    /// </summary>
    [TestMethod]
    public void Equals_SameEquation_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsTrue(a.Equals(b));
    }


    /// <summary>
    /// Testing that Formulas with the same formula are equal in reverse.
    /// </summary>
    [TestMethod]
    public void Equals_SameEquationReverse_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsTrue(b.Equals(a));
    }

    /// <summary>
    /// Testing that Formulas with the same formula plus whitespace are equal.
    /// </summary>
    [TestMethod]
    public void Equals_SameEquationWhiteSpace_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1    +    1");
        Assert.IsTrue(a.Equals(b));
    }

    /// <summary>
    /// Testing that Formulas with the same formula but different formats are equal.
    /// </summary>
    [TestMethod]
    public void Equals_SameEquationDoubleDifferences_Valid()
    {
        Formula a = new Formula("1+1.0");
        Formula b = new Formula("1+1");
        Formula c = new Formula("1+1e0");
        Assert.IsTrue(a.Equals(b));
        Assert.IsTrue(a.Equals(c));
    }

    /// <summary>
    /// Testing that Formulas with the same formula but different variable capitalization are equal.
    /// </summary>
    [TestMethod]
    public void Equals_SameEquationVariableDifferences_Valid()
    {
        Formula a = new Formula("a1+ba2");
        Formula b = new Formula("A1+BA2");
        Assert.IsTrue(a.Equals(b));
    }

    #endregion

    #region --- (a == b), (a != b) Tests ---

    /// <summary>
    /// Testing that Formulas with the same formula are equivalent.
    /// </summary>
    [TestMethod]
    public void EqOp_SameEquation_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsTrue(a == b);
    }

    /// <summary>
    /// Testing that Formulas with the same formula are equivalent in reverse.
    /// </summary>
    [TestMethod]
    public void EqOp_SameEquationReverse_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsTrue(b == a);
    }

    /// <summary>
    /// Testing that Formulas with the same formula plus whitespace are equivalent.
    /// </summary>
    [TestMethod]
    public void EqOp_SameEquationWhiteSpace_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1    +    1");
        Assert.IsTrue(a == b);
    }

    /// <summary>
    /// Testing that Formulas with the same formula with different formats are equivalent.
    /// </summary>
    [TestMethod]
    public void EqOp_SameEquationDoubleDifferences_Valid()
    {
        Formula a = new Formula("1+1.0");
        Formula b = new Formula("1+1");
        Formula c = new Formula("1+1e0");
        Assert.IsTrue(a == b);
        Assert.IsTrue(a == c);
    }

    /// <summary>
    /// Testing that Formulas with the same formula with different variable capitalization are equivalent.
    /// </summary>
    [TestMethod]
    public void EqOp_SameEquationVariableDifferences_Valid()
    {
        Formula a = new Formula("a1+ba2");
        Formula b = new Formula("A1+BA2");
        Assert.IsTrue(a == b);
    }

    /// <summary>
    /// Testing that Formulas with the same formula are (not not) equivalent.
    /// </summary>
    [TestMethod]
    public void NeqOp_SameEquation_Invalid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsFalse(a != b);
    }

    /// <summary>
    /// Testing that Formulas with the same formula are (not not) equivalent in reverse.
    /// </summary>
    [TestMethod]
    public void NeqOp_SameEquationReverse_Invalid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+1");
        Assert.IsFalse(b != a);
    }

    /// <summary>
    /// Testing that Formulas with different formulas are not equivalent.
    /// </summary>
    [TestMethod]
    public void NeqOp_DifferentEquation_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+2");
        Assert.IsTrue(a != b);
    }

    /// <summary>
    /// Testing that Formulas with different formulas are not equivalent in reverse.
    /// </summary>
    [TestMethod]
    public void NeqOp_DifferentEquationReverse_Valid()
    {
        Formula a = new Formula("1+1");
        Formula b = new Formula("1+2");
        Assert.IsTrue(b != a);
    }

    /// <summary>
    /// Testing that Formulas with different formulas but equivalent evaluations are not equivalent.
    /// </summary>
    [TestMethod]
    public void NeqOp_SameEquationValue_Valid()
    {
        Formula a = new Formula("1+2");
        Formula b = new Formula("2+1");
        Assert.IsTrue(a != b);
    }

    /// <summary>
    /// Testing that Formulas with different formulas with one being the evaluation of the other are not equivalent.
    /// </summary>
    [TestMethod]
    public void NeqOp_EquationValue_Valid()
    {
        Formula a = new Formula("1+2");
        Formula b = new Formula("3");
        Assert.IsTrue(a != b);
    }

    #endregion

    # region --- GetHashCode() Tests ---

    /// <summary>
    /// Testing that the hashcode between Formulas with the same formula are equivalent.
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameFormula_Valid()
    {
        Formula a = new Formula("1+2");
        Formula b = new Formula("1+2");
        Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
    }

    /// <summary>
    /// Testing that the hashcode between Formulas with the same formula plus whitespace are equivalent.
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameFormulaWhitespace_Valid()
    {
        Formula a = new Formula("1+2");
        Formula b = new Formula("1   +   2");
        Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
    }

    /// <summary>
    /// Testing that the hashcode between Formulas with the same formula but different variable capitalization are equivalent.
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameFormulaVariablesCapitalizationDifferent_Valid()
    {
        Formula a = new Formula("A1+B2");
        Formula b = new Formula("a1+b2");
        Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
    }

    /// <summary>
    /// Testing that the hashcode between Formulas with the same formula but different double formats are equivalent.
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameFormulaDoublesDifferent_Valid()
    {
        Formula a = new Formula("10");
        Formula b = new Formula("10.000");
        Formula c = new Formula("1e1");
        Assert.IsTrue(a.GetHashCode() == b.GetHashCode());
        Assert.IsTrue(a.GetHashCode() == c.GetHashCode());
    }

    /// <summary>
    /// Testing that the hashcode between Formulas with different formulas but the same evaluation are not equivalent.
    /// </summary>
    [TestMethod]
    public void GetHashCode_SameFormulaEvaluation_Invalid()
    {
        Formula a = new Formula("1+5");
        Formula b = new Formula("3+3");
        Assert.IsTrue(a.GetHashCode() != b.GetHashCode());
    }

    #endregion

    #region --- Evaluate() Tests ---

    #region --- Standard Operation Tests ---

    /// <summary>
    /// Testing the simplest formula for evaluation.
    /// </summary>
    [TestMethod]
    public void Evaluate_OneValue_Valid()
    {
        Formula a = new("1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 1, 1e-9);
    }

    /// <summary>
    /// Testing that the sum of two numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_Addition_Valid()
    {
        Formula a = new("1+1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing that the sum of multiple numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleAddition_Valid()
    {
        Formula a = new("1+1+1+1+1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 5, 1e-9);
    }

    /// <summary>
    /// Testing that the difference of two numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_Subtraction_Valid()
    {
        Formula a = new("1-1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 0, 1e-9);
    }

    /// <summary>
    /// Testing that the difference of multiple numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleSubtraction_Valid()
    {
        Formula a = new("1-1-1-1-1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), -3, 1e-9);
    }

    /// <summary>
    /// Testing that the product of two numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_Multiplication_Valid()
    {
        Formula a = new("2*2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 4, 1e-9);
    }

    /// <summary>
    /// Testing that the produce of multiple numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleMultiplication_Valid()
    {
        Formula a = new("2*2*2*2*2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 32, 1e-9);
    }

    /// <summary>
    /// Testing that the quotient of two numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_Division_Valid()
    {
        Formula a = new("2/2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 1, 1e-9);
    }

    /// <summary>
    /// Testing that the quotient of multiple numbers functions.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleDivision_Valid()
    {
        Formula a = new("32/2/2/2/2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing that mult operates before sum.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderMultFirst_Valid()
    {
        Formula a = new("2*2+1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 5, 1e-9);
    }

    /// <summary>
    /// Testing that mult operates before sum, even if sum is read first.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderMultLast_Valid()
    {
        Formula a = new("2+2*3");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 8, 1e-9);
    }

    /// <summary>
    /// Testing that mult operates before sum, where sum is in-between mult operators.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderMultBothSides_Valid()
    {
        Formula a = new("2*2+1-3*4");
        Assert.AreEqual((double)a.Evaluate(_ => 0), -7, 1e-9);
    }

    /// <summary>
    /// Testing that divide operates before sum.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderDivideFirst_Valid()
    {
        Formula a = new("2/2+1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing that divide operates before sum, even if sum is read first.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderDivideLast_Valid()
    {
        Formula a = new("1+6/3");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 3, 1e-9);
    }

    /// <summary>
    /// Testing that divide operates before sum, where sum is in-between divide operators.
    /// </summary>
    [TestMethod]
    public void Evaluate_OperationOrderDivideBothSides_Valid()
    {
        Formula a = new("2/2+1-8/4");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 0, 1e-9);
    }

    #endregion

    #region --- Parentheses Tests ---

    /// <summary>
    /// Testing parentheses around a number without operators.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesOneValue_Valid()
    {
        Formula a = new("(1)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 1, 1e-9);
    }

    /// <summary>
    /// Testing parentheses around sum.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSumWithin_Valid()
    {
        Formula a = new("(1+1)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing parentheses after sum.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSumOutBefore_Valid()
    {
        Formula a = new("1+(1)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing parentheses before sum.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSumOutAfter_Valid()
    {
        Formula a = new("(1)+1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 2, 1e-9);
    }

    /// <summary>
    /// Testing parentheses around subtract.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSubWithin_Valid()
    {
        Formula a = new("(1-1)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 0, 1e-9);
    }

    /// <summary>
    /// Testing parentheses after subtract.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSubOutBefore_Valid()
    {
        Formula a = new("1-(1)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 0, 1e-9);
    }

    /// <summary>
    /// Testing parentheses before subtract.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesSubOutAfter_Valid()
    {
        Formula a = new("(1)-1");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 0, 1e-9);
    }

    /// <summary>
    /// Testing parentheses around mult.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesMultWithin_Valid()
    {
        Formula a = new("(2*3)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 6, 1e-9);
    }

    /// <summary>
    /// Testing parentheses after mult.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesMultOutBefore_Valid()
    {
        Formula a = new("2*(3)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 6, 1e-9);
    }

    /// <summary>
    /// Testing parentheses before mult.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesMultOutAfter_Valid()
    {
        Formula a = new("(2)*3");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 6, 1e-9);
    }

    /// <summary>
    /// Testing parentheses around divide.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesDivWithin_Valid()
    {
        Formula a = new("(6/2)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 3, 1e-9);
    }

    /// <summary>
    /// Testing parentheses after divide.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesDivOutBefore_Valid()
    {
        Formula a = new("6/(2)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 3, 1e-9);
    }

    /// <summary>
    /// Testing parentheses before divide.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesDivOutAfter_Valid()
    {
        Formula a = new("(6)/2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 3, 1e-9);
    }

    /// <summary>
    /// Testing parentheses changing the order of operations for mult.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesOrderOperationMult_Valid()
    {
        Formula a = new("3*(1+2)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 9, 1e-9);
    }

    /// <summary>
    /// Testing parentheses changing the order of operations for divide.
    /// </summary>
    [TestMethod]
    public void Evaluate_ParenthesesOrderOperationDivide_Valid()
    {
        Formula a = new("3/(1+2)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 1, 1e-9);
    }

    /// <summary>
    /// Testing multiple parentheses.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleParentheses_Valid()
    {
        Formula a = new("((1))");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 1, 1e-9);
    }

    /// <summary>
    /// Testing multiple parentheses with operations within.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleParenthesesOperationWithinOne_Valid()
    {
        Formula a = new("((1+3)*2)");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 8, 1e-9);
    }

    /// <summary>
    /// Testing a long operation.
    /// </summary>
    [TestMethod]
    public void Evaluate_SampleLongFormula_Valid()
    {
        Formula a = new("3+(4/2)*2-8");
        Assert.AreEqual((double)a.Evaluate(_ => 0), -1, 1e-9);
    }

    /// <summary>
    /// Testing a longer operation.
    /// </summary>
    [TestMethod]
    public void Evaluate_SampleLongFormula2_Valid()
    {
        Formula a = new("2*(3+(4*(2/4)))-2");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 8, 1e-9);
    }

    /// <summary>
    /// Testing an even longer operation.
    /// </summary>
    [TestMethod]
    public void Evaluate_SampleLongFormula3_Valid()
    {
        Formula a = new("(1+2)-(3*4/2+(1*6)*(3-(4)))");
        Assert.AreEqual((double)a.Evaluate(_ => 0), 3, 1e-9);
    }

    #endregion

    #region --- Variable Search Tests ---

    /// <summary>
    /// Testing proper replacement of a variable via a Lookup.
    /// </summary>
    [TestMethod]
    public void Evaluate_OneVariable_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, _ => throw new ArgumentException()}; };
        Formula a = new("A1");
        Assert.AreEqual((double)a.Evaluate(lookup), 2, 1e-9);
    }

    /// <summary>
    /// Testing proper replacement of multiple variables via a Lookup.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleVariablesUppercase_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, "B2" => 4, _ => throw new ArgumentException() }; };
        Formula a = new("A1+B2");
        Assert.AreEqual((double)a.Evaluate(lookup), 6, 1e-9);
    }

    /// <summary>
    /// Testing proper replacement of multiple lowercase variables via a Lookup.
    /// </summary>
    [TestMethod]
    public void Evaluate_MultipleVariablesLowercase_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, "B2" => 4, _ => throw new ArgumentException() }; };
        Formula a = new("a1+b2");
        Assert.AreEqual((double)a.Evaluate(lookup), 6, 1e-9);
    }

    /// <summary>
    /// Testing proper replacement of the same variable multiple times via a Lookup.
    /// </summary>
    [TestMethod]
    public void Evaluate_SameVariableMultipleTimes_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, _ => throw new ArgumentException() }; };
        Formula a = new("a1*a1-a1+a1/a1");
        Assert.AreEqual((double)a.Evaluate(lookup), 3, 1e-9);
    }

    /// <summary>
    /// Testing proper replacement of a variable via a Lookup, along with an operation with a number.
    /// </summary>
    [TestMethod]
    public void Evaluate_VariablesAndNumbers_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, _ => throw new ArgumentException() }; };
        Formula a = new("a1+4.0");
        Assert.AreEqual((double)a.Evaluate(lookup), 6, 1e-9);
    }

    /// <summary>
    /// Testing proper replacement of multiple variables via a Lookup in multiple formulas.
    /// </summary>
    [TestMethod]
    public void Evaluate_DifferentVariables_Valid()
    {
        Lookup lookup = s => { return s switch { "A1" => 3, _ => 5, }; };
        Formula a = new("a1+3");
        Formula b = new("1+B1");
        Assert.AreEqual((double)a.Evaluate(lookup), (double)b.Evaluate(lookup), 1e-9);
    }

    #endregion

    #region --- FormulaError Tests ---

    /// <summary>
    /// Testing the result of FormulaError for a undefined variable.
    /// </summary>
    [TestMethod]
    public void Evaluate_VariableInvalid_FormulaErrorReturn()
    {
        Lookup lookup = s => { return s switch { _ => throw new ArgumentException() }; };
        Formula a = new("c3");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(lookup));
    }

    /// <summary>
    /// Testing the result of FormulaError for a undefined variable, even if other variables are successfully replaced.
    /// </summary>
    [TestMethod]
    public void Evaluate_VariableInvalidSomeValid_FormulaErrorReturn()
    {
        Lookup lookup = s => { return s switch { "A1" => 2, "B2" => 4, _ => throw new ArgumentException() }; };
        Formula a = new("a1+b2+c3");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(lookup));
    }

    /// <summary>
    /// Testing the result of FormulaError for division by 0.
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZero_FormulaErrorReturn()
    {
        Formula a = new("3/0");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(_ => 0));
    }

    /// <summary>
    /// Testing the result of FormulaError for division by 0 in parentheses.
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZeroParentheses_FormulaErrorReturn()
    {
        Formula a = new("3/(0)");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(_ => 0));
    }

    /// <summary>
    /// Testing the result of FormulaError for division of an operation result by 0.
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZeroNumeratorOperation_FormulaErrorReturn()
    {
        Formula a = new("(1+3)/0");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(_ => 0));
    }

    /// <summary>
    /// Testing the result of FormulaError for division by the double 0.
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZeroDouble_FormulaErrorReturn()
    {
        Formula a = new("3/0.0");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(_ => 0));
    }

    /// <summary>
    /// Testing the result of FormulaError for division by an evaluation of 0 (simplistic).
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZeroFromSimpleOperation_FormulaErrorReturn()
    {
        Formula a = new("3/(2-2)");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(_ => 0));
    }

    /// <summary>
    /// Testing the result of FormulaError for division by a variable equivalent to 0.
    /// </summary>
    [TestMethod]
    public void Evaluate_DivideByZeroFromLookup_FormulaErrorReturn()
    {
        Lookup lookup = s => { return s switch { "A1" => 0, "B2" => 4, _ => throw new ArgumentException() }; };
        Formula a = new("B2/A1");
        Assert.IsInstanceOfType<FormulaError>(a.Evaluate(lookup));
    }

    #endregion

    #endregion

    #endregion
}