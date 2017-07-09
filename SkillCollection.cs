using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer
{
    [DataObject(true)]

    
    public class SkillCollection : List<Skill>
    {
        public SkillCollection skillCollection { get { return skillCollection; } set { skillCollection = value; } }

        public SkillCollection()
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtSkills = DAL.RunMySQLDataTable("select * from Skills WHERE status = 1");

            foreach (DataRow drSkill in dtSkills.Rows)
            {
                Skill newSkill = new Skill(drSkill);
                this.Add(newSkill);
            }
        }

        public SkillCollection(Contractor con)
        {
            DataAccessLayer DAL = new DataAccessLayer(ConfigurationManager.ConnectionStrings["connString"].ConnectionString);

            DataTable dtSkills = DAL.RunMySQLDataTable("select * from Employee, Skills, Contractor, ContractorSkills WHERE Employee.employeeID = " + con.ContractorID + " AND Employee.employeeID = Contractor.contractorID AND Contractor.contractorID = ContractorSkills.contractorID AND ContractorSkills.skillsID = Skills.skillsID AND Skills.status = 1;");

            foreach (DataRow drSkill in dtSkills.Rows)
            {
                Skill newSkill = new Skill(drSkill);
                this.Add(newSkill);
            }
        }

       
    }
}
