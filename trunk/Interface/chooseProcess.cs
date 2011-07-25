﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace Interface
{

    public partial class chooseProcess : Form
    {
        int indexSperate = 0;

        public Dictionary<int, Dictionary<string, float>> resultFinal;

        public chooseProcess()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            // configurações iniciais
            refreshTableSoftware();
            refreshTableCaracteristics();
            buttonTestCons.Enabled = false;
            buttonNextDefinitonWeigths.Enabled = false;
            buttonFinish.Enabled = false;
            buttonTestConsitencyAHP.Enabled = false;
            buttonNextChooseSoftware.Enabled = false;
            dataGridViewTabelaSoftware.Columns[0].Visible = false;

            info();
        }


        private void info()
        {
            string info1 = "";
            info1 += "For new Comparation: ";
            info1 += "\n1 - Software -> Start New Comparation (Ctrl+N)";
            info1 += "\n2 - Choose between 2 up 16 software you want to be part of the decision process.";
            info1 += "\n3 - Click Next.";
            label_info1.Text = info1;

            string infoChooseCriteria = "";
            infoChooseCriteria += "Choose at least one characteristic to be classified.";
            label_infoChooseCriteria.Text = infoChooseCriteria;

            string definitionOfWeigths = "";
            definitionOfWeigths += "Here you have to define the weights for each characteristic you selected before.";
            definitionOfWeigths += "\nChoose between Smart and AHP method. To learn how the methods work , see the tutorials in Help menu.";
            label_DefinitionOfWeigths.Text = definitionOfWeigths;

            string definitionOfWeightsSmart = "";
            definitionOfWeightsSmart += "Please give 10 points to the  characteristic you consider the least important. To other characteristics give the points according to the first ranked (feature which gave 10 points).";
            definitionOfWeightsSmart += "\nThen click Calculate Final Weights button to get a  table with normalized values.";
            definitionOfWeightsSmart += "\nFinally press next button.";
            label_DefinitionOfWeightsSmart.Text = definitionOfWeightsSmart;

            string definitionOfWeightsAHP = "";
            definitionOfWeightsAHP += "This table pretends to describe the relation between all characteristics chosen.";
            definitionOfWeightsAHP += "\nThe main diagonal of the table associates the same two characteristics, so  is automatically filled.";
            definitionOfWeightsAHP += "\nHere you have to  fill the part of the table below the main diagonal, and give points to each criterion concerning other.";
            definitionOfWeightsAHP += "\nYou may adopt your own scale or consider  the scale described in AHP Tutorial.";
            definitionOfWeightsAHP += "\nAlso, fill the part of the table above the main diagonal with the inverse values previously assigned.";
            definitionOfWeightsAHP += "\nThen click Calculate Final Weights button to get a  table with normalized values, named final weight matrix. After that, is estimated the consistency rate of  this matrix clicking in Test Consistency";
            definitionOfWeightsAHP += "\nIf the value of consistency is good (written in green), you can proceed. If the consistency is bad (written in red), you should change the values to get a better result, or proceed anyway.";
            definitionOfWeightsAHP += "\nFinally press next button.";
            label_DefinitionOfWeightsAHP.Text = definitionOfWeightsAHP;
        }

        #region Refresh Tables
        private void refreshTableSoftware()
        {
            dataGridViewTabelaSoftware.DataSource = Business.ManagementDataBase.tableSoftware(false);
        }

        private void refreshTableCaracteristics()
        {
            dataGridViewCharacteristics.DataSource = Business.ManagementDataBase.tableCharacteristics();
        }

        private void refreshTableSmart()
        {
            dataGridViewSmart.DataSource = Business.ManagementDataBase.tableSmart();
            foreach (DataGridViewRow line in dataGridViewSmart.Rows)
            {
                line.ErrorText = "Please insert a value.";
            }

        }

        private void refreshTableAHP()
        {
            dataGridViewAHP.DataBindings.Clear();
            dataGridViewAHP.DataSource = Business.ManagementDataBase.tableAHP();

            int i = 0;
            int num_ca = Business.ManagementDataBase.totalCharacteristcSelect();

            // 
            while (i < num_ca)
            {
                dataGridViewAHP[i + 1, i].Value = "1";
                i++;
            }

            for (i = 0; i < dataGridViewAHP.ColumnCount; i++)
            {
                dataGridViewAHP.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            
        }

        private void refreshTableAHPPriority(string nameC)
        {
            dataGridViewAHPPriority.DataBindings.Clear();
            dataGridViewAHPPriority.DataSource = Business.ManagementDataBase.refreshTableAHPPriority(nameC);

            int i = 0;
            int num_ca = Business.ManagementDataBase.ids_dos_SoftwareSeleccionados.Count;

            dataGridViewAHPPriority.AllowUserToOrderColumns = false;

            while (i < num_ca)
            {
                dataGridViewAHPPriority[i + 1, i].Value = "1";
                dataGridViewAHPPriority[i + 1, i].ReadOnly = true;
                i++;
            }
        }

        #endregion

        #region File

        // open file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "beSmart files (*.beSmart)|*.beSmart|All files (*.*)|*.*";
            DialogResult ret = o.ShowDialog();
            String filename = o.FileName;

            if (ret == DialogResult.OK)
            {
                Business.ManagementDataBase.loadObject(filename);

                // ->>>> alterar isto par um evento, quando a base de dados muda faz refresh das tabelas
                refreshTableSoftware();
                refreshTableCaracteristics();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.Filter = "beSmart files (*.beSmart)|*.beSmart|All files (*.*)|*.*";
            DialogResult ret = s.ShowDialog();

            if (ret == DialogResult.OK)
            {
                string name = s.FileName;
                Business.ManagementDataBase.database.saveInObject(name);
            }
        }
        #endregion

        #region Edit information
        private void editSoftwareListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditSWList editList = new EditSWList();
            editList.ShowDialog();
            refreshTableSoftware();
            refreshTableCaracteristics();
        }

        #endregion


        #region Preivous

        private void buttonPreviousToSoftware_Click(object sender, EventArgs e)
        {
            tabControlSeparates.SelectedTab = tabPageChooseSoftware;
            progressBar1.Value = 0;
        }



        private void buttonPreviousDefiniotWeigths_Click(object sender, EventArgs e)
        {
            tabControlSeparates.SelectedTab = tabPageChooseCriteria;
            progressBar1.Value = 25;
        }

        #endregion

        // ->>>>>>> a partir daqui

        private void buttonCalFinalWe_Click(object sender, EventArgs e)
        {
            int flag = 0;
            foreach (DataGridViewColumn coluna in dataGridViewAHP.Columns)
            {
                if (flag == 0)
                {
                    flag = 1;
                }
                else
                {
                    string name = coluna.Name.ToString();
                    string idA = Business.ManagementDataBase.procuraIdCha(name);
                    foreach (DataGridViewRow linha in dataGridViewAHP.Rows)
                    {
                        string nameB = linha.Cells[0].Value.ToString();
                        string idB = Business.ManagementDataBase.procuraIdCha(nameB);
                        string pointsStr = linha.Cells[name].Value.ToString();
                        float pointf = (float)System.Convert.ToDouble(pointsStr);
                        Business.ManagementDataBase.decision.registerClassAHP(idA, idB, pointf);
                    }
                }
            }

            Dictionary<string, Dictionary<string, float>> tabelaNormAHP = new Dictionary<string, Dictionary<string, float>>();
            tabelaNormAHP = Business.ManagementDataBase.decision.normalizeAHP(Business.ManagementDataBase.decision.TableAHP);
            Business.ManagementDataBase.pesosFinaisClassAHP = new Dictionary<string, float>();

            // alterar para a managmente
            Business.ManagementDataBase.pesosFinaisClassAHP = Business.ManagementDataBase.decision.pesosFinais(tabelaNormAHP);


            dataGridViewPesosAHP.DataSource = Business.ManagementDataBase.tableFinalWeightAHP();

            // activa o butão de consistência
            buttonTestCons.Enabled = true;

            Business.ManagementDataBase.metodo_fase_1 = "ahp";
        }


        #region Definiton Priorities

        private void button2_Click(object sender, EventArgs e)
        {
            int linha = dataGridViewCaracteristicasPrioridades.CurrentRow.Index;
            if (linha >= 0)
            {
                string id = dataGridViewCaracteristicasPrioridades["ID", linha].Value.ToString();
                string name = dataGridViewCaracteristicasPrioridades["Name", linha].Value.ToString();
                //MessageBox.Show(id + "\t" + name);
                labelCaracteristicaValueFn.Text = name;
                labelCaracteristicaValueFnID.Text = id;

                labelIDAHP.Text = id;
                labelName_AHP.Text = name;

                refreshTableAHPPriority(name);
            }

            buttonCalculateValueFn.Enabled = true;
            buttonCalcPrioAHP.Enabled = true;
            buttonTestConsitencyAHP.Enabled = false;
            dataGridViewValueFn.DataSource = null;
            dataGridViewPesosAHPFinais.DataSource = null;
        }

        private void buttonCalculateValueFn_Click_1(object sender, EventArgs e)
        {
            buttonCalcPrioAHP.Enabled = false;
            Business.ManagementDataBase.decision.TableSW = Business.ManagementDataBase.database.SoftwareWithCaracteristics(Business.ManagementDataBase.ids_dos_SoftwareSeleccionados);

            Dictionary<string, Dictionary<string, int>> tableFilter = new Dictionary<string, Dictionary<string, int>>();

            string id_carac = labelCaracteristicaValueFnID.Text;
            tableFilter = Business.ManagementDataBase.decision.filter(id_carac);

            int min = Business.ManagementDataBase.decision.calMin(id_carac, tableFilter);
            int max = Business.ManagementDataBase.decision.calMax(id_carac, tableFilter);

            //MessageBox.Show("Min: " + min + "\tMax: " + max);

            Dictionary<string, float> aux = new Dictionary<string, float>();
            // maximizar
            if (radioButtonMaximize.Checked)
            {
                aux = Business.ManagementDataBase.decision.calValueMax(min, max, tableFilter);

            }

            // maximizar
            if (radioButtonMinimize.Checked)
            {
                aux = Business.ManagementDataBase.decision.calValueMin(min, max, tableFilter);
            }

            Business.ManagementDataBase.decision.registerPriority(id_carac, aux);

            DataTable prioridades = new DataTable();
            prioridades.Columns.Add("ID");
            prioridades.Columns.Add("Priority");

            Dictionary<string, float> a;

            Business.ManagementDataBase.decision.TableResult.TryGetValue(id_carac, out a);
            foreach (KeyValuePair<string, float> pair2 in a)
            {
                prioridades.Rows.Add(pair2.Key, pair2.Value);
            }



            DataView view = new DataView(prioridades);
            dataGridViewValueFn.DataSource = view;

            buttonFinish.Enabled = true;


        }


        private void buttonCalcPrioAHP_Click(object sender, EventArgs e)
        {
            buttonCalculateValueFn.Enabled = false;
            int flag = 0;
            foreach (DataGridViewColumn coluna in dataGridViewAHPPriority.Columns)
            {
                if (flag == 0)
                {
                    flag = 1;
                }
                else
                {
                    string idSofA = coluna.Name.ToString();
                    foreach (DataGridViewRow linha in dataGridViewAHPPriority.Rows)
                    {
                        string idSofB = linha.Cells[0].Value.ToString();
                        //MessageBox.Show(idSofB);
                        string pointsStr = linha.Cells[idSofA].Value.ToString();
                        float pointf = (float)System.Convert.ToDouble(pointsStr);
                        //MessageBox.Show("idA: " + idA + "\tName: " + name + "\nIDB: " + idB + "\tNameB: " + nameB + "\nPoints: " + pointf);
                        Business.ManagementDataBase.decision.registerPriorAHP(labelIDAHP.Text, idSofA, idSofB, pointf);

                    }
                }
            }

            Dictionary<string, Dictionary<string, Dictionary<string, float>>> tabelaNormAHP = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
            tabelaNormAHP = Business.ManagementDataBase.decision.normalizePriorityAHP(Business.ManagementDataBase.decision.TablePriorAHP);
            Business.ManagementDataBase.decision.pesosPriorFinais(tabelaNormAHP);

            DataTable prioridades = new DataTable();
            prioridades.Columns.Add("ID");
            prioridades.Columns.Add("Priority");

            Dictionary<string, float> a;

            Business.ManagementDataBase.decision.TableResult.TryGetValue(labelIDAHP.Text, out a);
            foreach (KeyValuePair<string, float> pair2 in a)
            {
                prioridades.Rows.Add(pair2.Key, pair2.Value);
            }

            DataView view = new DataView(prioridades);
            dataGridViewPesosAHPFinais.DataSource = view;


            /*
            foreach (KeyValuePair<string, double> pair in pesosFinaisClassAHP)
            {
                MessageBox.Show(pair.Key + "\t" + pair.Value);
            }*/

            // activa o butão de consistência
            buttonTestConsitencyAHP.Enabled = true;

        }
        #endregion

        #region Button Finish
        private void buttonFinish_Click(object sender, EventArgs e)
        {
            resultFinal = new Dictionary<int, Dictionary<string, float>>();
            if (Business.ManagementDataBase.metodo_fase_1.Equals("smart"))
            {
                resultFinal = Business.ManagementDataBase.decision.analiseFinalSmart(Business.ManagementDataBase.tabelaSmartNorm, Business.ManagementDataBase.decision.TableResult);
            }

            if (Business.ManagementDataBase.metodo_fase_1.Equals("ahp"))
            {
                resultFinal = Business.ManagementDataBase.decision.analiseFinalAHP(Business.ManagementDataBase.pesosFinaisClassAHP, Business.ManagementDataBase.decision.TableResult);
            }


            DataTable final = new DataTable();
            final.Columns.Add("RANK");
            final.Columns.Add("Software");
            final.Columns.Add("Priority");


            foreach (KeyValuePair<int, Dictionary<string, float>> pair in resultFinal)
            {
                Dictionary<string, float> a;
                resultFinal.TryGetValue(pair.Key, out a);
                foreach (KeyValuePair<string, float> pair2 in a)
                {
                    final.Rows.Add(pair.Key, pair2.Key, pair2.Value);
                }
            }

            DataView view = new DataView(final);
            dataGridViewFinal.DataSource = view;

            tabControlSeparates.SelectedTab = tabPageFinal;
            indexSperate = tabControlSeparates.SelectedIndex;
            progressBar1.Value = 100;
        }

        #endregion

        #region Test Concistency

        private void buttonTestCons_Click(object sender, EventArgs e)
        {
            Dictionary<int, double> matrixC = new Dictionary<int, double>();
            Dictionary<int, double> matrixD = new Dictionary<int, double>();



            matrixC = Business.ManagementDataBase.decision.calculaMatrizC(Business.ManagementDataBase.decision.TableAHP, Business.ManagementDataBase.pesosFinaisClassAHP);
            matrixD = Business.ManagementDataBase.decision.calculaMatrizD(matrixC, Business.ManagementDataBase.pesosFinaisClassAHP);
            double taxa = Business.ManagementDataBase.decision.taxaConsitencia(matrixD);

            if (taxa <= 0.10)
            {
                MessageBox.Show("The consistency Rate is good: " + taxa);
                labelConsistencyRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(19)))));
            }
            else
            {
                MessageBox.Show("The consistency Rate is bad: " + taxa);
                labelConsistencyRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            }

            // actualiza a label com a taxa
            labelConsistencyRate.Text = "" + taxa;
            // activa o botão next
            buttonNextDefinitonWeigths.Enabled = true;
        }


        private void buttonTestConsitencyAHP_Click(object sender, EventArgs e)
        {
            Dictionary<int, double> matrixC = new Dictionary<int, double>();
            Dictionary<int, double> matrixD = new Dictionary<int, double>();
            Dictionary<string, Dictionary<string, float>> aux;
            Dictionary<string, float> aux1;
            Business.ManagementDataBase.decision.TablePriorAHP.TryGetValue(labelIDAHP.Text, out aux);
            Business.ManagementDataBase.decision.TableResult.TryGetValue(labelIDAHP.Text, out aux1);
            matrixC = Business.ManagementDataBase.decision.calculaMatrizC(aux, aux1);
            matrixD = Business.ManagementDataBase.decision.calculaMatrizD(matrixC, aux1);
            double taxa = Business.ManagementDataBase.decision.taxaConsitencia(matrixD);

            if (taxa <= 0.10)
            {
                MessageBox.Show("The consistency Rate is good: " + taxa);
                labelAHPPrioCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(81)))), ((int)(((byte)(19)))));
            }
            else
            {
                MessageBox.Show("The consistency Rate is bad: " + taxa);
                labelAHPPrioCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            }

            // actualiza a label com a taxa
            labelAHPPrioCons.Text = "" + taxa;

            buttonFinish.Enabled = true;
        }
        #endregion

        #region Button Help
        private void aHPTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = Path.GetFullPath("Files\\Tutorials_html\\AHPtutorial.htm");
            View_HTML t = new View_HTML(url);
            t.Show();
        }

        private void sMARTTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = Path.GetFullPath("Files\\Tutorials_html\\SMARTtutorial.htm");
            View_HTML t = new View_HTML(url);
            t.Show();
        }

        private void valueFNTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = Path.GetFullPath("Files\\Tutorials_html\\ValueFntutorial.htm");
            View_HTML t = new View_HTML(url);
            t.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox a = new AboutBox();
            a.Show();
        }
        #endregion

        #region Start New Comparation
        private void startANewComparationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Business.ManagementDataBase.ids_dos_SoftwareSeleccionados = new List<int>();
            Business.ManagementDataBase.caracteristicas_escolhidas = new Dictionary<int, string>();
            Business.ManagementDataBase.tabelaSmartNorm = new Dictionary<string, float>();
            Business.ManagementDataBase.pesosFinaisClassAHP = new Dictionary<string, float>();
            refreshTableSoftware();
            refreshTableCaracteristics();
            Business.ManagementDataBase.ids_dos_SoftwareSeleccionados.Clear();
            Business.ManagementDataBase.caracteristicas_escolhidas.Clear();
            Business.ManagementDataBase.decision.TableCH.Clear();
            Business.ManagementDataBase.decision.TableAHP.Clear();
            Business.ManagementDataBase.decision.TableResult.Clear();
            Business.ManagementDataBase.tabelaSmartNorm.Clear();
            Business.ManagementDataBase.pesosFinaisClassAHP.Clear();

            buttonCalFinalWe.Enabled = true;
            buttonNextChooseSoftware.Enabled = true;
            buttonTestCons.Enabled = false;
            buttonCalculateValueFn.Enabled = true;
            buttonTestConsitencyAHP.Enabled = false;
            buttonCalcPrioAHP.Enabled = true;


            dataGridViewPesosAHP.DataSource = null;
            dataGridViewPesosFinaisSmart.DataSource = null;
            dataGridViewPesosAHPFinais.DataSource = null;
            dataGridViewValueFn.DataSource = null;
            dataGridViewFinal.DataSource = null;

            progressBar1.Value = 0;

            labelAHPPrioCons.Text = "consitencia";
            labelIDAHP.Text = "ID";
            labelName_AHP.Text = "name";
            labelCaracteristicaValueFnID.Text = "ID";
            labelConsistencyRate.Text = "";
            labelCaracteristicaValueFn.Text = "name";
            tabControlSeparates.SelectedTab = tabPageChooseSoftware;
            dataGridViewTabelaSoftware.Columns[0].Visible = true;

        }

        #endregion

        #region Button Next

        private void buttonNextChooseCriteria_Click(object sender, EventArgs e)
        {
            // limpar a estrutura
            Business.ManagementDataBase.caracteristicas_escolhidas = new Dictionary<int, string>();
            Business.ManagementDataBase.caracteristicas_escolhidas.Clear();

            // vai a todas as linhas das tabelas ver quais estão seleccionadas
            foreach (DataGridViewRow linha in dataGridViewCharacteristics.Rows)
            {
                if (linha.Cells[0].Value != null)
                {
                    int id = System.Convert.ToInt32(linha.Cells[1].Value);
                    string name = (string)linha.Cells[2].Value;
                    Business.ManagementDataBase.caracteristicas_escolhidas.Add(id, name);
                }
            }

            // condição para se ter de seleccionar pelo menos uma caracteristica
            if (Business.ManagementDataBase.caracteristicas_escolhidas.Count < 1)
            {
                MessageBox.Show("Select at least one characteristics!");
            }
            else
            {
                buttonNextChooseCriteria_message();
                tabControlSeparates.SelectedTab = tabPageClassificaoes;
                indexSperate = tabControlSeparates.SelectedIndex;
                progressBar1.Value = 50;
                refreshTableSmart();
                refreshTableAHP();
            }
        }

        // messagem que deve aparecer quando se clica no next e aparece sucesso
        private void buttonNextChooseCriteria_message()
        {
            string message = "Select Characteristics:\n";

            foreach (KeyValuePair<int, string> pair in Business.ManagementDataBase.caracteristicas_escolhidas)
            {
                message += pair.Key + "\t" + pair.Value + "\n";
            }

            MessageBox.Show(message, "Characteristics", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void buttonNextChooseSoftware_Click(object sender, EventArgs e)
        {
            Business.ManagementDataBase.ids_dos_SoftwareSeleccionados = new List<int>();

            foreach (DataGridViewRow linha in dataGridViewTabelaSoftware.Rows)
            {
                // seleccionada apenas as linhas que tem o checbox activo
                if (linha.Cells[0].Value != null && (bool)linha.Cells[0].Value == true)
                {
                    int id = System.Convert.ToInt32(linha.Cells[1].Value);
                    string name = linha.Cells[2].Value.ToString();

                    Business.ManagementDataBase.addIdSoftwareelect(id);
                }
            }

            if (Business.ManagementDataBase.totalSoftwareelect() < 2 || Business.ManagementDataBase.totalSoftwareelect() > 16)
            {
                MessageBox.Show("Select between 2 and 16 Software!");
            }
            else
            {
                // apresenta os Software seleccionados
                buttonNextChooseSoftware_message();

                tabControlSeparates.SelectedTab = tabPageChooseCriteria;

                indexSperate = tabControlSeparates.SelectedIndex;

                progressBar1.Value = 25;
            }
        }

        // messagem que deve aparecer quando se clica no next e aparece sucesso
        private void buttonNextChooseSoftware_message()
        {
            string message = "Select Software:\n";

            foreach (Business.Software s in Business.ManagementDataBase.infoSoftware_byID().Values)
            {
                message += s.Id + "\t" + s.Name + "\n";
            }

            MessageBox.Show(message, "Software", MessageBoxButtons.OK, MessageBoxIcon.None);
        }


        private void buttonNextDefinitonWeigths_Click(object sender, EventArgs e)
        {
            dataGridViewCaracteristicasPrioridades.DataSource = Business.ManagementDataBase.tableCaracteristicasPrioridades();
            tabControlSeparates.SelectedTab = tabPageDefinitionPriorities;
            indexSperate = tabControlSeparates.SelectedIndex;
            progressBar1.Value = 75;
        }

        private void buttonNextChooseSoftware_MouseEnter(object sender, EventArgs e)
        {
            buttonNextChooseSoftware.ForeColor = System.Drawing.Color.Blue;
        }

        private void buttonNextChooseSoftware_MouseLeave(object sender, EventArgs e)
        {
            buttonNextChooseSoftware.ForeColor = System.Drawing.Color.Black;
        }

        #endregion

        #region ViewWebPage

        private void buttonViewWebPage_Click(object sender, EventArgs e)
        {
            ConsultWebpage cwp = new ConsultWebpage();
            cwp.Show();
        }
        private void buttonViewWebPage_MouseEnter(object sender, EventArgs e)
        {
            buttonViewWebPage.Font = new Font(buttonViewWebPage.Font, FontStyle.Bold);
            buttonViewWebPage.ForeColor = System.Drawing.Color.Blue;
        }

        private void buttonViewWebPage_MouseLeave(object sender, EventArgs e)
        {
            buttonViewWebPage.Font = new Font(buttonViewWebPage.Font, FontStyle.Regular);
            buttonViewWebPage.ForeColor = System.Drawing.Color.Black;
        }

        private void viewSoftwareWebpageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsultWebpage cwp = new ConsultWebpage();
            cwp.Show();

        }
        #endregion

        #region exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show("Want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                this.Dispose();
            }
        }


        private void chooseProcess_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r = MessageBox.Show("Want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {   // cancel dispose
                e.Cancel = true;
            }
        }
        #endregion

        #region TabSeparates
        private void tabControlSeparates_Click(object sender, EventArgs e)
        {
            // falta o Previous
            //tabControlSeparates.SelectedIndex = indexSperate;
            //MessageBox.Show("Use the buttons Next and Previous for navigate in process.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        private void dataGridViewTabelaSoftware_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow line in dataGridViewTabelaSoftware.Rows)
            {
                if (line.Cells[0].Value != null && line.Cells[0].Value.ToString().Equals("True"))
                {
                    line.Selected = true;
                }
                if (line.Cells[0].Value != null && line.Cells[0].Value.ToString().Equals("False"))
                {
                    line.Selected = false;
                }
            }
        }

        private void dataGridViewTabelaSoftware_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow line in dataGridViewTabelaSoftware.Rows)
            {
                if (line.Cells[0].Value != null && line.Cells[0].Value.ToString().Equals("True"))
                {
                    line.Selected = true;
                }
                if (line.Cells[0].Value != null && line.Cells[0].Value.ToString().Equals("False"))
                {
                    line.Selected = false;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "Want to create a new Data Base?\nThe information has not saved will be lost.";
            DialogResult r = MessageBox.Show(msg, "New Data Base", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

            if (r == DialogResult.Yes)
            {
                Business.ManagementDataBase.database = new Business.DataBaseUser();
                init();
            }

        }

        private void reportErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportError r = new ReportError();
            r.Show();
        }


        #region Definition of Weights Smart
        private void dataGridViewSmart_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            int c = e.ColumnIndex;
            int l = e.RowIndex;

            int newNumber = 0;

            if (dataGridViewSmart.Rows[l].Cells[0].Value == null /*|| dataGridViewSmart.CurrentCell.Value.ToString().Equals("") == true*/) return;

            if (c == 0)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out newNumber))
                {
                    dataGridViewSmart.Rows[l].ErrorText = "The Value is not a number!";
                    MessageBox.Show("The Value is not a number!");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    if (newNumber < 10)
                    {
                        dataGridViewSmart.Rows[l].ErrorText = "The value can not be less than 10.";
                        MessageBox.Show("The value can not be less than 10.");
                        e.Cancel = true;
                    }
                    else
                    {
                        dataGridViewSmart.Rows[l].ErrorText = null;
                    }
                }
            }

            verifyTableSmart();

        }


        private void dataGridViewSmart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            verifyTableSmart();
        }


        // verifica se está tudo preenchido e calcula os pesos
        private void verifyTableSmart()
        {
            // se estiver alguma coisa vazia não faz mais nada
            foreach (DataGridViewRow line in dataGridViewSmart.Rows)
            {
                int n = 0;
                if (line.Cells[0].Value == null)
                {
                    line.ErrorText = "Please insert a value.";
                    return;
                }
                else { line.ErrorText = null; }

                if (!int.TryParse(line.Cells[0].Value.ToString(), out n))
                {
                    line.ErrorText = "The Value is not a number!";
                    return;
                }
                else { line.ErrorText = null; }

                if (n < 10)
                {
                    line.ErrorText = "The value can not be less than 10.";
                    return;
                }
            }

            // para remover possiveis erros que ainda existam
            foreach (DataGridViewRow line in dataGridViewSmart.Rows)
            {
                line.ErrorText = null;
            }

            // para verificar se existe um 10
            int num_10 = 0;
            foreach (DataGridViewRow line in dataGridViewSmart.Rows)
            {
                try
                {
                    int v = -1;
                    int.TryParse(line.Cells[0].Value.ToString(), out v);
                    if (v == 10) num_10++;
                }
                catch (Exception)
                {
                    return;
                }
            }

            // se não existir altera a msg
            if (num_10 != 1)
            {
                label_DefinitionOfWeigths.Text = "Assign 10 to a characteristics.";
                return;
            }


            // para fazer os calulos
            foreach (DataGridViewRow linha in dataGridViewSmart.Rows)
            {
                string idChar = linha.Cells[1].Value.ToString();
                int points = System.Convert.ToInt32(linha.Cells[0].Value.ToString());
                Business.ManagementDataBase.decision.registerClass(idChar, points);
            }

            Business.ManagementDataBase.tabelaSmartNorm.Clear();
            Business.ManagementDataBase.tabelaSmartNorm = Business.ManagementDataBase.decision.normalizeSMART(Business.ManagementDataBase.decision.TableCH);

            dataGridViewPesosFinaisSmart.DataSource = Business.ManagementDataBase.tableFinalWeightSmart();

            buttonNextDefinitonWeigths.Enabled = true;
            buttonCalFinalWe.Enabled = false;

            if (tabControlSmartAHP.SelectedIndex == 0)
            {
                Business.ManagementDataBase.metodo_fase_1 = "smart";

                string inf = "Currently smart method chosen.";
                label_DefinitionOfWeigths.Text = inf;

            }

        }

        #endregion





    }
}
