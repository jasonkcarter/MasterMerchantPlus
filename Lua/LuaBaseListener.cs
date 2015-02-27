//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.3
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from C:\Code\MasterMerchantPlus\Lua\Lua.g4 by ANTLR 4.3

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591

namespace Lua {

using Antlr4.Runtime.Misc;
using IErrorNode = Antlr4.Runtime.Tree.IErrorNode;
using ITerminalNode = Antlr4.Runtime.Tree.ITerminalNode;
using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

/// <summary>
/// This class provides an empty implementation of <see cref="ILuaListener"/>,
/// which can be extended to create a listener which only needs to handle a subset
/// of the available methods.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.3")]
[System.CLSCompliant(false)]
public partial class LuaBaseListener : ILuaListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorUnary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorUnary([NotNull] LuaParser.OperatorUnaryContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorUnary"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorUnary([NotNull] LuaParser.OperatorUnaryContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.funcname"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFuncname([NotNull] LuaParser.FuncnameContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.funcname"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFuncname([NotNull] LuaParser.FuncnameContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorAnd"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorAnd([NotNull] LuaParser.OperatorAndContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorAnd"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorAnd([NotNull] LuaParser.OperatorAndContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.fieldsep"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFieldsep([NotNull] LuaParser.FieldsepContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.fieldsep"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFieldsep([NotNull] LuaParser.FieldsepContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterString([NotNull] LuaParser.StringContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.@string"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitString([NotNull] LuaParser.StringContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.functioncall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctioncall([NotNull] LuaParser.FunctioncallContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.functioncall"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctioncall([NotNull] LuaParser.FunctioncallContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.parlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterParlist([NotNull] LuaParser.ParlistContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.parlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitParlist([NotNull] LuaParser.ParlistContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.chunk"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterChunk([NotNull] LuaParser.ChunkContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.chunk"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitChunk([NotNull] LuaParser.ChunkContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.explist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExplist([NotNull] LuaParser.ExplistContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.explist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExplist([NotNull] LuaParser.ExplistContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.retstat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterRetstat([NotNull] LuaParser.RetstatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.retstat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitRetstat([NotNull] LuaParser.RetstatContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.varOrExp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVarOrExp([NotNull] LuaParser.VarOrExpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.varOrExp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVarOrExp([NotNull] LuaParser.VarOrExpContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNumber([NotNull] LuaParser.NumberContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.number"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNumber([NotNull] LuaParser.NumberContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.prefixexp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterPrefixexp([NotNull] LuaParser.PrefixexpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.prefixexp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitPrefixexp([NotNull] LuaParser.PrefixexpContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.nameAndArgs"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNameAndArgs([NotNull] LuaParser.NameAndArgsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.nameAndArgs"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNameAndArgs([NotNull] LuaParser.NameAndArgsContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.namelist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterNamelist([NotNull] LuaParser.NamelistContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.namelist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitNamelist([NotNull] LuaParser.NamelistContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.functiondef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFunctiondef([NotNull] LuaParser.FunctiondefContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.functiondef"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFunctiondef([NotNull] LuaParser.FunctiondefContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterBlock([NotNull] LuaParser.BlockContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.block"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitBlock([NotNull] LuaParser.BlockContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorComparison"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorComparison([NotNull] LuaParser.OperatorComparisonContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorComparison"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorComparison([NotNull] LuaParser.OperatorComparisonContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.varlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVarlist([NotNull] LuaParser.VarlistContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.varlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVarlist([NotNull] LuaParser.VarlistContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.exp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterExp([NotNull] LuaParser.ExpContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.exp"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitExp([NotNull] LuaParser.ExpContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.funcbody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFuncbody([NotNull] LuaParser.FuncbodyContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.funcbody"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFuncbody([NotNull] LuaParser.FuncbodyContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterStat([NotNull] LuaParser.StatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.stat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitStat([NotNull] LuaParser.StatContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorPower"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorPower([NotNull] LuaParser.OperatorPowerContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorPower"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorPower([NotNull] LuaParser.OperatorPowerContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorOr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorOr([NotNull] LuaParser.OperatorOrContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorOr"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorOr([NotNull] LuaParser.OperatorOrContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.var"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVar([NotNull] LuaParser.VarContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.var"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVar([NotNull] LuaParser.VarContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorAddSub"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorAddSub([NotNull] LuaParser.OperatorAddSubContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorAddSub"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorAddSub([NotNull] LuaParser.OperatorAddSubContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorMulDivMod"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorMulDivMod([NotNull] LuaParser.OperatorMulDivModContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorMulDivMod"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorMulDivMod([NotNull] LuaParser.OperatorMulDivModContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterLabel([NotNull] LuaParser.LabelContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.label"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitLabel([NotNull] LuaParser.LabelContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.fieldlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterFieldlist([NotNull] LuaParser.FieldlistContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.fieldlist"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitFieldlist([NotNull] LuaParser.FieldlistContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.operatorStrcat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterOperatorStrcat([NotNull] LuaParser.OperatorStrcatContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.operatorStrcat"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitOperatorStrcat([NotNull] LuaParser.OperatorStrcatContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.args"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterArgs([NotNull] LuaParser.ArgsContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.args"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitArgs([NotNull] LuaParser.ArgsContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.field"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterField([NotNull] LuaParser.FieldContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.field"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitField([NotNull] LuaParser.FieldContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.varSuffix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterVarSuffix([NotNull] LuaParser.VarSuffixContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.varSuffix"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitVarSuffix([NotNull] LuaParser.VarSuffixContext context) { }

	/// <summary>
	/// Enter a parse tree produced by <see cref="LuaParser.tableconstructor"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void EnterTableconstructor([NotNull] LuaParser.TableconstructorContext context) { }
	/// <summary>
	/// Exit a parse tree produced by <see cref="LuaParser.tableconstructor"/>.
	/// <para>The default implementation does nothing.</para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	public virtual void ExitTableconstructor([NotNull] LuaParser.TableconstructorContext context) { }

	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void EnterEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void ExitEveryRule([NotNull] ParserRuleContext context) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitTerminal([NotNull] ITerminalNode node) { }
	/// <inheritdoc/>
	/// <remarks>The default implementation does nothing.</remarks>
	public virtual void VisitErrorNode([NotNull] IErrorNode node) { }
}
} // namespace Lua
