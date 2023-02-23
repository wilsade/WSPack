using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Internals;

namespace WSPack.Lib.CSharp.Walkers
{
  /// <summary>
  /// HalsteadWalker
  /// </summary>
  /// <seealso cref="CSharpSyntaxWalker" />
  internal sealed class HalsteadWalker : CSharpSyntaxWalker
  {
    DefaultHalsteadMetrics _metrics;

    internal class DefaultHalsteadMetrics
    {
      public static readonly DefaultHalsteadMetrics GenericInstanceSetPropertyMetrics = new DefaultHalsteadMetrics
      {
        NumOperands = 5,
        NumOperators = 3,
        NumUniqueOperands = 4,
        NumUniqueOperators = 3
      };

      public static readonly DefaultHalsteadMetrics GenericStaticSetPropertyMetrics = new DefaultHalsteadMetrics
      {
        NumOperands = 4,
        NumOperators = 3,
        NumUniqueOperands = 3,
        NumUniqueOperators = 3
      };

      public static readonly DefaultHalsteadMetrics GenericInstanceGetPropertyMetrics = new DefaultHalsteadMetrics
      {
        NumOperands = 3,
        NumOperators = 2,
        NumUniqueOperands = 3,
        NumUniqueOperators = 2
      };

      public static readonly DefaultHalsteadMetrics GenericStaticGetPropertyMetrics = new DefaultHalsteadMetrics
      {
        NumOperands = 2,
        NumOperators = 1,
        NumUniqueOperands = 2,
        NumUniqueOperators = 1
      };

      public int NumOperands { get; internal set; }

      public int NumUniqueOperands { get; internal set; }

      public int NumOperators { get; internal set; }

      public int NumUniqueOperators { get; internal set; }

      public int GetLength()
      {
        return checked(this.NumOperators + this.NumOperands);
      }

      public int GetVocabulary()
      {
        return checked(this.NumUniqueOperators + this.NumUniqueOperands);
      }

      public double GetDifficulty()
      {
        return (double)this.NumUniqueOperators / 2.0 * ((double)this.NumOperands / (double)this.NumUniqueOperands);
      }

      public double? GetVolume()
      {
        double newBase = 2.0;
        double num = (double)this.GetVocabulary();
        double num2 = (double)this.GetLength();
        if (num == 0.0)
        {
          return null;
        }
        return new double?(num2 * Math.Log(num, newBase));
      }

      public double? GetEffort()
      {
        double difficulty = this.GetDifficulty();
        double? volume = this.GetVolume();
        if (!volume.HasValue)
        {
          return null;
        }
        double num = difficulty;
        double? num2 = volume;
        if (!num2.HasValue)
        {
          return null;
        }
        return new double?(num * num2.GetValueOrDefault());
      }

      public double? GetBugs()
      {
        double? volume = this.GetVolume();
        if (!volume.HasValue)
        {
          return null;
        }
        double? num = volume;
        if (!num.HasValue)
        {
          return null;
        }
        return new double?(num.GetValueOrDefault() / 3000.0);
      }
    }

