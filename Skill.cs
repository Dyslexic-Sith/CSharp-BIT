using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITClientServer
{
    public class Skill
    {
        private int skillID;
        private string skillTitle;
        private string skillDescription;
        private int status;
        private DataRow drSkill;
        private string _connectionString;

        #region Getters and Setters

        public int SkillID
        {
            get
            {
                return skillID;
            }

            set
            {
                skillID = value;
            }
        }

        public string SkillTitle
        {
            get
            {
                return skillTitle;
            }

            set
            {
                skillTitle = value;
            }
        }

        public string SkillDescription
        {
            get
            {
                return skillDescription;
            }

            set
            {
                skillDescription = value;
            }
        }

        public int Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
        #endregion

        #region Public Constructors
        public Skill(int skillID, string skillTitle, string skillDescription, int status)
        {
            this.skillID = skillID;
            this.skillTitle = skillTitle;
            this.skillDescription = skillDescription;
            this.status = status;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Skill(string skillTitle, string skillDescription, int status)
        {
            this.skillTitle = skillTitle;
            this.skillDescription = skillDescription;
            this.status = status;
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Skill(string skillTitle, string skillDescription)
        {
            this.skillTitle = skillTitle;
            this.skillDescription = skillDescription;
            this.status = 1;
        }

        public Skill(DataRow drSkill)
        {
            this.skillID = int.Parse(drSkill["skillsID"].ToString());
            this.skillTitle = drSkill["skillsTitle"].ToString();
            this.skillDescription = drSkill["skillsDescription"].ToString();
            this.status = int.Parse(drSkill["status"].ToString());
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public Skill()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }
        #endregion

        #region Public Methods

        public static DataTable ReadSkill()
        {
            string readQuery = "SELECT * FROM Skills WHERE status = 1";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        public static DataTable ReadSkillCombo()
        {
            string readQuery = "SELECT skillsID, skillsTitle FROM Skills WHERE status = '1'";
            DataAccessLayer dal = new DataAccessLayer();
            return (dal.ReadRecords(readQuery));
        }

        public void AddSkill()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(this._connectionString);
                int rowsAffected = dal.RunQuery("INSERT INTO Skills (skillsTitle, skillsDescription, status) VALUES ('" + SkillTitle + "', '" + SkillDescription + "', 1)");
                if (rowsAffected == 0)
                {
                    throw new Exception("Skill was not added. Message 1.");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void DeleteSkill()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(this._connectionString);

                int rowsAffected = dal.RunQuery("UPDATE Skills SET status = 0 WHERE skillsID = '" + skillID + "'");
                if (rowsAffected == 0)
                {
                    throw new Exception("The skill was not deleted. Message 1");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("The skill was not deleted. Message 2", ex);
            }
        }

        public void UpdateSkill()
        {
            try
            {
                DataAccessLayer dal = new DataAccessLayer(this._connectionString);

                int rowsAffected = dal.RunQuery("UPDATE Skills SET skillsTitle = '" + skillTitle + "', skillsDescription = '" + skillDescription + "' WHERE skillsID = '" + skillID + "'");
                if (rowsAffected == 0)
                {
                    throw new Exception("The skill was not updated. Message 1");
                }
            }
            catch (Exception ex)
            {

                throw new Exception("The skill was not updated. Message 2", ex);
            }
        }
        #endregion
    }
}
