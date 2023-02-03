using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using WSPack.VisualStudio.Shared.Options;
using WSPack.VisualStudio.Shared.Commands;

namespace WSPack.VisualStudio.Shared.UserControls
{
  /// <summary>
  /// Controle para página de opção do Template de Check In
  /// </summary>
  /// <seealso cref="System.Windows.Forms.UserControl" />
  public partial class TemplateCheckInControl : UserControl
  {
    PageTemplateCheckIn _customOptionsPage;
    string _patternMacros;

    #region Construtor
    /// <summary>
    /// Cria uma instância da classe <see cref="TemplateCheckInControl"/>
    /// </summary>
    public TemplateCheckInControl()
    {
      InitializeComponent();
    }
    #endregion

    /// <summary>
    /// Gets or sets the reference to the underlying OptionsPage object.
    /// </summary>
    public PageTemplateCheckIn OptionsPage
    {
      get { return _customOptionsPage; }
      set
      {
        _customOptionsPage = value;
        TemplateCheckIn = _customOptionsPage.TemplateCheckIn;
      }
    }

    void InsertMacro(string text)
    {
      memoTemplate.SelectedText = text;
    }

    private void menuItemCopiar_Click(object sender, EventArgs e)
    {
      CopyLocalPathBaseCommand.CopyToClipboard(Convert.ToString(menuItemCopiar.Tag));
    }

    private void menuMacros_Opening(object sender, CancelEventArgs e)
    {
      e.Cancel = menuItemCopiar.Tag == null;
    }

    private void lbChagesetId_MouseDown(object sender, MouseEventArgs e)
    {
      if (e.Button == MouseButtons.Right)
        menuItemCopiar.Tag = (sender as Label).Text;
    }

    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      _patternMacros = "";
      foreach (Label esteLabel in gbxMacros.Controls.OfType<Label>().OrderBy(x => x.Text))
      {
        ToolStripMenuItem item = new ToolStripMenuItem
        {
          Text = esteLabel.Text,
          ToolTipText = toolTipProvider.GetToolTip(esteLabel)
        };
        item.Click += (x, y) =>
        {
          InsertMacro(item.Text);
        };
        _patternMacros += item.Text + "|";
        menuMemo.Items.Add(item);
      }
      _patternMacros = "(" + _patternMacros.Substring(0, _patternMacros.Length - 1) + ")";
      memoTemplate_TextChanged(this, EventArgs.Empty);
      memoTemplate.SelectionStart = 0;
    }

    #region Propriedades    
    /// <summary>
    /// Template com informações do Check In definido pelo usuário
    /// </summary>
    public string TemplateCheckIn
    {
      get { return memoTemplate.Text; }
      set { memoTemplate.Text = value; }
    }
    #endregion

    private void lbChagesetId_DoubleClick(object sender, EventArgs e)
    {
      memoTemplate.SelectedText = ((sender as Label).Text);
    }

    private void memoTemplate_TextChanged(object sender, EventArgs e)
    {
      MatchCollection keywordMatches = Regex.Matches(memoTemplate.Text, _patternMacros);

      // saving the original caret position + forecolor
      int originalIndex = memoTemplate.SelectionStart;
      int originalLength = memoTemplate.SelectionLength;
      Color originalColor = Color.Black;

      // MANDATORY - focuses a label before highlighting (avoids blinking)
      groupBox1.Focus();

      // removes any previous highlighting (so modified words won't remain highlighted)
      memoTemplate.SelectionStart = 0;
      memoTemplate.SelectionLength = memoTemplate.Text.Length;
      memoTemplate.SelectionColor = originalColor;

      // scanning...
      foreach (Match m in keywordMatches)
      {
        memoTemplate.SelectionStart = m.Index;
        memoTemplate.SelectionLength = m.Length;
        //memoTemplate.SelectionColor = Color.Blue;
        memoTemplate.SelectionFont = new Font(memoTemplate.Font, FontStyle.Bold);
      }

      // restoring the original colors, for further writing
      memoTemplate.SelectionStart = originalIndex;
      memoTemplate.SelectionLength = originalLength;
      memoTemplate.SelectionColor = originalColor;

      // giving back the focus
      memoTemplate.Focus();
    }
  }
}