    /// <summary>
    /// HalsteadOperands
    /// </summary>
    internal class HalsteadOperands
    {
      /// <summary>
      /// All operands
      /// </summary>
      public static readonly IEnumerable<SyntaxKind> All = new SyntaxKind[]
      {
      SyntaxKind.IdentifierToken,
      SyntaxKind.StringLiteralToken,
      SyntaxKind.NumericLiteralToken,
      SyntaxKind.AddKeyword,
      SyntaxKind.AliasKeyword,
      SyntaxKind.AscendingKeyword,
      SyntaxKind.AsKeyword,
      SyntaxKind.AsyncKeyword,
      SyntaxKind.AwaitKeyword,
      SyntaxKind.BaseKeyword,
      SyntaxKind.BoolKeyword,
      SyntaxKind.BreakKeyword,
      SyntaxKind.ByKeyword,
      SyntaxKind.ByteKeyword,
      SyntaxKind.CaseKeyword,
      SyntaxKind.CatchKeyword,
      SyntaxKind.CharKeyword,
      SyntaxKind.CheckedKeyword,
      SyntaxKind.ChecksumKeyword,
      SyntaxKind.ClassKeyword,
      SyntaxKind.ConstKeyword,
      SyntaxKind.ContinueKeyword,
      SyntaxKind.DecimalKeyword,
      SyntaxKind.DefaultKeyword,
      SyntaxKind.DefineKeyword,
      SyntaxKind.DelegateKeyword,
      SyntaxKind.DescendingKeyword,
      SyntaxKind.DisableKeyword,
      SyntaxKind.DoKeyword,
      SyntaxKind.DoubleKeyword,
      SyntaxKind.ElifKeyword,
      SyntaxKind.ElseKeyword,
      SyntaxKind.EndIfKeyword,
      SyntaxKind.EndRegionKeyword,
      SyntaxKind.EnumKeyword,
      SyntaxKind.EqualsKeyword,
      SyntaxKind.ErrorKeyword,
      SyntaxKind.EventKeyword,
      SyntaxKind.ExplicitKeyword,
      SyntaxKind.ExternKeyword,
      SyntaxKind.FalseKeyword,
      SyntaxKind.FieldKeyword,
      SyntaxKind.FinallyKeyword,
      SyntaxKind.FixedKeyword,
      SyntaxKind.FloatKeyword,
      SyntaxKind.ForEachKeyword,
      SyntaxKind.ForKeyword,
      SyntaxKind.FromKeyword,
      SyntaxKind.GetKeyword,
      SyntaxKind.GlobalKeyword,
      SyntaxKind.GotoKeyword,
      SyntaxKind.GroupKeyword,
      SyntaxKind.HiddenKeyword,
      SyntaxKind.IfKeyword,
      SyntaxKind.ImplicitKeyword,
      SyntaxKind.InKeyword,
      SyntaxKind.InterfaceKeyword,
      SyntaxKind.InternalKeyword,
      SyntaxKind.IntKeyword,
      SyntaxKind.IntoKeyword,
      SyntaxKind.IsKeyword,
      SyntaxKind.JoinKeyword,
      SyntaxKind.LetKeyword,
      SyntaxKind.LineKeyword,
      SyntaxKind.LockKeyword,
      SyntaxKind.LongKeyword,
      SyntaxKind.MakeRefKeyword,
      SyntaxKind.MethodKeyword,
      SyntaxKind.ModuleKeyword,
      SyntaxKind.NamespaceKeyword,
      SyntaxKind.NullKeyword,
      SyntaxKind.ObjectKeyword,
      SyntaxKind.OnKeyword,
      SyntaxKind.OperatorKeyword,
      SyntaxKind.OrderByKeyword,
      SyntaxKind.OutKeyword,
      SyntaxKind.OverrideKeyword,
      SyntaxKind.ParamKeyword,
      SyntaxKind.ParamsKeyword,
      SyntaxKind.PartialKeyword,
      SyntaxKind.PragmaKeyword,
      SyntaxKind.PrivateKeyword,
      SyntaxKind.PropertyKeyword,
      SyntaxKind.ProtectedKeyword,
      SyntaxKind.PublicKeyword,
      SyntaxKind.ReadOnlyKeyword,
      SyntaxKind.ReferenceKeyword,
      SyntaxKind.RefKeyword,
      SyntaxKind.RefTypeKeyword,
      SyntaxKind.RefValueKeyword,
      SyntaxKind.RegionKeyword,
      SyntaxKind.RemoveKeyword,
      SyntaxKind.RestoreKeyword,
      SyntaxKind.ReturnKeyword,
      SyntaxKind.SByteKeyword,
      SyntaxKind.SealedKeyword,
      SyntaxKind.SelectKeyword,
      SyntaxKind.SetKeyword,
      SyntaxKind.ShortKeyword,
      SyntaxKind.SizeOfKeyword,
      SyntaxKind.StackAllocKeyword,
      SyntaxKind.StaticKeyword,
      SyntaxKind.StringKeyword,
      SyntaxKind.StructKeyword,
      SyntaxKind.SwitchKeyword,
      SyntaxKind.ThisKeyword,
      SyntaxKind.TrueKeyword,
      SyntaxKind.TryKeyword,
      SyntaxKind.TypeKeyword,
      SyntaxKind.TypeOfKeyword,
      SyntaxKind.TypeVarKeyword,
      SyntaxKind.UIntKeyword,
      SyntaxKind.ULongKeyword,
      SyntaxKind.UncheckedKeyword,
      SyntaxKind.UndefKeyword,
      SyntaxKind.UnsafeKeyword,
      SyntaxKind.UShortKeyword,
      SyntaxKind.UsingKeyword,
      SyntaxKind.VirtualKeyword,
      SyntaxKind.VoidKeyword,
      SyntaxKind.VolatileKeyword,
      SyntaxKind.WarningKeyword,
      SyntaxKind.WhereKeyword,
      SyntaxKind.WhileKeyword,
      SyntaxKind.YieldKeyword
      };

    }

