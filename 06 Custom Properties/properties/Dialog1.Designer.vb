<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PropsDialog
  Inherits System.Windows.Forms.Form

  'Form overrides dispose to clean up the component list.
  <System.Diagnostics.DebuggerNonUserCode()> _
  Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    Try
      If disposing AndAlso components IsNot Nothing Then
        components.Dispose()
      End If
    Finally
      MyBase.Dispose(disposing)
    End Try
  End Sub

  'Required by the Windows Form Designer
  Private components As System.ComponentModel.IContainer

  'NOTE: The following procedure is required by the Windows Form Designer
  'It can be modified using the Windows Form Designer.  
  'Do not modify it using the code editor.
  <System.Diagnostics.DebuggerStepThrough()> _
  Private Sub InitializeComponent()
    Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    Me.ApplyButton = New System.Windows.Forms.Button()
    Me.Cancel_Button = New System.Windows.Forms.Button()
    Me.PropsListBox = New System.Windows.Forms.ListBox()
    Me.ValueTextBox = New System.Windows.Forms.TextBox()
    Me.AddButton = New System.Windows.Forms.Button()
    Me.DeleteButton = New System.Windows.Forms.Button()
    Me.SaveButton = New System.Windows.Forms.Button()
    Me.LoadButton = New System.Windows.Forms.Button()
    Me.TableLayoutPanel1.SuspendLayout()
    Me.SuspendLayout()
    '
    'TableLayoutPanel1
    '
    Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.TableLayoutPanel1.ColumnCount = 2
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.Controls.Add(Me.ApplyButton, 0, 0)
    Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
    Me.TableLayoutPanel1.Location = New System.Drawing.Point(41, 374)
    Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4)
    Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    Me.TableLayoutPanel1.RowCount = 1
    Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
    Me.TableLayoutPanel1.Size = New System.Drawing.Size(195, 36)
    Me.TableLayoutPanel1.TabIndex = 0
    '
    'ApplyButton
    '
    Me.ApplyButton.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.ApplyButton.Location = New System.Drawing.Point(4, 4)
    Me.ApplyButton.Margin = New System.Windows.Forms.Padding(4)
    Me.ApplyButton.Name = "ApplyButton"
    Me.ApplyButton.Size = New System.Drawing.Size(89, 28)
    Me.ApplyButton.TabIndex = 0
    Me.ApplyButton.Text = "Apply"
    '
    'Cancel_Button
    '
    Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
    Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
    Me.Cancel_Button.Location = New System.Drawing.Point(101, 4)
    Me.Cancel_Button.Margin = New System.Windows.Forms.Padding(4)
    Me.Cancel_Button.Name = "Cancel_Button"
    Me.Cancel_Button.Size = New System.Drawing.Size(89, 28)
    Me.Cancel_Button.TabIndex = 1
    Me.Cancel_Button.Text = "Close"
    '
    'PropsListBox
    '
    Me.PropsListBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.PropsListBox.FormattingEnabled = True
    Me.PropsListBox.ItemHeight = 16
    Me.PropsListBox.Location = New System.Drawing.Point(13, 13)
    Me.PropsListBox.Name = "PropsListBox"
    Me.PropsListBox.Size = New System.Drawing.Size(227, 244)
    Me.PropsListBox.TabIndex = 1
    '
    'ValueTextBox
    '
    Me.ValueTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
    Me.ValueTextBox.Location = New System.Drawing.Point(13, 271)
    Me.ValueTextBox.Name = "ValueTextBox"
    Me.ValueTextBox.Size = New System.Drawing.Size(227, 22)
    Me.ValueTextBox.TabIndex = 2
    '
    'AddButton
    '
    Me.AddButton.Location = New System.Drawing.Point(24, 299)
    Me.AddButton.Name = "AddButton"
    Me.AddButton.Size = New System.Drawing.Size(89, 28)
    Me.AddButton.TabIndex = 3
    Me.AddButton.Text = "Add"
    Me.AddButton.UseVisualStyleBackColor = True
    '
    'DeleteButton
    '
    Me.DeleteButton.Location = New System.Drawing.Point(141, 301)
    Me.DeleteButton.Name = "DeleteButton"
    Me.DeleteButton.Size = New System.Drawing.Size(89, 28)
    Me.DeleteButton.TabIndex = 4
    Me.DeleteButton.Text = "Delete"
    Me.DeleteButton.UseVisualStyleBackColor = True
    '
    'SaveButton
    '
    Me.SaveButton.Location = New System.Drawing.Point(24, 334)
    Me.SaveButton.Name = "SaveButton"
    Me.SaveButton.Size = New System.Drawing.Size(89, 28)
    Me.SaveButton.TabIndex = 5
    Me.SaveButton.Text = "Save"
    Me.SaveButton.UseVisualStyleBackColor = True
    '
    'LoadButton
    '
    Me.LoadButton.Location = New System.Drawing.Point(141, 335)
    Me.LoadButton.Name = "LoadButton"
    Me.LoadButton.Size = New System.Drawing.Size(89, 28)
    Me.LoadButton.TabIndex = 6
    Me.LoadButton.Text = "Load"
    Me.LoadButton.UseVisualStyleBackColor = True
    '
    'PropsDialog
    '
    Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
    Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    Me.CancelButton = Me.Cancel_Button
    Me.ClientSize = New System.Drawing.Size(252, 425)
    Me.Controls.Add(Me.LoadButton)
    Me.Controls.Add(Me.SaveButton)
    Me.Controls.Add(Me.DeleteButton)
    Me.Controls.Add(Me.AddButton)
    Me.Controls.Add(Me.ValueTextBox)
    Me.Controls.Add(Me.PropsListBox)
    Me.Controls.Add(Me.TableLayoutPanel1)
    Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    Me.Margin = New System.Windows.Forms.Padding(4)
    Me.MaximizeBox = False
    Me.MinimizeBox = False
    Me.Name = "PropsDialog"
    Me.ShowInTaskbar = False
    Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    Me.Text = "Properties"
    Me.TableLayoutPanel1.ResumeLayout(False)
    Me.ResumeLayout(False)
    Me.PerformLayout()

  End Sub
  Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
  Friend WithEvents ApplyButton As System.Windows.Forms.Button
  Friend WithEvents Cancel_Button As System.Windows.Forms.Button
  Friend WithEvents PropsListBox As Windows.Forms.ListBox
  Friend WithEvents ValueTextBox As Windows.Forms.TextBox
  Friend WithEvents AddButton As Windows.Forms.Button
  Friend WithEvents DeleteButton As Windows.Forms.Button
  Friend WithEvents SaveButton As Windows.Forms.Button
  Friend WithEvents LoadButton As Windows.Forms.Button
End Class
