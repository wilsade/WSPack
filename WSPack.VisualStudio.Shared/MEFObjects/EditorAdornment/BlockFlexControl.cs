using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using WSPack.Lib.CSharp.Models;

namespace WSPack.VisualStudio.Shared.MEFObjects.EditorAdornment
{
  /// <summary>
  /// Objeto 'Flex' para definir o adereço do bloco independentemente da versão do Visual Studio
  /// </summary>
  public class BlockFlexControl
  {
    const string SimboloLabel = "<<";
    const double Opacidade = 0.5;
    const int TamanhoFonte = 10;
    const int TamanhoFonteSimbolo = 9;
    const string NomeFonte = "Consolas";

    ResourceDictionary _resourceDic;

    /// <summary>
    /// Cria uma instância da classe <see cref="BlockFlexControl"/>
    /// </summary>
    private BlockFlexControl()
    {
      if (!CriarResourceDictionary(RegistroVisualStudioObj.Instance.Version))
        return;

      Block = _resourceDic["kBlock"] as TextBlock;
      LabelBlockName = (Block.Inlines.ElementAt(2) as System.Windows.Documents.InlineUIContainer).Child as Label;
      LabelSymbol = (Block.Inlines.ElementAt(0) as System.Windows.Documents.InlineUIContainer).Child as Label;

      Configure();
      SetEvents();
    }

    bool CriarResourceDictionary(string versao)
    {
      string baseUrl = $"/WSPack{versao};component/MEFObjects/EditorAdornment/BlockAdornmentDictionary.xaml";

      void Criar()
      {
        _resourceDic = new ResourceDictionary
        {
          Source = new Uri(baseUrl, UriKind.RelativeOrAbsolute)
        };
      }

      if (_resourceDic == null)
      {
        try
        {
          Criar();
        }
        catch (Exception ex)
        {
          Utils.LogDebugError($"Erro ao criar BlockAdornmentDictionary{versao}: {ex.Message}");
        }
      }
      return _resourceDic != null;
    }

    private void Configure()
    {
      Block.Opacity = LabelBlockName.Opacity = LabelSymbol.Opacity = Opacidade;
      Block.FontFamily = LabelBlockName.FontFamily = LabelSymbol.FontFamily = new FontFamily(NomeFonte);
      Block.FontSize = LabelBlockName.FontSize = TamanhoFonte;
      LabelSymbol.FontSize = TamanhoFonteSimbolo;
      LabelSymbol.Content = SimboloLabel;

      LabelSymbol.ToolTip = "Ir para o início do bloco";
      LabelSymbol.Visibility = WSPackPackage.ParametrosMEFObjects.BlocksEndObj.EnableNavigation ? Visibility.Visible : Visibility.Collapsed;
      LabelSymbol.Visibility = Visibility.Visible;
    }

    private void SetEvents()
    {
      if (WSPackPackage.ParametrosMEFObjects.BlocksEndObj.MouseHoverEffect)
      {
        Block.MouseEnter += Block_MouseEnter;
        Block.MouseLeave += Block_MouseLeave;
      }

      if (WSPackPackage.ParametrosMEFObjects.BlocksEndObj.EnableNavigation)
      {
        LabelSymbol.MouseEnter += LabelSymbol_MouseEnter;
        LabelSymbol.MouseLeave += LabelSymbol_MouseLeave;
        LabelSymbol.MouseLeftButtonUp += LabelSymbol_MouseLeftButtonUp;
      }
    }

    void LabelSymbol_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
      Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
      e.Handled = true;
      BlockModel block = LabelSymbol.Tag as BlockModel;
      try
      {
        EnvDTE80.DTE2 _dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.Shell.Interop.SDTE)) as EnvDTE80.DTE2;
        EnvDTE.TextSelection selection = (EnvDTE.TextSelection)_dte.ActiveDocument.Selection;
        selection.MoveToLineAndOffset(block.Start.Line + 1, block.Start.Column + 1);
      }
      catch (Exception ex)
      {
        System.Windows.Forms.MessageBox.Show(ex.Message);
      }
    }

    void LabelSymbol_MouseLeave(object sender, MouseEventArgs e)
    {
      LabelSymbol.Cursor = null;
    }

    void LabelSymbol_MouseEnter(object sender, MouseEventArgs e)
    {
      LabelSymbol.Cursor = Cursors.Hand;
    }

    void Block_MouseLeave(object sender, MouseEventArgs e)
    {
      LabelBlockName.Opacity = Opacidade;
      LabelBlockName.FontSize = TamanhoFonte;
      LabelSymbol.FontSize = TamanhoFonteSimbolo;
    }

    void Block_MouseEnter(object sender, MouseEventArgs e)
    {
      LabelBlockName.Opacity = 1.0;
      LabelBlockName.FontSize += 1;
      LabelBlockName.FontSize += 1;
    }

    /// <summary>
    /// Creates the block.
    /// </summary>
    /// <param name="block">The block.</param>
    /// <returns></returns>
    internal static System.Windows.UIElement CreateBlock(BlockModel block)
    {
      var flexControl = new BlockFlexControl();
      if (flexControl.LabelBlockName == null)
        return null;
      flexControl.LabelBlockName.Content = block.Name;
      flexControl.LabelSymbol.Tag = block;
      return flexControl.Block;
    }

    /// <summary>
    /// Recuperar o TextBlock que contém o LabelSymbol e o LabelBlock
    /// </summary>
    /// <returns></returns>
    TextBlock Block { get; set; }

    /// <summary>
    /// Recuperar o LabelBlock (label responsável pelo nome do block)
    /// </summary>
    Label LabelBlockName { get; set; }

    /// <summary>
    /// Recuperar o LabelSymbol (label que contém o símbolo 'menor-menor')
    /// </summary>
    Label LabelSymbol { get; set; }

  }
}