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
namespace BITFirstDraftScreens
{
    /// <summary>
    /// Interaction logic for AddContractorSkills.xaml
    /// </summary>
    public partial class AddContractorSkills : Window
    {
        public AddContractorSkills()
        {
            InitializeComponent();
            
        }

        public CheckBox addTextBox()
        {
            SkillCollection skills = new SkillCollection();
            foreach(Skill s in skills)
            {
                CheckBox txt = new CheckBox();
                lstBxSkills.Items.Add(txt);
                txt.Content = s.SkillTitle;
            }

            return new CheckBox();
        }
        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            addTextBox();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnAddSkills_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
