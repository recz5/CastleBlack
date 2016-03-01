using CastleBlackApplication.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CastleBlackApplication.View
{
    /// <summary>
    /// Interaction logic for ExistingCaseWindow.xaml
    /// </summary>
    public partial class ExistingCaseWindow : Window
    {
        CastleBlackEntities cbEntities = new CastleBlackEntities();
        public string selectedDeveloper;
        public int selectedDeveloperID;
        public string qaUserName = Environment.UserName;
        public int caseID = 0;
        public int qaID = 0;
        public int devID = 0;
        public string qaName;
        public string devName;
        public DateTime? caseDueDate = null;
        public DateTime? createdDtTm = null;        
        public DataTable dt = new DataTable();
        public DataTable dtModified = new DataTable();
        DataRow row;
        List<int> caseIDs = new List<int>();
        
        public ExistingCaseWindow()
        {
            InitializeComponent();
            dtModified.Columns.Add("ContextKey", typeof(System.String));
            dtModified.Columns.Add("SalesForceCaseID", typeof(System.Int32));
            dtModified.Columns.Add("Category", typeof(System.String));
            dtModified.Columns.Add("ChecklistName", typeof(System.String));
            dtModified.Columns.Add("Error_Comment", typeof(System.String));
            dtModified.Columns.Add("Error", typeof(System.String));
            dtModified.Columns.Add("LastUpdatedTime", typeof(System.DateTime));

            caseIDs =
                (from csi in cbEntities.CaseItems
                 select csi.SalesForce_Case_ID).ToList();

            salesForceCaseIDAuto.TextChanged += new TextChangedEventHandler(salesForceCaseIDAuto_TextChanged);
        }

        private void Go_Button_Click(object sender, RoutedEventArgs e)
        {
            csiSuggst.Visibility = Visibility.Collapsed;

            var checkCaseID =
                            (from csi in cbEntities.CaseItems
                             where csi.SalesForce_Case_ID == caseID
                             join qa in cbEntities.QAs on csi.QA_ID equals qa.Id
                             join dev in cbEntities.Developers on csi.Developer_ID equals dev.Id
                             select new { 
                                 case_id = csi.SalesForce_Case_ID, 
                                 qa_id = csi.QA_ID, 
                                 dev_id = csi.Developer_ID,
                                 qa_name = qa.First_Name +" "+ qa.Last_Name,
                                 dev_name = dev.First_Name +" "+ dev.Last_Name,
                                 crt_dttm = csi.Creation_Dttm, 
                                 due_dttm = csi.SalesForce_Due_Date
                             });

            if (!checkCaseID.Any())
            {
                MessageBox.Show("Case ID does not exists. Must be valid case ID.");
                return;
            }

            foreach (var item in checkCaseID)
            {
                caseID = item.case_id;
                qaID = item.qa_id;
                qaName = item.qa_name;
                caseDueDate = item.due_dttm;
                devID = item.dev_id;
                devName = item.dev_name;
                createdDtTm = item.crt_dttm;
            }

            ShowDetails();

            Load_WorkItem(sender, e);

        }

        private void salesForceCaseIDAuto_TextChanged(object sender, TextChangedEventArgs e)
        {
            string typedString = salesForceCaseIDAuto.Text;

            List<string> autoList = new List<string>();
            autoList.Clear();

            foreach (int c in caseIDs)
            {
                if (!string.IsNullOrEmpty(salesForceCaseIDAuto.Text))
                {
                    if (c.ToString().StartsWith(typedString))
                    {
                        autoList.Add(c.ToString());
                    }
                }
            }

            if (autoList.Count > 0)
            {
                csiSuggst.ItemsSource = autoList;
                csiSuggst.Visibility = Visibility.Visible;
            }
            else if (salesForceCaseIDAuto.Text.Equals(""))
            {
                csiSuggst.Visibility = Visibility.Collapsed;
                csiSuggst.ItemsSource = null;
            }
            //else
            //{
            //    csiSuggst.Visibility = Visibility.Collapsed;
            //    csiSuggst.ItemsSource = null;
            //}
            int caseIDVal;
            if (int.TryParse(salesForceCaseIDAuto.Text.ToString(), out caseIDVal))
            {
                caseID = caseIDVal;
            }
            //else
            //{
            //    MessageBox.Show("SalesForce Case ID must be numbers only.");
            //}
        }

        private void csiSuggst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (csiSuggst.ItemsSource != null)
            {
                csiSuggst.Visibility = Visibility.Collapsed;

                salesForceCaseIDAuto.TextChanged -= new TextChangedEventHandler(salesForceCaseIDAuto_TextChanged);

                if (csiSuggst.SelectedIndex != -1)
                {
                    salesForceCaseIDAuto.Text = csiSuggst.SelectedItem.ToString();
                }
                salesForceCaseIDAuto.TextChanged += new TextChangedEventHandler(salesForceCaseIDAuto_TextChanged);
            }
        }

        private void Load_WorkItem(object sender, RoutedEventArgs e)
        {
            //qaBy.Text = string.Format("QA by: {0}", qaUserName);
            var query =
            from workItems in cbEntities.CaseItemChecklists
            where workItems.SalesForce_Case_ID == caseID
            orderby workItems.Category
            select new { Category = workItems.Category, ChecklistName = workItems.ChecklistName, Error = workItems.Error, Error_Comment = workItems.Error_Comment };

            dt = LINQToDataTable(query);

            ICollectionView checklistsView = CollectionViewSource.GetDefaultView(dt);

            checklistsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            newWorkItem.DataContext = checklistsView;
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            ExistingCaseWindow refresh = new ExistingCaseWindow();
            refresh.Show();
            this.Close();
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            #region simple validation
            var checkCaseID =
                (from csi in cbEntities.CaseItems
                 where csi.SalesForce_Case_ID == caseID
                 select csi.SalesForce_Case_ID);

            if (!checkCaseID.Any())
            {
                MessageBox.Show("Case ID does not exists");
                return;
            }
            #endregion
            dtModified.Rows.Clear();

            foreach (DataRowView dr in newWorkItem.ItemsSource.OfType<DataRowView>())
            {
                row = dtModified.NewRow();
                row["ContextKey"] = string.Format("{0}|{1}|{2}", caseID.ToString(), dr[0].ToString(), dr[1].ToString());
                row["SalesForceCaseID"] = caseID;
                row["Category"] = dr[0].ToString();
                row["ChecklistName"] = dr[1].ToString();
                row["Error"] = dr[2].ToString();
                row["Error_Comment"] = dr[3].ToString();
                row["LastUpdatedTime"] = DateTime.Now;
                dtModified.Rows.Add(row);
            }

            UpdateCaseItem();

            SaveToDatabase(dtModified);

            LogEvent();

            MessageBox.Show(string.Format("Case Item: {0} updated.", caseID.ToString()));
        }

        private void Switch_Button_Click(object sender, RoutedEventArgs e)
        { 
            NewCaseWindow switchWin = new NewCaseWindow();
            switchWin.Show();
            this.Close();
        }

        private void ShowDetails()
        {
            caseIDTB.Text = string.Format("SalesForce Case ID:  {0}", caseID.ToString());
            qaByTB.Text =  string.Format("Last QAed by:  {0}", qaName.ToString());
            dueDateTB.Text =  string.Format("Due Date:  {0}", caseDueDate.ToString());
            devTB.Text = string.Format("Developer:  {0}", devName.ToString());
            createDtTB.Text = string.Format("Created On:  {0}", createdDtTm.ToString());
        }

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;
        }

        public static DataTable DataViewAsDataTable(DataView dv)
        {
            DataTable dt = dv.Table.Clone();
            foreach (DataRowView drv in dv)
                dt.ImportRow(drv.Row);
            return dt;
        }

        private void SaveToDatabase(DataTable tbl)
        {
            string conn = "Server={TARGET_SERVER};Database=CastleBlack;Initial Catalog=master;Integrated Security=True;Connect Timeout=2";

            SQLHelper.UpsertDataTable(conn.Replace("{TARGET_SERVER}", Environment.MachineName), tbl, "[CastleBlack].[Towers].[proc_CaseItemChecklistsDoUpsert]");
        }

        private void UpdateCaseItem()
        {
            var qaID = 
                (from qa in cbEntities.QAs
                 where qa.UserName == qaUserName
                 select qa.Id).First();

            var query =
                (from csi in cbEntities.CaseItems
                    where csi.SalesForce_Case_ID == caseID
                    select csi);
            
            foreach(CaseItem ci in query)
            {
                ci.LastUpdateDttm = DateTime.Now;
                ci.QA_ID = qaID;
            }

            cbEntities.SaveChanges();
        }

        private void LogEvent()
        {
            var query =
                (from qa in cbEntities.QAs
                 where qa.UserName == qaUserName
                 select qa.Id).First();

            CaseItemsLog log = new CaseItemsLog
            {
               LogDtTm = DateTime.Now,
               SalesForce_Case_ID = caseID,
               Developer_ID = devID,
               QA_ID = qaID,
               Event = "Update",

            };

            cbEntities.CaseItemsLogs.Add(log);

            cbEntities.SaveChanges();
        }
    }
}
