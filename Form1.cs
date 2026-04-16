namespace FileCompare
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void btnRightDir_Click(object sender, System.EventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";

                if (!string.IsNullOrWhiteSpace(txtRightDir.Text) &&
                    System.IO.Directory.Exists(txtRightDir.Text))
                {
                    dlg.SelectedPath = txtRightDir.Text;
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtRightDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwRightDir, dlg.SelectedPath);

                    if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) &&
                        System.IO.Directory.Exists(txtLeftDir.Text))
                    {
                        PopulateListView(lvwLeftDir, txtLeftDir.Text);
                    }
                }
            }
        }

        private void btnLeftDir_Click(object sender, System.EventArgs e)
        {
            using (var dlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                dlg.Description = "폴더를 선택하세요.";

                if (!string.IsNullOrWhiteSpace(txtLeftDir.Text) &&
                    System.IO.Directory.Exists(txtLeftDir.Text))
                {
                    dlg.SelectedPath = txtLeftDir.Text;
                }

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtLeftDir.Text = dlg.SelectedPath;
                    PopulateListView(lvwLeftDir, dlg.SelectedPath);

                    if (!string.IsNullOrWhiteSpace(txtRightDir.Text) &&
                        System.IO.Directory.Exists(txtRightDir.Text))
                    {
                        PopulateListView(lvwRightDir, txtRightDir.Text);
                    }
                }
            }
        }

        private string FormatSizeInKb(long bytes)
        {
            double kb = bytes / 1024.0;
            return kb.ToString("N1") + " KB";
        }

        private void PopulateListView(System.Windows.Forms.ListView lv, string folderPath)
        {
            lv.BeginUpdate();
            lv.Items.Clear();

            try
            {
                // 폴더 목록
                string[] dirPaths = System.IO.Directory.GetDirectories(folderPath);
                System.Array.Sort(dirPaths);

                foreach (string path in dirPaths)
                {
                    System.IO.DirectoryInfo ld = new System.IO.DirectoryInfo(path);
                    System.IO.DirectoryInfo rd = null;
                    string otherDirPath = "";

                    if (lv == lvwLeftDir)
                    {
                        if (!string.IsNullOrWhiteSpace(txtRightDir.Text))
                        {
                            otherDirPath = System.IO.Path.Combine(txtRightDir.Text, ld.Name);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(txtLeftDir.Text))
                        {
                            otherDirPath = System.IO.Path.Combine(txtLeftDir.Text, ld.Name);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(otherDirPath) &&
                        System.IO.Directory.Exists(otherDirPath))
                    {
                        rd = new System.IO.DirectoryInfo(otherDirPath);
                    }

                    var ditem = new System.Windows.Forms.ListViewItem(ld.Name);
                    ditem.SubItems.Add("<DIR>");
                    ditem.SubItems.Add(ld.LastWriteTime.ToString("g"));

                    if (rd != null)
                    {
                        if (ld.LastWriteTime == rd.LastWriteTime)
                        {
                            ditem.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (ld.LastWriteTime > rd.LastWriteTime)
                        {
                            ditem.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            ditem.ForeColor = System.Drawing.Color.Gray;
                        }
                    }
                    else
                    {
                        ditem.ForeColor = System.Drawing.Color.Purple;
                    }

                    lv.Items.Add(ditem);
                }

                // 파일 목록
                string[] filePaths = System.IO.Directory.GetFiles(folderPath);
                System.Array.Sort(filePaths);

                foreach (string path in filePaths)
                {
                    System.IO.FileInfo lf = new System.IO.FileInfo(path);
                    System.IO.FileInfo rf = null;
                    string otherPath = "";

                    if (lv == lvwLeftDir)
                    {
                        if (!string.IsNullOrWhiteSpace(txtRightDir.Text))
                        {
                            otherPath = System.IO.Path.Combine(txtRightDir.Text, lf.Name);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(txtLeftDir.Text))
                        {
                            otherPath = System.IO.Path.Combine(txtLeftDir.Text, lf.Name);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(otherPath) &&
                        System.IO.File.Exists(otherPath))
                    {
                        rf = new System.IO.FileInfo(otherPath);
                    }

                    var litem = new System.Windows.Forms.ListViewItem(lf.Name);
                    litem.SubItems.Add(FormatSizeInKb(lf.Length));
                    litem.SubItems.Add(lf.LastWriteTime.ToString("g"));

                    // 상태 결정 및 색상 적용
                    if (rf != null)
                    {
                        if (lf.LastWriteTime == rf.LastWriteTime)
                        {
                            litem.ForeColor = System.Drawing.Color.Black;
                        }
                        else if (lf.LastWriteTime > rf.LastWriteTime)
                        {
                            litem.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            litem.ForeColor = System.Drawing.Color.Gray;
                        }
                    }
                    else
                    {
                        litem.ForeColor = System.Drawing.Color.Purple;
                    }

                    lv.Items.Add(litem);
                }

                // 컬럼 너비 자동 조정
                for (int i = 0; i < lv.Columns.Count; i++)
                {
                    lv.AutoResizeColumn(i,
                        System.Windows.Forms.ColumnHeaderAutoResizeStyle.ColumnContent);
                }
            }
            catch (System.IO.DirectoryNotFoundException)
            {
                System.Windows.Forms.MessageBox.Show(this, "폴더를 찾을 수 없습니다.", "오류",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (System.IO.IOException ex)
            {
                System.Windows.Forms.MessageBox.Show(this, "입출력 오류: " + ex.Message, "오류",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                lv.EndUpdate();
            }
        }
    }
}