using System;
using System.Collections.Generic;
using System.Linq;
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

namespace BITClientServer

{
    
    /// <summary>
    /// Interaction logic for AddNewSkill.xaml
    /// </summary>
    public partial class AddNewSkill : Window
    {
        Login cred = new Login();
        public AddNewSkill(string empType)
        {
            cred.EmpType = empType;
            InitializeComponent();
            ShowData();
        }

        private void ShowData()
        {
            try
            {
                SkillCollection mySkills = new SkillCollection();
                lvSkills.ItemsSource = mySkills;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClientGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainPage mainp = new MainPage(cred.EmpType);
            mainp.Show();
            Close();
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainw = new MainWindow();
            mainw.Show();
            Close();
        }

        private void btnSkillDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Skill selectedSkill = (Skill)lvSkills.SelectedItem;
                selectedSkill.DeleteSkill();
                ShowData();
                ClearFields();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }
            
        }

        private void ClearFields()
        {
            txtBxSkillTitle.Clear();
            txtBxSkillDescription.Clear();
            lvSkills.SelectedIndex = -1;
        }

        private void btnSkillAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBxSkillTitle.Text == "" || txtBxSkillDescription.Text == "")
                {
                    MessageBox.Show("Please make sure there are no empty fields before adding a Skill.");
                }

                else
                {
                    Skill newSkill = new Skill();
                    newSkill.SkillTitle = txtBxSkillTitle.Text;
                    newSkill.SkillDescription = txtBxSkillDescription.Text;
                    newSkill.AddSkill();
                    ShowData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }

        }

        private void btnSkillUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtBxSkillDescription.Text == "" || txtBxSkillTitle.Text == "")
                {
                    var result = MessageBox.Show("Is this correct? Do you want to proceed with the update?", "You have some empty fields.", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Skill updateSkill = (Skill)lvSkills.SelectedItem;
                        updateSkill.SkillDescription = txtBxSkillDescription.Text;
                        updateSkill.SkillTitle = txtBxSkillTitle.Text;
                        updateSkill.UpdateSkill();
                        ShowData();
                        ClearFields();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Something went wrong.");
            }

        }

        private void btnSkillClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }
    }
}
