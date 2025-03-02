namespace Tic_tac_toe_game
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxCuttingOffBranches = new System.Windows.Forms.CheckBox();
            this.btnNewGame = new System.Windows.Forms.Button();
            this.btnEngineRunning = new System.Windows.Forms.Button();
            this.listBoxListOfRatings = new System.Windows.Forms.ListBox();
            this.btnTestPosition = new System.Windows.Forms.Button();
            this.buttonArray = new System.Windows.Forms.Button();
            this.pictureBoxGameBoard = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxCuttingOffBranches
            // 
            this.checkBoxCuttingOffBranches.AutoSize = true;
            this.checkBoxCuttingOffBranches.Location = new System.Drawing.Point(177, 8);
            this.checkBoxCuttingOffBranches.Name = "checkBoxCuttingOffBranches";
            this.checkBoxCuttingOffBranches.Size = new System.Drawing.Size(112, 17);
            this.checkBoxCuttingOffBranches.TabIndex = 14;
            this.checkBoxCuttingOffBranches.Text = "Отсечение веток";
            this.checkBoxCuttingOffBranches.UseVisualStyleBackColor = true;
            this.checkBoxCuttingOffBranches.CheckStateChanged += new System.EventHandler(this.checkBoxCuttingOffBranches_CheckStateChanged);
            // 
            // btnNewGame
            // 
            this.btnNewGame.Location = new System.Drawing.Point(3, 4);
            this.btnNewGame.Name = "btnNewGame";
            this.btnNewGame.Size = new System.Drawing.Size(75, 23);
            this.btnNewGame.TabIndex = 13;
            this.btnNewGame.Text = "Новая игра";
            this.btnNewGame.UseVisualStyleBackColor = true;
            this.btnNewGame.Click += new System.EventHandler(this.btnNewGame_Click);
            // 
            // btnEngineRunning
            // 
            this.btnEngineRunning.Location = new System.Drawing.Point(84, 4);
            this.btnEngineRunning.Name = "btnEngineRunning";
            this.btnEngineRunning.Size = new System.Drawing.Size(75, 23);
            this.btnEngineRunning.TabIndex = 12;
            this.btnEngineRunning.Text = "вперед";
            this.btnEngineRunning.UseVisualStyleBackColor = true;
            this.btnEngineRunning.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGameBoard_MouseUp);
            // 
            // listBoxListOfRatings
            // 
            this.listBoxListOfRatings.FormattingEnabled = true;
            this.listBoxListOfRatings.Location = new System.Drawing.Point(477, 33);
            this.listBoxListOfRatings.Name = "listBoxListOfRatings";
            this.listBoxListOfRatings.Size = new System.Drawing.Size(215, 303);
            this.listBoxListOfRatings.TabIndex = 11;
            // 
            // btnTestPosition
            // 
            this.btnTestPosition.Location = new System.Drawing.Point(309, 185);
            this.btnTestPosition.Name = "btnTestPosition";
            this.btnTestPosition.Size = new System.Drawing.Size(142, 46);
            this.btnTestPosition.TabIndex = 10;
            this.btnTestPosition.Text = "Тест позиции";
            this.btnTestPosition.UseVisualStyleBackColor = true;
            this.btnTestPosition.Visible = false;
            this.btnTestPosition.Click += new System.EventHandler(this.btnTestPosition_Click);
            // 
            // buttonArray
            // 
            this.buttonArray.Location = new System.Drawing.Point(319, 301);
            this.buttonArray.Name = "buttonArray";
            this.buttonArray.Size = new System.Drawing.Size(75, 23);
            this.buttonArray.TabIndex = 9;
            this.buttonArray.Text = "Массив";
            this.buttonArray.UseVisualStyleBackColor = true;
            this.buttonArray.Visible = false;
            this.buttonArray.Click += new System.EventHandler(this.buttonArray_Click);
            // 
            // pictureBoxGameBoard
            // 
            this.pictureBoxGameBoard.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pictureBoxGameBoard.Location = new System.Drawing.Point(3, 33);
            this.pictureBoxGameBoard.Name = "pictureBoxGameBoard";
            this.pictureBoxGameBoard.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxGameBoard.TabIndex = 8;
            this.pictureBoxGameBoard.TabStop = false;
            this.pictureBoxGameBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGameBoard_MouseDown);
            this.pictureBoxGameBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxGameBoard_MouseUp);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 427);
            this.Controls.Add(this.checkBoxCuttingOffBranches);
            this.Controls.Add(this.btnNewGame);
            this.Controls.Add(this.btnEngineRunning);
            this.Controls.Add(this.listBoxListOfRatings);
            this.Controls.Add(this.btnTestPosition);
            this.Controls.Add(this.buttonArray);
            this.Controls.Add(this.pictureBoxGameBoard);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Крестики-нолики";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Click += new System.EventHandler(this.MainForm_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxCuttingOffBranches;
        private System.Windows.Forms.Button btnNewGame;
        private System.Windows.Forms.Button btnEngineRunning;
        private System.Windows.Forms.ListBox listBoxListOfRatings;
        private System.Windows.Forms.Button btnTestPosition;
        private System.Windows.Forms.Button buttonArray;
        private System.Windows.Forms.PictureBox pictureBoxGameBoard;
    }
}

