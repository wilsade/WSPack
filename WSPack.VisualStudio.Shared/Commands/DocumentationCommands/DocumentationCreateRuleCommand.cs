using System;
using System.IO;
using System.Linq;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.DocumentationObjects;
using WSPack.Lib.Extensions;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.DocumentationObjects.FileCodeModelWrapper;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

using Task = System.Threading.Tasks.Task;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class DocumentationCreateRuleCommand : BaseCommand
  {
    bool _isCommandRead = false;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.DocumentationCreateRule;

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentationCreateRuleCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected DocumentationCreateRuleCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
      _menu.BeforeQueryStatus += _menu_BeforeQueryStatus;
    }

    private void _menu_BeforeQueryStatus(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _isCommandRead = true;
      string local = null;
      if (WSPackPackage.Dte.GetActiveDocument(out var doc))
        local = doc.FullName;

      _menu.Enabled = !local.IsNullOrEmptyEx() &&
        File.Exists(local) &&
        Path.GetExtension(local).EqualsInsensitive(".cs") &&
        WSPackPackage.Dte?.ActiveDocument?.Selection is TextSelection;
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static DocumentationCreateRuleCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new DocumentationCreateRuleCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_isCommandRead)
      {
        TextSelection selection = (TextSelection)WSPackPackage.Dte.ActiveDocument.Selection;
        string texto = selection.Text;
        IBaseElementEx baseElement = selection.ActivePoint.GetTypeAtLine();
        if (baseElement == null)
          baseElement = selection.ActivePoint.GetMemberAtLine(vsCMElement.vsCMElementParameter);
        if (baseElement != null)
        {
          if (texto.IsNullOrWhiteSpaceEx())
          {
            if (selection.ActivePoint.LineLength > 0)
            {
              EditPoint ep = selection.ActivePoint.CreateEditPoint();
              int position = ep.DisplayColumn;
              ep.StartOfLine();
              string source = ep.GetText(selection.ActivePoint.LineLength);

              texto = source.GetWordAt(position);
            }
          }

          ShowForm(baseElement, texto);
        }
        else
          MessageBoxUtils.ShowWarning(ResourcesLib.StrTipoOuMembroNaoEncontrado);
      }
      else
        Utils.LogOutputMessageForceShow(ResourcesLib.StrNaoFoiPossivelRealizarEstaOperacao);
    }

    private void ShowForm(IBaseElementEx baseElement, string texto)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var form = CreateRuleForm(baseElement, out var typesEnum, out var memberEnum))
      {
        string xml = baseElement.GetDocComment();

        XMLSummaryDocObj docObj = XMLSummaryDocObj.LoadOrCreate(xml);
        if (memberEnum != null && memberEnum.Value == MemberTypesEnum.Parameters)
        {
          var lstParams = docObj.GetParamNodeList();
          var achei = lstParams.FirstOrDefault(x => x.ParamName == texto);
          if (achei != null)
            form.Self.Summary = achei.ParamSummary;
          else
            form.Self.Summary = texto;
        }
        else
        {
          form.Self.Summary = docObj.SummaryNode;
          form.Self.Returns = docObj.ReturnsNode;
          if (!docObj.ValueNode.IsNullOrWhiteSpaceEx())
            form.Self.Returns = docObj.ValueNode;
          form.Self.Remarks = docObj.RemarksNode;
        }

        form.Self.Condition = new ConditionItem(SearchConditionsEnum.Equals, texto, false);
        if (typesEnum.HasValue)
          form.Self.SetValidoPara(typesEnum.Value);
        else
          form.Self.SetValidoPara(memberEnum.Value);
        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          int id = form.Self.GetNextRuleId();
          var rule = form.CreateRule(id);
          var docParams = DocumentationUtils.ReadDocumentationParams(WSPackConsts.DocumentationConfigPath);
          docParams.RuleList.Add(rule);
          DocumentationUtils.SaveDocumentationParams(docParams, WSPackConsts.DocumentationConfigPath);
        }
      }
    }

    DocumentationRuleBaseForm CreateRuleForm(IBaseElementEx baseElement,
  out TypeTypesEnum? typeEnum, out MemberTypesEnum? memberEnum)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      typeEnum = null;
      memberEnum = null;
      if (baseElement is TypeElementEx tipo)
      {
        tipo.GetTypeType(out var tenum);
        typeEnum = tenum;
        return new DocumentationRuleTypeForm();
      }
      else if (baseElement is MemberElementEx member)
      {
        memberEnum = member.MemberType;
        return new DocumentationRuleMemberForm();
      }
      else
        throw new NotImplementedException();
    }
  }
}