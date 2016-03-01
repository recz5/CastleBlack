using CastleBlackApplication.Helper;
using CastleBlackApplication.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CastleBlackApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class NewCaseWindow : Window
    {
        CastleBlackEntities cbEntities = new CastleBlackEntities();
        public string selectedDeveloper;
        public int selectedDeveloperID;
        public string qaUserName = Environment.UserName;
        public int caseID = 0;
        public DateTime? caseDueDate = null;
        public List<string> errorPointsSource = new List<string>();
        public DataTable dt = new DataTable();
        public DataTable dtModified = new DataTable();
        DataRow row;

        public NewCaseWindow()
        {
            InitializeComponent();
            dtModified.Columns.Add("ContextKey", typeof(System.String));
            dtModified.Columns.Add("SalesForceCaseID", typeof(System.Int32));
            dtModified.Columns.Add("Category", typeof(System.String));
            dtModified.Columns.Add("ChecklistName", typeof(System.String));
            dtModified.Columns.Add("Error_Comment", typeof(System.String));
            dtModified.Columns.Add("Error", typeof(System.String));
            dtModified.Columns.Add("LastUpdatedTime", typeof(System.DateTime));

        }

        private void workItem_Loaded(object sender, RoutedEventArgs e)
        {
            qaBy.Text = string.Format("QA by: {0}", qaUserName);
            var query =
            from workItems in cbEntities.BlankWorkItems
            orderby workItems.Category
            select new { Category = workItems.Category, ChecklistName = workItems.ChecklistName, Error = workItems.Error, Error_Comment = workItems.Error_Comment };

            dt = LINQToDataTable(query);

            ICollectionView checklistsView = CollectionViewSource.GetDefaultView(dt);

            checklistsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            newWorkItem.DataContext = checklistsView;

        }

        private void DevComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var query =
            from developers in cbEntities.Developers
            orderby developers.First_Name, developers.Last_Name
            select new { ID = developers.Id, Name = developers.First_Name +" "+ developers.Last_Name };

            devComboBox.ItemsSource = query.ToList();
            devComboBox.DisplayMemberPath = "Name";
            devComboBox.SelectedValuePath = "ID";
        }

        private void DevComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = devComboBox.SelectedItem as string;
            int valueID = Int32.Parse(devComboBox.SelectedValue.ToString());
            selectedDeveloper = value;
            selectedDeveloperID = valueID;
        }

        private void DueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime? datePicked = DueDatePicker.SelectedDate;
            caseDueDate = datePicked == null ? now : DateTime.Parse(datePicked.ToString());
        }

        private void salesForceCaseID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int caseIDVal;
            if (int.TryParse(salesForceCaseID.Text.ToString(), out caseIDVal))
            {
                caseID = caseIDVal;
            }
            else
            {
                MessageBox.Show("SalesForce Case ID must be numbers only.");
            }
        }

        private void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            NewCaseWindow refresh = new NewCaseWindow();
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

            if (checkCaseID.Any())
            {
                MessageBox.Show("This already exists pick a new SalesForce Case ID\r \nOR \r\rSwitch to Existing Work Item to modify/continue on existing SalesForce Case ID.");
                return;
            }

            if (caseDueDate == null)
            {
                MessageBox.Show("Please pick a due date");
                return;
            }

            if (selectedDeveloperID == 0)
            {
                MessageBox.Show("Please pick a developer");
                return;
            }


            #endregion
            dtModified.Rows.Clear();

            foreach (DataRowView dr in newWorkItem.ItemsSource.OfType<DataRowView>())
            {
                row = dtModified.NewRow();
                row["ContextKey"] = string.Format("{0}|{1}|{2}",caseID.ToString(),dr[0].ToString(),dr[1].ToString()) ;
                row["SalesForceCaseID"] = caseID;
                row["Category"] = dr[0].ToString();
                row["ChecklistName"] = dr[1].ToString();
                row["Error"] = dr[2].ToString();
                row["Error_Comment"] = dr[3].ToString();
                row["LastUpdatedTime"] = DateTime.Now;
                dtModified.Rows.Add(row);
            }

            CreateCaseItem();

            SaveToDatabase(dtModified);

            LogEvent();

            MessageBox.Show(string.Format("Case Item: {0} saved.", caseID.ToString()));
        }

        private void Switch_Button_Click(object sender, RoutedEventArgs e)
        {
            ExistingCaseWindow switchWin = new ExistingCaseWindow();
            switchWin.Show();
            this.Close();
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
                            ==typeof(Nullable<>)))
                             {
                                 colType = colType.GetGenericArguments()[0];
                             }

                            dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                       }
                  }

                  DataRow dr = dtReturn.NewRow();

                  foreach (PropertyInfo pi in oProps)
                  {
                       dr[pi.Name] = pi.GetValue(rec, null) == null ?DBNull.Value :pi.GetValue
                       (rec,null);
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

        private void CreateCaseItem()
        {
            var qaID = 
                (from qa in cbEntities.QAs
                 where qa.UserName == qaUserName
                 select qa.Id).First();

            CaseItem csi = new CaseItem
            {
                SalesForce_Case_ID = caseID
              ,
                Creation_Dttm = DateTime.Now
              ,
                QA_ID = qaID
              ,
                Developer_ID = selectedDeveloperID
              ,
                SalesForce_Due_Date = caseDueDate
              ,
                LastUpdateDttm = DateTime.Now
            };

            cbEntities.CaseItems.Add(csi);

            cbEntities.SaveChanges();
        }

        private void LogEvent()
        {
            var qaID =
                (from qa in cbEntities.QAs
                 where qa.UserName == qaUserName
                 select qa.Id).First();

            CaseItemsLog log = new CaseItemsLog
            {
                LogDtTm = DateTime.Now,
                SalesForce_Case_ID = caseID,
                Developer_ID = selectedDeveloperID,
                QA_ID = qaID,
                Event = "Create",

            };

            cbEntities.CaseItemsLogs.Add(log);

            cbEntities.SaveChanges();
        }
    }
}
