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
        public string qaUserName = Environment.UserName;
        public int caseID;
        public DateTime caseDueDate;
        public List<string> errorPointsSource = new List<string>();
        public DataTable dt = new DataTable();
        public DataTable dtModified = new DataTable();

        public NewCaseWindow()
        {
            InitializeComponent();
        }

        private void workItem_Loaded(object sender, RoutedEventArgs e)
        {
            qaBy.Text = string.Format("QA by: {0}", qaUserName);
            var query =
            from workItems in cbEntities.BlankWorkItems
            orderby workItems.Category
            select new { Category = workItems.Category, ChecklistName = workItems.ChecklistName, Error = workItems.Error, Error_Comment = workItems.Error_Comment };

            dt = LINQToDataTable(query);

            //newWorkItem.ItemsSource = query.ToList();
            newWorkItem.ItemsSource = dt.AsDataView();

            //ICollectionView checklistsView = CollectionViewSource.GetDefaultView(query.ToList());

            ICollectionView checklistsView = CollectionViewSource.GetDefaultView(dt);

            checklistsView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            newWorkItem.DataContext = checklistsView;

            //newWorkItem.SelectionChanged += new SelectionChangedEventHandler(newWorkItem_SelectionChanges);

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

        private void ErrorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var query =
            from developers in cbEntities.Developers
            orderby developers.First_Name, developers.Last_Name
            select new { ID = developers.Id, Name = developers.First_Name + " " + developers.Last_Name };

            devComboBox.ItemsSource = query.ToList();
            devComboBox.DisplayMemberPath = "Name";
            devComboBox.SelectedValuePath = "ID";
        }

        private void DevComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string value = devComboBox.SelectedItem as string;
            selectedDeveloper = value;
        }

        private void DueDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime? datePicked = DueDatePicker.SelectedDate;
            caseDueDate = datePicked == null ? now : DateTime.Parse(datePicked.ToString());
        }

        private void newWorkItem_SelectionChanges(object sender, SelectionChangedEventArgs e)
        {
           
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
                MessageBox.Show("Hey, we need an int over here.");
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            saveButton.IsEnabled = false;
            //dtModified.Clear();
            if (!dtModified.Columns.Contains("SalesForceCaseID"))
            {
                DataColumn caseIDColumn = new DataColumn("SalesForceCaseID", typeof(System.Int32));

                caseIDColumn.DefaultValue = caseID;

                dtModified.Columns.Add(caseIDColumn);
            }
            else 
            {
                foreach (DataRow r in dtModified.Rows)
                {
                    r.SetField("SalesForceCaseID", caseID);
                }       
            }
           

            DataView view = (DataView) newWorkItem.ItemsSource;
            
            dtModified = DataViewAsDataTable(view);

            newWorkItem.ItemsSource = dtModified.AsDataView();

            saveButton.IsEnabled = true;
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
    }
}
