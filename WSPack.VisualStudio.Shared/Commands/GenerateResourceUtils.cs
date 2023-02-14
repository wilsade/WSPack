using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using EnvDTE;

using Microsoft.VisualStudio.Shell;

using VSLangProj;

using WSPack.Lib;
using WSPack.Lib.Extensions;
using WSPack.Lib.Items;
using WSPack.Lib.Properties;
using WSPack.VisualStudio.Shared.Extensions;
using WSPack.VisualStudio.Shared.Forms;

namespace WSPack.VisualStudio.Shared.Commands
{
  static class GenerateResourceUtils
  {
    static Dictionary<string, GenerateResourceParams> _dicParams;

    static GenerateResourceUtils()
    {
      _dicParams = new Dictionary<string, GenerateResourceParams>(StringComparer.OrdinalIgnoreCase);
    }

    internal static List<ResourcesFileItem> GetResources()
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      var lst = new List<ResourcesFileItem>();
      void GetResources(ProjectItems lstItems)
      {
        if (lstItems != null)
        {
          foreach (ProjectItem item in lstItems)
          {
            string nome = item.Name;
            if (nome.EndsWithInsensitive(".resx"))
            {
              lst.Add(new ResourcesFileItem(item, item.FileNames[0]));
            }

            string subType = item.GetProperty("SubType") as string;
            if (!((subType == @"Form") || (subType == @"UserControl")))
              GetResources(item.ProjectItems);
          }
        }
      }

      ProjectItem projectItem = WSPackPackage.Dte.ActiveDocument.ProjectItem;
      Project project = projectItem.ContainingProject;
      GetResources(project.ProjectItems);
      return lst.OrderBy(x => x.ResxFileName).ToList();
    }

