using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using WSPack.Lib.CSharp.Models;
using WSPack.Lib.CSharp.Walkers;

namespace WSPack.Lib.CSharp
{
  /// <summary>
  /// Objeto para calcular métricas de um método
  /// </summary>
  public static class MethodMetricsObj
  {
    private static double CalculateMaintainablityIndex(double cyclomaticComplexity, double linesOfCode,
      HalsteadWalker.DefaultHalsteadMetrics halsteadMetrics)
    {
      if (linesOfCode == 0.0)
      {
        return 100.0;
      }
      double? volume = halsteadMetrics.GetVolume();
      double num = 1.0;
      if (volume.HasValue)
      {
        num = Math.Log(volume.Value);
      }
      double val = (171.0 - 5.2 * num - 0.23 * cyclomaticComplexity - 16.2 * Math.Log(linesOfCode)) * 100.0 / 171.0;
      return Math.Round(Math.Max(0.0, val));
    }

    static int CalculeNumOverloads(BaseMethodDeclarationSyntax method)
    {
      int conta = 0;
      if (!(method.Parent is TypeDeclarationSyntax parent))
        return conta;

      if (method is MethodDeclarationSyntax)
        conta = parent.Members.OfType<MethodDeclarationSyntax>().Count(m =>
          m.Identifier.Text.Equals(((MethodDeclarationSyntax)method).Identifier.Text));

      else if (method is ConstructorDeclarationSyntax)
        conta = parent.Members.OfType<ConstructorDeclarationSyntax>().Count(m =>
          m.Identifier.Text.Equals(((ConstructorDeclarationSyntax)method).Identifier.Text));

      return conta;
    }

    /// <summary>
    /// Calcular métricas de um método
    /// </summary>
    /// <param name="method">Method</param>
    /// <returns>Métricas</returns>
    public static MetricsModel Calculate(MethodModel method)
    {
      var node = method.MemberNode;
      int numVariavies = 0;
      var cyclomaticModel = new CyclomaticComplexityModel(1);
      var cognitiveModel = new CognitiveComplexityModel(1, false);
      var maintainabilityModel = new MaintainabilityModel(100);
      var linesOfCodeModel = new LinesOfCodeModel(1);

      // Complexidade ciclomática
      var t1 = Task.Run(() =>
      {
        cyclomaticModel.Value = CyclomaticComplexityWalker.Calculate(node);
      });

      // Linhas de código
      var t2 = Task.Run(() =>
      {
        linesOfCodeModel.Value = StatementsWalker.CalculateLineNumbers(node);
      });

      // Número de variáveis
      var t3 = Task.Run(() =>
      {
        numVariavies = MethodLocalVariablesWalker.Calculate(node);
      });

      var t4 = Task.Run(() =>
        {
          var num = CognitiveComplexityWalker.Calculate(node.SyntaxNode);
          cognitiveModel = new CognitiveComplexityModel(num, method.IsGetterOrSetter);
        });

      Task.WaitAll(t1, t2, t3, t4);

      // Índice de manutenção
      HalsteadWalker.DefaultHalsteadMetrics HalsteadMetrics = HalsteadWalker.Calculate(node);
      if (HalsteadMetrics != null)
      {
        maintainabilityModel.Value = CalculateMaintainablityIndex(cyclomaticModel.Value, linesOfCodeModel.Value, HalsteadMetrics);
      }

      // Número de overloads
      int numOverloads = 0;
      int numParametros = 0;
      if (node.SyntaxNode is BaseMethodDeclarationSyntax baseNode)
      {
        numParametros = baseNode.ParameterList.Parameters.Count;
        numOverloads = CalculeNumOverloads(baseNode);
      }

      var metrics = new MetricsModel
      {
        CyclomaticComplexity = cyclomaticModel,
        Maintainability = maintainabilityModel,
        LinesOfCode = linesOfCodeModel,
        NumOfLocalVariables = numVariavies,
        NumOfOverloads = numOverloads,
        NumOfParameters = numParametros,
        CognitiveComplexity = cognitiveModel
      };
      return metrics;
    }

  }
}
