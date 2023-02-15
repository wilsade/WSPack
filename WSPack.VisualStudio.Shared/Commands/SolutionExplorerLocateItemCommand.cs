using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using EnvDTE;

using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.VisualStudio.Shell;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Forms;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;

using Task = System.Threading.Tasks.Task;
using mvsi = Microsoft.VisualStudio.Shell.Interop;

namespace WSPack.VisualStudio.Shared.Commands
{
  /// <summary>
  /// Comando para exibição do Sobre
  /// </summary>
  internal sealed class SolutionExplorerLocateItemCommand : BaseCommand
  {
    (UIHierarchyItem Item, string FullName) _ultimoItem;
    Dictionary<string, object> _dicItens;
    string _matchItem;

    /// <summary>
    /// Identificador do comando
    /// </summary>
    public override int CommandId => CommandIds.SolutionExplorerLocalizarItem;

    /// <summary>
    /// Initializes a new instance of the <see cref="SolutionExplorerLocateItemCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected SolutionExplorerLocateItemCommand(AsyncPackage package, OleMenuCommandService commandService)
      : base(package, commandService)
    {
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static SolutionExplorerLocateItemCommand Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package, OleMenuCommandService commandService)
    {
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
      Instance = new SolutionExplorerLocateItemCommand(package, commandService);
    }

    protected override void DoExecute(object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      _dicItens = null;
      using (var form = new SolutionExplorerLocateItemForm(GetAllItens))
      {
        if (_ultimoItem.Item != null)
          form.Item = _ultimoItem.FullName;
        if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
          _matchItem = null;
          var dic = GetAllItensSearching(form.Item);
          if (dic.TryGetValue(form.Item, out object achei) || (_matchItem != null && dic.TryGetValue(_matchItem, out achei)))
          {
            _ultimoItem = ((UIHierarchyItem)achei, form.Item);
            ClearSearchFilter();
            ((UIHierarchyItem)achei).Select(vsUISelectionType.vsUISelectionTypeSelect);
          }
          else
            MessageBoxUtils.ShowWarning(ResourcesLib.StrItemNaoEncontrado.FormatWith(form.Item));
        }
      }
    }

    private void ClearSearchFilter()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      try
      {
        mvsi.IVsUIHierarchyWindow solutionExplorer = VsShellUtilities.GetUIHierarchyWindow(_package, Microsoft.VisualStudio.VSConstants.StandardToolWindows.SolutionExplorer);
        var window = solutionExplorer as mvsi.IVsWindowSearch;
        window?.ClearSearch();
      }
      catch (Exception ex)
      {
        Utils.LogDebugError($"Erro ao limpar filtro do solution explorer: {ex.GetCompleteMessage()}");
      }
    }

    Dictionary<string, object> GetAllItens()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      return GetAllItensSearching(null);
    }

    Dictionary<string, object> GetAllItensSearching(string searchItem)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (_dicItens != null)
        return _dicItens;
      _breakRecurse = false;
      _dicItens = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
      ClearSearchFilter();
      LoadAllItensRecursive(WSPackPackage.Dte.ToolWindows.SolutionExplorer.UIHierarchyItems, _dicItens, searchItem);
      Utils.LogDebugMessage($"Total de itens: {_dicItens.Count}");
      return _dicItens;
    }

    bool _breakRecurse;
    void LoadAllItensRecursive(UIHierarchyItems itens, Dictionary<string, object> dic, string searchItem)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      foreach (UIHierarchyItem esteItem in itens)
      {
        if (_breakRecurse)
          return;

        bool needExpand = false;
        if (esteItem.Object is Project acheiProjeto)
        {
          needExpand = true;
          if (!acheiProjeto.FullName.IsNullOrWhiteSpaceEx())
          {
            if (!dic.ContainsKey(acheiProjeto.FullName))
              dic.Add(acheiProjeto.FullName, esteItem);
            if (acheiProjeto.FullName.EndsWithInsensitive(searchItem))
            {
              _matchItem = acheiProjeto.FullName;
              _breakRecurse = true;
              return;
            }
          }
        }

        else if (esteItem.Object is ProjectItem projectItem)
        {
          needExpand = true;
          for (int i = 1; i <= projectItem.FileCount; i++)
          {
            string nome = "";
            try
            {
              nome = projectItem.FileNames[(short)i];
            }
            catch (Exception ex)
            {
              Utils.LogDebugError(ex.ToString());
              try
              {
                nome = projectItem.FileNames[0];
              }
              catch (Exception ex2)
              {
                Utils.LogDebugError(ex2.ToString());
              }
            }
            if (!nome.IsNullOrWhiteSpaceEx())
            {
              if (!dic.ContainsKey(nome))
                dic.Add(nome, esteItem);
              if (nome.EndsWithInsensitive(searchItem))
              {
                _matchItem = nome;
                _breakRecurse = true;
                return;
              }
            }
          }
        }
        else
        {
          Utils.LogDebugMessage($"Item não inserido no dicionário: {esteItem.Name}");
        }

        if (_breakRecurse)
          return;

        var childItens = esteItem.UIHierarchyItems;
        bool antes = childItens.Expanded;
        try
        {
          if (needExpand)
            childItens.Expanded = true;
          LoadAllItensRecursive(childItens, dic, searchItem);
        }
        finally
        {
          childItens.Expanded = antes;
        }
      }
    }
  }
}