    internal static void ShowForm(TextSelection selection, List<ResourcesFileItem> lstResources, string palavra)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      using (var form = new GenerateResourceForm())
      {
        form.Self.SetResourcesFiles(lstResources);
        form.Self.ArquivoResource = lstResources.FirstOrDefault(x => x.IsExpectedDefaultResourceFileName);
        if (form.Self.ArquivoResource == null)
          form.Self.ArquivoResource = lstResources.First();
        form.Self.ValorResource = palavra;
        form.Self.NomeResource = palavra;

        #region Parâmetros da última geração
        string projFullName = selection.Parent?.Parent?.ProjectItem?.ContainingProject?.FullName ?? "";

        GenerateResourceParams usedParam = ReadLastUsed(selection.Parent?.DTE?.Solution?.FullName);
        ProjectUsedResource projetoUsado = usedParam.RecentsProjects.FirstOrDefault(x => x.ProjectFullName.EqualsInsensitive(projFullName));

        if (projetoUsado == null)
          projetoUsado = new ProjectUsedResource(projFullName, form.Self.ArquivoResource?.ResxFileName);
        else if (File.Exists(projetoUsado.LastResxFileUsed))
          form.Self.ArquivoResource = lstResources.FirstOrDefault(x => x.ResxFileName.EqualsInsensitive(projetoUsado.LastResxFileUsed));
        form.Self.Prefixo = usedParam.Prefixo;
        #endregion

        if (form.ShowDialog(out var entry, out bool createNew))
        {
          usedParam.Prefixo = form.Self.Prefixo;
          projetoUsado.LastResxFileUsed = form.Self.ArquivoResource.ResxFileName;
          if (!usedParam.RecentsProjects.Any(x => x.ProjectFullName.EqualsInsensitive(projetoUsado.ProjectFullName)))
            usedParam.RecentsProjects.Add(projetoUsado);

          try
          {
            if (createNew)
            {
              bool canSave = true;
              if (form.Self.ArquivoResource.ResxFileName.IsReadOnlyFile())
              {
                canSave = false;
                var tfs = WSPackPackage.Dte.GetTeamFoundationServerExt();
                if (tfs?.IsSourceControlExplorerActive() == true)
                {
                  var vcServer = tfs.GetVersionControlServer();
                  if (vcServer != null && vcServer.GetWorkspaceForLocalItem(form.Self.ArquivoResource.ResxFileName,
                    out var ws, out string serverItem))
                  {
                    int number = ws.PendEdit(serverItem);
                    canSave = number == 1;
                  }
                }
              }
              if (canSave)
                AddResource(form.Self.ArquivoResource, entry);
              else
                MessageBoxUtils.ShowWarning(ResourcesLib.StrArquivoResourceSomenteLeitura);
            }
          }
          catch (Exception ex)
          {
            MessageBoxUtils.ShowError(ResourcesLib.StrNaoFoiPossivelGerarResource.FormatWith(ex.Message));
          }
          selection.Text = form.Self.CodigoGerado;
          selection.CharLeft();
          selection.CharRight();
        }
      }
    }

    /// <summary>
    /// Parâmetros da última geração
    /// </summary>
    /// <returns>Parâmetros da última geração</returns>
    static GenerateResourceParams ReadLastUsed(string solutionFullName)
    {
      if (solutionFullName == null)
        solutionFullName = "";
      solutionFullName = Path.GetFileName(solutionFullName);

      GenerateResourceParams usedParam;
      if (_dicParams.ContainsKey(solutionFullName))
        usedParam = _dicParams[solutionFullName];
      else
      {
        usedParam = new GenerateResourceParams(solutionFullName, "");
        string auxPrefix = usedParam.SolutionPath.Replace(".", "").Replace("-", "");
        var str = new StringBuilder();
        for (int i = 0; i < 3; i++)
        {
          str.Append(auxPrefix.Substring(i, 1));
        }
        usedParam.Prefixo = $"S{str.ToString()}";
        _dicParams.Add(solutionFullName, usedParam);
      }
      return usedParam;
    }

    static void AddResource(ResourcesFileItem resourcesFileItem, ResourceEntry entry)
    {
      var doc = new XmlDocument();
      doc.Load(resourcesFileItem.ResxFileName);

      XmlNode nodeData = doc.CreateNode(XmlNodeType.Element, "data", "");

      XmlAttribute attName = doc.CreateAttribute("name");
      attName.Value = entry.Name;
      nodeData.Attributes.Append(attName);

      XmlAttribute attxmlspace = doc.CreateAttribute("space", "xml");
      attxmlspace.Prefix = "xml";
      attxmlspace.Value = "preserve";
      nodeData.Attributes.Append(attxmlspace);

      XmlNode nodeValue = doc.CreateNode(XmlNodeType.Element, "value", "");
      nodeValue.InnerText = entry.Value;
      nodeData.AppendChild(nodeValue);

      if (!entry.Comment.IsNullOrWhiteSpaceEx())
      {
        XmlNode nodeComment = doc.CreateNode(XmlNodeType.Element, "comment", "");
        nodeComment.InnerText = entry.Comment;
        nodeData.AppendChild(nodeComment);
      }

      XmlNode root = doc["root"];
      if (root != null)
      {
        root.AppendChild(nodeData);
        doc.Save(resourcesFileItem.ResxFileName);
        if (resourcesFileItem.Parent?.Object is VSProjectItem vSProject)
          vSProject.RunCustomTool();
      }
    }

    internal static string LocateString(SelectionReg selection, bool moveSelection)
    {
      ThreadHelper.ThrowIfNotOnUIThread();
      if (selection == null)
        return null;

      var secondQuote = -1;
      var column = selection.Begin.LineCharOffset - 1;
      var line = selection.Line;
      if (string.IsNullOrEmpty(line))
        return null;

      while (secondQuote < line.Length)
      {
        var firstQuote = line.IndexOf(@"""", secondQuote + 1, StringComparison.Ordinal);
        if (firstQuote == -1)
          return null;

        if (line.Length <= firstQuote + 1)
          return null;

        if (column < firstQuote)
          return null;

        secondQuote = line.IndexOf(@"""", firstQuote + 1, StringComparison.Ordinal);
        if (secondQuote == -1)
          return null;

        if (column >= secondQuote)
          continue;

        var startIndex = firstQuote + 1;
        var length = secondQuote - firstQuote - 1;

        if (moveSelection)
        {
          var startColumn = firstQuote + 1;
          var endColumn = secondQuote + 2;

          selection.MoveTo(startColumn, endColumn);
        }

        return line.Substring(startIndex, length);
      }

      return null;
    }
  }
}