    /// <summary>
    /// HalsteadOperators
    /// </summary>
    internal class HalsteadOperators
    {
      /// <summary>
      /// All operators
      /// </summary>
      public static readonly IEnumerable<SyntaxKind> All = new SyntaxKind[]
      {
      SyntaxKind.DotToken,
      SyntaxKind.EqualsToken,
      SyntaxKind.SemicolonToken,
      SyntaxKind.PlusPlusToken,
      SyntaxKind.PlusToken,
      SyntaxKind.PlusEqualsToken,
      SyntaxKind.MinusMinusToken,
      SyntaxKind.MinusToken,
      SyntaxKind.MinusEqualsToken,
      SyntaxKind.AsteriskToken,
      SyntaxKind.AsteriskEqualsToken,
      SyntaxKind.SlashToken,
      SyntaxKind.SlashEqualsToken,
      SyntaxKind.PercentToken,
      SyntaxKind.PercentEqualsToken,
      SyntaxKind.AmpersandToken,
      SyntaxKind.BarToken,
      SyntaxKind.CaretToken,
      SyntaxKind.TildeToken,
      SyntaxKind.ExclamationToken,
      SyntaxKind.ExclamationEqualsToken,
      SyntaxKind.GreaterThanToken,
      SyntaxKind.GreaterThanEqualsToken,
      SyntaxKind.LessThanToken,
      SyntaxKind.LessThanEqualsToken
      };

    }


    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="HalsteadWalker"/>
    /// </summary>
    private HalsteadWalker()
      : base(SyntaxWalkerDepth.Node)
    { }
    #endregion

    /// <summary>
    /// Calcular a métrica
    /// </summary>
    /// <param name="node">The node.</param>
    /// <returns></returns>
    public static DefaultHalsteadMetrics Calculate(RoslynMemberNode node)
    {
      var walker = new HalsteadWalker();

      BlockSyntax body = MemberBodySelector.FindBody(node);
      if (body != null)
      {
        walker.Visit(body);
        return walker._metrics;
      }

      if (walker.CalculateGenericPropertyMetrics(node))
      {
        return walker._metrics;
      }
      return new DefaultHalsteadMetrics();
    }

    private bool CalculateGenericPropertyMetrics(RoslynMemberNode node)
    {
      PropertyDeclarationSyntax propertyDeclarationSyntax;
      if ((propertyDeclarationSyntax = (node.SyntaxNode as PropertyDeclarationSyntax)) != null)
      {
        bool flag = propertyDeclarationSyntax.Modifiers.Any((SyntaxToken x) => x.ValueText == "static");
        if (MemberBodySelector.FindBody(node) == null)
        {
          if (node.Kind == MemberKindEnum.GetProperty)
          {
            _metrics = (flag ? DefaultHalsteadMetrics.GenericStaticGetPropertyMetrics : DefaultHalsteadMetrics.GenericInstanceGetPropertyMetrics);
            return true;
          }

          else if (node.Kind == MemberKindEnum.SetProperty)
          {
            _metrics = (flag ? DefaultHalsteadMetrics.GenericStaticSetPropertyMetrics : DefaultHalsteadMetrics.GenericInstanceSetPropertyMetrics);
            return true;
          }
        }
      }
      return false;
    }

    /// <summary>
    /// Called when the visitor visits a BlockSyntax node.
    /// </summary>
    /// <param name="node"></param>
    public override void VisitBlock(BlockSyntax node)
    {
      base.VisitBlock(node);
      IEnumerable<SyntaxToken> tokens = node.DescendantTokens(null, false).ToList();
      IDictionary<SyntaxKind, IList<string>> dictionary = HalsteadWalker.ParseTokens(tokens, HalsteadOperands.All);
      IDictionary<SyntaxKind, IList<string>> dictionary2 = HalsteadWalker.ParseTokens(tokens, HalsteadOperators.All);
      DefaultHalsteadMetrics halsteadMetrics = new DefaultHalsteadMetrics
      {
        NumOperands = dictionary.Values.SelectMany((IList<string> x) => x).Count(),
        NumUniqueOperands = dictionary.Values.SelectMany((IList<string> x) => x).Distinct().Count(),
        NumOperators = dictionary2.Values.SelectMany((IList<string> x) => x).Count(),
        NumUniqueOperators = dictionary2.Values.SelectMany((IList<string> x) => x).Distinct().Count()
      };
      _metrics = halsteadMetrics;
    }

    private static IDictionary<SyntaxKind, IList<string>> ParseTokens(IEnumerable<SyntaxToken> tokens, IEnumerable<SyntaxKind> filter)
    {
      IDictionary<SyntaxKind, IList<string>> dictionary = new Dictionary<SyntaxKind, IList<string>>();
      foreach (SyntaxToken current in tokens)
      {
        SyntaxKind kind = current.Kind();
        if (filter.Any((SyntaxKind x) => x == kind))
        {
          string valueText = current.ValueText;
          if (!dictionary.TryGetValue(kind, out IList<string> list))
          {
            dictionary[kind] = new List<string>();
            list = dictionary[kind];
          }
          list.Add(valueText);
        }
      }
      return dictionary;
    }

  }
}
