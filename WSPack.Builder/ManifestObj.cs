using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

using WSPack.Lib;

namespace WSPack.Builder
{
  public class ManifestObj
  {
    const string Description =
"Pacote de funcionalidades para Visual Studio, como por exemplo:\n" +
"- StartPage, Métricas, Marcadores numerados, Geração de documentação (summary), Geração de ResourceString, Entre outros...";
    const string MoreInfo = Constantes.GitHubWSPack;
    static readonly string GettingStartedGuide = Constantes.GitHubWSPackWiki;
    static readonly string ReleaseNotes = Constantes.GitHubWSPackReleaseNotes;
    const string Icon = "Resources\\WSPackLogo.ico";
    const string Tags = "StartPage TFS GIT Favorite Locate Merge Bookmark SourceControlExplorer documentation summary metrics cyclomatic cognitive maintainablity margin search copy Resource TFSOffLine";

    public static void UpdateManifest(string baseDir)
    {
      var files = Directory.EnumerateFiles(baseDir, "source.extension.vsixmanifest", SearchOption.AllDirectories);
      if (!files.Any())
      {
        CommandClass.WriteWarning("Não foram encontrados arquivos manifest");
        return;
      }

      int alteracoes;
      foreach (var item in files)
      {
        Console.WriteLine();
        Console.WriteLine($"Verificando: {item}");
        alteracoes = 0;
        var doc = XDocument.Load(item);
        var root = doc.Root;
        XNamespace ns = doc.Root.Name.Namespace;
        XElement metaDataNode = root.Element(ns + "Metadata");
        alteracoes += CheckIdentityAttributes(metaDataNode.Element(ns + "Identity"));
        alteracoes += CheckNodeValue(metaDataNode, nameof(Description), Description);
        alteracoes += CheckNodeValue(metaDataNode, nameof(MoreInfo), MoreInfo);
        alteracoes += CheckNodeValue(metaDataNode, nameof(GettingStartedGuide), GettingStartedGuide);
        alteracoes += CheckNodeValue(metaDataNode, nameof(ReleaseNotes), ReleaseNotes);
        alteracoes += CheckNodeValue(metaDataNode, nameof(Icon), Icon);
        alteracoes += CheckNodeValue(metaDataNode, nameof(Tags), Tags);
        if (alteracoes > 0)
        {
          doc.Save(item);
          CommandClass.WriteSuccess("Salvo");
        }
      }
    }

    private static int CheckIdentityAttributes(XElement nodoIdentity)
    {
      int retorno = 0;
      XAttribute attribute = nodoIdentity.Attribute("Version");
      if (attribute.Value != Constantes.NumeroVersao)
      {
        CommandClass.Write($"  Alterado: Version", ConsoleColor.Blue);
        attribute.Value = Constantes.NumeroVersao;
        retorno++;
      }

      attribute = nodoIdentity.Attribute("Publisher");
      if (attribute.Value != Constantes.WilliamSadeDePaiva)
      {
        CommandClass.Write($"  Alterado: Publisher", ConsoleColor.Blue);
        attribute.Value = Constantes.WilliamSadeDePaiva;
        retorno++;
      }
      return retorno;
    }

    private static int CheckNodeValue(XElement metaDataNode, string nodeName, string expectedValue)
    {
      XNamespace ns = metaDataNode.GetDefaultNamespace();
      XElement node = metaDataNode.Element(ns + nodeName);
      if (node == null)
      {
        node = new XElement(ns + nodeName)
        {
          Value = expectedValue
        };
        metaDataNode.Add(node);
        CommandClass.WriteWarning($"  Criado: {nodeName}");
        return 1;
      }

      if (node.Value.Equals(expectedValue))
        return 0;

      CommandClass.Write($"  Alterado: {nodeName}", ConsoleColor.Blue);
      node.Value = expectedValue;
      return 1;
    }
  }
}
